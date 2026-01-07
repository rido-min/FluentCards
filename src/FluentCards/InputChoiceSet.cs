using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
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
    /// Initial selected value(s), comma-separated for multi-select.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Placeholder text (compact style only).
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Whether to wrap choice text.
    /// </summary>
    [JsonPropertyName("wrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Wrap { get; set; }
}
