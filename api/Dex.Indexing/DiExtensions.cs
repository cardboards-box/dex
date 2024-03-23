namespace Dex.Indexing;

public static class DiExtensions
{
    public static IDependencyResolver AddIndexing(this IDependencyResolver resolver)
    {
        return resolver
            .Transient<IMatchService, MatchService>()
            .Transient<IIndexQueueHandler, IndexQueueHandler>()
            .Transient<IFileIndexService, FileIndexService>()
            .Transient<IIndexerService, IndexerService>();
    }
}
