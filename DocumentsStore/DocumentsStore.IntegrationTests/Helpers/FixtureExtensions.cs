using AutoFixture;

namespace DocumentsStore.IntegrationTests.Helpers;

public static class FixtureExtensions
{
    public static string CreateEmail(this IFixture fixture)
    {
        return $"{fixture.Create<string>()}@domain.com";
    }
}