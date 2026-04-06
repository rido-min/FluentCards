using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Metadata for an Adaptive Card.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.6.</remarks>
public class CardMetadata
{
    /// <summary>
    /// The URL associated with the card metadata.
    /// </summary>
    [JsonPropertyName("webUrl")]
    public string? WebUrl { get; set; }
}
