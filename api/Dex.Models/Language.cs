namespace Dex.Models;

/// <summary>
/// Represents a language 
/// </summary>
[Table("languages")]
public class Language : DbObject
{
    /// <summary>
    /// The ISO 639-1 code of the language
    /// </summary>
    [Column("code", Unique = true)]
    public required string Code { get; set; }

    /// <summary>
    /// The English name of the language
    /// </summary>
    [Column("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The name of the language in the language itself
    /// </summary>
    [Column("native")]
    public string? Native { get; set; }

    /// <summary>
    /// The country flag for the language
    /// </summary>
    [Column("flag")]
    public string? Flag { get; set; }
}
