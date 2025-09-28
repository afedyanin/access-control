using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class FeatureKeyEntityConfiguration : IEntityTypeConfiguration<FeatureKey>
{
    public void Configure(EntityTypeBuilder<FeatureKey> builder)
    {
        builder.ToTable("feature_key");

        builder.HasKey(e => e.Id)
            .HasName("feature_key_pkey");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();
    }
}
