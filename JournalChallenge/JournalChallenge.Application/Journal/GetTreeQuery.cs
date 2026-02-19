namespace JournalChallenge.Application.Journal;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record GetTreeQuery(string TreeName) : IQuery<NodeDto>;
