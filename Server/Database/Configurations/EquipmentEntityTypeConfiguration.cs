using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities;

namespace Server.Database.Configurations;

public class EquipmentEntityTypeConfiguration : IEntityTypeConfiguration<EquipmentEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Name)
            .HasMaxLength(EquipmentEntity.MaxNameLength)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasMaxLength(EquipmentEntity.MaxDescriptionLength);

        builder
            .Property(e => e.Status)
            .IsRequired();

        builder
            .Property(x => x.SerialNumber)
            .HasMaxLength(EquipmentEntity.SerialNumberLength);

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder
            .HasIndex(x => x.SerialNumber)
            .IsUnique();

        // Indexes for filtering
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Status);
    }
}