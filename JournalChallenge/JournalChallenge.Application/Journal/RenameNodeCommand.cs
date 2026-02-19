namespace JournalChallenge.Application.Journal;

using JournalChallenge.Application.Core.Abstractions.Messaging;

public sealed record RenameNodeCommand(
    long NodeId,
    string NewNodeName) : ICommand;
