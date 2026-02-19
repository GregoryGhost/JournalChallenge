namespace JournalChallenge.Application.Journal;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;

using Microsoft.EntityFrameworkCore;

internal sealed class RenameNodeCommandHandler(IApplicationDbContext context)
    : ICommandHandler<RenameNodeCommand>
{
    public async Task<UnitResult<IError<DomainError>>> HandleAsync(
        RenameNodeCommand command,
        CancellationToken cancellationToken)
    {
        var node = await context.Nodes
                                .FirstOrDefaultAsync(n => n.Id == command.NodeId, cancellationToken);

        if (node == null)
        {
            return UnitResult.Failure(DomainError.NotFound("Node not found."));
        }

        // Sibling Uniqueness Check
        var isDuplicate = await context.Nodes.AnyAsync(
            n => n.TreeId == node.TreeId 
                 && n.ParentId == node.ParentId 
                 && n.Name == command.NewNodeName 
                 && n.Id != node.Id,
            cancellationToken);

        if (isDuplicate)
        {
            return UnitResult.Failure(DomainError.Secure("A node with this name already exists among siblings."));
        }

        node.Name = command.NewNodeName;
        await context.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<IError<DomainError>>();
    }
}
