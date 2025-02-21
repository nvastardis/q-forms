using QForms.DbMigrator.Data;

namespace QForms.DbMigrator.Services;

public class DbMigratorService: IHostedService
{
    private readonly ILogger<DbMigratorService> _logger;
    private readonly QFormsDbMigrationJob _migrationJob;
    private readonly QFormsDbSeedingJob _seedingJob;

    public DbMigratorService(
        ILogger<DbMigratorService> logger,
        QFormsDbMigrationJob migrationJob,
        QFormsDbSeedingJob seedingJob)
    {
        _logger = logger;
        _migrationJob = migrationJob;
        _seedingJob = seedingJob;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _migrationJob.MigrateAsync(cancellationToken);
            await _seedingJob.SeedAsync(cancellationToken);
            await StopAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message, ex.StackTrace);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("DbMigratorService finished database modifications.");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}