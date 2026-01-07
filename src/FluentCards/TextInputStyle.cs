namespace FluentCards;

/// <summary>
/// Specifies the style for text input.
/// </summary>
public enum TextInputStyle
{
    /// <summary>
    /// Normal text input.
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines the style of text input.
/// </summary>
[JsonConverter(typeof(CamelCaseEnumConverter<TextInputStyle>))]
public enum TextInputStyle
{
    /// <summary>
    /// Plain text input.
    /// </summary>
    Text,

    /// <summary>
    /// Telephone number input.
    /// </summary>
    Tel,

    /// <summary>
    /// URL input.
    /// </summary>
    Url,

    /// <summary>
    /// Email address input.
    /// Email input.
    /// </summary>
    Email,

    /// <summary>
    /// Password input (hidden text).
    /// Password input (masked).
    /// </summary>
    Password
}
