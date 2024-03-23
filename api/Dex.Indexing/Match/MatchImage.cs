namespace Dex.Indexing;

public class MatchImage : MatchScore
{
    [JsonPropertyName("filepath")]
    public string FilePath { get; set; } = string.Empty;
}
