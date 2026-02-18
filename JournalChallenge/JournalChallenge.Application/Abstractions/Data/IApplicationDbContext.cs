namespace JournalChallenge.Application.Abstractions.Data;

public interface IApplicationDbContext: IDbContext
{
    DbSet<TestEntity> Tests { get; set; }
}