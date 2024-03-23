using Dex.Cli.Verbs;
using Dex.Core;
using Dex.Indexing;
using Dex.Polling;

var services = new ServiceCollection().AddConfig(c => 
    c.AddEnvironmentVariables()
     .AddJsonFile("appsettings.json")
     .AddCommandLine(args),
    out var config);

await services.AddServices(config, c => 
    c.AddCore()
     .AddIndexing()
     .AddPolling());

return await services.Cli(args, c =>
{
    c.Add<ImageIndexVerb>();
});