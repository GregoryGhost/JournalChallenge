namespace JournalChallenge.Application.Journal.GetSingle;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;

using Microsoft.EntityFrameworkCore;

public interface IGetJournalSingleQueryHandler : IQueryHandler<GetJournalSingleQuery, JournalDetailDto>;

internal sealed class GetJournalSingleQueryHandler(IApplicationDbContext context)
    : IGetJournalSingleQueryHandler
{
    public async Task<Result<JournalDetailDto, IError<DomainError>>> HandleAsync(
        GetJournalSingleQuery query,
        CancellationToken cancellationToken)
    {
        var entry = await context.ExceptionJournals
                                 .FirstOrDefaultAsync(x => x.EventId == query.EventId, cancellationToken);

        if (entry == null)
        {
            return Result.Failure<JournalDetailDto, IError<DomainError>>(DomainError.NotFound("Journal entry not found."));
        }

        return new JournalDetailDto
        {
            Id = entry.Id,
            EventId = entry.EventId.ToString(),
            CreatedAt = entry.Timestamp,
            Text = $"Message: {entry.Message}\nStack Trace: {entry.StackTrace}"
        };
    }
}
