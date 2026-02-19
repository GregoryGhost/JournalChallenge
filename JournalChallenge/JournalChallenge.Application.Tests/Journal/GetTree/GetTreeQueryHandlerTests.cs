namespace JournalChallenge.Application.Tests.Journal.GetTree;

using FluentAssertions;

using JournalChallenge.Application.Journal.GetTree;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

[TestFixture]
public sealed class GetTreeQueryHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public GetTreeQueryHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task HandleAsync_ShouldCreateTreeAndRootNode_WhenTreeDoesNotExist()
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
}
