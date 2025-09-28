using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class FeatureKeyEntityConfiguration : IEntityTypeConfiguration<FeatureKey>
{
    public void Configure(EntityTypeBuilder<FeatureKey> builder)
    {
        builder.ToTable("feature_keys");

        builder.HasKey(e => e.Id)
            .HasName("feature_keys_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-and-join-table-with-payload
        builder.HasMany(e => e.AccessRoles)
            .WithMany()
            .UsingEntity<FeatureKeyAccessRole>(
                j =>
                {
                    j.ToTable("feature_key_role");
                    j.Property(e => e.Permissions).HasColumnName("permissions");
                    j.Property(e => e.FeatureKeyId).HasColumnName("feature_key_id");
                    j.Property(e => e.AccessRoleId).HasColumnName("access_role_id");
                });
    }
}
