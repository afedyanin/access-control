using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class FeatureKeyEntityConfiguration : IEntityTypeConfiguration<FeatureKey>
{
    public void Configure(EntityTypeBuilder<FeatureKey> builder)
    {
        builder.ToTable("feature_keys");

        builder.HasKey(e => e.Name)
            .HasName("feature_keys_pkey");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<FeatureKeyRole>(
                j =>
                {
                    j.ToTable("feature_key_role");
                    j.Property(e => e.Permissions).HasColumnName("permissions");
                    j.Property(e => e.FeatureKeyName).HasColumnName("feature_key_name");
                    j.Property(e => e.RoleName).HasColumnName("role_name");
                });
    }
}
