using System.Text.Json;

namespace FluentCards;

/// <summary>
/// Extension methods for AdaptiveCard serialization and deserialization.
/// </summary>
public static class AdaptiveCardExtensions
{
    /// <summary>
    /// Serializes an AdaptiveCard to JSON string.
    /// </summary>
    /// <param name="card">The AdaptiveCard to serialize.</param>
    /// <returns>JSON string representation of the card.</returns>
    public static string ToJson(this AdaptiveCard card)
    {
        return JsonSerializer.Serialize(card, FluentCardsJsonContext.Default.AdaptiveCard);
    }

    /// <summary>
    /// Deserializes a JSON string to an AdaptiveCard.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized AdaptiveCard, or null if deserialization fails.</returns>
    public static AdaptiveCard? FromJson(string json)
    {
        return JsonSerializer.Deserialize(json, FluentCardsJsonContext.Default.AdaptiveCard);
    }

    /// <summary>
    /// Validates the card structure and returns any issues found.
    /// </summary>
    /// <param name="card">The AdaptiveCard to validate.</param>
    /// <returns>A read-only list of validation issues. Empty list if the card is valid.</returns>
    public static IReadOnlyList<string> Validate(this AdaptiveCard card)
    {
        var issues = new List<string>();
        
        // Schema version validation
        if (string.IsNullOrEmpty(card.Schema))
        {
            issues.Add("Missing '$schema' property");
        }
        
        // Type validation
        if (card.Type != "AdaptiveCard")
        {
            issues.Add($"Invalid type '{card.Type}', expected 'AdaptiveCard'");
        }
        
        // Version validation
        if (string.IsNullOrEmpty(card.Version))
        {
            issues.Add("Missing 'version' property");
        }
        
        // Body validation
        if (card.Body != null)
        {
            ValidateElements(card.Body, issues, "body");
        }
        
        // Actions validation
        if (card.Actions != null)
        {
            ValidateActions(card.Actions, issues, "actions");
        }
        
        return issues;
    }
    
    private static void ValidateElements(IEnumerable<AdaptiveElement> elements, List<string> issues, string path)
    {
        int index = 0;
        foreach (var element in elements)
        {
            var elementPath = $"{path}[{index}]";
            
            // Check for required properties based on element type
            switch (element)
            {
                case InputElement input when string.IsNullOrEmpty(input.Id):
                    issues.Add($"{elementPath}: Input element missing required 'id' property");
                    break;
                case Image image when string.IsNullOrEmpty(image.Url):
                    issues.Add($"{elementPath}: Image element missing required 'url' property");
                    break;
            }
            
            // Recursively validate nested elements
            if (element is Container container && container.Items != null)
            {
                ValidateElements(container.Items, issues, $"{elementPath}.items");
            }
            
            index++;
        }
    }
    
    private static void ValidateActions(IEnumerable<AdaptiveAction> actions, List<string> issues, string path)
    {
        int index = 0;
        foreach (var action in actions)
        {
            var actionPath = $"{path}[{index}]";
            
            switch (action)
            {
                case OpenUrlAction openUrl when string.IsNullOrEmpty(openUrl.Url):
                    issues.Add($"{actionPath}: OpenUrl action missing required 'url' property");
                    break;
                case ShowCardAction showCard when showCard.Card == null:
                    issues.Add($"{actionPath}: ShowCard action missing required 'card' property");
                    break;
            }
            
            index++;
        }
    }
}
