using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Base class for all actions in an Adaptive Card.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OpenUrlAction), "Action.OpenUrl")]
[JsonDerivedType(typeof(SubmitAction), "Action.Submit")]
[JsonDerivedType(typeof(ShowCardAction), "Action.ShowCard")]
[JsonDerivedType(typeof(ToggleVisibilityAction), "Action.ToggleVisibility")]
[JsonDerivedType(typeof(ExecuteAction), "Action.Execute")]
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
    
    /// <summary>
    /// Optional icon to display on the action button.
    /// </summary>
    public string? IconUrl { get; set; }
    
    /// <summary>
    /// Controls the style of the action button.
    /// </summary>
    public ActionStyle? Style { get; set; }
    
    /// <summary>
    /// Determines whether the action should be enabled.
    /// </summary>
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    /// Defines text that should be displayed as a tooltip.
    /// </summary>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Describes what to do when an unknown action is encountered or the requires of this can't be met.
    /// </summary>
    [JsonConverter(typeof(FallbackConverter))]
    public object? Fallback { get; set; }

    /// <summary>
    /// Determines whether the action should be displayed in the button area or in the overflow menu.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ActionMode>))]
    public ActionMode? Mode { get; set; }
}
