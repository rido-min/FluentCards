using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a target element for Action.ToggleVisibility.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.2.</remarks>
public class TargetElement
{
    /// <summary>
    /// The ID of the element to toggle visibility.
    /// </summary>
    [JsonPropertyName("elementId")]
    public string ElementId { get; set; } = string.Empty;
    
    /// <summary>
    /// If true, the element will be shown. If false, it will be hidden. If null, visibility will be toggled.
    /// </summary>
    [JsonPropertyName("isVisible")]
    public bool? IsVisible { get; set; }
}
