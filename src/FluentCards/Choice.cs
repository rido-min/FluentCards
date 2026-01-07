using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a choice in a choice set.
/// </summary>
public class Choice
{
    /// <summary>
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
