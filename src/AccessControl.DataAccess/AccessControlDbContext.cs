using AccessControl.DataAccess.Entities;
using AccessControl.Model;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess;

public class AccessControlDbContext : DbContext
{
    public DbSet<FeatureKey> FeatureKeys { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Resource> Resources { get; set; }


    public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new FeatureKeyEntityConfiguration().Configure(modelBuilder.Entity<FeatureKey>());
        new RoleEntityConfiguration().Configure(modelBuilder.Entity<Role>());
        new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
        new ResourceEntityConfiguration().Configure(modelBuilder.Entity<Resource>());
    }
}
