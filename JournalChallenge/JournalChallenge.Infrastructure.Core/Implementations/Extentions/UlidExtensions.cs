namespace JournalChallenge.Infrastructure.Core.Implementations.Extentions;

using CSharpFunctionalExtensions;

using MoreLinq.Extensions;

public static class UlidExtensions
{
    public static Maybe<Ulid> ParseUlid(this string ulidStr)
    {
        return Ulid.TryParse(ulidStr, out var ulid) ? ulid : Maybe<Ulid>.None;
    }

    public static Result<IEnumerable<Ulid>, IEnumerable<int>> ParseUlids(this IEnumerable<string> input)
    {
        var array = input as string[] ?? input.ToArray();
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (input == null || array.Length == 0) return Result.Failure<IEnumerable<Ulid>, IEnumerable<int>>(Array.Empty<int>());
        
        var (failed, succeeded) = array
                                  .Index()
                                  .Select(x => (x.Key, Maybe: ParseUlid(x.Value)))
                                  .Partition(x => x.Maybe.HasNoValue);

        var arrayOfFailed = failed as (int Key, Maybe<Ulid> Maybe)[] ?? failed.ToArray();
        if (arrayOfFailed.Length != 0)
        {
            var failedIndexes = arrayOfFailed.Select(x => x.Key).ToArray();
            
            return Result.Failure<IEnumerable<Ulid>, IEnumerable<int>>(failedIndexes);
        }

        var oks = succeeded.Select(x => x.Maybe.Value).ToArray();
        
        return Result.Success<IEnumerable<Ulid>, IEnumerable<int>>(oks);
    }
}