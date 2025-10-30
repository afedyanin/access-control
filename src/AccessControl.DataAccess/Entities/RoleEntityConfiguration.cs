using AccessControl.DataAccess.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class RoleEntityConfiguration : IEntityTypeConfiguration<RoleDbo>
{
    public void Configure(EntityTypeBuilder<RoleDbo> builder)
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

