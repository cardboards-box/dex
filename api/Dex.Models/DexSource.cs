namespace Dex.Models;

/// <summary>
/// Represents a source that dex can scrape from.
/// </summary>
[Table("dex_source")]
public class DexSource : DbObject
{
    /// <summary>
    /// The name of the source
    /// </summary>
    [Column("name", Unique = true)]
    public required string Name { get; set; }

    /// <summary>
    /// A description of the source
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The URL of the source
    /// </summary>
    [Column("url")]
    public required string Url { get; set; }

    /// <summary>
    /// The base URL of the source scraper
    /// </summary>
    [Column("base_url")]
    public required string BaseUrl { get; set; }

    /// <summary>
    /// The mime-type of the requests made to scrape
    /// </summary>
    [Column("request_mime_type")]
    public string RequestMimeType { get; set; } = "application/json";

    /// <summary>
    /// Whether or not to scrap and index the source
    /// </summary>
    [Column("enabled")]
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// The referrer to use when requesting images or pages
    /// </summary>
    [Column("referrer")]
    public string? Referrer { get; set; }

    /// <summary>
    /// The user agent to use when requesting images or pages
    /// </summary>
    [Column("user_agent")]
    public string? UserAgent { get; set; }

    /// <summary>
    /// How many requests can be made before delaying to avoid rate limits.
    /// </summary>
    [Column("rate_limit_count")]
    public int RateLimitCount { get; set; }

    /// <summary>
    /// How many seconds to wait between batches of requests
    /// </summary>
    [Column("rate_limit_delay")]
    public int RateLimitDelay { get; set; }

    /// <summary>
    /// The other sources this source should not be threaded against
    /// </summary>
    [Column("stacks_with")]
    public Guid[] StacksWith { get; set; } = [];
}
