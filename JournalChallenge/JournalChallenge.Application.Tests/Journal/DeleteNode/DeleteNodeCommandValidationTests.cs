namespace JournalChallenge.Application.Tests.Journal.DeleteNode;

using JournalChallenge.Application.Journal.DeleteNode;

using FluentValidation.TestHelper;

using NUnit.Framework;

[TestFixture]
public sealed class DeleteNodeCommandValidationTests
{
    private readonly DeleteNodeCommandValidator _validator = new();

    [Test]
    public void ShouldHaveErrorWhenNodeIdIsZero()
    {
        var command = new DeleteNodeCommand(0, false);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.NodeId);
    }

    [Test]
    public void ShouldNotHaveErrorWhenValid()
    {
        var command = new DeleteNodeCommand(1, false);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
