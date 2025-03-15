using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace QForms.EntityFrameworkCore;

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
            .AddJsonFile("appsettings.Development.json", optional: false)
            .AddUserSecrets("8e6992ca-654b-48ea-af67-25a264e6eecc");

        return builder.Build();
    }
}