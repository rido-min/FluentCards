using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a media source with MIME type and URL.
/// </summary>
public class MediaSource
{
    /// <summary>
    /// The MIME type of the media.
    /// </summary>
    [JsonPropertyName("mimeType")]
    public string? MimeType { get; set; }

    /// <summary>
    /// The URL of the media source.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}
