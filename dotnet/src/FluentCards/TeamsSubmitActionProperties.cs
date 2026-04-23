using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Teams-specific action-level properties for Submit actions,
/// serialized as the <c>msteams</c> object on the action itself.
/// </summary>
public class TeamsSubmitActionProperties
{
    /// <summary>
    /// Feedback display settings for the submit action.
    /// </summary>
    [JsonPropertyName("feedback")]
    public TeamsSubmitActionFeedback? Feedback { get; set; }

    /// <summary>
    /// Captures any additional Teams properties not modeled by this class,
    /// ensuring unknown keys round-trip through serialization.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}
