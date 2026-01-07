using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Root element of an Adaptive Card.
/// </summary>
public class AdaptiveCard
{
    /// <summary>
    /// Must be "AdaptiveCard".
    /// </summary>
    public string Type => "AdaptiveCard";

    /// <summary>
    /// Schema version that this card requires.
    /// </summary>
    public string Version { get; set; } = "1.5";

    /// <summary>
    /// The schema URL (optional).
    /// </summary>
    [JsonPropertyName("$schema")]
    public string? Schema { get; set; } = "http://adaptivecards.io/schemas/adaptive-card.json";

    /// <summary>
    /// The card elements to show in the primary card region.
    /// </summary>
    public List<AdaptiveElement>? Body { get; set; }

    /// <summary>
    /// The Actions to show in the card's action bar.
    /// </summary>
    public List<AdaptiveAction>? Actions { get; set; }

    /// <summary>
    /// Defines auto-refresh configuration for the card (Adaptive Cards 1.4+).
    /// </summary>
    [JsonPropertyName("refresh")]
    public RefreshConfiguration? Refresh { get; set; }

    /// <summary>
    /// Defines authentication configuration for the card (Adaptive Cards 1.4+).
    /// </summary>
    [JsonPropertyName("authentication")]
    public AuthenticationConfiguration? Authentication { get; set; }

    /// <summary>
    /// Additional metadata for the card.
    /// </summary>
    [JsonPropertyName("metadata")]
    public CardMetadata? Metadata { get; set; }
}
