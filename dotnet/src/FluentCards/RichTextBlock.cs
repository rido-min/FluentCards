using System.Text.Json.Serialization;
using FluentCards.Serialization;

namespace FluentCards;

/// <summary>
/// Displays text with inline formatting via TextRun elements.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.2.</remarks>
public class RichTextBlock: AdaptiveElement
{
    /// <summary>
    /// The inline text elements (TextRun objects or plain strings).
    /// </summary>
    [JsonPropertyName("inlines")]
    [JsonConverter(typeof(InlinesConverter))]
    public List<object>? Inlines { get; set; }

    /// <summary>
    /// The horizontal alignment of the text.
    /// </summary>
    [JsonPropertyName("horizontalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }
}
