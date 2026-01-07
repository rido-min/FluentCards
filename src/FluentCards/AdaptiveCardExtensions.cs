using System.Text.Json;

namespace FluentCards;

/// <summary>
/// Extension methods for AdaptiveCard serialization and deserialization.
/// </summary>
public static class AdaptiveCardExtensions
{
    /// <summary>
    /// Serializes an AdaptiveCard to JSON string.
    /// </summary>
    /// <param name="card">The AdaptiveCard to serialize.</param>
    /// <returns>JSON string representation of the card.</returns>
    public static string ToJson(this AdaptiveCard card)
    {
        return JsonSerializer.Serialize(card, FluentCardsJsonContext.Default.AdaptiveCard);
    }

    /// <summary>
    /// Deserializes a JSON string to an AdaptiveCard.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized AdaptiveCard, or null if deserialization fails.</returns>
    public static AdaptiveCard? FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, FluentCardsJsonContext.Default.AdaptiveCard);
    }
}
