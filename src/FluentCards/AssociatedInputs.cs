using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Controls which inputs are associated with a given submit or execute action.
/// </summary>
[JsonConverter(typeof(CamelCaseEnumConverter<AssociatedInputs>))]
public enum AssociatedInputs
{
    /// <summary>
    /// Automatically gather input values.
    /// </summary>
    Auto,
    
    /// <summary>
    /// Do not gather any input values.
    /// </summary>
    None
}
