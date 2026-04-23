using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// A mentioned entity (user) in a Teams Adaptive Card.
/// </summary>
public class MentionedEntity
{
    /// <summary>
    /// The Teams user ID (e.g. "29:1241241...").
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The display name (e.g. "John Doe").
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
