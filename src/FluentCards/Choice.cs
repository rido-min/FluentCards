using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a choice option in an InputChoiceSet.
/// Represents a choice in a choice set.
/// </summary>
public class Choice
{
    /// <summary>
    /// The display text for the choice.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The internal value for the choice.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
    /// Text to display for the choice.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Internal value for the choice.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}
