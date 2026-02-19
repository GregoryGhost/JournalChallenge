namespace JournalChallenge.Application.Tests.Journal.RenameNode;

using FluentAssertions;

using JournalChallenge.Application.Journal.RenameNode;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public sealed class RenameNodeCommandHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public RenameNodeCommandHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task HandleAsync_ShouldSucceed_WhenValid()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        await DbContext.SaveChangesAsync();

        var handler = new RenameNodeCommandHandler(DbContext);
        var command = new RenameNodeCommand(root.Id, "NewName");

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var renamedNode = await DbContext.Nodes.FindAsync(root.Id);
        renamedNode!.Name.Should().Be("NewName");
    }

    [Test]
    public async Task HandleAsync_ShouldFail_WhenNameExistsAmongSiblings()
    {
        // Arrange
        var tree = new Tree { Name = "Tree1" };
        DbContext.Trees.Add(tree);
        var root = new Node { Name = "Root", Tree = tree };
        DbContext.Nodes.Add(root);
        var child1 = new Node { Name = "Child1", Parent = root, Tree = tree };
        var child2 = new Node { Name = "Child2", Parent = root, Tree = tree };
        DbContext.Nodes.AddRange(child1, child2);
        await DbContext.SaveChangesAsync();

        var handler = new RenameNodeCommandHandler(DbContext);
        var command = new RenameNodeCommand(child1.Id, "Child2");

        // Act
        var result = await handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Error.Type.Should().Be(ErrorType.Secure);
    }
}
