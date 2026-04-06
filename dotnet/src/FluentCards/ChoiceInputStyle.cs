using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines the style for rendering a choice set.
/// </summary>
[JsonConverter(typeof(CamelCaseEnumConverter<ChoiceInputStyle>))]
public enum ChoiceInputStyle
{
    /// <summary>
    /// Dropdown list.
    /// </summary>
    Compact,

    /// <summary>
    /// Radio buttons or checkboxes.
    /// </summary>
    Expanded,

    /// <summary>
    /// Searchable dropdown.
    /// </summary>
    /// <remarks>Added in Adaptive Cards 1.6.</remarks>
    Filtered
}
