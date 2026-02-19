namespace JournalChallenge.Application.Journal.DeleteNode;

using FluentValidation;

internal sealed class DeleteNodeCommandValidator : AbstractValidator<DeleteNodeCommand>
{
    public DeleteNodeCommandValidator()
    {
        RuleFor(x => x.NodeId).GreaterThan(0);
    }
}
