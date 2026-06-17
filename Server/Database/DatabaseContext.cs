using Microsoft.EntityFrameworkCore;
using Server.Database.Configurations;
using Server.Database.Entities;

namespace Server.Database;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<EquipmentEntity> Equipment { get; set; }
    public DbSet<LoanEntity> Loans { get; set; }
    public DbSet<BorrowerEntity> Borrowers { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
    }
}