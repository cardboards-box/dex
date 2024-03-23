namespace Dex.Core;

/// <summary>
/// A helper service for interacting with the application's configuration.
/// </summary>
public interface IConfigurationService
{
    /// <summary>
    /// Fetches an optional configuration value.
    /// </summary>
    /// <param name="key">The configuration key</param>
    /// <returns></returns>
    string? Optional(string key);

    /// <summary>
    /// Fetches an optional int configuration value
    /// </summary>
    /// <param name="key">The configuration key</param>
    /// <returns></returns>
    int? OptionalInt(string key);

    /// <summary>
    /// Fetches a required configuration value.
    /// </summary>
    /// <param name="key">The configuration key</param>
    /// <returns></returns>
    string Required(string key);

    /// <summary>
    /// Fetches a required int configuration value.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    int RequiredInt(string key);

    /// <summary>
    /// Fetches the configuration section with the given name.
    /// </summary>
    /// <param name="name">The name of the section</param>
    /// <returns>The configuration section</returns>
    IConfigurationSection Section(string name);
}

internal class ConfigurationService(IConfiguration _config) : IConfigurationService
{
    public string? Optional(string key) => _config[key];

    public int? OptionalInt(string key) => int.TryParse(Optional(key), out var result) ? result : null;

    public string Required(string key)
    {
        return Optional(key) ?? throw new NullReferenceException($"Configuration option required by not specified: {key}");
    }

    public int RequiredInt(string key)
    {
        return OptionalInt(key) ?? throw new NullReferenceException($"Configuration option required by not specified or invalid: {key}");
    }

    public IConfigurationSection Section(string name) => _config.GetSection(name);
}
