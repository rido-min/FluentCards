using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FluentCards.Serialization;

/// <summary>
/// Static helper class for AOT-compatible serialization of AdaptiveCard instances.
/// </summary>
public static class AdaptiveCardSerializer
{
    /// <summary>
    /// Serializes an AdaptiveCard to JSON string using source-generated context.
    /// </summary>
    /// <param name="card">The AdaptiveCard to serialize.</param>
    /// <param name="indented">If true, the output JSON will be formatted with indentation.</param>
    /// <returns>JSON string representation of the card.</returns>
    public static string Serialize(AdaptiveCard card, bool indented = false)
    {
        if (indented)
        {
            return JsonSerializer.Serialize(card, FluentCardsJsonContext.Default.AdaptiveCard);
        }

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            TypeInfoResolver = FluentCardsJsonContext.Default
        };

        return JsonSerializer.Serialize(card, options.GetTypeInfo(typeof(AdaptiveCard)) as JsonTypeInfo<AdaptiveCard>
            ?? throw new InvalidOperationException("Failed to resolve AdaptiveCard type info."));
    }
    
    /// <summary>
    /// Deserializes JSON string to AdaptiveCard using source-generated context.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized AdaptiveCard, or null if deserialization fails.</returns>
    public static AdaptiveCard? Deserialize(string json)
    {
        return JsonSerializer.Deserialize(json, FluentCardsJsonContext.Default.AdaptiveCard);
    }
    
    /// <summary>
    /// Tries to deserialize JSON, returning false on failure instead of throwing.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="card">The deserialized card if successful, null otherwise.</param>
    /// <param name="errorMessage">Error message if deserialization fails, null otherwise.</param>
    /// <returns>True if deserialization succeeded, false otherwise.</returns>
    public static bool TryDeserialize(string json, out AdaptiveCard? card, out string? errorMessage)
    {
        try
        {
            card = Deserialize(json);
            errorMessage = null;
            return card != null;
        }
        catch (JsonException ex)
        {
            card = null;
            errorMessage = ex.Message;
            return false;
        }
    }
    
    /// <summary>
    /// Serializes to UTF-8 bytes for optimal performance.
    /// </summary>
    /// <param name="card">The AdaptiveCard to serialize.</param>
    /// <returns>UTF-8 encoded JSON bytes.</returns>
    public static byte[] SerializeToUtf8Bytes(AdaptiveCard card)
    {
        return JsonSerializer.SerializeToUtf8Bytes(card, FluentCardsJsonContext.Default.AdaptiveCard);
    }
    
    /// <summary>
    /// Deserializes from UTF-8 bytes for optimal performance.
    /// </summary>
    /// <param name="utf8Json">UTF-8 encoded JSON bytes.</param>
    /// <returns>The deserialized AdaptiveCard, or null if deserialization fails.</returns>
    public static AdaptiveCard? DeserializeFromUtf8Bytes(ReadOnlySpan<byte> utf8Json)
    {
        return JsonSerializer.Deserialize(utf8Json, FluentCardsJsonContext.Default.AdaptiveCard);
    }
    
    /// <summary>
    /// Asynchronously serializes to a stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="card">The AdaptiveCard to serialize.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task SerializeAsync(Stream stream, AdaptiveCard card, CancellationToken cancellationToken = default)
    {
        await JsonSerializer.SerializeAsync(stream, card, FluentCardsJsonContext.Default.AdaptiveCard, cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously deserializes from a stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The deserialized AdaptiveCard, or null if deserialization fails.</returns>
    public static async Task<AdaptiveCard?> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        return await JsonSerializer.DeserializeAsync(stream, FluentCardsJsonContext.Default.AdaptiveCard, cancellationToken);
    }
}
