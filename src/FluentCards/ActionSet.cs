using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays a set of actions inline in the card body.
/// </summary>
public class ActionSet : AdaptiveElement
{
    /// <summary>
    /// The actions to display.
    /// </summary>
    [JsonPropertyName("actions")]
    public List<AdaptiveAction>? Actions { get; set; }
}
