namespace JournalChallenge.Application.Tests.Journal.GetRange;

using FluentAssertions;

using JournalChallenge.Application.Journal.GetRange;
using JournalChallenge.Domain.Journal;
using JournalChallenge.Infrastructure.Database;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public sealed class GetJournalRangeQueryHandlerTests : BaseDbTest<AppDbContext>
{
    protected override AppDbContext DbContext { get; init; }

    public GetJournalRangeQueryHandlerTests()
    {
        DbContext = new AppDbContext(Options);
    }

    [Test]
    public async Task TestValidQueryShouldReturnRange()
    {
        // Arrange
        var entry1 = new ExceptionJournal
        {
            EventId = 1,
            Timestamp = DateTime.UtcNow.AddMinutes(-10),
            QueryParams = "{}",
            BodyParams = "{}",
            StackTrace = "stack",
            ExceptionType = "Ex",
            Message = "msg1"
        };
        var entry2 = new ExceptionJournal
        {
            EventId = 2,
            Timestamp = DateTime.UtcNow,
            QueryParams = "{}",
            BodyParams = "{}",
            StackTrace = "stack",
            ExceptionType = "Ex",
            Message = "msg2"
        };
        DbContext.ExceptionJournals.AddRange(entry1, entry2);
        await DbContext.SaveChangesAsync();

        var handler = new GetJournalRangeQueryHandler(DbContext);
        var query = new GetJournalRangeQuery(0, 10, null);

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(2);
        result.Value.Count.Should().Be(2);
    }

    [Test]
    public async Task TestSearchFilterShouldFilterResults()
    {
        // Arrange
        var entry1 = new ExceptionJournal
        {
            EventId = 1,
            Timestamp = DateTime.UtcNow.AddMinutes(-10),
            QueryParams = "{}",
            BodyParams = "{}",
            StackTrace = "stack",
            ExceptionType = "Ex",
            Message = "unique_message"
        };
        var entry2 = new ExceptionJournal
        {
            EventId = 2,
            Timestamp = DateTime.UtcNow,
            QueryParams = "{}",
            BodyParams = "{}",
            StackTrace = "stack",
            ExceptionType = "Ex",
            Message = "other"
        };
        DbContext.ExceptionJournals.AddRange(entry1, entry2);
        await DbContext.SaveChangesAsync();

        var handler = new GetJournalRangeQueryHandler(DbContext);
        var query = new GetJournalRangeQuery(0, 10, new JournalFilter(null, null, "unique"));

        // Act
        var result = await handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(1);
        result.Value.Items.First().EventId.Should().Be(1);
    }
}
