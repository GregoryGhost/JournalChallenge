namespace JournalChallenge.Application.Journal;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;
using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;

internal sealed class DeleteNodeCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteNodeCommand>
{
    public async Task<UnitResult<IError<DomainError>>> HandleAsync(
        DeleteNodeCommand command,
        CancellationToken cancellationToken)
    {
        var node = await context.Nodes
                                .Include(n => n.Children)
                                .FirstOrDefaultAsync(n => n.Id == command.NodeId, cancellationToken);

        if (node == null)
        {
            return UnitResult.Failure(DomainError.NotFound("Node not found."));
        }

        if (node.Children.Any() && !command.IsForcedDeletion)
        {
            return UnitResult.Failure(DomainError.Secure("You have to delete all children nodes first"));
        }

        if (command.IsForcedDeletion)
        {
            // Recursive deletion logic if children exist.
            // In a real database with many nodes, this might be slow.
            // But for a challenge, we can fetch all descendants or just delete them.
            // Since we use EF Core, we can just delete the node and descendants.
            await DeleteDescendantsRecursive(node, cancellationToken);
        }

        context.Nodes.Remove(node);
        await context.SaveChangesAsync(cancellationToken);

        return UnitResult.Success<IError<DomainError>>();
    }

    private async Task DeleteDescendantsRecursive(Node node, CancellationToken cancellationToken)
    {
        // Fetch all descendants of this node within the same tree.
        // A more efficient way would be to fetch all nodes of the tree and find descendants in memory.
        var allNodesInTree = await context.Nodes
                                          .Where(n => n.TreeId == node.TreeId)
                                          .ToListAsync(cancellationToken);

        var descendants = GetDescendants(node.Id, allNodesInTree);
        
        context.Nodes.RemoveRange(descendants);
    }

    private static List<Node> GetDescendants(long parentId, List<Node> allNodes)
    {
        var result = new List<Node>();
        var children = allNodes.Where(n => n.ParentId == parentId).ToList();
        
        foreach (var child in children)
        {
            result.Add(child);
            result.AddRange(GetDescendants(child.Id, allNodes));
        }
        
        return result;
    }
}
