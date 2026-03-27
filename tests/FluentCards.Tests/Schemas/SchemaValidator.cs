using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;

namespace FluentCards.Tests.Schemas;

/// <summary>
/// Test helper that validates serialized AdaptiveCard JSON against the official 1.6.0 JSON schema.
/// </summary>
public static class SchemaValidator
{
    private static readonly Lazy<JsonSchema> Schema = new(LoadSchema);

    private static JsonSchema LoadSchema()
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream("FluentCards.Tests.Schemas.adaptive-card-1.6.0.json")
            ?? throw new InvalidOperationException("Could not find embedded schema resource.");
        using var reader = new StreamReader(stream);
        var schemaText = reader.ReadToEnd();

        // The Adaptive Cards schema uses "required": false on some properties (draft-03 convention).
        // JsonSchema.Net expects "required" to be an array (draft-04+). Strip non-array required values.
        var node = JsonNode.Parse(schemaText)!;
        RemoveNonArrayRequired(node);
        schemaText = node.ToJsonString();

        return JsonSchema.FromText(schemaText);
    }

    private static void RemoveNonArrayRequired(JsonNode? node)
    {
        if (node is JsonObject obj)
        {
            var keysToRemove = new List<string>();
            foreach (var kvp in obj)
            {
                if (kvp.Key == "required" && kvp.Value is not JsonArray)
                {
                    keysToRemove.Add(kvp.Key);
                }
                else
                {
                    RemoveNonArrayRequired(kvp.Value);
                }
            }
            foreach (var key in keysToRemove)
            {
                obj.Remove(key);
            }
        }
        else if (node is JsonArray arr)
        {
            foreach (var item in arr)
            {
                RemoveNonArrayRequired(item);
            }
        }
    }

    /// <summary>
    /// Validates that a card's JSON output conforms to the Adaptive Cards 1.6.0 schema.
    /// Returns the evaluation results for detailed inspection.
    /// </summary>
    public static EvaluationResults Evaluate(AdaptiveCard card)
    {
        var json = card.ToJson();
        var document = JsonDocument.Parse(json);
        var options = new EvaluationOptions
        {
            OutputFormat = OutputFormat.List
        };
        return Schema.Value.Evaluate(document.RootElement, options);
    }

    /// <summary>
    /// Asserts that a card's JSON output conforms to the Adaptive Cards 1.6.0 schema.
    /// Throws on validation failure with details.
    /// </summary>
    public static void AssertValid(AdaptiveCard card)
    {
        var results = Evaluate(card);
        if (!results.IsValid)
        {
            var errors = results.Details?
                .Where(d => !d.IsValid && d.Errors != null)
                .SelectMany(d => d.Errors!.Select(e => $"  [{d.InstanceLocation}] {e.Key}: {e.Value}"))
                .ToList() ?? new List<string>();

            var json = card.ToJson();
            var errorText = errors.Count > 0
                ? string.Join(Environment.NewLine, errors)
                : "Unknown schema validation error";

            throw new Xunit.Sdk.XunitException(
                $"Card JSON does not conform to Adaptive Cards 1.6.0 schema:{Environment.NewLine}{errorText}{Environment.NewLine}{Environment.NewLine}Card JSON:{Environment.NewLine}{json}");
        }
    }
}
