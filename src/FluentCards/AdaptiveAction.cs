using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Base class for all actions in an Adaptive Card.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OpenUrlAction), "Action.OpenUrl")]
public abstract class AdaptiveAction
{
    /// <summary>
    /// A unique identifier associated with the action.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Label for button or link that represents this action.
    /// </summary>
    public string? Title { get; set; }
}
