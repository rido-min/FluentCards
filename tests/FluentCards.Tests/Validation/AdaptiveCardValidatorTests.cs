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
    public void Validate_EmptyTextBlock_ReturnsWarning()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText(""))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var warning = Assert.Single(issues, i => i.Code == "EMPTY_TEXT");
        Assert.Equal(ValidationSeverity.Warning, warning.Severity);
        Assert.Equal("body[0].text", warning.Path);
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
        Assert.Contains(issues, i => i.Code == "EMPTY_TEXT" && i.Path == "body[0].items[0].text");
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
}
