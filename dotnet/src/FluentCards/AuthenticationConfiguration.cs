using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines authentication configuration for Adaptive Cards.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.4.</remarks>
public class AuthenticationConfiguration
{
    /// <summary>
    /// Text to display for authentication prompt.
    /// </summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>
    /// The name of the connection to use for authentication.
    /// </summary>
    [JsonPropertyName("connectionName")]
    public string? ConnectionName { get; set; }

    /// <summary>
    /// The token exchange resource configuration.
    /// </summary>
    [JsonPropertyName("tokenExchangeResource")]
    public TokenExchangeResource? TokenExchangeResource { get; set; }

    /// <summary>
    /// The authentication buttons to display.
    /// </summary>
    [JsonPropertyName("buttons")]
    public List<AuthCardButton>? Buttons { get; set; }
}
