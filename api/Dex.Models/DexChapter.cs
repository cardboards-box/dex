namespace Dex.Models;

[Table("dex_chapter")]
public class DexChapter : DbObject
{
    /// <summary>
    /// The ID of the <see cref="DexManga"/> this chapter belongs to.
    /// </summary>
    [Column("manga_id", Unique = true)]
    public required Guid MangaId { get; set; }

    /// <summary>
    /// The ID of the chapter from the source
    /// </summary>
    [Column("source_id", Unique = true)]
    public required string SourceId { get; set; }

    /// <summary>
    /// The ID of the language this chapter is in.
    /// </summary>
    [Column("language_id")]
    public required Guid LanguageId { get; set; }

    /// <summary>
    /// The URL of the chapter
    /// </summary>
    [Column("url")]
    public required string Url { get; set; }

    /// <summary>
    /// The chapter ordinal number
    /// </summary>
    [Column("ordinal")]
    public double Ordinal { get; set; }

    /// <summary>
    /// The volume number the chapter belongs to
    /// </summary>
    [Column("volume")]
    public double? Volume { get; set; }

    /// <summary>
    /// The title of the chapter
    /// </summary>
    [Column("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The Image URLs of the pages
    /// </summary>
    [Column("pages")]
    public string[] Pages { get; set; } = [];

    /// <summary>
    /// The URL of the chapter on the source's website
    /// </summary>
    /// <remarks>This is to handle official publishers on MangaDex</remarks>
    [Column("external_url")]
    public string? ExternalUrl { get; set; }

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
