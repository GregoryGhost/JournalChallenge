namespace JournalChallenge.Infrastructure.Database;

using JournalChallenge.Application.Abstractions.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<TestEntity> Tests { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.DEFAULT);
    }
}