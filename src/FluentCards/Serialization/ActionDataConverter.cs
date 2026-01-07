using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentCards.Serialization;

/// <summary>
/// Custom JSON converter for the data property in SubmitAction/ExecuteAction that can be any JSON object.
/// </summary>
public class ActionDataConverter : JsonConverter<JsonElement?>
{
    public override JsonElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;
            
        return JsonElement.ParseValue(ref reader);
    }
    
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
