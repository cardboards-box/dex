namespace Dex.Cli.Verbs;

using Indexing;

[Verb("index-images", isDefault: true, HelpText = "Reads the image index queue and indexes the images with the matching service")]
internal class ImageIndexVerbOptions { }

internal class ImageIndexVerb(
    ILogger<ImageIndexVerb> logger,
    IIndexerService _indexer) : BooleanVerb<ImageIndexVerbOptions>(logger)
{
    public override Task<bool> Execute(ImageIndexVerbOptions _, CancellationToken token)
    {
        return _indexer.Run(token);
    }
}
