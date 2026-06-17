using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Entities;

namespace Server.Database.Configurations;

public class CategoryEntityTypeConfiguration: IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(e => e.Name)
            .HasMaxLength(CategoryEntity.MaxNameLength)
            .IsRequired();
        
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}