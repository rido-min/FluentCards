using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards.Serialization;

/// <summary>
/// Custom JSON converter for ToggleVisibilityAction.TargetElements that can contain strings or TargetElement objects.
/// </summary>
public class TargetElementListConverter : JsonConverter<List<object>?>
{
    public override List<object>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;
            
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array for targetElements");
            
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
                var element = JsonSerializer.Deserialize<TargetElement>(ref reader, options);
                if (element != null)
                    result.Add(element);
            }
        }
        return result;
    }
    
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
            else if (item is TargetElement element)
            {
                JsonSerializer.Serialize(writer, element, options);
            }
        }
        writer.WriteEndArray();
    }
}
