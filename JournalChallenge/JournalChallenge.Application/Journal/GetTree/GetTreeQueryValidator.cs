namespace JournalChallenge.Application.Journal.GetTree;

using FluentValidation;

internal sealed class GetTreeQueryValidator : AbstractValidator<GetTreeQuery>
{
    public GetTreeQueryValidator()
    {
        RuleFor(x => x.TreeName)
            .NotEmpty()
            .MaximumLength(255);
    }
}
