using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Controls the style of an action button.
/// </summary>
[JsonConverter(typeof(CamelCaseEnumConverter<ActionStyle>))]
public enum ActionStyle
{
    /// <summary>
    /// Default style.
    /// </summary>
    Default,
    
    /// <summary>
    /// Positive action style (e.g., for confirmation or success actions).
    /// </summary>
    Positive,
    
    /// <summary>
    /// Destructive action style (e.g., for delete or cancel actions).
    /// </summary>
    Destructive
}
