using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using QForms.Identity;

namespace QForms.DbMigrator.Data;

public class QFormsDataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ILogger _logger;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public QFormsDataSeeder(
        ILogger<QFormsDataSeeder> logger,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedUsersIfNewAsync(
        string entityName,
        string jsonFileName,
        CancellationToken cancellationToken = default)
    {
        var jsonFileNameClean =
            Path.GetFileNameWithoutExtension(jsonFileName).Replace('.', Path.DirectorySeparatorChar);
        var seedObjects = await LoadDataAsync<ApplicationUser>(
            entityName,
            jsonFileNameClean + ".json",
            cancellationToken);

        foreach (var seedObj in seedObjects)
        {
            if (await _userManager.FindByIdAsync(seedObj.Id.ToString()) is not null)
            {
                continue;
            }

            await _userManager.CreateAsync(seedObj);
            await _userManager.AddPasswordAsync(seedObj, "1q2w3E*");

            if (seedObj.UserName == "admin")
            {
                await _userManager.AddToRoleAsync(seedObj, IdentityRoleConsts.AdministratorRoleName);
                continue;
            }
            await _userManager.AddToRoleAsync(seedObj, IdentityRoleConsts.UserRoleName);
        }
        _logger.LogInformation($"Succeeded Seeding Entity: {entityName}");
    }

    public async Task SeedRolesIfNewAsync(
        string entityName,
        string jsonFileName,
        CancellationToken cancellationToken)
    {
        var jsonFileNameClean =
            Path.GetFileNameWithoutExtension(jsonFileName).Replace('.', Path.DirectorySeparatorChar);
        var seedObjects = await LoadDataAsync<IdentityRole<Guid>>(
            entityName,
            jsonFileNameClean + ".json", 
            cancellationToken);

        foreach (var seedObj in seedObjects)
        {
            if (await _roleManager.FindByIdAsync(seedObj.Id.ToString()) is null)
            {
                await _roleManager.CreateAsync(seedObj);
            }
        }
        _logger.LogInformation($"Succeeded Seeding Entity: {entityName}");
    }

    private async Task<List<T>> LoadDataAsync<T>(
        string entityName,
        string jsonFileName,
        CancellationToken cancellationToken = default)
    {
        var dataAsJson = await File.ReadAllTextAsync(jsonFileName, cancellationToken);
        var seedObjects = JsonSerializer.Deserialize<List<T>>(dataAsJson, _jsonSerializerOptions) ??
                throw new SerializationException($"Unable to Deserialize JSON line to {entityName}");
        return seedObjects;
    }
}