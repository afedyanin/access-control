using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class ResourceEntityConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.ToTable("resources");

        builder.HasKey(e => e.Id)
            .HasName("resources_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<ResourceRole>(
                j =>
                {
                    j.ToTable("resource_role");
                    j.Property(e => e.Permissions).HasColumnName("permissions");
                    j.Property(e => e.Resource).HasColumnName("resource_id");
                    j.Property(e => e.RoleName).HasColumnName("role_name");
                });
    }
}
