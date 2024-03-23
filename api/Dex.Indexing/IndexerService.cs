namespace Dex.Indexing;

public interface IIndexerService
{
    Task<bool> Run(CancellationToken token);
}

internal class IndexerService(
    IIndexQueueHandler _queue,
    ILogger<IndexerService> _logger) : IIndexerService
{
    public async Task<bool> Run(CancellationToken token)
    {
        var errored = false;
        try
        {
            _logger.LogInformation("Starting index queue.");
            await _queue.Setup(token);
            await Task.Delay(-1, token);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Index queue cancelled. Was exit requested?");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Index queue failed.");
            errored = true;
        }
        finally
        {
            _queue.Dispose();
        }

        return !errored;
    }
}
