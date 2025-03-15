using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QForms.Data;
using QForms.EntityFrameworkCore;
using QForms.Identity;

namespace QForms;

public static class QFormsInfrastructureModule
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDatabaseConnection(services, configuration);
        ConfigureRepositories(services, configuration);
    }

    private static void ConfigureRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient(typeof(IRepository<>),typeof(QFormsRepositoryBase<>));
        services.AddTransient(typeof(IRepository<,>),typeof(QFormsRepositoryBase<,>));
    }

    private static void ConfigureDatabaseConnection(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = 
            configuration.GetConnectionString(QFormsInfrastructureConsts.DbConnectionStringKey)
                ?? throw new Exception("Default connection string missing!");

        services.AddDbContext<QFormsDbContext>(options =>
        {
            options.UseSqlite(dbConnectionString);
        });

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<QFormsDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders()
            .AddApiEndpoints();
        
        services.AddDataProtection();
    }
}