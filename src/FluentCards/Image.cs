using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays an image.
/// </summary>
public class Image : AdaptiveElement
{
    /// <summary>
    /// The URL of the image to display.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Alternate text describing the image.
    /// </summary>
    public string? AltText { get; set; }

    /// <summary>
    /// Controls the size of the image.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageSize>))]
    public ImageSize? Size { get; set; }

    /// <summary>
    /// Controls the display style of the image.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageStyle>))]
    public ImageStyle? Style { get; set; }

    /// <summary>
    /// The desired width of the image (e.g., "50px").
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// The desired height of the image (e.g., "50px"). 
    /// Note: This is different from AdaptiveElement.Height which controls layout height ("auto" or "stretch").
    /// This property sets the actual pixel height of the image.
    /// </summary>
    [JsonPropertyName("height")]
    public new string? Height { get; set; }

    /// <summary>
    /// Controls the horizontal alignment of the image.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// An Action that will be invoked when the image is tapped or selected.
    /// </summary>
    public AdaptiveAction? SelectAction { get; set; }

    /// <summary>
    /// Applies a background color to the image. Useful when displaying transparent images.
    /// </summary>
    public string? BackgroundColor { get; set; }
}
