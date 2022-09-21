using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .HasIndex(u => u.Email)
            .IsUnique();

        builder
            .Property(u => u.Username)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(u => u.Role)
            .HasDefaultValue(UserRole.User);
    }
}
