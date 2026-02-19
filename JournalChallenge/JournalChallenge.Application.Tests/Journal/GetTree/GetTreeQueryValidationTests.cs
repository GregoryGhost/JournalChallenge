namespace JournalChallenge.Application.Tests.Journal.GetTree;

using JournalChallenge.Application.Journal.GetTree;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class GetTreeQueryValidationTests
{
    private readonly GetTreeQueryValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenTreeNameIsEmpty()
    {
        var query = new GetTreeQuery(string.Empty);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.TreeName);
    }

    [Test]
    public void ShouldHaveErrorWhenTreeNameExceedsMaxLength()
    {
        var query = new GetTreeQuery(new string('a', 256));
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.TreeName);
    }

    [Test]
    public void ShouldNotHaveErrorWhenTreeNameIsSpecified()
    {
        var query = new GetTreeQuery("ValidTree");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
