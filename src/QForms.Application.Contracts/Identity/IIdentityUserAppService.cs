using System.ComponentModel.DataAnnotations;

namespace QForms.Identity;

public interface IIdentityUserAppService
{
    Task<IReadOnlyList<IdentityUserLookUpDto>> GetListAsync(
        IdentityUserFilterInputDto? input = null,
        CancellationToken cancellationToken = default);
    
    Task<IdentityUserDto?> GetAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<IdentityUserLookUpDto> CreateAsync(
        IdentityUserCreateOrUpdateDto input,
        CancellationToken cancellationToken = default);
    
    Task<IdentityUserDto?> UpdateAsync(
        Guid id,
        IdentityUserCreateOrUpdateDto input,
        CancellationToken cancellationToken = default);
    
    Task<IdentityUserDto?> UpdateRolesAsync(
        Guid id,
        string[] newRoles,
        CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}

public class IdentityUserCreateOrUpdateDto
{
    [Required]
    public string UserName { get; init; }
    
    [Required]
    public string Email { get; init; }

    public string? PhoneNumber { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? RoleName { get; init; }
}

public class IdentityUserDto
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Role { get; init; }
}

public class IdentityUserLookUpDto
{
    public string? UserName { get; init; }
    public string? Name { get; init; }
    public string? Surname { get; init; }
    public string? Role { get; init; }
}


