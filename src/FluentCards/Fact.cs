using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a key-value pair (fact) to display.
/// </summary>
public class Fact
{
    /// <summary>
    /// The title/label of the fact.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The value of the fact.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
