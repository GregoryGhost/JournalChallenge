namespace JournalChallenge.Tests.Core.Abstractions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

public abstract class BaseDbTest<TDbContext> where TDbContext : DbContext
{
    protected abstract TDbContext DbContext { get; init; }
    protected DbContextOptions<TDbContext> Options { get; init; }

    protected BaseDbTest()
    {
        Options  = new DbContextOptionsBuilder<TDbContext>()
                      .UseInMemoryDatabase("Test")
                      .Options;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        DbContext?.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.ChangeTracker.Clear();
    }
}