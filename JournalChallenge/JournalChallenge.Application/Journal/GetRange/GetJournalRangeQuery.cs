namespace JournalChallenge.Application.Journal.GetRange;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record JournalFilter(
    DateTime? From,
    DateTime? To,
    string? Search);

public sealed record GetJournalRangeQuery(
    int Skip,
    int Take,
    JournalFilter? Filter) : IQuery<JournalRangeResponse>;
