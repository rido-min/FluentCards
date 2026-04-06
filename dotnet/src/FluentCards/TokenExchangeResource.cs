using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines a token exchange resource for authentication.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.4.</remarks>
public class TokenExchangeResource
{
    /// <summary>
    /// The identifier of the token exchange resource.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// The URI of the token exchange resource.
    /// </summary>
    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    /// <summary>
    /// The provider ID for the token exchange.
    /// </summary>
    [JsonPropertyName("providerId")]
    public string? ProviderId { get; set; }
}
