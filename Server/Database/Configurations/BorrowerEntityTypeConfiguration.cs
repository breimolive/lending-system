using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities;

namespace Server.Database.Configurations;

public class BorrowerEntityTypeConfiguration: IEntityTypeConfiguration<BorrowerEntity>
{
    public void Configure(EntityTypeBuilder<BorrowerEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Email)
            .HasMaxLength(BorrowerEntity.MaxEmailLength)
            .IsRequired();
            
        builder
            .Property(e => e.FirstName)
            .HasMaxLength(BorrowerEntity.MaxFirstNameLength)
            .IsRequired();
            
        builder
            .Property(e => e.LastName)
            .HasMaxLength(BorrowerEntity.MaxLastNameLength)
            .IsRequired();
            
        builder
            .Property(e => e.PhoneNumber)
            .HasMaxLength(BorrowerEntity.MaxPhoneNumberLength)
            .IsRequired();
        
        builder
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}