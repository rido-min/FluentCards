using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Single or multi-line text input field.
/// </summary>
public class InputText : InputElement
{
    /// <summary>
    /// Display as multi-line text box.
    /// </summary>
    [JsonPropertyName("isMultiline")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsMultiline { get; set; }

    /// <summary>
    /// Maximum number of characters.
    /// </summary>
    [JsonPropertyName("maxLength")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLength { get; set; }

    /// <summary>
    /// Placeholder text.
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Initial value.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Style of the text input.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<TextInputStyle>))]
    public TextInputStyle? Style { get; set; }

    /// <summary>
    /// Regex pattern for validation.
    /// </summary>
    [JsonPropertyName("regex")]
    public string? Regex { get; set; }

    /// <summary>
    /// Action displayed inline with input.
    /// </summary>
    [JsonPropertyName("inlineAction")]
    public AdaptiveAction? InlineAction { get; set; }
}
