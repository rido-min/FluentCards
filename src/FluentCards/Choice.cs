using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a choice option in an InputChoiceSet.
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
}
