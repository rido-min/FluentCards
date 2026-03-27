using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;

namespace FluentCards.Tests.Schemas;

/// <summary>
/// Test helper that validates serialized AdaptiveCard JSON against official Adaptive Cards JSON schemas.
/// Supports versions 1.2 through 1.6. Schemas are cached per version to avoid re-parsing.
/// </summary>
public static class SchemaValidator
{
    private static readonly ConcurrentDictionary<AdaptiveCardVersion, JsonSchema> SchemaCache = new();

    private static JsonSchema GetSchema(AdaptiveCardVersion version)
    {
        return SchemaCache.GetOrAdd(version, static v =>
        {
            var versionString = v.ToVersionString();
            var resourceName = $"FluentCards.Tests.Schemas.adaptive-card-{versionString}.0.json";

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Could not find embedded schema resource '{resourceName}'. " +
                    $"Ensure the schema file exists and is registered as an EmbeddedResource.");
            using var reader = new StreamReader(stream);
            var schemaText = reader.ReadToEnd();

            // The Adaptive Cards schema uses "required": false on some properties (draft-03 convention).
            // JsonSchema.Net expects "required" to be an array (draft-04+). Strip non-array required values.
            var node = JsonNode.Parse(schemaText)!;
            RemoveNonArrayRequired(node);
            schemaText = node.ToJsonString();

            return JsonSchema.FromText(schemaText);
        });
    }

    private static void ValidateVersionSupported(AdaptiveCardVersion version)
    {
        if (version is AdaptiveCardVersion.V1_0 or AdaptiveCardVersion.V1_1)
        {
            throw new ArgumentException(
                $"Schema validation is not supported for version {version.ToVersionString()}. " +
                $"Only versions 1.2 through 1.6 have embedded schemas.",
                nameof(version));
        }
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
    public static EvaluationResults Evaluate(AdaptiveCard card) =>
        Evaluate(card, AdaptiveCardVersion.V1_6);

    /// <summary>
    /// Validates that a card's JSON output conforms to the specified Adaptive Cards schema version.
    /// Returns the evaluation results for detailed inspection.
    /// </summary>
    /// <param name="card">The card to validate.</param>
    /// <param name="version">The schema version to validate against (1.2–1.6).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="version"/> is V1_0 or V1_1.</exception>
    public static EvaluationResults Evaluate(AdaptiveCard card, AdaptiveCardVersion version)
    {
        ValidateVersionSupported(version);

        var schema = GetSchema(version);
        var json = card.ToJson();
        var document = JsonDocument.Parse(json);
        var options = new EvaluationOptions
        {
            OutputFormat = OutputFormat.List
        };
        return schema.Evaluate(document.RootElement, options);
    }

    /// <summary>
    /// Asserts that a card's JSON output conforms to the Adaptive Cards 1.6.0 schema.
    /// Throws on validation failure with details.
    /// </summary>
    public static void AssertValid(AdaptiveCard card) =>
        AssertValid(card, AdaptiveCardVersion.V1_6);

    /// <summary>
    /// Asserts that a card's JSON output conforms to the specified Adaptive Cards schema version.
    /// Throws on validation failure with details.
    /// </summary>
    /// <param name="card">The card to validate.</param>
    /// <param name="version">The schema version to validate against (1.2–1.6).</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="version"/> is V1_0 or V1_1.</exception>
    public static void AssertValid(AdaptiveCard card, AdaptiveCardVersion version)
    {
        ValidateVersionSupported(version);

        var results = Evaluate(card, version);
        if (!results.IsValid)
        {
            var errors = results.Details?
                .Where(d => !d.IsValid && d.Errors != null)
                .SelectMany(d => d.Errors!.Select(e => $"  [{d.InstanceLocation}] {e.Key}: {e.Value}"))
                .ToList() ?? [];

            var json = card.ToJson();
            var versionString = version.ToVersionString();
            var errorText = errors.Count > 0
                ? string.Join(Environment.NewLine, errors)
                : "Unknown schema validation error";

            throw new Xunit.Sdk.XunitException(
                $"Card JSON does not conform to Adaptive Cards {versionString} schema:{Environment.NewLine}{errorText}{Environment.NewLine}{Environment.NewLine}Card JSON:{Environment.NewLine}{json}");
        }
    }
}
