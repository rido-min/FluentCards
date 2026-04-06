using System.Text.Json;
using System.Text.Json.Serialization;
using FluentCards.Serialization;

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
}
