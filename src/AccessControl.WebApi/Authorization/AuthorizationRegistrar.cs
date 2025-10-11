using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.WebApi.Authorization;
public static class AuthorizationRegistrar
{
    public static IServiceCollection AddApiKeyuthorization(this IServiceCollection services)
    {
        services.AddSingleton<ApiKeyAuthorizationFilter>();
        services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

        return services;
    }
}
