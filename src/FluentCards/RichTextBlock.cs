using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays text with inline formatting via TextRun elements.
/// </summary>
public class RichTextBlock : AdaptiveElement
{
    /// <summary>
    /// The inline text elements (TextRun objects or plain strings).
    /// </summary>
    [JsonPropertyName("inlines")]
    public List<object>? Inlines { get; set; }

    /// <summary>
    /// The horizontal alignment of the text.
    /// </summary>
    [JsonPropertyName("horizontalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }
}
