using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// JSON converter for the fallback property which can be either "drop" or an AdaptiveElement.
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
public class FallbackConverter : JsonConverter<object?>
{
    /// <summary>
    /// Reads a fallback value from JSON.
    /// </summary>
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString(); // "drop"
        }
        else if (reader.TokenType == JsonTokenType.StartObject)
        {
            return JsonSerializer.Deserialize<AdaptiveElement>(ref reader, FluentCardsJsonContext.Default.AdaptiveElement);
        }
        return null;
    }

    /// <summary>
    /// Writes a fallback value to JSON.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        if (value is string str)
        {
            writer.WriteStringValue(str);
        }
        else if (value is AdaptiveElement element)
        {
            JsonSerializer.Serialize(writer, element, FluentCardsJsonContext.Default.AdaptiveElement);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
