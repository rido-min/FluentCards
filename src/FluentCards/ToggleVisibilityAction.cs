using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// When invoked, toggles the visibility of one or more elements.
/// </summary>
public class ToggleVisibilityAction : AdaptiveAction
{
    /// <summary>
    /// The list of elements to toggle visibility. Can be a string (element ID) or a TargetElement object.
    /// </summary>
    public List<object>? TargetElements { get; set; }
}
