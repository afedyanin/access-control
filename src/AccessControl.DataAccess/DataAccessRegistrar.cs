using AccessControl.Contracts.Repositories;
using AccessControl.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.DataAccess;

public static class DataAccessRegistrar
{
    public static IServiceCollection AddAccessCotrolDataAccess(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(sp => new DbConnectionFactory(connectionString!));
        services.AddDbContextFactory<AccessControlDbContext>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<IRolesRepository, RolesRepository>();
        services.AddSingleton<IFeatureKeysRepository, FeatureKeysRepository>();
        services.AddSingleton<IUsersRepository, UsersRepository>();
        services.AddSingleton<IResourcesRepository, ResourcesRepository>();

        return services;
    }
}
