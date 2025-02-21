using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using QForms.Identity;

namespace QForms.DbMigrator.Data;

public class QFormsDbSeedingJob
{
    private readonly Dictionary<string, string> _pathToDataFile = new()
    {
        { "Roles", "Data.Identity.RoleData.json" },
        { "Users", "Data.Identity.UserData.json" },
    };

    private readonly QFormsDataSeeder _dataSeeder;
    private readonly ILogger _logger;


    public QFormsDbSeedingJob(
        ILogger<QFormsDbSeedingJob> logger,
        QFormsDataSeeder dataSeeder)
    {
        _logger = logger;
        _dataSeeder = dataSeeder;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting database seeding...");
        await _dataSeeder.SeedRolesIfNewAsync(
            nameof(IdentityRole<Guid>),
            _pathToDataFile["Roles"],
            cancellationToken);

        await _dataSeeder.SeedUsersIfNewAsync(
            nameof(ApplicationUser),
            _pathToDataFile["Users"],
            cancellationToken);
        _logger.LogInformation("Successfully completed host database seeding.");
    }
}