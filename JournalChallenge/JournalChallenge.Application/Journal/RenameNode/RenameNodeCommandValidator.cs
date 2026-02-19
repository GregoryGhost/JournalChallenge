namespace JournalChallenge.Application.Journal.RenameNode;

using FluentValidation;

internal sealed class RenameNodeCommandValidator : AbstractValidator<RenameNodeCommand>
{
    public RenameNodeCommandValidator()
    {
        RuleFor(x => x.NodeId).GreaterThan(0);
        RuleFor(x => x.NewNodeName)
            .NotEmpty()
            .MaximumLength(255);
    }
}
