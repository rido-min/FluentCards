using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Metadata for an Adaptive Card.
/// </summary>
public class CardMetadata
{
    /// <summary>
    /// The URL associated with the card metadata.
    /// </summary>
    [JsonPropertyName("webUrl")]
    public string? WebUrl { get; set; }
}
