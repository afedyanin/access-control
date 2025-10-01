using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(e => e.Name)
            .HasName("roles_pkey");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("description");
    }
}

