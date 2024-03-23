namespace Dex.Indexing;

public record class IndexSource(
    string Name,
    string? UserAgent,
    string? Referrer,
    int RateLimitCount,
    int RateLimitDelay);
