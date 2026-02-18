namespace JournalChallenge.Application.Abstractions.Data;

using JournalChallenge.Application.Core.Abstractions;

using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext: IDbContext
{
    DbSet<TestEntity> Tests { get; set; }
}