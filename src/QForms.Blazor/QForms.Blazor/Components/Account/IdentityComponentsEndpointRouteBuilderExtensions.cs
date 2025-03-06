using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QForms.Identity;

namespace QForms.Blazor.Components.Account;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var accountGroup = endpoints.MapGroup("/Account");

        accountGroup.MapPost("/Logout", async (
            [FromServices] SignInManager<ApplicationUser> signInManager,
            [FromForm] string returnUrl) =>
        {
            await signInManager.SignOutAsync();
            return TypedResults.LocalRedirect($"~/{returnUrl}");
        });
        return accountGroup;
    }
    
    /// <summary>
    /// Configures the Identity API based on the IdentityEndpointOptions 
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="options"></param>
    /// <typeparam name="TUser"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static IEndpointConventionBuilder MapIdentityApi<TUser>(
        this IEndpointRouteBuilder endpoints,
        IOptions<IdentityEndpointOptions> options)
        where TUser : class, new()
    {
        var identityEndpoints = endpoints.MapIdentityApi<TUser>();
        var endpointOptions = options.Value;
        if (!endpointOptions.IncludeRegister)
        {
            identityEndpoints = identityEndpoints.FilterIdentityRegisterApi();
        }
        
        if (!endpointOptions.IncludeLogin)
        {
            identityEndpoints = identityEndpoints.FilterIdentityLoginApi();
        }
        else
        {
            endpoints.MapAdditionalIdentityEndpoints();
        }

        if (!endpointOptions.IncludeRefresh)
        {
            identityEndpoints = identityEndpoints.FilterIdentityRefreshApi();
        }
        
        return identityEndpoints;
    }

    /// <summary>
    /// Filters out the Registration API endpoints
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static IEndpointConventionBuilder FilterIdentityRegisterApi(this IEndpointConventionBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.AddEndpointFilter(
            async (context, next) =>
                context.HttpContext.Request.Path == "/register"
                    ? Results.NotFound()
                    : await next(context)
        );

        return endpoints;
    }

    /// <summary>
    /// Filters out the Login API endpoints
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static IEndpointConventionBuilder FilterIdentityLoginApi(this IEndpointConventionBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        endpoints.AddEndpointFilter(
            async (context, next) =>
            {
                if (!context.HttpContext.Request.Path.HasValue)
                {
                    return await next(context);
                }
                var path = context.HttpContext.Request.Path.Value;
                if (
                    path[..].Contains("login") ||
                    path[..].Contains("Password") ||
                    path[..].Contains("2fa") ||
                    path[..].Contains("info") ||
                    path[..].Contains("Email") ||
                    path[..].Contains("Logout"))
                {
                    Results.NotFound();
                }
                    
                return await next(context);
            }
        );

        return endpoints;
    }

    /// <summary>
    /// Filters out the Refresh API endpoints
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static IEndpointConventionBuilder FilterIdentityRefreshApi(this IEndpointConventionBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        
        endpoints.AddEndpointFilter(
            async (context, next) =>
                context.HttpContext.Request.Path == "/refresh"
                    ? Results.NotFound()
                    : await next(context)
        );
        
        throw new NotImplementedException();
    }
}