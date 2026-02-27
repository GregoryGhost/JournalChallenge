namespace JournalChallenge.Application.Tests.Journal.GetSingle;

using JournalChallenge.Application.Journal.GetSingle;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class GetJournalSingleQueryValidationTests
{
    private readonly GetJournalSingleQueryValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenIdIsZero()
    {
        var query = new GetJournalSingleQuery(0);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.EventId);
    }

    [Test]
    public void ShouldNotHaveErrorWhenIdIsPositive()
    {
        var query = new GetJournalSingleQuery(1);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
