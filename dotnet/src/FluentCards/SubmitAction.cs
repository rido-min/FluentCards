using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// When invoked, gathers input fields and merges with optional data field, then sends an event to the client.
/// </summary>
public class SubmitAction : AdaptiveAction
{
    /// <summary>
    /// Initial data to merge with input values.
    /// </summary>
    [JsonConverter(typeof(ActionDataConverter))]
    public JsonElement? Data { get; set; }
    
    /// <summary>
    /// Controls which inputs are associated with the action.
    /// </summary>
    public AssociatedInputs? AssociatedInputs { get; set; }

    /// <summary>
    /// Microsoft Teams–specific submit action properties (feedback control, etc.).
    /// </summary>
    [JsonPropertyName("msteams")]
    public TeamsSubmitActionProperties? Msteams { get; set; }
}
