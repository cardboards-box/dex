namespace Dex.Indexing;

public class Metadata
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("source")]
    public required string Source { get; set; }

    [JsonPropertyName("type")]
    public required MetadataType Type { get; set; }

    [JsonPropertyName("mangaId")]
    public required string MangaId { get; set; }

    [JsonPropertyName("chapterId")]
    public required string? ChapterId { get; set; }

    [JsonPropertyName("page")]
    public required int? Page { get; set; }
}
