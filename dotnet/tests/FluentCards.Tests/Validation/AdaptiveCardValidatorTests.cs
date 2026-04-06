using Xunit;

namespace FluentCards.Tests.Validation;

public class AdaptiveCardValidatorTests
{
    [Fact]
    public void Validate_ValidCard_ReturnsNoIssues()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_MissingVersion_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard { Version = "" };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_VERSION");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("version", error.Path);
    }

    [Fact]
    public void Validate_MissingSchema_ReturnsWarning()
    {
        // Arrange
        var card = new AdaptiveCard { Schema = null, Version = "1.5" };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "MISSING_SCHEMA");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("$schema", warning.Path);
    }

    [Fact]
    public void Validate_EmptyCard_ReturnsWarning()
    {
        // Arrange
        var card = new AdaptiveCard { Version = "1.5", Body = null, Actions = null };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.Contains(issues, i => i.Code == "EMPTY_CARD" && i.Severity == ValidationSeverity.Warning);
    }

    [Fact]
    public void Validate_EmptyTextBlock_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText(""))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_TEXT");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].text", error.Path);
    }

    [Fact]
    public void Validate_MissingImageUrl_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImage(img => img.WithUrl(""))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_IMAGE_URL");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].url", error.Path);
    }

    [Fact]
    public void Validate_InvalidImageUrl_ReturnsWarning()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImage(img => img.WithUrl("not-a-url"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "INVALID_IMAGE_URL");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[0].url", warning.Path);
    }

    [Fact]
    public void Validate_MissingInputId_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputText(input => input.WithPlaceholder("Enter text"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_INPUT_ID");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].id", error.Path);
    }

    [Fact]
    public void Validate_TooManyActions_ReturnsWarning()
    {
        // Arrange
        var builder = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Test"));

        for (int i = 0; i < 6; i++)
        {
            builder.AddAction(a => a.OpenUrl($"https://example.com/{i}").WithTitle($"Action {i}"));
        }

        var card = builder.Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "TOO_MANY_ACTIONS");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("actions", warning.Path);
    }

    [Fact]
    public void Validate_MissingOpenUrlActionUrl_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();
        
        card.Actions = new List<AdaptiveAction> { new OpenUrlAction() };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_ACTION_URL");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("actions[0].url", error.Path);
    }

    [Fact]
    public void Validate_InvalidOpenUrlActionUrl_ReturnsWarning()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Test"))
            .AddAction(a => a.OpenUrl("not-a-url").WithTitle("Test"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "INVALID_ACTION_URL");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("actions[0].url", warning.Path);
    }

    [Fact]
    public void Validate_UnknownVersion_ReturnsWarning()
    {
        // Arrange
        var card = new AdaptiveCard { Version = "9.9" };
        card.Body = new List<AdaptiveElement> { new TextBlock { Text = "Test" } };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "UNKNOWN_VERSION");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("version", warning.Path);
    }

    [Fact]
    public void Validate_NestedContainerValidation_Works()
    {
        // Arrange
        var inputText = new InputTextBuilder()
            .WithPlaceholder("Test")
            .Build();
        
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .AddTextBlock(tb => tb.WithText(""))
                .AddElement(inputText))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.Contains(issues, i => i.Code == "MISSING_TEXT" && i.Path == "body[0].items[0].text");
        Assert.Contains(issues, i => i.Code == "MISSING_INPUT_ID" && i.Path == "body[0].items[1].id");
    }

    [Fact]
    public void Validate_NestedColumnSetValidation_Works()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .AddImage(img => img.WithUrl(""))))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_IMAGE_URL");
        Assert.Equal("body[0].columns[0].items[0].url", error.Path);
    }

    [Fact]
    public void ValidateAndThrow_WithErrors_ThrowsException()
    {
        // Arrange
        var card = new AdaptiveCard { Version = "" };

        // Act & Assert
        var ex = Assert.Throws<AdaptiveCardValidationException>(() => 
            AdaptiveCardValidator.ValidateAndThrow(card));
        
        Assert.Single(ex.Errors);
        Assert.Contains("version", ex.Message);
    }

    [Fact]
    public void ValidateAndThrow_WithWarningsOnly_DoesNotThrow()
    {
        // Arrange
        var card = new AdaptiveCard { Schema = null, Version = "1.5" };
        card.Body = new List<AdaptiveElement> { new TextBlock { Text = "Test" } };

        // Act & Assert
        var exception = Record.Exception(() => AdaptiveCardValidator.ValidateAndThrow(card));
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateAndThrow_ValidCard_DoesNotThrow()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();

        // Act & Assert
        var exception = Record.Exception(() => AdaptiveCardValidator.ValidateAndThrow(card));
        Assert.Null(exception);
    }

    [Fact]
    public void ValidationIssue_HasCorrectPaths()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText(""))
            .AddImage(img => img.WithUrl(""))
            .AddInputText(input => input.WithPlaceholder("Test"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.Contains(issues, i => i.Path == "body[0].text");
        Assert.Contains(issues, i => i.Path == "body[1].url");
        Assert.Contains(issues, i => i.Path == "body[2].id");
    }

    [Fact]
    public void AdaptiveCardValidationException_FormatsSingleErrorCorrectly()
    {
        // Arrange
        var errors = new List<ValidationIssue>
        {
            new ValidationIssue
            {
                Severity = ValidationSeverity.Error,
                Code = "TEST",
                Path = "test.path",
                Message = "Test error message"
            }
        };

        // Act
        var ex = new AdaptiveCardValidationException(errors);

        // Assert
        Assert.Equal("Adaptive Card validation failed: Test error message", ex.Message);
    }

    [Fact]
    public void AdaptiveCardValidationException_FormatsMultipleErrorsCorrectly()
    {
        // Arrange
        var errors = new List<ValidationIssue>
        {
            new ValidationIssue { Severity = ValidationSeverity.Error, Code = "TEST1", Path = "path1", Message = "Error 1" },
            new ValidationIssue { Severity = ValidationSeverity.Error, Code = "TEST2", Path = "path2", Message = "Error 2" }
        };

        // Act
        var ex = new AdaptiveCardValidationException(errors);

        // Assert
        Assert.Contains("Adaptive Card validation failed with 2 errors", ex.Message);
        Assert.Contains("[path1] Error 1", ex.Message);
        Assert.Contains("[path2] Error 2", ex.Message);
    }

    [Fact]
    public void Validate_FactSetWithNullFacts_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement> { new FactSet { Facts = null } }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_FACTS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].facts", error.Path);
    }

    [Fact]
    public void Validate_FactSetWithEmptyFacts_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddFactSet(fs => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_FACTS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].facts", error.Path);
    }

    [Fact]
    public void Validate_ActionSetWithNullActions_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement> { new ActionSet { Actions = null } }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_ACTIONSET_ACTIONS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].actions", error.Path);
    }

    [Fact]
    public void Validate_ActionSetWithEmptyActions_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddActionSet(a => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_ACTIONSET_ACTIONS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].actions", error.Path);
    }

    [Fact]
    public void Validate_RichTextBlockWithNullInlines_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement> { new RichTextBlock { Inlines = null } }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_INLINES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].inlines", error.Path);
    }

    [Fact]
    public void Validate_RichTextBlockWithEmptyInlines_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddRichTextBlock(rtb => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_INLINES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].inlines", error.Path);
    }

    [Fact]
    public void Validate_MediaWithNullSources_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement> { new Media { Sources = null } }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_MEDIA_SOURCES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].sources", error.Path);
    }

    [Fact]
    public void Validate_MediaWithEmptySources_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddMedia(m => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_MEDIA_SOURCES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].sources", error.Path);
    }

    [Fact]
    public void Validate_ImageSetWithNullImages_ReturnsError()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement> { new ImageSet { Images = null } }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_IMAGES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].images", error.Path);
    }

    [Fact]
    public void Validate_ImageSetWithEmptyImages_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImageSet(iset => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_IMAGES");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].images", error.Path);
    }

    [Fact]
    public void Validate_InputToggleWithEmptyTitle_ReturnsError()
    {
        // Arrange — builder creates InputToggle with default empty Title
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputToggle(t => t.WithId("toggle1"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_TOGGLE_TITLE");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].title", error.Path);
    }

    [Fact]
    public void Validate_ToggleVisibilityActionWithNullTargetElements_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();

        card.Actions = new List<AdaptiveAction>
        {
            new ToggleVisibilityAction { TargetElements = null }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_TARGET_ELEMENTS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("actions[0].targetElements", error.Path);
    }

    [Fact]
    public void Validate_ToggleVisibilityActionWithEmptyTargetElements_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Test"))
            .Build();

        card.Actions = new List<AdaptiveAction>
        {
            new ToggleVisibilityAction { TargetElements = new List<object>() }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_TARGET_ELEMENTS");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("actions[0].targetElements", error.Path);
    }

    [Fact]
    public void Validate_InputNumberMinGreaterThanMax_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputNumber(n => n.WithId("num1").WithMin(100).WithMax(10))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MIN_GREATER_THAN_MAX");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0]", error.Path);
    }

    [Fact]
    public void Validate_InputDateMinGreaterThanMax_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputDate(d => d.WithId("date1").WithMin("2025-12-31").WithMax("2025-01-01"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MIN_GREATER_THAN_MAX");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0]", error.Path);
    }

    [Fact]
    public void Validate_InputTimeMinGreaterThanMax_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputTime(t => t.WithId("time1").WithMin("23:00").WithMax("08:00"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MIN_GREATER_THAN_MAX");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0]", error.Path);
    }

    [Fact]
    public void Validate_ShowCardActionAsSelectAction_ReturnsError()
    {
        // Arrange — ShowCardAction used as selectAction on a Container
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement> { new TextBlock { Text = "Test" } },
                    SelectAction = new ShowCardAction()
                }
            }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "INVALID_SELECT_ACTION");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].selectAction", error.Path);
    }

    [Fact]
    public void Validate_DuplicateIds_ReturnsWarning()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Id = "duplicate", Text = "First" },
                new TextBlock { Id = "duplicate", Text = "Second" }
            }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "DUPLICATE_ID");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[1]", warning.Path);
    }

    [Fact]
    public void Validate_EmptyContainer_ReturnsWarning()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new Container { Items = null }
            }
        };

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "EMPTY_CONTAINER");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[0].items", warning.Path);
    }

    [Fact]
    public void Validate_EmptyContainerViaEmptyItems_ReturnsWarning()
    {
        // Arrange — Container with empty Items list (via builder)
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => { })
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "EMPTY_CONTAINER");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[0].items", warning.Path);
    }

    [Fact]
    public void Validate_NestedTableValidation_ValidatesElementsInsideCells()
    {
        // Arrange — Table with a cell containing a TextBlock with empty text
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTable(t => t
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell
                        {
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "" }
                            }
                        }
                    }
                }))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_TEXT");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].rows[0].cells[0].items[0].text", error.Path);
    }

    [Fact]
    public void Validate_NestedImageSetValidation_ValidatesImageUrls()
    {
        // Arrange — ImageSet with an image that has an empty URL
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImageSet(iset => iset
                .AddImage(img => img.WithUrl("")))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "MISSING_IMAGE_URL");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].images[0].url", error.Path);
    }

    [Fact]
    public void Validate_NestedImageSetValidation_InvalidUrlReturnsWarning()
    {
        // Arrange — ImageSet with an image that has an invalid (non-absolute) URL
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImageSet(iset => iset
                .AddImage(img => img.WithUrl("not-a-url")))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "INVALID_IMAGE_URL");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[0].images[0].url", warning.Path);
    }

    [Fact]
    public void Validate_TableInV1_2Card_ReturnsVersionMismatchWarning()
    {
        // Arrange — Table requires V1.5; card declares V1.2
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTable(t => t.AddColumn(new TableColumnDefinition()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("Table", warning.Message);
        Assert.Contains("1.5", warning.Message);
        Assert.Contains("1.2", warning.Message);
    }

    [Fact]
    public void Validate_ExecuteActionInV1_2Card_ReturnsVersionMismatchWarning()
    {
        // Arrange — Action.Execute requires V1.4; card declares V1.2
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .AddAction(a => a.Execute("Run"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("Action.Execute", warning.Message);
        Assert.Contains("1.4", warning.Message);
        Assert.Contains("1.2", warning.Message);
    }

    [Fact]
    public void Validate_RefreshInV1_2Card_ReturnsVersionMismatchWarning()
    {
        // Arrange — refresh property requires V1.4; card declares V1.2
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .WithRefresh(r => r.WithAction(a => a.Execute()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var refreshWarning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH" && i.Path == "refresh");
        Assert.Equal(ValidationSeverity.Warning, refreshWarning.Severity);
        Assert.Contains("refresh", refreshWarning.Message);
        Assert.Contains("1.4", refreshWarning.Message);
    }

    [Fact]
    public void Validate_TableInV1_5Card_ReturnsNoVersionMismatch()
    {
        // Arrange — Table requires V1.5; card declares V1.5 — no mismatch
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTable(t => t.AddColumn(new TableColumnDefinition()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.DoesNotContain(issues, i => i.Code == "VERSION_MISMATCH");
    }

    [Fact]
    public void Validate_AllV1_0ElementsInV1_0Card_NoVersionMismatch()
    {
        // Arrange — only V1.0 elements in a V1.0 card
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.0")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .AddImage(img => img.WithUrl("https://example.com/img.png"))
            .AddAction(a => a.OpenUrl("https://example.com", "Go"))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.DoesNotContain(issues, i => i.Code == "VERSION_MISMATCH");
    }

    [Fact]
    public void Validate_UnknownVersion_SkipsVersionMismatchCheck()
    {
        // Arrange — version "2.0" is unknown; should skip version mismatch even with newer elements
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("2.0")
            .AddTable(t => t.AddColumn(new TableColumnDefinition()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert — UNKNOWN_VERSION warning expected, but no VERSION_MISMATCH
        Assert.Contains(issues, i => i.Code == "UNKNOWN_VERSION");
        Assert.DoesNotContain(issues, i => i.Code == "VERSION_MISMATCH");
    }

    [Fact]
    public void Validate_NestedTableInContainer_ReturnsVersionMismatchWarning()
    {
        // Arrange — Table nested inside a Container; card declares V1.2
        var table = new TableBuilder()
            .AddColumn(new TableColumnDefinition())
            .Build();

        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddContainer(c => c.AddElement(table))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("Table", warning.Message);
        Assert.Equal("body[0].items[0]", warning.Path);
    }

    [Fact]
    public void Validate_RtlPropertyInV1_2Card_ReturnsVersionMismatchWarning()
    {
        // Arrange — rtl property requires V1.5; card declares V1.2
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTextBlock(tb => tb.WithText("مرحبا"))
            .WithRtl()
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH" && i.Path == "rtl");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("rtl", warning.Message);
        Assert.Contains("1.5", warning.Message);
        Assert.Contains("1.2", warning.Message);
    }

    [Fact]
    public void Validate_MetadataInV1_5Card_ReturnsVersionMismatchWarning()
    {
        // Arrange — metadata property requires V1.6; card declares V1.5
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .WithMetadata("https://example.com/card")
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH" && i.Path == "metadata");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("metadata", warning.Message);
        Assert.Contains("1.6", warning.Message);
        Assert.Contains("1.5", warning.Message);
    }

    [Fact]
    public void Validate_ShowCardWithNewerElements_ReturnsVersionMismatch()
    {
        // Arrange — ShowCard contains a Table (V1.5) in a V1.2 card
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .AddAction(a => a.ShowCard("Details"))
            .Build();

        var showCard = (ShowCardAction)card.Actions![0];
        showCard.Card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTable(t => t.AddColumn(new TableColumnDefinition()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert — Table inside ShowCard should trigger VERSION_MISMATCH using top-level card version
        var warning = Assert.Single(issues, i => i.Code == "VERSION_MISMATCH");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Contains("Table", warning.Message);
        Assert.Contains("1.5", warning.Message);
        Assert.Equal("actions[0].card.body[0]", warning.Path);
    }

    [Fact]
    public void Validate_MultipleVersionMismatches_ReturnsAllWarnings()
    {
        // Arrange — Table (V1.5) and refresh (V1.4) both newer than V1.2
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTable(t => t.AddColumn(new TableColumnDefinition()))
            .WithRefresh(r => r.WithAction(a => a.Execute()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert — should return multiple VERSION_MISMATCH warnings, one for each mismatch
        var mismatches = issues.Where(i => i.Code == "VERSION_MISMATCH").ToList();
        Assert.True(mismatches.Count >= 2, $"Expected at least 2 VERSION_MISMATCH warnings but found {mismatches.Count}.");
        Assert.Contains(mismatches, m => m.Message.Contains("Table"));
        Assert.Contains(mismatches, m => m.Message.Contains("refresh"));
    }
}