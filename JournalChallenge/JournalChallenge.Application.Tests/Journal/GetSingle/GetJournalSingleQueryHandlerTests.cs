namespace JournalChallenge.Application.Tests.Journal.GetSingle;

using FluentAssertions;

using JournalChallenge.Application.Journal.GetSingle;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public sealed class GetJournalSingleQueryHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public GetJournalSingleQueryHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task TestEntryExistsShouldReturnDetail()
    {
        // Arrange
        var entry = new ExceptionJournal
        {
            EventId = 123,
            Timestamp = DateTime.UtcNow,
            QueryParams = "{}",
            BodyParams = "{}",
            StackTrace = "stack",
            ExceptionType = "Ex",
            Message = "msg"
        };
        DbContext.ExceptionJournals.Add(entry);
        await DbContext.SaveChangesAsync();

        var handler = new GetJournalSingleQueryHandler(DbContext);
        var query = new GetJournalSingleQuery(entry.EventId);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.EventId.Should().Be("123");
        result.Value.Text.Should().Contain("msg");
        result.Value.Text.Should().Contain("stack");
    }

    [Test]
    public async Task TestEntryDoesNotExistShouldBeFailure()
    {
        // Arrange
        var handler = new GetJournalSingleQueryHandler(DbContext);
        var query = new GetJournalSingleQuery(999);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
