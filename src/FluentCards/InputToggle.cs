using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Allows users to toggle a boolean value on/off.
/// </summary>
public class InputToggle : AdaptiveElement
{
    /// <summary>
    /// The label to display for this input.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// The title to display next to the toggle.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The value to submit when the toggle is on.
/// Boolean toggle/checkbox input.
/// </summary>
public class InputToggle : InputElement
{
    /// <summary>
    /// Label displayed next to toggle.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Current value ("true" or "false").
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Value when toggled on (default "true").
    /// </summary>
    [JsonPropertyName("valueOn")]
    public string? ValueOn { get; set; }

    /// <summary>
    /// The value to submit when the toggle is off.
    /// Value when toggled off (default "false").
    /// </summary>
    [JsonPropertyName("valueOff")]
    public string? ValueOff { get; set; }

    /// <summary>
    /// The default value for the toggle.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

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
    /// If true, wrap the title text.
    /// </summary>
    [JsonPropertyName("wrap")]
    public bool? Wrap { get; set; }
    /// Whether to wrap the title.
    /// </summary>
    [JsonPropertyName("wrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Wrap { get; set; }
}
