using AccessControl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessControl.DataAccess.Entities;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Name)
            .HasName("users_pkey");

        builder.Property(e => e.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnName("email");

        builder.HasMany(e => e.Roles)
            .WithMany()
            .UsingEntity<UserRole>(
                j =>
                {
                    j.ToTable("user_role");
                    j.Property(e => e.UserName).HasColumnName("user_name");
                    j.Property(e => e.RoleName).HasColumnName("role_name");
                });
    }
}
