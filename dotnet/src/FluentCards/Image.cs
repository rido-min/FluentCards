using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents an image in an Adaptive Card.
/// </summary>
public class Image : AdaptiveElement
{
    /// <summary>
    /// The URL of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Alternate text for the image for accessibility.
    /// </summary>
    [JsonPropertyName("altText")]
    public string? AltText { get; set; }

    /// <summary>
    /// The size of the image.
    /// </summary>
    [JsonPropertyName("size")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageSize>))]
    public ImageSize? Size { get; set; }

    /// <summary>
    /// The style of the image.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageStyle>))]
    public ImageStyle? Style { get; set; }

    /// <summary>
    /// The horizontal alignment of the image.
    /// </summary>
    [JsonPropertyName("horizontalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// The background color for the image.
    /// </summary>
    [JsonPropertyName("backgroundColor")]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// The width of the image.
    /// </summary>
    [JsonPropertyName("width")]
    public string? Width { get; set; }

    /// <summary>
    /// The height of the image.
    /// </summary>
    [JsonPropertyName("height")]
    public new string? Height { get; set; }

    /// <summary>
    /// Action to invoke when the image is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }
}
