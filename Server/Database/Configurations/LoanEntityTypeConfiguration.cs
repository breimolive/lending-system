using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities;

namespace Server.Database.Configurations;

public class LoanEntityTypeConfiguration: IEntityTypeConfiguration<LoanEntity>
{
    public void Configure(EntityTypeBuilder<LoanEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.LoanDate)
            .IsRequired();
        
        builder
            .Property(e => e.DueDate)
            .IsRequired();
        
        builder
            .Property(e => e.Status)
            .IsRequired();

        builder
            .HasOne(e => e.Equipment)
            .WithMany()
            .HasForeignKey(e => e.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}