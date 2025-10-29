using AccessControl.Model.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.DataAccess.Tests;

public class RepositoryTestBase
{
    private static readonly string LocalConnection = "Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=access_control;Include Error Detail=True";

    private readonly IServiceProvider _serviceProvider;

    protected IDbContextFactory<AccessControlDbContext> DbContextFactory => _serviceProvider.GetRequiredService<IDbContextFactory<AccessControlDbContext>>();

    protected IRolesRepository RolesRepository => _serviceProvider.GetRequiredService<IRolesRepository>();
    protected IUsersRepository UsersRepository => _serviceProvider.GetRequiredService<IUsersRepository>();

    protected IFeatureKeysRepository FeatureKeysRepository => _serviceProvider.GetRequiredService<IFeatureKeysRepository>();

    protected RepositoryTestBase()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddAccessCotrolDataAccess(LocalConnection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }
}
