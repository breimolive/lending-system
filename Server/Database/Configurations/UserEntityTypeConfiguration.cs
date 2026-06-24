using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities;

namespace Server.Database.Configurations;

public class UserEntityTypeConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Email)
            .HasMaxLength(UserEntity.MaxEmailLength)
            .IsRequired();
            
        builder
            .Property(e => e.FullName)
            .HasMaxLength(UserEntity.MaxFullNameLength)
            .IsRequired();

        builder
            .Property(e => e.PasswordHash)
            .HasMaxLength(UserEntity.MaxPasswordHashLength)
            .IsRequired();

        builder
            .HasIndex(x => x.Email)
            .IsUnique();
    }
}