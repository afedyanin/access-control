using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccessControl.DataAccess;

internal class AccessControlDbContextFactory : IDesignTimeDbContextFactory<AccessControlDbContext>
{
    private static readonly string LocalConnection = "Server=localhost;Port=5432;User Id=postgres;Password=admin;Database=access_control;";

    public AccessControlDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AccessControlDbContext>();

        optionsBuilder.UseNpgsql(
            LocalConnection, b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));

        return new AccessControlDbContext(optionsBuilder.Options);
    }
}
