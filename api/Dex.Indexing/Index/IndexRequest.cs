namespace Dex.Indexing;

public class IndexRequest
{
    public const string TYPE_PAGES = "pages";
    public const string TYPE_COVER = "cover";

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("mangaId")]
    public required string MangaId { get; set; }

    [JsonPropertyName("chapterId")]
    public string? ChapterId { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

