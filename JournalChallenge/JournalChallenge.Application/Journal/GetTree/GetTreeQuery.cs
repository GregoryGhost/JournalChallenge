namespace JournalChallenge.Application.Journal.GetTree;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record GetTreeQuery(string TreeName) : IQuery<NodeDto>;
