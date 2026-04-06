using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Specifies a background image for a container.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.2.</remarks>
public class BackgroundImage
{
    /// <summary>
    /// The URL of the background image.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The fill mode for the background image.
    /// </summary>
    [JsonPropertyName("fillMode")]
    [JsonConverter(typeof(CamelCaseEnumConverter<BackgroundImageFillMode>))]
    public BackgroundImageFillMode? FillMode { get; set; }

    /// <summary>
    /// The horizontal alignment of the background image.
    /// </summary>
    [JsonPropertyName("horizontalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// The vertical alignment of the background image.
    /// </summary>
    [JsonPropertyName("verticalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalAlignment { get; set; }
}

/// <summary>
/// Specifies the fill mode for a background image.
/// </summary>
public enum BackgroundImageFillMode
{
    /// <summary>
    /// Cover the entire area.
    /// </summary>
    Cover,

    /// <summary>
    /// Repeat the image horizontally.
    /// </summary>
    RepeatHorizontally,

    /// <summary>
    /// Repeat the image vertically.
    /// </summary>
    RepeatVertically,

    /// <summary>
    /// Repeat the image in both directions.
    /// </summary>
    Repeat
}
