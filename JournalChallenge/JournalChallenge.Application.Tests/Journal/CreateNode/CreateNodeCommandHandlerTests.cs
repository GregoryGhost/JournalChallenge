namespace JournalChallenge.Application.Tests.Journal.CreateNode;

using FluentAssertions;

using JournalChallenge.Application.Journal.CreateNode;
using JournalChallenge.Application.Core.Abstractions.ErrorHandling;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public sealed class CreateNodeCommandHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public CreateNodeCommandHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task TestAddingSecondRootShouldBeFailure()
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
    public async Task TestNameExistsAmongSiblingsShouldBeFailure()
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
}
