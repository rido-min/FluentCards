using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Controls which inputs are associated with a given submit or execute action.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.3.</remarks>
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
