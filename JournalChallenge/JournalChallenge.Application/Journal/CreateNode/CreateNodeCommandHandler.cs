namespace JournalChallenge.Application.Journal.CreateNode;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;
using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;

public interface ICreateNodeCommandHandler : ICommandHandler<CreateNodeCommand>;

internal sealed class CreateNodeCommandHandler(IApplicationDbContext context)
    : ICreateNodeCommandHandler
{
    public async Task<UnitResult<IError<DomainError>>> HandleAsync(
        CreateNodeCommand command,
        CancellationToken cancellationToken)
    {
        var tree = await context.Trees
                                .FirstOrDefaultAsync(t => t.Name == command.TreeName, cancellationToken);

        if (tree == null)
        {
            tree = new Tree { Name = command.TreeName };
            context.Trees.Add(tree);
            await context.SaveChangesAsync(cancellationToken);
        }

        if (command.ParentNodeId == null)
        {
            // Single Root Check
            var hasRoot = await context.Nodes.AnyAsync(
                n => n.TreeId == tree.Id && n.ParentId == null,
                cancellationToken);

            if (hasRoot)
            {
                return UnitResult.Failure(DomainError.Secure("A tree cannot have more than one root node"));
            }
        }
        else
        {
            // Verify Parent exists and belongs to tree
            var parent = await context.Nodes.FirstOrDefaultAsync(
                n => n.Id == command.ParentNodeId && n.TreeId == tree.Id,
                cancellationToken);

            if (parent == null)
            {
                return UnitResult.Failure(DomainError.NotFound("Parent node not found in this tree."));
            }
        }

        // Sibling Uniqueness Check
        var isDuplicate = await context.Nodes.AnyAsync(
            n => n.TreeId == tree.Id && n.ParentId == command.ParentNodeId && n.Name == command.NodeName,
            cancellationToken);

        if (isDuplicate)
        {
            return UnitResult.Failure(DomainError.Secure("A node with this name already exists among siblings."));
        }

        var newNode = new Node
        {
            Name = command.NodeName,
            ParentId = command.ParentNodeId,
            TreeId = tree.Id
        };

        context.Nodes.Add(newNode);
        await context.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<IError<DomainError>>();
    }
}
