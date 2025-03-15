using Microsoft.EntityFrameworkCore;
using QForms.EntityFrameworkCore;

namespace QForms.DbMigrator.Data;

public class QFormsDbMigrationJob
{
    private readonly QFormsDbContext _dbContext;
    private readonly ILogger _logger;
    

    public QFormsDbMigrationJob(
        ILogger<QFormsDbMigrationJob> logger,
        QFormsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Started database migrations...");
        if (!await _dbContext.Database.CanConnectAsync(cancellationToken))
        {
            _logger.LogInformation("Creating initial migration...");
            await _dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }

        if (!(await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            _logger.LogInformation("Database already up to date.");
            return;
        }

        _logger.LogInformation("Migrating schema for host database...");
        await _dbContext.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Successfully completed host database migrations.");
    }
}