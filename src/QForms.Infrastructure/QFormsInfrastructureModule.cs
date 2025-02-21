using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QForms.Database;
using QForms.Identity;

namespace QForms;

public static class QFormsInfrastructureModule
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        ConfigureDatabaseConnection(services, configuration);
        ConfigureIdentity(services, configuration);
    }

    private static void ConfigureDatabaseConnection(IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString(QFormsInfrastructureConsts.DbConnectionStringKey) ??
                                 throw new Exception("Default connection string missing!");
        services.AddDataProtection();
        services.AddDbContext<QFormsDbContext>(options =>
        {
            options.UseSqlite(dbConnectionString);
        });
    }

    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<QFormsDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = false;
        });
    }
}