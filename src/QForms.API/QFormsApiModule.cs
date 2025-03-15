using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QForms.Identity;

namespace QForms;

public static class QFormsApiModule
{
    public static void ConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureIdentity(services, configuration);
        services.ConfigureApplication(configuration);
    }

    public static void ConfigureEndpoints(this IEndpointRouteBuilder endpoints, IServiceProvider services)
    {
        endpoints.MapIdentityApi<ApplicationUser>(
            services.GetRequiredService<IOptions<IdentityEndpointOptions>>()); 
    }
    
    private static void ConfigureIdentity(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityEndpointOptions>(configuration.GetSection("IdentityEndpointOptions"));

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