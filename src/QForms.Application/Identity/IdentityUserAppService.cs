using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using QForms.Exceptions;
using QForms.Utils;

namespace QForms.Identity;

[Authorize(Roles = IdentityRoleConsts.AdministratorRoleName)]
public class IdentityUserAppService: QFormsAppServiceBase, IIdentityUserAppService
{
    private IRepository<ApplicationUser, Guid> Repository { get; set; }
    private UserManager<ApplicationUser> Manager { get; set; }
    private RoleManager<IdentityRole<Guid>> RoleManager { get; set; }
    
    public IdentityUserAppService(
        UserManager<ApplicationUser> manager,
        IRepository<ApplicationUser, Guid> repository,
        RoleManager<IdentityRole<Guid>> roleManager,
        IMapper objectMapper)
        : base(objectMapper)
    {
        Repository = repository;
        RoleManager = roleManager;
        Manager = manager;
    }

    public async Task<IReadOnlyList<IdentityUserLookUpDto>> GetListAsync(
        IdentityUserFilterInputDto? input = null,
        CancellationToken cancellationToken = default)
    {
        List<ApplicationUser> result;

        if (input == null)
        {
            result = await Repository.GetListAsync(includeDetails:false,cancellationToken);
        }
        else
        {
            var filterInput = ObjectMapper.Map<IdentityUserFilterInputDto, IdentityUserFilter>(input);
            result = await Repository.GetListAsync(
                filterInput,
                includeDetails:false,
                cancellationToken: cancellationToken);
        }
        
        return ObjectMapper.Map<List<ApplicationUser>, List<IdentityUserLookUpDto>>(result);
    }

    public async Task<IdentityUserDto?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await Repository.GetAsync(id, includeDetails: false, cancellationToken);
        return ObjectMapper.Map<ApplicationUser, IdentityUserDto>(result);        
    }

    public async Task<IdentityUserLookUpDto> CreateAsync(
        IdentityUserCreateOrUpdateDto input,
        CancellationToken cancellationToken = default)
    {
        var createObject = ObjectMapper.Map<IdentityUserCreateOrUpdateDto, ApplicationUser>(input);
        var result = await Repository.InsertAsync(createObject, autoSave: true, cancellationToken);
        result = await UpdateUserRoleAsync(input.RoleName, result);
        return ObjectMapper.Map<ApplicationUser, IdentityUserLookUpDto>(result);
    }

    public async Task<IdentityUserDto?> UpdateAsync(
        Guid id,
        IdentityUserCreateOrUpdateDto input,
        CancellationToken cancellationToken = default)
    {
        var currentObject = await Repository.GetAsync(id, includeDetails: false, cancellationToken);
        var updateObject = GenerateUpdateObject(input, currentObject);
        var result = await Repository.UpdateAsync(updateObject, autoSave:true, cancellationToken);
        return ObjectMapper.Map<ApplicationUser, IdentityUserDto>(result);
    }

    public async Task<IdentityUserDto?> UpdateRolesAsync(
        Guid id,
        string[] newRoles,
        CancellationToken cancellationToken = default)
    {
        var user = await Repository.GetAsync(id, includeDetails: false, cancellationToken);
        await Manager.RemoveFromRolesAsync(user, IdentityRoleConsts.RoleNameList);
        foreach (var role in newRoles)
        {
            await UpdateUserRoleAsync(role, user);
        }
        
        return ObjectMapper.Map<ApplicationUser, IdentityUserDto>(user);
    }


    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default) 
        => await Repository.DeleteAsync(id, autoSave:true, cancellationToken);

    private static ApplicationUser GenerateUpdateObject(
        IdentityUserCreateOrUpdateDto input,
        ApplicationUser user)
    {
        user.Name = ConditionallyUpdateProperty(user.Name, input.Name)!;
        user.Surname = ConditionallyUpdateProperty(user.Surname, input.Surname)!;
        user.Email = ConditionallyUpdateProperty(user.Email, input.Email);
        user.PhoneNumber = ConditionallyUpdateProperty(user.PhoneNumber, input.PhoneNumber);
        return user;

        string? ConditionallyUpdateProperty(string? property, string? newValue)
        {
            if (!newValue.IsNullOrWhiteSpace() && property != newValue)
            {
                property = newValue;
            }

            return property;
        }
    }

    private async Task<ApplicationUser> UpdateUserRoleAsync(string? roleName, ApplicationUser user)
    {
        if (roleName.IsNullOrWhiteSpace() 
            || !await RoleManager.RoleExistsAsync(roleName!)
            || await Manager.IsInRoleAsync(user, roleName!))
        {
            return user;
        }
        
        var identityResult = await Manager.AddToRoleAsync(user, roleName!);
        if (!identityResult.Succeeded)
        {
            var errors = identityResult.Errors
                .Select(e => new KeyValuePair<string, object>(e.Code, e.Description))
                .ToDictionary();

            throw new BusinessException()
                .WithData(errors);   
        }
        
        return user;
    }
}