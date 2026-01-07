using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Allows users to select from a set of choices.
/// </summary>
public class InputChoiceSet : AdaptiveElement
{
    /// <summary>
    /// The label to display for this input.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// The choices to display.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<Choice>? Choices { get; set; }

    /// <summary>
    /// The style for displaying choices.
/// Dropdown, radio button, or checkbox list.
/// </summary>
public class InputChoiceSet : InputElement
{
    /// <summary>
    /// Available choices.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = new();

    /// <summary>
    /// Allow multiple selections.
    /// </summary>
    [JsonPropertyName("isMultiSelect")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsMultiSelect { get; set; }

    /// <summary>
    /// Style of the choice set (compact, expanded, filtered).
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ChoiceInputStyle>))]
    public ChoiceInputStyle? Style { get; set; }

    /// <summary>
    /// If true, allow multiple choices to be selected.
    /// </summary>
    [JsonPropertyName("isMultiSelect")]
    public bool? IsMultiSelect { get; set; }

    /// <summary>
    /// The default selected value(s).
    /// Initial selected value(s), comma-separated for multi-select.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Placeholder text to display when nothing is selected.
    /// Placeholder text (compact style only).
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

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
    /// If true, wrap choice text.
    /// </summary>
    [JsonPropertyName("wrap")]
    public bool? Wrap { get; set; }
    /// Whether to wrap choice text.
    /// </summary>
    [JsonPropertyName("wrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Wrap { get; set; }
}
