using AccessControl.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AccessControl.Client.Tests;

public abstract class ApiClientBase
{
    private readonly IServiceProvider _serviceProvider;

    protected readonly IAccessControlClient AdminClient;
    protected ApiClientBase()
    {
        var services = new ServiceCollection();

        services.AddRefitClient<IAccessControlClient>()
               .ConfigureHttpClient(c =>
               {
                   c.BaseAddress = new Uri(ApiConsts.BaseUrl);
                   c.DefaultRequestHeaders.Add(ApiConsts.ApiKeyHeaderName, ApiConsts.ApiKey);
               });

        _serviceProvider = services.BuildServiceProvider();
        AdminClient = _serviceProvider.GetRequiredService<IAccessControlClient>();
    }
}
