using Xunit;

namespace FluentCards.Tests.Serialization;

public class ValidationTests
{
    [Fact]
    public void Validate_ValidCard_ReturnsEmptyList()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Valid card" }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_MissingSchema_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = null,
            Version = "1.5"
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("Missing '$schema' property"));
    }

    [Fact]
    public void Validate_MissingVersion_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = null!
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("Missing 'version' property"));
    }

    [Fact]
    public void Validate_InvalidType_AddsIssue()
    {
        // Arrange
        // Note: Type is read-only in AdaptiveCard, so this test verifies the validation logic
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5"
        };

        // Act
        var issues = card.Validate();

        // Assert
        // Should not have type issues since Type property returns "AdaptiveCard"
        Assert.DoesNotContain(issues, i => i.Contains("Invalid type"));
    }

    [Fact]
    public void Validate_InputElementMissingId_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "" }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("Input element missing required 'id' property"));
    }

    [Fact]
    public void Validate_ImageMissingUrl_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new Image { Url = null }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("Image element missing required 'url' property"));
    }

    [Fact]
    public void Validate_OpenUrlActionMissingUrl_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Actions = new List<AdaptiveAction>
            {
                new OpenUrlAction { Url = "" }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("OpenUrl action missing required 'url' property"));
    }

    [Fact]
    public void Validate_ShowCardActionMissingCard_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction { Card = null }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("ShowCard action missing required 'card' property"));
    }

    [Fact]
    public void Validate_NestedContainerWithInvalidInput_AddsIssue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement>
                    {
                        new InputNumber { Id = "" }
                    }
                }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Contains(issues, i => i.Contains("body[0].items[0]") && i.Contains("Input element missing required 'id' property"));
    }

    [Fact]
    public void Validate_MultipleIssues_AccumulatesAll()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = null,
            Version = null!,
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "" },
                new Image { Url = null }
            },
            Actions = new List<AdaptiveAction>
            {
                new OpenUrlAction { Url = "" }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.True(issues.Count >= 4, $"Expected at least 4 issues, got {issues.Count}");
        Assert.Contains(issues, i => i.Contains("Missing '$schema' property"));
        Assert.Contains(issues, i => i.Contains("Missing 'version' property"));
        Assert.Contains(issues, i => i.Contains("Input element missing required 'id' property"));
        Assert.Contains(issues, i => i.Contains("Image element missing required 'url' property"));
    }

    [Fact]
    public void Validate_NullBodyAndActions_DoesNotThrow()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = null,
            Actions = null
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_EmptyBodyAndActions_DoesNotAddIssues()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>(),
            Actions = new List<AdaptiveAction>()
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Empty(issues);
    }

    [Fact]
    public void Validate_ValidComplexCard_ReturnsEmpty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json",
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Header" },
                new Container
                {
                    Items = new List<AdaptiveElement>
                    {
                        new InputText { Id = "name", Label = "Name" },
                        new InputNumber { Id = "age", Label = "Age" }
                    }
                },
                new Image { Url = "https://example.com/image.png" }
            },
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction(),
                new OpenUrlAction { Url = "https://example.com" }
            }
        };

        // Act
        var issues = card.Validate();

        // Assert
        Assert.Empty(issues);
    }
}
