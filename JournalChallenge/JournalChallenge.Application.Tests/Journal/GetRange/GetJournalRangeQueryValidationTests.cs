namespace JournalChallenge.Application.Tests.Journal.GetRange;

using JournalChallenge.Application.Journal.GetRange;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class GetJournalRangeQueryValidationTests
{
    private readonly GetJournalRangeQueryValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenSkipIsNegative()
    {
        var query = new GetJournalRangeQuery(-1, 10, null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Skip);
    }

    [Test]
    public void ShouldHaveErrorWhenTakeIsZero()
    {
        var query = new GetJournalRangeQuery(0, 0, null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Take);
    }

    [Test]
    public void ShouldHaveErrorWhenTakeIsTooLarge()
    {
        var query = new GetJournalRangeQuery(0, 101, null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Take);
    }

    [Test]
    public void ShouldNotHaveErrorWhenValid()
    {
        var query = new GetJournalRangeQuery(0, 10, null);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
