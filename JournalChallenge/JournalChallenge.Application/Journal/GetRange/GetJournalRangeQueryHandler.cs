namespace JournalChallenge.Application.Journal.GetRange;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;

using Microsoft.EntityFrameworkCore;

public interface IGetJournalRangeQueryHandler : IQueryHandler<GetJournalRangeQuery, JournalRangeResponse>;

internal sealed class GetJournalRangeQueryHandler(IApplicationDbContext context)
    : IGetJournalRangeQueryHandler
{
    public async Task<Result<JournalRangeResponse, IError<DomainError>>> HandleAsync(
        GetJournalRangeQuery query,
        CancellationToken cancellationToken)
    {
        var dbQuery = context.ExceptionJournals.AsQueryable();

        if (query.Filter != null)
        {
            if (query.Filter.From.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.Timestamp >= query.Filter.From.Value);
            }

            if (query.Filter.To.HasValue)
            {
                dbQuery = dbQuery.Where(x => x.Timestamp <= query.Filter.To.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Filter.Search))
            {
                dbQuery = dbQuery.Where(x => x.Message.Contains(query.Filter.Search) 
                                             || x.StackTrace.Contains(query.Filter.Search));
            }
        }

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
                          .OrderByDescending(x => x.Timestamp)
                          .Skip(query.Skip)
                          .Take(query.Take)
                          .Select(x => new JournalEntryDto
                          {
                              Id = x.Id,
                              EventId = x.EventId,
                              CreatedAt = x.Timestamp
                          })
                          .ToListAsync(cancellationToken);

        return new JournalRangeResponse
        {
            Skip = query.Skip,
            Count = totalCount,
            Items = items
        };
    }
}
