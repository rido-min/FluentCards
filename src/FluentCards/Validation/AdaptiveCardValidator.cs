namespace FluentCards;

/// <summary>
/// Provides validation for Adaptive Cards to ensure they meet schema requirements.
/// </summary>
public static class AdaptiveCardValidator
{
    /// <summary>
    /// Validates an Adaptive Card and returns all issues found.
    /// </summary>
    /// <param name="card">The card to validate.</param>
    /// <returns>A list of validation issues. An empty list indicates the card is valid.</returns>
    public static IReadOnlyList<ValidationIssue> Validate(AdaptiveCard card)
    {
        var issues = new List<ValidationIssue>();
        
        ValidateCard(card, issues);
        
        return issues;
    }
    
    /// <summary>
    /// Validates an Adaptive Card and throws if any errors are found.
    /// </summary>
    /// <param name="card">The card to validate.</param>
    /// <exception cref="AdaptiveCardValidationException">Thrown when validation errors are found.</exception>
    public static void ValidateAndThrow(AdaptiveCard card)
    {
        var issues = Validate(card);
        var errors = issues.Where(i => i.Severity == ValidationSeverity.Error).ToList();
        
        if (errors.Count > 0)
        {
            throw new AdaptiveCardValidationException(errors);
        }
    }
    
    private static void ValidateCard(AdaptiveCard card, List<ValidationIssue> issues)
    {
        // Schema validation
        if (string.IsNullOrEmpty(card.Schema))
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = "$schema",
                Code = "MISSING_SCHEMA",
                Message = "The '$schema' property is missing. While optional, including it enables better tooling support."
            });
        }
        
        // Type validation
        if (card.Type != "AdaptiveCard")
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Error,
                Path = "type",
                Code = "INVALID_TYPE",
                Message = $"The 'type' property must be 'AdaptiveCard', but found '{card.Type}'."
            });
        }
        
        // Version validation
        if (string.IsNullOrEmpty(card.Version))
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Error,
                Path = "version",
                Code = "MISSING_VERSION",
                Message = "The 'version' property is required. Use a value like '1.5' to specify the schema version."
            });
        }
        else if (!IsValidVersion(card.Version))
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = "version",
                Code = "UNKNOWN_VERSION",
                Message = $"The version '{card.Version}' is not a known Adaptive Cards version. Known versions: 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6."
            });
        }
        
        // Body validation
        if (card.Body != null)
        {
            ValidateElements(card.Body, issues, "body");
        }
        else if (card.Actions == null || card.Actions.Count == 0)
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = "",
                Code = "EMPTY_CARD",
                Message = "The card has no body elements and no actions. It will render as empty."
            });
        }
        
        // Actions validation
        if (card.Actions != null)
        {
            ValidateActions(card.Actions, issues, "actions");
            
            if (card.Actions.Count > 5)
            {
                issues.Add(new ValidationIssue
                {
                    Severity = ValidationSeverity.Warning,
                    Path = "actions",
                    Code = "TOO_MANY_ACTIONS",
                    Message = $"The card has {card.Actions.Count} actions. Some hosts limit the number of visible actions to 5."
                });
            }
        }
    }
    
    private static bool IsValidVersion(string version) =>
        version is "1.0" or "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6";
    
    private static void ValidateElements(IEnumerable<AdaptiveElement> elements, List<ValidationIssue> issues, string path)
    {
        int index = 0;
        foreach (var element in elements)
        {
            var elementPath = $"{path}[{index}]";
            ValidateElement(element, issues, elementPath);
            index++;
        }
    }
    
    private static void ValidateElement(AdaptiveElement element, List<ValidationIssue> issues, string path)
    {
        switch (element)
        {
            case TextBlock textBlock:
                if (string.IsNullOrEmpty(textBlock.Text))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Warning,
                        Path = $"{path}.text",
                        Code = "EMPTY_TEXT",
                        Message = "TextBlock has empty or null text. It will render as invisible."
                    });
                }
                break;
                
            case Image image:
                if (string.IsNullOrEmpty(image.Url))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.url",
                        Code = "MISSING_IMAGE_URL",
                        Message = "Image element is missing the required 'url' property."
                    });
                }
                else if (!Uri.TryCreate(image.Url, UriKind.Absolute, out _))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Warning,
                        Path = $"{path}.url",
                        Code = "INVALID_IMAGE_URL",
                        Message = $"Image URL '{image.Url}' is not a valid absolute URL."
                    });
                }
                break;
                
            case InputElement input:
                if (string.IsNullOrEmpty(input.Id))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.id",
                        Code = "MISSING_INPUT_ID",
                        Message = "Input element is missing the required 'id' property. Inputs cannot be submitted without an id."
                    });
                }
                break;
                
            case Container container:
                if (container.Items != null)
                {
                    ValidateElements(container.Items, issues, $"{path}.items");
                }
                break;
                
            case ColumnSet columnSet:
                if (columnSet.Columns != null)
                {
                    for (int i = 0; i < columnSet.Columns.Count; i++)
                    {
                        var column = columnSet.Columns[i];
                        if (column.Items != null)
                        {
                            ValidateElements(column.Items, issues, $"{path}.columns[{i}].items");
                        }
                    }
                }
                break;
        }
    }
    
    private static void ValidateActions(IEnumerable<AdaptiveAction> actions, List<ValidationIssue> issues, string path)
    {
        int index = 0;
        foreach (var action in actions)
        {
            var actionPath = $"{path}[{index}]";
            
            switch (action)
            {
                case OpenUrlAction openUrl:
                    if (string.IsNullOrEmpty(openUrl.Url))
                    {
                        issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            Path = $"{actionPath}.url",
                            Code = "MISSING_ACTION_URL",
                            Message = "Action.OpenUrl is missing the required 'url' property."
                        });
                    }
                    else if (!Uri.TryCreate(openUrl.Url, UriKind.Absolute, out _))
                    {
                        issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Warning,
                            Path = $"{actionPath}.url",
                            Code = "INVALID_ACTION_URL",
                            Message = $"Action.OpenUrl URL '{openUrl.Url}' is not a valid absolute URL."
                        });
                    }
                    break;
                    
                case ShowCardAction showCard:
                    if (showCard.Card == null)
                    {
                        issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            Path = $"{actionPath}.card",
                            Code = "MISSING_SHOWCARD",
                            Message = "Action.ShowCard is missing the required 'card' property."
                        });
                    }
                    else
                    {
                        ValidateCard(showCard.Card, issues);
                    }
                    break;
            }
            
            index++;
        }
    }
}
