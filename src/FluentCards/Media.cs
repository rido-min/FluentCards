using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays audio or video content.
/// </summary>
public class Media : AdaptiveElement
{
    /// <summary>
    /// The media sources with fallback options.
    /// </summary>
    [JsonPropertyName("sources")]
    public List<MediaSource>? Sources { get; set; }

    /// <summary>
    /// The URL of the poster image for video content.
    /// </summary>
    [JsonPropertyName("poster")]
    public string? Poster { get; set; }

    /// <summary>
    /// Alternate text for accessibility.
    /// </summary>
    [JsonPropertyName("altText")]
    public string? AltText { get; set; }

    /// <summary>
    /// Caption sources for the media (e.g., subtitles).
    /// </summary>
    [JsonPropertyName("captionSources")]
    public List<CaptionSource>? CaptionSources { get; set; }
}
