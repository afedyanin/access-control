using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.DataAccess.Tests;

public class RepositoryTestBase
{
    private static readonly string LocalConnection = "Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=access_control;";

    private readonly IServiceProvider _serviceProvider;

    protected IDbContextFactory<AccessControlDbContext> DbContextFactory => _serviceProvider.GetRequiredService<IDbContextFactory<AccessControlDbContext>>();

    protected IAccessRoleRepository AccessRoleRepository => _serviceProvider.GetRequiredService<IAccessRoleRepository>();

    protected IFeatureKeyRepository FeatureKeyRepository => _serviceProvider.GetRequiredService<IFeatureKeyRepository>();

    protected RepositoryTestBase()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAccessCotrolDataAccess(LocalConnection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
