using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines auto-refresh configuration for Adaptive Cards (version 1.4+).
/// </summary>
public class RefreshConfiguration
{
    /// <summary>
    /// The action to invoke when refreshing the card.
    /// </summary>
    [JsonPropertyName("action")]
    public AdaptiveAction? Action { get; set; }

    /// <summary>
    /// A list of user IDs for which the refresh should trigger.
    /// </summary>
    [JsonPropertyName("userIds")]
    public List<string>? UserIds { get; set; }

    /// <summary>
    /// ISO 8601 datetime string indicating when the card expires.
    /// </summary>
    [JsonPropertyName("expires")]
    public string? Expires { get; set; }
}
