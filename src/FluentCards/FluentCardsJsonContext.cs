using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON serialization context for FluentCards with source generation support.
/// </summary>
[JsonSerializable(typeof(AdaptiveCard))]
[JsonSerializable(typeof(TextBlock))]
[JsonSerializable(typeof(OpenUrlAction))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
public partial class FluentCardsJsonContext : JsonSerializerContext
{
}
