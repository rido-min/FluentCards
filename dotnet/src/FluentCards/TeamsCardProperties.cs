using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Teams-specific card-level properties, serialized as the <c>msteams</c> object on <see cref="AdaptiveCard"/>.
/// </summary>
public class TeamsCardProperties
{
    /// <summary>
    /// The card width. Use <see cref="TeamsCardWidth.Full"/> for full-width cards.
    /// Omit (leave null) for default width.
    /// </summary>
    [JsonPropertyName("width")]
    public TeamsCardWidth? Width { get; set; }

    /// <summary>
    /// List of mention entities referenced in card body text.
    /// </summary>
    [JsonPropertyName("entities")]
    public List<Mention>? Entities { get; set; }
}
