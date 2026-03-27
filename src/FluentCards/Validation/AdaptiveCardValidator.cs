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
    
    private static bool IsValidVersion(string version) =>
        version is "1.0" or "1.1" or "1.2" or "1.3" or "1.4" or "1.5" or "1.6";
    
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
}
