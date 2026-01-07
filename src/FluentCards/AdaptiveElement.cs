using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Base class for all Adaptive Card elements.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TextBlock), "TextBlock")]
public abstract class AdaptiveElement
{
    /// <summary>
    /// A unique identifier associated with the element.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// If false, the element will be hidden.
    /// </summary>
    public bool? IsVisible { get; set; }

    /// <summary>
    /// Controls the amount of spacing between this element and the preceding element.
    /// </summary>
    public string? Spacing { get; set; }

    /// <summary>
    /// When true, draw a separating line at the top of the element.
    /// </summary>
    public bool? Separator { get; set; }
}
