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
}
