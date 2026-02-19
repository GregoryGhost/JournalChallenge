namespace JournalChallenge.Application.Journal;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record CreateNodeCommand(
    string TreeName,
    long? ParentNodeId,
    string NodeName) : ICommand;
