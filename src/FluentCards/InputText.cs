using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Allows users to enter text.
/// </summary>
public class InputText : AdaptiveElement
{
    /// <summary>
    /// The label to display for this input.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Placeholder text to display when the input is empty.
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The default value for the input.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// The maximum number of characters allowed.
    /// </summary>
    [JsonPropertyName("maxLength")]
    public int? MaxLength { get; set; }

    /// <summary>
    /// If true, allow multiple lines of text.
    /// </summary>
    [JsonPropertyName("isMultiline")]
    public bool? IsMultiline { get; set; }

    /// <summary>
    /// The style of the text input.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<TextInputStyle>))]
    public TextInputStyle? Style { get; set; }

    /// <summary>
    /// Regular expression for input validation.
    /// </summary>
    [JsonPropertyName("regex")]
    public string? Regex { get; set; }

    /// <summary>
    /// If true, this input is required.
    /// </summary>
    [JsonPropertyName("isRequired")]
    public bool? IsRequired { get; set; }

    /// <summary>
    /// Error message to display when validation fails.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// An inline action that can be invoked from the input.
    /// </summary>
    [JsonPropertyName("inlineAction")]
    public AdaptiveAction? InlineAction { get; set; }
}
