using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines a button in an authentication card.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.4.</remarks>
public class AuthCardButton
{
    /// <summary>
    /// The type of the button (default: "signIn").
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "signIn";

    /// <summary>
    /// The title/text to display on the button.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The URL of an image to display on the button.
    /// </summary>
    [JsonPropertyName("image")]
    public string? Image { get; set; }

    /// <summary>
    /// The value associated with the button.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
