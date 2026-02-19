namespace JournalChallenge.Application.Journal.GetSingle;

using FluentValidation;

internal sealed class GetJournalSingleQueryValidator : AbstractValidator<GetJournalSingleQuery>
{
    public GetJournalSingleQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
