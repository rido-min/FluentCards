using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards.Serialization;

/// <summary>
/// Custom JSON converter for the data property in SubmitAction/ExecuteAction that can be any JSON object.
/// </summary>
public class ActionDataConverter : JsonConverter<JsonElement?>
{
    /// <summary>
    /// Reads and converts JSON to a JsonElement.
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Serialization options.</param>
    /// <returns>A nullable JsonElement containing the parsed value.</returns>
    public override JsonElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;
            
        return JsonElement.ParseValue(ref reader);
    }
    
    /// <summary>
    /// Writes a JsonElement to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">Serialization options.</param>
    public override void Write(Utf8JsonWriter writer, JsonElement? value, JsonSerializerOptions options)
    {
        if (value == null || value.Value.ValueKind == JsonValueKind.Undefined)
        {
            writer.WriteNullValue();
            return;
        }
        
        value.Value.WriteTo(writer);
    }
}
