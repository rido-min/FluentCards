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
    /// <remarks>Added in Adaptive Cards 1.1.</remarks>
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
    /// <remarks>Added in Adaptive Cards 1.2.</remarks>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// When true, content in this card should be presented right to left.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.5.</remarks>
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
    /// <remarks>Added in Adaptive Cards 1.2.</remarks>
    [JsonPropertyName("verticalContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// Defines auto-refresh configuration for the card.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.4.</remarks>
    [JsonPropertyName("refresh")]
    public RefreshConfiguration? Refresh { get; set; }

    /// <summary>
    /// Defines authentication configuration for the card.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.4.</remarks>
    [JsonPropertyName("authentication")]
    public AuthenticationConfiguration? Authentication { get; set; }

    /// <summary>
    /// Additional metadata for the card.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.6.</remarks>
    [JsonPropertyName("metadata")]
    public CardMetadata? Metadata { get; set; }

    /// <summary>
    /// Specifies a background image for the card.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.2.</remarks>
    [JsonPropertyName("backgroundImage")]
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Applies a known <see cref="AdaptiveCardVersion"/>, setting both <see cref="Version"/> and <see cref="Schema"/>.
    /// </summary>
    /// <param name="version">The version to apply.</param>
    internal void ApplyVersion(AdaptiveCardVersion version)
    {
        Version = version.ToVersionString();
        Schema = version.ToSchemaUrl();
    }
}
