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
        var ids = new HashSet<string>(StringComparer.Ordinal);
        
        ValidateCard(card, issues, ids);
        
        // Version mismatch checking — only for recognized versions
        if (!string.IsNullOrEmpty(card.Version) &&
            AdaptiveCardVersionExtensions.TryParse(card.Version, out var cardVersion))
        {
            ValidateVersionMismatch(card, cardVersion, issues);
        }
        
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
    
    private static void ValidateCard(AdaptiveCard card, List<ValidationIssue> issues, HashSet<string> ids)
    {
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
        else if (!AdaptiveCardVersionExtensions.TryParse(card.Version, out _))
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = "version",
                Code = "UNKNOWN_VERSION",
                Message = $"The version '{card.Version}' is not a known Adaptive Cards version. Known versions: 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6."
            });
        }
        
        if (card.Body != null)
        {
            ValidateElements(card.Body, issues, "body", ids);
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
        
        if (card.Actions != null)
        {
            ValidateActions(card.Actions, issues, "actions", ids);
            
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

        ValidateSelectAction(card.SelectAction, issues, "selectAction");
    }
    
    
    private static void ValidateElements(IEnumerable<AdaptiveElement> elements, List<ValidationIssue> issues, string path, HashSet<string> ids)
    {
        int index = 0;
        foreach (var element in elements)
        {
            var elementPath = $"{path}[{index}]";
            TrackId(element.Id, elementPath, issues, ids);
            ValidateElement(element, issues, elementPath, ids);
            index++;
        }
    }
    
    private static void ValidateElement(AdaptiveElement element, List<ValidationIssue> issues, string path, HashSet<string> ids)
    {
        switch (element)
        {
            case TextBlock textBlock:
                if (string.IsNullOrEmpty(textBlock.Text))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.text",
                        Code = "MISSING_TEXT",
                        Message = "TextBlock is missing the required 'text' property."
                    });
                }
                break;
                
            case Image image:
                ValidateImage(image, issues, path);
                break;

            case ImageSet imageSet:
                if (imageSet.Images == null || imageSet.Images.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.images",
                        Code = "MISSING_IMAGES",
                        Message = "ImageSet is missing the required 'images' property."
                    });
                }
                else
                {
                    for (int i = 0; i < imageSet.Images.Count; i++)
                    {
                        ValidateImage(imageSet.Images[i], issues, $"{path}.images[{i}]");
                    }
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
                else
                {
                    TrackId(input.Id, path, issues, ids);
                }
                ValidateInputElement(input, issues, path);
                break;
                
            case Container container:
                if (container.Items == null || container.Items.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Warning,
                        Path = $"{path}.items",
                        Code = "EMPTY_CONTAINER",
                        Message = "Container has no items. It will render as empty."
                    });
                }
                else
                {
                    ValidateElements(container.Items, issues, $"{path}.items", ids);
                }
                ValidateSelectAction(container.SelectAction, issues, $"{path}.selectAction");
                break;
                
            case ColumnSet columnSet:
                if (columnSet.Columns != null)
                {
                    for (int i = 0; i < columnSet.Columns.Count; i++)
                    {
                        var column = columnSet.Columns[i];
                        var colPath = $"{path}.columns[{i}]";
                        TrackId(column.Id, colPath, issues, ids);
                        if (column.Items != null)
                        {
                            ValidateElements(column.Items, issues, $"{colPath}.items", ids);
                        }
                        ValidateSelectAction(column.SelectAction, issues, $"{colPath}.selectAction");
                    }
                }
                ValidateSelectAction(columnSet.SelectAction, issues, $"{path}.selectAction");
                break;

            case FactSet factSet:
                if (factSet.Facts == null || factSet.Facts.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.facts",
                        Code = "MISSING_FACTS",
                        Message = "FactSet is missing the required 'facts' property."
                    });
                }
                break;

            case ActionSet actionSet:
                if (actionSet.Actions == null || actionSet.Actions.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.actions",
                        Code = "MISSING_ACTIONSET_ACTIONS",
                        Message = "ActionSet is missing the required 'actions' property."
                    });
                }
                else
                {
                    ValidateActions(actionSet.Actions, issues, $"{path}.actions", ids);
                }
                break;

            case RichTextBlock richTextBlock:
                if (richTextBlock.Inlines == null || richTextBlock.Inlines.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.inlines",
                        Code = "MISSING_INLINES",
                        Message = "RichTextBlock is missing the required 'inlines' property."
                    });
                }
                break;

            case Media media:
                if (media.Sources == null || media.Sources.Count == 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.sources",
                        Code = "MISSING_MEDIA_SOURCES",
                        Message = "Media is missing the required 'sources' property."
                    });
                }
                break;

            case Table table:
                if (table.Rows != null)
                {
                    for (int r = 0; r < table.Rows.Count; r++)
                    {
                        var row = table.Rows[r];
                        if (row.Cells != null)
                        {
                            for (int c = 0; c < row.Cells.Count; c++)
                            {
                                var cell = row.Cells[c];
                                if (cell.Items != null)
                                {
                                    ValidateElements(cell.Items, issues, $"{path}.rows[{r}].cells[{c}].items", ids);
                                }
                                ValidateSelectAction(cell.SelectAction, issues, $"{path}.rows[{r}].cells[{c}].selectAction");
                            }
                        }
                    }
                }
                break;
        }
    }

    private static void ValidateImage(Image image, List<ValidationIssue> issues, string path)
    {
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
        ValidateSelectAction(image.SelectAction, issues, $"{path}.selectAction");
    }

    private static void ValidateInputElement(InputElement input, List<ValidationIssue> issues, string path)
    {
        switch (input)
        {
            case InputNumber inputNumber:
                if (inputNumber.Min.HasValue && inputNumber.Max.HasValue && inputNumber.Min.Value > inputNumber.Max.Value)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = path,
                        Code = "MIN_GREATER_THAN_MAX",
                        Message = $"Input.Number 'min' ({inputNumber.Min}) is greater than 'max' ({inputNumber.Max})."
                    });
                }
                break;

            case InputDate inputDate:
                if (!string.IsNullOrEmpty(inputDate.Min) && !string.IsNullOrEmpty(inputDate.Max) &&
                    string.Compare(inputDate.Min, inputDate.Max, StringComparison.Ordinal) > 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = path,
                        Code = "MIN_GREATER_THAN_MAX",
                        Message = $"Input.Date 'min' ({inputDate.Min}) is greater than 'max' ({inputDate.Max})."
                    });
                }
                break;

            case InputTime inputTime:
                if (!string.IsNullOrEmpty(inputTime.Min) && !string.IsNullOrEmpty(inputTime.Max) &&
                    string.Compare(inputTime.Min, inputTime.Max, StringComparison.Ordinal) > 0)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = path,
                        Code = "MIN_GREATER_THAN_MAX",
                        Message = $"Input.Time 'min' ({inputTime.Min}) is greater than 'max' ({inputTime.Max})."
                    });
                }
                break;

            case InputToggle inputToggle:
                if (string.IsNullOrEmpty(inputToggle.Title))
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Error,
                        Path = $"{path}.title",
                        Code = "MISSING_TOGGLE_TITLE",
                        Message = "Input.Toggle is missing the required 'title' property."
                    });
                }
                break;
        }
    }

    private static void ValidateSelectAction(AdaptiveAction? action, List<ValidationIssue> issues, string path)
    {
        if (action is ShowCardAction)
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Error,
                Path = path,
                Code = "INVALID_SELECT_ACTION",
                Message = "Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, Action.Execute, or Action.ToggleVisibility."
            });
        }
    }
    
    private static void ValidateActions(IEnumerable<AdaptiveAction> actions, List<ValidationIssue> issues, string path, HashSet<string> ids)
    {
        int index = 0;
        foreach (var action in actions)
        {
            var actionPath = $"{path}[{index}]";
            TrackId(action.Id, actionPath, issues, ids);
            
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
                        ValidateCard(showCard.Card, issues, ids);
                    }
                    break;

                case ToggleVisibilityAction toggleVisibility:
                    if (toggleVisibility.TargetElements == null || toggleVisibility.TargetElements.Count == 0)
                    {
                        issues.Add(new ValidationIssue
                        {
                            Severity = ValidationSeverity.Error,
                            Path = $"{actionPath}.targetElements",
                            Code = "MISSING_TARGET_ELEMENTS",
                            Message = "Action.ToggleVisibility is missing the required 'targetElements' property."
                        });
                    }
                    break;
            }
            
            index++;
        }
    }

    private static void TrackId(string? id, string path, List<ValidationIssue> issues, HashSet<string> ids)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        if (!ids.Add(id))
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = path,
                Code = "DUPLICATE_ID",
                Message = $"Duplicate id '{id}' found. Element IDs should be unique within a card."
            });
        }
    }

    /// <summary>
    /// Walks the card tree and emits VERSION_MISMATCH warnings for elements, actions,
    /// or card-level properties that require a newer schema version than declared.
    /// </summary>
    private static void ValidateVersionMismatch(AdaptiveCard card, AdaptiveCardVersion cardVersion, List<ValidationIssue> issues)
    {
        // Card-level properties
        if (card.Refresh != null)
            CheckCardPropertyVersion("refresh", cardVersion, issues);
        if (card.Authentication != null)
            CheckCardPropertyVersion("authentication", cardVersion, issues);
        if (card.Rtl.HasValue)
            CheckCardPropertyVersion("rtl", cardVersion, issues);
        if (card.Metadata != null)
            CheckCardPropertyVersion("metadata", cardVersion, issues);
        if (card.SelectAction != null)
            CheckCardPropertyVersion("selectAction", cardVersion, issues);
        if (!string.IsNullOrEmpty(card.MinHeight))
            CheckCardPropertyVersion("minHeight", cardVersion, issues);
        if (card.VerticalContentAlignment.HasValue)
            CheckCardPropertyVersion("verticalContentAlignment", cardVersion, issues);

        // Body elements
        if (card.Body != null)
            CheckElementVersions(card.Body, cardVersion, issues, "body");

        // Top-level actions
        if (card.Actions != null)
            CheckActionVersions(card.Actions, cardVersion, issues, "actions");
    }

    /// <summary>
    /// Checks whether a card-level property requires a newer schema version than the card declares.
    /// </summary>
    private static void CheckCardPropertyVersion(string propertyName, AdaptiveCardVersion cardVersion, List<ValidationIssue> issues)
    {
        var requiredVersion = VersionInfo.GetCardPropertyVersion(propertyName);
        if (requiredVersion > cardVersion)
        {
            issues.Add(new ValidationIssue
            {
                Severity = ValidationSeverity.Warning,
                Path = propertyName,
                Code = "VERSION_MISMATCH",
                Message = $"Property '{propertyName}' requires Adaptive Cards {requiredVersion.ToVersionString()} but card version is {cardVersion.ToVersionString()}."
            });
        }
    }

    /// <summary>
    /// Checks whether a selectAction's action type requires a newer schema version than the card declares.
    /// </summary>
    private static void CheckSelectActionVersion(AdaptiveAction? action, AdaptiveCardVersion cardVersion, List<ValidationIssue> issues, string path)
    {
        if (action == null)
            return;

        var typeDiscriminator = GetActionTypeDiscriminator(action);
        if (typeDiscriminator != null)
        {
            var requiredVersion = VersionInfo.GetElementVersion(typeDiscriminator);
            if (requiredVersion > cardVersion)
            {
                issues.Add(new ValidationIssue
                {
                    Severity = ValidationSeverity.Warning,
                    Path = path,
                    Code = "VERSION_MISMATCH",
                    Message = $"Action '{typeDiscriminator}' requires Adaptive Cards {requiredVersion.ToVersionString()} but card version is {cardVersion.ToVersionString()}."
                });
            }
        }
    }

    /// <summary>
    /// Recursively checks all elements in a collection for version mismatches against the declared card version.
    /// </summary>
    private static void CheckElementVersions(IEnumerable<AdaptiveElement> elements, AdaptiveCardVersion cardVersion, List<ValidationIssue> issues, string path)
    {
        int index = 0;
        foreach (var element in elements)
        {
            if (element == null) { index++; continue; }

            var elementPath = $"{path}[{index}]";
            var typeDiscriminator = GetElementTypeDiscriminator(element);

            if (typeDiscriminator != null)
            {
                var requiredVersion = VersionInfo.GetElementVersion(typeDiscriminator);
                if (requiredVersion > cardVersion)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Warning,
                        Path = elementPath,
                        Code = "VERSION_MISMATCH",
                        Message = $"Element '{typeDiscriminator}' requires Adaptive Cards {requiredVersion.ToVersionString()} but card version is {cardVersion.ToVersionString()}."
                    });
                }
            }

            // Walk into nested structures and check element-level selectActions
            switch (element)
            {
                case Container container:
                    if (container.Items != null)
                        CheckElementVersions(container.Items, cardVersion, issues, $"{elementPath}.items");
                    CheckSelectActionVersion(container.SelectAction, cardVersion, issues, $"{elementPath}.selectAction");
                    break;
                case ColumnSet columnSet:
                    if (columnSet.Columns != null)
                    {
                        for (int i = 0; i < columnSet.Columns.Count; i++)
                        {
                            var column = columnSet.Columns[i];
                            if (column.Items != null)
                                CheckElementVersions(column.Items, cardVersion, issues, $"{elementPath}.columns[{i}].items");
                            CheckSelectActionVersion(column.SelectAction, cardVersion, issues, $"{elementPath}.columns[{i}].selectAction");
                        }
                    }
                    CheckSelectActionVersion(columnSet.SelectAction, cardVersion, issues, $"{elementPath}.selectAction");
                    break;
                case Table table:
                    if (table.Rows != null)
                    {
                        for (int r = 0; r < table.Rows.Count; r++)
                        {
                            var row = table.Rows[r];
                            if (row.Cells != null)
                            {
                                for (int c = 0; c < row.Cells.Count; c++)
                                {
                                    var cell = row.Cells[c];
                                    if (cell.Items != null)
                                        CheckElementVersions(cell.Items, cardVersion, issues, $"{elementPath}.rows[{r}].cells[{c}].items");
                                    CheckSelectActionVersion(cell.SelectAction, cardVersion, issues, $"{elementPath}.rows[{r}].cells[{c}].selectAction");
                                }
                            }
                        }
                    }
                    break;
                case ActionSet actionSet:
                    if (actionSet.Actions != null)
                        CheckActionVersions(actionSet.Actions, cardVersion, issues, $"{elementPath}.actions");
                    break;
            }

            index++;
        }
    }

    /// <summary>
    /// Recursively checks all actions in a collection for version mismatches against the declared card version.
    /// </summary>
    private static void CheckActionVersions(IEnumerable<AdaptiveAction> actions, AdaptiveCardVersion cardVersion, List<ValidationIssue> issues, string path)
    {
        int index = 0;
        foreach (var action in actions)
        {
            if (action == null) { index++; continue; }

            var actionPath = $"{path}[{index}]";
            var typeDiscriminator = GetActionTypeDiscriminator(action);

            if (typeDiscriminator != null)
            {
                var requiredVersion = VersionInfo.GetElementVersion(typeDiscriminator);
                if (requiredVersion > cardVersion)
                {
                    issues.Add(new ValidationIssue
                    {
                        Severity = ValidationSeverity.Warning,
                        Path = actionPath,
                        Code = "VERSION_MISMATCH",
                        Message = $"Action '{typeDiscriminator}' requires Adaptive Cards {requiredVersion.ToVersionString()} but card version is {cardVersion.ToVersionString()}."
                    });
                }
            }

            // Walk into ShowCard nested cards
            if (action is ShowCardAction showCard && showCard.Card != null)
            {
                if (showCard.Card.Body != null)
                    CheckElementVersions(showCard.Card.Body, cardVersion, issues, $"{actionPath}.card.body");
                if (showCard.Card.Actions != null)
                    CheckActionVersions(showCard.Card.Actions, cardVersion, issues, $"{actionPath}.card.actions");
                CheckSelectActionVersion(showCard.Card.SelectAction, cardVersion, issues, $"{actionPath}.card.selectAction");
            }

            index++;
        }
    }

    /// <summary>
    /// Returns the JSON type discriminator string for the given element, or <c>null</c> if the type is unrecognized.
    /// </summary>
    private static string? GetElementTypeDiscriminator(AdaptiveElement element) => element switch
    {
        TextBlock => "TextBlock",
        Image => "Image",
        Container => "Container",
        ColumnSet => "ColumnSet",
        Table => "Table",
        FactSet => "FactSet",
        ImageSet => "ImageSet",
        ActionSet => "ActionSet",
        RichTextBlock => "RichTextBlock",
        Media => "Media",
        InputText => "Input.Text",
        InputNumber => "Input.Number",
        InputDate => "Input.Date",
        InputTime => "Input.Time",
        InputToggle => "Input.Toggle",
        InputChoiceSet => "Input.ChoiceSet",
        _ => null
    };

    /// <summary>
    /// Returns the JSON type discriminator string for the given action, or <c>null</c> if the type is unrecognized.
    /// </summary>
    private static string? GetActionTypeDiscriminator(AdaptiveAction action) => action switch
    {
        OpenUrlAction => "Action.OpenUrl",
        SubmitAction => "Action.Submit",
        ShowCardAction => "Action.ShowCard",
        ToggleVisibilityAction => "Action.ToggleVisibility",
        ExecuteAction => "Action.Execute",
        _ => null
    };
}
