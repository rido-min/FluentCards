using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents an inline text run with formatting options.
/// </summary>
public class TextRun
{
    /// <summary>
    /// The type discriminator for TextRun.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "TextRun";

    /// <summary>
    /// The text content.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// The size of the text.
    /// </summary>
    [JsonPropertyName("size")]
    [JsonConverter(typeof(CamelCaseEnumConverter<TextSize>))]
    public TextSize? Size { get; set; }

    /// <summary>
    /// The weight (boldness) of the text.
    /// </summary>
    [JsonPropertyName("weight")]
    [JsonConverter(typeof(CamelCaseEnumConverter<TextWeight>))]
    public TextWeight? Weight { get; set; }

    /// <summary>
    /// The color of the text.
    /// </summary>
    [JsonPropertyName("color")]
    [JsonConverter(typeof(CamelCaseEnumConverter<TextColor>))]
    public TextColor? Color { get; set; }

    /// <summary>
    /// The type of font to use for rendering.
    /// </summary>
    [JsonPropertyName("fontType")]
    [JsonConverter(typeof(CamelCaseEnumConverter<FontType>))]
    public FontType? FontType { get; set; }

    /// <summary>
    /// If true, displays the text with subtle styling.
    /// </summary>
    [JsonPropertyName("isSubtle")]
    public bool? IsSubtle { get; set; }

    /// <summary>
    /// If true, displays the text in italic.
    /// </summary>
    [JsonPropertyName("italic")]
    public bool? Italic { get; set; }

    /// <summary>
    /// If true, displays the text with strikethrough.
    /// </summary>
    [JsonPropertyName("strikethrough")]
    public bool? Strikethrough { get; set; }

    /// <summary>
    /// If true, displays the text with underline.
    /// </summary>
    [JsonPropertyName("underline")]
    public bool? Underline { get; set; }

    /// <summary>
    /// If true, displays the text with highlight.
    /// </summary>
    [JsonPropertyName("highlight")]
    public bool? Highlight { get; set; }

    /// <summary>
    /// Action to invoke when the text is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }
}
