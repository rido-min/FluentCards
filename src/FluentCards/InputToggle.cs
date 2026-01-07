using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
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
    /// Value when toggled off (default "false").
    /// </summary>
    [JsonPropertyName("valueOff")]
    public string? ValueOff { get; set; }

    /// <summary>
    /// Whether to wrap the title.
    /// </summary>
    [JsonPropertyName("wrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Wrap { get; set; }
}
