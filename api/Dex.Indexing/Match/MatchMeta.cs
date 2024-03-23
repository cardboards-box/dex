namespace Dex.Indexing;

public class MatchMeta<T> : MatchImage
{
    [JsonPropertyName("metadata")]
    public T? Metadata { get; set; }
}
