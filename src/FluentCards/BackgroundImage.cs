using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Specifies a background image for a container.
/// </summary>
public class BackgroundImage
{
    /// <summary>
    /// The URL of the background image.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Describes how the image should fill the container.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageFillMode>))]
    public ImageFillMode? FillMode { get; set; }

    /// <summary>
    /// Horizontal alignment of the background image.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// Vertical alignment of the background image.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalAlignment { get; set; }
}
