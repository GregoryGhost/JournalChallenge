namespace JournalChallenge.Application.Tests.Journal.RenameNode;

using JournalChallenge.Application.Journal.RenameNode;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class RenameNodeCommandValidationTests
{
    private readonly RenameNodeCommandValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenNodeIdIsZero()
    {
        var command = new RenameNodeCommand(0, "NewName");
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NodeId);
    }

    [Test]
    public void ShouldHaveErrorWhenNewNodeNameIsEmpty()
    {
        var command = new RenameNodeCommand(1, string.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewNodeName);
    }

    [Test]
    public void ShouldHaveErrorWhenNewNodeNameExceedsMaxLength()
    {
        var command = new RenameNodeCommand(1, new string('a', 256));
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NewNodeName);
    }

    [Test]
    public void ShouldNotHaveErrorWhenValid()
    {
        var command = new RenameNodeCommand(1, "ValidName");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
