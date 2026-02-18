namespace JournalChallenge.Application.Abstractions.Data;

using JournalChallenge.Application.Core.Abstractions;
using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext : IDbContext
{
    DbSet<ExceptionJournal> ExceptionJournals { get; set; }

    DbSet<Node> Nodes { get; set; }
}