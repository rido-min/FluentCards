using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards.Serialization;

/// <summary>
/// Custom JSON converter for RichTextBlock.Inlines that can contain strings or TextRun objects.
/// </summary>
public class InlinesConverter : JsonConverter<List<object>?>
{
    /// <summary>
    /// Reads and converts JSON to a list of inline elements (strings or TextRun objects).
    /// </summary>
    /// <param name="reader">The JSON reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">Serialization options.</param>
    /// <returns>A list of objects containing strings or TextRun instances.</returns>
    public override List<object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;
            
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array for inlines");
            
        var result = new List<object>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
                
            if (reader.TokenType == JsonTokenType.String)
            {
                result.Add(reader.GetString()!);
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                var textRun = JsonSerializer.Deserialize<TextRun>(ref reader, options);
                if (textRun != null)
                    result.Add(textRun);
            }
        }
        return result;
    }
    
    /// <summary>
    /// Writes a list of inline elements to JSON.
    /// </summary>
    /// <param name="writer">The JSON writer.</param>
    /// <param name="value">The list of inline elements to write.</param>
    /// <param name="options">Serialization options.</param>
    public override void Write(Utf8JsonWriter writer, List<object>? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }
        
        writer.WriteStartArray();
        foreach (var item in value)
        {
            if (item is string str)
            {
                writer.WriteStringValue(str);
            }
            else if (item is TextRun textRun)
            {
                JsonSerializer.Serialize(writer, textRun, options);
            }
        }
        writer.WriteEndArray();
    }
}
