namespace JournalChallenge.Application.Journal.CreateNode;

using FluentValidation;

internal sealed class CreateNodeCommandValidator : AbstractValidator<CreateNodeCommand>
{
    public CreateNodeCommandValidator()
    {
        RuleFor(x => x.TreeName)
            .NotEmpty()
            .MaximumLength(255);
            
        RuleFor(x => x.NodeName)
            .NotEmpty()
            .MaximumLength(255);
    }
}
