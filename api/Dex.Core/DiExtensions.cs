namespace Dex.Core;

public static class DiExtensions
{
    public static IDependencyResolver AddCore(this IDependencyResolver bob)
    {
        return bob
            .AddServices((services, config) =>
            {
                var userAgent = config["MangaDex:UserAgent"] ?? throw new NullReferenceException("MangaDex:UserAgent is required");

                services
                    .AddRedis()
                    .AddJson()
                    .AddCardboardHttp()
                    .AddSerilog()
                    .AddFileCache()
                    .AddMangaDex(string.Empty, userAgent: userAgent, throwOnError: true);
            })
            .Transient<IConfigurationService, ConfigurationService>()
            .Transient<IMangaDexService, MangaDexService>();
    }
}
