using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON serialization context for FluentCards with source generation support.
/// </summary>
[JsonSerializable(typeof(AdaptiveCard))]
[JsonSerializable(typeof(TextBlock))]
[JsonSerializable(typeof(Image))]
[JsonSerializable(typeof(Container))]
[JsonSerializable(typeof(Column))]
[JsonSerializable(typeof(ColumnSet))]
[JsonSerializable(typeof(BackgroundImage))]
[JsonSerializable(typeof(OpenUrlAction))]
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = true)]
public partial class FluentCardsJsonContext : JsonSerializerContext
{
}
