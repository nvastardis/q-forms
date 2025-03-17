using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QForms.Identity;

namespace QForms;

public static class QFormsApplicationModule
{
    public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureAutomapper(services);
        ConfigureAppServices(services);
    }

    private static void ConfigureAppServices(IServiceCollection services)
    {
        services.AddTransient<IIdentityUserAppService, IdentityUserAppService>();
    }

    private static void ConfigureAutomapper(IServiceCollection services)
    {
        var currentAssembly = typeof(QFormsApplicationModule).Assembly;
        
        var filterAutomapperProfiles = currentAssembly
            .GetTypes()
            .Where(e => 
                e is { IsClass: true, IsAbstract: false } &&
                e.IsSubclassOf(typeof(Profile)))
            .Select(e => (Profile)Activator.CreateInstance(e)!);

        services.AddAutoMapper(
            e =>
            e.AddProfiles(filterAutomapperProfiles));
    }
}