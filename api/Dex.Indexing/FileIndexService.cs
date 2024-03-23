namespace Dex.Indexing;

using Core;

public interface IFileIndexService
{
    Task<bool> Index(string url, IndexSource source, IndexRequest request, int? pageIndex);

    IndexSource? Source(string name);

    IndexSource RequiredSource(string name);
}

internal class FileIndexService(
    IMatchService _match,
    ILogger<FileIndexService> _logger,
    IApiService _api,
    IConfigurationService _config) : IFileIndexService
{
    public string DefaultUserAgent => _config.Required("DefaultUserAgent");

    public static string GenerateId(string url) => url.MD5Hash();

    public static string GenerateFileId(Metadata data)
    {
        return data.Type switch
        {
            MetadataType.Page => $"page:{data.MangaId}:{data.ChapterId}:{data.Page}",
            MetadataType.Cover => $"cover:{data.MangaId}:{data.Id}",
            _ => $"unknown:{data.Id}"
        };
    }

    public static Metadata GenerateMetadata(string url, string source, IndexRequest request, int? pageIndex)
    {
        return new Metadata
        {
            Id = GenerateId(url),
            Url = url,
            Source = source,
            Type = request.Type switch
            {
                IndexRequest.TYPE_COVER => MetadataType.Cover,
                IndexRequest.TYPE_PAGES => MetadataType.Page,
                _ => throw new Exception($"Invalid index request type: {request.Type}")
            },
            MangaId = request.MangaId,
            ChapterId = request.ChapterId,
            Page = pageIndex is null ? null : pageIndex + 1,
        };
    }

    public async Task<byte[]> DownloadFile(string url, IndexSource source)
    {
        using var io = new MemoryStream();
        var (stream, _, file, type) = await _api.GetData(url, c =>
        {
            if (string.IsNullOrEmpty(source.Referrer)) return;

            c.Headers.Add("Referrer", source.Referrer);
            c.Headers.Add("Sec-Fetch-Dest", "document");
            c.Headers.Add("Sec-Fetch-Mode", "navigate");
            c.Headers.Add("Sec-Fetch-Site", "cross-site");
            c.Headers.Add("Sec-Fetch-User", "?1");
        }, source.UserAgent ?? DefaultUserAgent);
        await stream.CopyToAsync(io);
        await stream.DisposeAsync();
        io.Position = 0;
        return io.ToArray();
    }

    public async Task<bool> Index(string url, IndexSource source, IndexRequest request, int? pageIndex)
    {
        var metadata = GenerateMetadata(url, source.Name, request, pageIndex);
        var file = await DownloadFile(url, source);
        var fileId = GenerateFileId(metadata);
        var result = await _match.Add(fileId, metadata, file);

        if (result == null)
        {
            _logger.LogError("Error occurred while indexing image: {Id} >> {Source} >> the result was null!",
                metadata.Id,
                metadata.Source);
            return false;
        }

        if (!result.Success)
        {
            _logger.LogError("Error occurred while indexing image: {Id} >> {Source} >> {Error}",
                string.Join(", ", result.Error),
                metadata.Id,
                metadata.Source);
            return false;
        }

        return true;
    }

    public IndexSource? Source(string name)
    {
        var section = _config.Section(name);
        if (!section.Exists()) return null;

        return new(name,
            _config.Optional($"{name}:UserAgent"),
            _config.Optional($"{name}:Referrer"),
            _config.OptionalInt($"{name}:RateLimit:Count") ?? 0,
            (_config.OptionalInt($"{name}:RateLimit:DelaySec") ?? 0) * 1000);
    }

    public IndexSource RequiredSource(string name)
    {
        return Source(name) ?? throw new NullReferenceException($"Configuration section required by not specified: {name}");
    }
}
