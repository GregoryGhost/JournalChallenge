namespace JournalChallenge.Application.Tests.Journal.DeleteNode;

using FluentAssertions;

using JournalChallenge.Application.Journal.DeleteNode;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public sealed class DeleteNodeCommandHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public DeleteNodeCommandHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task HandleAsync_ShouldFail_WhenHasChildrenAndNotForced()
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
    public async Task HandleAsync_ShouldSucceed_WhenForcedDeletionIsTrue()
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
