using Dex.Core;

namespace Dex.Indexing;

public interface IIndexQueueHandler : IRedisQueueHandler { }

internal class IndexQueueHandler(
    IRedisService _redis,
    ILogger<IndexQueueHandler> _logger,
    IMangaDex _md,
    IFileIndexService _indexer,
    IConfigurationService _config) : RedisQueueHandler<IndexRequest>(_redis, _logger), IIndexQueueHandler
{
    public int Delay => _config.RequiredInt("Redis:Queue:DelaySec");
    public override string Name => _config.Required("Redis:Queue:Name");
    public IndexSource Config => _indexer.RequiredSource("MangaDex");

    public override async Task On(IndexRequest item)
    {
        try
        {
            var task = item.Type switch
            {
                IndexRequest.TYPE_PAGES => IndexPages(item),
                IndexRequest.TYPE_COVER => IndexCover(item),
                _ => throw new NotImplementedException($"Index type not implemented: {item.Type}")
            };
            await task;

            if (Delay > 0) await Task.Delay(Delay);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing index request: {Type} :: {ChapterId} ({MangaId})", item.Type, item.ChapterId, item.MangaId);
        }
    }

    public async Task IndexCover(IndexRequest request)
    {
        if (string.IsNullOrEmpty(request.Url))
        {
            _logger.LogWarning("Index request missing url: {MangaId}", request.MangaId);
            return;
        }

        await _indexer.Index(request.Url, Config, request, null);
    }

    public async Task IndexPages(IndexRequest request)
    {
        if (string.IsNullOrEmpty(request.ChapterId))
        {
            _logger.LogWarning("Index request missing chapter id: {MangaId}", request.MangaId);
            return;
        }

        var pages = (await _md.Pages.Pages(request.ChapterId)).Images;

        var requests = 0;

        for (var i = 0; i < pages.Length; i++)
        {
            if (requests + 1 >= Config.RateLimitCount)
            {
                _logger.LogInformation("Indexing rate limit break: {Type} :: {ChapterId} ({MangaId})",
                    request.Type, request.ChapterId, request.MangaId);
                await Task.Delay(Config.RateLimitDelay);
                requests = 0;
            }

            var page = pages[i];
            requests++;

            await _indexer.Index(page, Config, request, i);
        }
    }
}
