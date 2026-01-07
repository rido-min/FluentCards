using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays text, allowing control over font sizes, weight, and color.
/// </summary>
public class TextBlock : AdaptiveElement
{
    /// <summary>
    /// The text to display.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Controls the size of the text.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<TextSize>))]
    public TextSize? Size { get; set; }

    /// <summary>
    /// Controls the weight of the text.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<TextWeight>))]
    public TextWeight? Weight { get; set; }

    /// <summary>
    /// Controls the color of the text.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<TextColor>))]
    public TextColor? Color { get; set; }

    /// <summary>
    /// If true, allow text to wrap. Otherwise, text is clipped.
    /// </summary>
    public bool? Wrap { get; set; }

    /// <summary>
    /// Maximum number of lines to display.
    /// </summary>
    public int? MaxLines { get; set; }

    /// <summary>
    /// Controls the horizontal alignment of the text.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }
}
