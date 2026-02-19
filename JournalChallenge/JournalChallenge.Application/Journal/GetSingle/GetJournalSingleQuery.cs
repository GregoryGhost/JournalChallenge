namespace JournalChallenge.Application.Journal.GetSingle;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record GetJournalSingleQuery(long Id) : IQuery<JournalDetailDto>;
