namespace JournalChallenge.Application.Journal.DeleteNode;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record DeleteNodeCommand(
    long NodeId,
    bool IsForcedDeletion) : ICommand;
