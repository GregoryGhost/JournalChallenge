using JournalChallenge.Tests.Core.Settings;

[assembly: FluentAssertions.Extensibility.AssertionEngineInitializer(
    typeof(AssertionEngineInitializer),
    nameof(AssertionEngineInitializer.AcknowledgeSoftWarning))]

namespace JournalChallenge.Tests.Core.Settings;

public static class AssertionEngineInitializer
{
    public static void AcknowledgeSoftWarning()
    {
        FluentAssertions.License.Accepted = true;
    }
}