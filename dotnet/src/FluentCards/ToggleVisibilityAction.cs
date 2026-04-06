using System.Text.Json.Serialization;
using FluentCards.Serialization;

namespace FluentCards;

/// <summary>
/// When invoked, toggles the visibility of one or more elements.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.2.</remarks>
public class ToggleVisibilityAction: AdaptiveAction
{
    /// <summary>
    /// The list of elements to toggle visibility. Can be a string (element ID) or a TargetElement object.
    /// </summary>
    [JsonConverter(typeof(TargetElementListConverter))]
    public List<object>? TargetElements { get; set; }
}
