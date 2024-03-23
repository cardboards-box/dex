namespace Dex.Models;

/// <summary>
/// Represents a localized string
/// </summary>
public class Localization
{
    /// <summary>
    /// The ID of the language this localization belongs to.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The value of the localization.
    /// </summary>
    public required string Value { get; set; }
}
