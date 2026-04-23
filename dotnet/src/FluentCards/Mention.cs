using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// An @mention entity in a Teams Adaptive Card.
/// </summary>
public class Mention
{
    /// <summary>
    /// Must be "mention".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mention";

    /// <summary>
    /// The display text for this mention (e.g. "John Doe").
    /// This text should appear as <c>&lt;at&gt;John Doe&lt;/at&gt;</c> in card body.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// The mentioned entity (user ID and display name).
    /// </summary>
    [JsonPropertyName("mentioned")]
    public MentionedEntity Mentioned { get; set; } = new();
}
