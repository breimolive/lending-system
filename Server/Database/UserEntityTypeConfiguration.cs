using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Server.Database;

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
            .Property(e => e.Hash)
            .HasMaxLength(70)
            .IsRequired();

        builder.Property(e => e.Salt)
            .HasMaxLength(40)
            .IsRequired();
    }
}