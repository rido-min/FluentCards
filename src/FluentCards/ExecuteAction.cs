using System.Text.Json;
using System.Text.Json.Serialization;
using FluentCards.Serialization;

namespace FluentCards;

/// <summary>
/// When invoked, executes an action with a verb and optional data.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.4.</remarks>
public class ExecuteAction: AdaptiveAction
{
    /// <summary>
    /// A string that describes the semantic verb for the action.
    /// </summary>
    public string? Verb { get; set; }
    
    /// <summary>
    /// Data to send with the action.
    /// </summary>
    [JsonConverter(typeof(ActionDataConverter))]
    public JsonElement? Data { get; set; }
    
    /// <summary>
    /// Controls which inputs are associated with the action.
    /// </summary>
    public AssociatedInputs? AssociatedInputs { get; set; }
}
