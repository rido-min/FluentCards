using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Groups items together in a container.
/// </summary>
public class Container : AdaptiveElement
{
    /// <summary>
    /// The items to display in the container.
    /// </summary>
    [JsonPropertyName("items")]
    public List<AdaptiveElement>? Items { get; set; }

    /// <summary>
    /// The style of the container.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// The vertical alignment of content within the container.
    /// </summary>
    [JsonPropertyName("verticalContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// If true, allow content to bleed through the container's padding.
    /// </summary>
    [JsonPropertyName("bleed")]
    public bool? Bleed { get; set; }

    /// <summary>
    /// Minimum height of the container.
    /// </summary>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// Background image for the container.
    /// </summary>
    [JsonPropertyName("backgroundImage")]
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Action to invoke when the container is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }
}
