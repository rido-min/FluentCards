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
    /// Email input.
    /// </summary>
    Email,

    /// <summary>
    /// Password input (masked).
    /// </summary>
    Password
}
