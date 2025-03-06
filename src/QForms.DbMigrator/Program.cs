using QForms.DbMigrator.Data;
using QForms.DbMigrator.Services;

namespace QForms.DbMigrator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var currentEnvironment = Environment.GetEnvironmentVariable("RUNTIME_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>()
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{currentEnvironment}.json", false, true)
            .Build();

        var hostBuilder = Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(configOptions =>
            {
                configOptions.AddConfiguration(configuration);
            })
            .ConfigureServices(services =>
            {
                services.AddLogging(options =>
                {
                    options.AddSimpleConsole(formatterOptions =>
                    {
                        formatterOptions.IncludeScopes = false;
                    });
                    options.AddConfiguration(configuration.GetSection("Logging"));
                });
                services.ConfigureInfrastructure(configuration);
                services.AddTransient<QFormsDataSeeder>();
                services.AddTransient<QFormsDbSeedingJob>();
                services.AddTransient<QFormsDbMigrationJob>();
                services.AddHostedService<DbMigratorService>();
            });
        
        var host = hostBuilder.Build();
        
        await host.RunAsync();
    }
}