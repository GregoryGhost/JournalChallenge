namespace JournalChallenge.Application.Tests.Journal;

using FluentAssertions;

using JournalChallenge.Application.Journal;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Application.Journal.CreateNode;
using JournalChallenge.Application.Journal.DeleteNode;
using JournalChallenge.Application.Journal.GetTree;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

[TestFixture]
public sealed class JournalTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public JournalTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task GetTree_ShouldCreateTreeAndRootNode_WhenTreeDoesNotExist()
    {
        // Arrange
        var handler = new GetTreeQueryHandler(DbContext);
        var query = new GetTreeQuery("NewTree");

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("NewTree");
        
        var tree = await DbContext.Trees.FirstOrDefaultAsync(t => t.Name == "NewTree");
        tree.Should().NotBeNull();
        
        var root = await DbContext.Nodes.FirstOrDefaultAsync(n => n.TreeId == tree!.Id && n.ParentId == null);
        root.Should().NotBeNull();
        root!.Name.Should().Be("NewTree");
    }

    [Test]
    public async Task CreateNode_ShouldFail_WhenAddingSecondRoot()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        await DbContext.SaveChangesAsync();

        var handler = new CreateNodeCommandHandler(DbContext);
        var command = new CreateNodeCommand("Tree1", null, "NewRoot");

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Error.Type.Should().Be(ErrorType.Secure);
        result.Error.Error.Message.Should().Be("A tree cannot have more than one root node");
    }

    [Test]
    public async Task CreateNode_ShouldFail_WhenNameExistsAmongSiblings()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        var child1 = new Node { Name = "Child1", Parent = root, Tree = tree };
        DbContext.Nodes.Add(child1);
        await DbContext.SaveChangesAsync();

        var handler = new CreateNodeCommandHandler(DbContext);
        var command = new CreateNodeCommand("Tree1", root.Id, "Child1");

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Error.Type.Should().Be(ErrorType.Secure);
    }

    [Test]
    public async Task DeleteNode_ShouldFail_WhenHasChildrenAndNotForced()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        var child1 = new Node { Name = "Child1", Parent = root, Tree = tree };
        DbContext.Nodes.Add(child1);
        await DbContext.SaveChangesAsync();

        var handler = new DeleteNodeCommandHandler(DbContext);
        var command = new DeleteNodeCommand(root.Id, false);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Error.Message.Should().Be("You have to delete all children nodes first");
    }

    [Test]
    public async Task DeleteNode_ShouldSucceed_WhenForcedDeletionIsTrue()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        var child1 = new Node { Name = "Child1", Parent = root, Tree = tree };
        DbContext.Nodes.Add(child1);
        await DbContext.SaveChangesAsync();

        var handler = new DeleteNodeCommandHandler(DbContext);
        var command = new DeleteNodeCommand(root.Id, true);

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var deletedRoot = await DbContext.Nodes.FindAsync(root.Id);
        deletedRoot.Should().BeNull();
        
        var deletedChild = await DbContext.Nodes.FindAsync(child1.Id);
        deletedChild.Should().BeNull();
    }
}
