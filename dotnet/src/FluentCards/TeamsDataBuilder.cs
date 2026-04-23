using System.Buffers;
using System.Text.Json;

namespace FluentCards;

/// <summary>
/// Fluent builder for Teams-specific action data payloads.
/// Produces a <see cref="JsonElement"/> containing an optional <c>msteams</c> object
/// and arbitrary custom properties.
/// </summary>
public class TeamsDataBuilder
{
    private JsonElement? _msteams;
    private readonly List<(string Key, JsonElement Value)> _properties = new();

    /// <summary>
    /// Sets the <c>msteams</c> object to <c>{ "type": "task/fetch" }</c>.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsDataBuilder WithTaskFetch()
    {
        using var doc = JsonDocument.Parse("""{"type":"task/fetch"}""");
        _msteams = doc.RootElement.Clone();
        return this;
    }

    /// <summary>
    /// Sets the <c>msteams</c> object from a raw JSON string.
    /// </summary>
    /// <param name="rawJson">A JSON object string for the msteams value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the JSON is not an object.</exception>
    public TeamsDataBuilder WithMsteams(string rawJson)
    {
        using var doc = JsonDocument.Parse(rawJson);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The msteams value must be a JSON object.", nameof(rawJson));
        }
        _msteams = doc.RootElement.Clone();
        return this;
    }

    /// <summary>
    /// Sets the <c>msteams</c> object from a <see cref="JsonElement"/>.
    /// </summary>
    /// <param name="json">A JSON object element for the msteams value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="ArgumentException">Thrown when the element is not an object.</exception>
    public TeamsDataBuilder WithMsteams(JsonElement json)
    {
        if (json.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The msteams value must be a JSON object.", nameof(json));
        }
        _msteams = json.Clone();
        return this;
    }

    /// <summary>
    /// Adds a custom string property to the data payload.
    /// </summary>
    /// <param name="key">The property name. Cannot be "msteams".</param>
    /// <param name="value">The string value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsDataBuilder WithProperty(string key, string value)
    {
        ValidateKey(key);
        _properties.Add((key, ToJsonElement(w => w.WriteStringValue(value))));
        return this;
    }

    /// <summary>
    /// Adds a custom integer property to the data payload.
    /// </summary>
    /// <param name="key">The property name. Cannot be "msteams".</param>
    /// <param name="value">The integer value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsDataBuilder WithProperty(string key, int value)
    {
        ValidateKey(key);
        _properties.Add((key, ToJsonElement(w => w.WriteNumberValue(value))));
        return this;
    }

    /// <summary>
    /// Adds a custom boolean property to the data payload.
    /// </summary>
    /// <param name="key">The property name. Cannot be "msteams".</param>
    /// <param name="value">The boolean value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsDataBuilder WithProperty(string key, bool value)
    {
        ValidateKey(key);
        _properties.Add((key, ToJsonElement(w => w.WriteBooleanValue(value))));
        return this;
    }

    /// <summary>
    /// Adds a custom property with a <see cref="JsonElement"/> value to the data payload.
    /// </summary>
    /// <param name="key">The property name. Cannot be "msteams".</param>
    /// <param name="value">The JSON element value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsDataBuilder WithProperty(string key, JsonElement value)
    {
        ValidateKey(key);
        _properties.Add((key, value.Clone()));
        return this;
    }

    /// <summary>
    /// Builds and returns the data payload as a <see cref="JsonElement"/>.
    /// </summary>
    /// <returns>A JSON object element containing the msteams and custom properties.</returns>
    public JsonElement Build()
    {
        var buffer = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(buffer))
        {
            writer.WriteStartObject();

            if (_msteams.HasValue)
            {
                writer.WritePropertyName("msteams");
                _msteams.Value.WriteTo(writer);
            }

            foreach (var (key, value) in _properties)
            {
                writer.WritePropertyName(key);
                value.WriteTo(writer);
            }

            writer.WriteEndObject();
        }

        using var doc = JsonDocument.Parse(buffer.WrittenMemory);
        return doc.RootElement.Clone();
    }

    private static void ValidateKey(string key)
    {
        if (string.Equals(key, "msteams", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Cannot use 'msteams' as a property key. Use WithMsteams() or WithTaskFetch() instead.",
                nameof(key));
        }
    }

    private static JsonElement ToJsonElement(Action<Utf8JsonWriter> writeValue)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(buffer))
        {
            writeValue(writer);
        }
        using var doc = JsonDocument.Parse(buffer.WrittenMemory);
        return doc.RootElement.Clone();
    }
}
