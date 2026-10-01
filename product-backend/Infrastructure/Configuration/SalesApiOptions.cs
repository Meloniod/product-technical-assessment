namespace Infrastructure.Configuration
{
    public sealed class SalesApiOptions
    {
        public const string SectionName = "SalesApi";

        public string BaseUrl { get; init; } = string.Empty;

        public int TimeoutSeconds { get; init; } = 20;
    }
}
