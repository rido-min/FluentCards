namespace FluentCards;

/// <summary>
/// Specifies the style for choice set input.
/// </summary>
public enum ChoiceInputStyle
{
    /// <summary>
    /// Display choices in a compact dropdown.
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
    /// Display choices as an expanded list.
    /// Radio buttons or checkboxes.
    /// </summary>
    Expanded,

    /// <summary>
    /// Display choices with filter/search capability.
    /// Searchable dropdown (Adaptive Cards 1.5+).
    /// </summary>
    Filtered
}
