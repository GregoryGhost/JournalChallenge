namespace JournalChallenge.Application.Tests.Journal.CreateNode;

using JournalChallenge.Application.Journal.CreateNode;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class CreateNodeCommandValidationTests
{
    private readonly CreateNodeCommandValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenTreeNameIsEmpty()
    {
        var command = new CreateNodeCommand(string.Empty, null, "NodeName");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TreeName);
    }

    [Test]
    public void ShouldHaveErrorWhenNodeNameIsEmpty()
    {
        var command = new CreateNodeCommand("TreeName", null, string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NodeName);
    }

    [Test]
    public void ShouldHaveErrorWhenTreeNameExceedsMaxLength()
    {
        var command = new CreateNodeCommand(new string('a', 256), null, "NodeName");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.TreeName);
    }

    [Test]
    public void ShouldHaveErrorWhenNodeNameExceedsMaxLength()
    {
        var command = new CreateNodeCommand("TreeName", null, new string('a', 256));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NodeName);
    }

    [Test]
    public void ShouldNotHaveErrorWhenValid()
    {
        var command = new CreateNodeCommand("TreeName", 1, "NodeName");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
