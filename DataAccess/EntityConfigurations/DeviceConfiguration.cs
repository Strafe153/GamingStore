using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.EntityConfigurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder
            .HasKey(d => d.Id);

        builder
            .HasIndex(d => d.Name)
            .IsUnique();

        builder
            .Property(d => d.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(d => d.Price)
            .HasColumnType("decimal(8,2)")
            .IsRequired();

        builder
            .Property(d => d.Category)
            .HasDefaultValue(DeviceCategory.Mouse);

        builder
            .Property(d => d.CompanyId)
            .IsRequired();
    }
}
