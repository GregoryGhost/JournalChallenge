namespace JournalChallenge.Application.Journal.GetRange;

using FluentValidation;

internal sealed class GetJournalRangeQueryValidator : AbstractValidator<GetJournalRangeQuery>
{
    public GetJournalRangeQueryValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).GreaterThan(0).LessThanOrEqualTo(100);
    }
}
