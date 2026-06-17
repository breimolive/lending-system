using Microsoft.EntityFrameworkCore;

namespace Server.Database;

public class DatabaseContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
    }
}