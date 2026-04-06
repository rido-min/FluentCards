using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays a collection of images in a grid.
/// </summary>
public class ImageSet : AdaptiveElement
{
    /// <summary>
    /// The images to display in the set.
    /// </summary>
    [JsonPropertyName("images")]
    public List<Image>? Images { get; set; }

    /// <summary>
    /// The size for all images in the set.
    /// </summary>
    [JsonPropertyName("imageSize")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ImageSize>))]
    public ImageSize? ImageSize { get; set; }
}
