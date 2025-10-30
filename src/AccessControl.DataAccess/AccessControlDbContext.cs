using AccessControl.DataAccess.Dbos;
using AccessControl.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.DataAccess;

internal class AccessControlDbContext : DbContext
{
    public DbSet<FeatureKeyDbo> FeatureKeys { get; set; }

    public DbSet<RoleDbo> Roles { get; set; }

    public DbSet<UserDbo> Users { get; set; }

    public DbSet<ResourceDbo> Resources { get; set; }


    public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("access_control");

        new FeatureKeyEntityConfiguration().Configure(modelBuilder.Entity<FeatureKeyDbo>());
        new RoleEntityConfiguration().Configure(modelBuilder.Entity<RoleDbo>());
        new UserEntityConfiguration().Configure(modelBuilder.Entity<UserDbo>());
        new ResourceEntityConfiguration().Configure(modelBuilder.Entity<ResourceDbo>());
    }
}
