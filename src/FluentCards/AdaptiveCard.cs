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
    /// Action to invoke when the card is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }

    /// <summary>
    /// Text shown when the client doesn't support the version specified (may contain markdown).
    /// </summary>
    [JsonPropertyName("fallbackText")]
    public string? FallbackText { get; set; }

    /// <summary>
    /// Specifies the minimum height of the card.
    /// </summary>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// When true, content in this card should be presented right to left.
    /// </summary>
    [JsonPropertyName("rtl")]
    public bool? Rtl { get; set; }

    /// <summary>
    /// Specifies what should be spoken for this entire card. This is simple text or SSML fragment.
    /// </summary>
    [JsonPropertyName("speak")]
    public string? Speak { get; set; }

    /// <summary>
    /// The 2-letter ISO-639-1 language used in the card. Used to localize any date/time functions.
    /// </summary>
    [JsonPropertyName("lang")]
    public string? Lang { get; set; }

    /// <summary>
    /// Defines how the content should be aligned vertically within the container.
    /// </summary>
    [JsonPropertyName("verticalContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

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
