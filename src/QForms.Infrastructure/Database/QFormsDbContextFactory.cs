using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QForms.Database;

public class QFormsDbContextFactory: IDesignTimeDbContextFactory<QFormsDbContext>
{
    public QFormsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<QFormsDbContext>()
            .UseSqlite(configuration.GetConnectionString(QFormsInfrastructureConsts.DbConnectionStringKey));

        return new QFormsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "../QForms.DbMigrator/")))
            .AddJsonFile("appsettings.Development.json", optional: false);

        return builder.Build();
    }
}