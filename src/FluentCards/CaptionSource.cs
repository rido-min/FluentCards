using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a caption source for media elements.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.6.</remarks>
public class CaptionSource
{
    /// <summary>
    /// The label for the caption (e.g., language name).
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// The MIME type of the caption source.
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    /// <summary>
    /// The URL of the caption source.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
