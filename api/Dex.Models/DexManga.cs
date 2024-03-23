namespace Dex.Models;

/// <summary>
/// Represents a cached manga entity.
/// </summary>
[Table("dex_manga")]
public class DexManga : DbObject
{
    /// <summary>
    /// The ID of the source that the manga is from.
    /// </summary>
    [Column("dex_source_id", Unique = true)]
    public required Guid DexSourceId { get; set; }

    /// <summary>
    /// The ID of the manga from the source
    /// </summary>
    [Column("source_id", Unique = true)]
    public required string SourceId { get; set; }

    /// <summary>
    /// The URL of the manga's home page
    /// </summary>
    [Column("url")]
    public required string Url { get; set; }

    /// <summary>
    /// The manga's primary English title
    /// </summary>
    [Column("title")]
    public required string Title { get; set; }

    /// <summary>
    /// Any alternative titles for the manga
    /// </summary>
    [Column("alt_titles")]
    public Localization[] AltTitles { get; set; } = [];

    /// <summary>
    /// A brief description of the manga
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The manga's tags
    /// </summary>
    [Column("tags")]
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// Whether or not the manga is NSFW
    /// </summary>
    [Column("nsfw")]
    public bool Nsfw { get; set; } = false;

    /// <summary>
    /// The manga's cover image URL
    /// </summary>
    [Column("cover_url")]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// Whether or not the chapter numbers reset in each volume
    /// </summary>
    [Column("volume_ordinal_reset")]
    public bool VolumeOrdinalReset { get; set; } = false;

    /// <summary>
    /// The serialized content of the last fetch
    /// </summary>
    [Column("last_fetch_content")]
    public string? LastFetchContent { get; set; }

    /// <summary>
    /// The last time the <see cref="LastFetchContent"/> was filled.
    /// </summary>
    [Column("last_fetch_time")]
    public DateTime? LastFetchTime { get; set; }
}
