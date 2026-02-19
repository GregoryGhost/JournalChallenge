namespace JournalChallenge.Application.Journal;

using CSharpFunctionalExtensions;

using JournalChallenge.Application.Abstractions.Data;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Core.Abstractions.Messaging;
using JournalChallenge.Domain.Journal;

using Microsoft.EntityFrameworkCore;

internal sealed class GetTreeQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTreeQuery, NodeDto>
{
    public async Task<Result<NodeDto, IError<DomainError>>> HandleAsync(
        GetTreeQuery query,
        CancellationToken cancellationToken)
    {
        var tree = await context.Trees
                                .FirstOrDefaultAsync(t => t.Name == query.TreeName, cancellationToken);

        if (tree == null)
        {
            tree = new Tree { Name = query.TreeName };
            context.Trees.Add(tree);
            await context.SaveChangesAsync(cancellationToken);

            // Create a root node with the same name as the tree.
            var rootNode = new Node
            {
                Name = query.TreeName,
                TreeId = tree.Id
            };
            context.Nodes.Add(rootNode);
            await context.SaveChangesAsync(cancellationToken);
            
            return MapToDto(rootNode, new List<Node>());
        }

        var allNodes = await context.Nodes
                                    .Where(n => n.TreeId == tree.Id)
                                    .ToListAsync(cancellationToken);

        var root = allNodes.FirstOrDefault(n => n.ParentId == null);
        
        if (root == null)
        {
            root = new Node
            {
                Name = query.TreeName,
                TreeId = tree.Id
            };
            context.Nodes.Add(root);
            await context.SaveChangesAsync(cancellationToken);
            allNodes.Add(root);
        }

        return MapToDto(root, allNodes);
    }

    private static NodeDto MapToDto(Node node, List<Node> allNodes)
    {
        var children = allNodes
                       .Where(n => n.ParentId == node.Id)
                       .Select(n => MapToDto(n, allNodes))
                       .ToList();

        return new NodeDto
        {
            Id = node.Id,
            Name = node.Name,
            Children = children
        };
    }
}
