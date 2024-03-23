namespace Dex.Indexing;

public class MatchResult
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("error")]
    public string[] Error { get; set; } = [];

    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonIgnore]
    public bool Success => Status == "ok";
}

public class MatchResult<T> : MatchResult
{
    [JsonPropertyName("result")]
    public T[] Result { get; set; } = [];
}

