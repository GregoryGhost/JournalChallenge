namespace JournalChallenge.Infrastructure.Database;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<ExceptionJournal> ExceptionJournals { get; set; } = null!;

    public DbSet<Node> Nodes { get; set; } = null!;
    
    public DbSet<Tree> Trees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.DEFAULT);
    }
}