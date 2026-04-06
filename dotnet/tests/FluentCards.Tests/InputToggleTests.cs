using Xunit;

namespace FluentCards.Tests;

public class InputToggleTests
{
    [Fact]
    public void BasicToggle_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle 
                { 
                    Id = "accept",
                    Title = "I accept the terms"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.Toggle\"", json);
        Assert.Contains("\"id\": \"accept\"", json);
        Assert.Contains("\"title\": \"I accept the terms\"", json);
    }

    [Fact]
    public void Toggle_WithCustomValueOnOff_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "notifications",
                    Title = "Enable notifications",
                    ValueOn = "yes",
                    ValueOff = "no"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"valueOn\": \"yes\"", json);
        Assert.Contains("\"valueOff\": \"no\"", json);
    }

    [Fact]
    public void Toggle_RoundtripSerialization_PreservesToggleState()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "toggle1",
                    Title = "Subscribe to newsletter",
                    Value = "true",
                    ValueOn = "subscribed",
                    ValueOff = "unsubscribed"
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);
        
        var toggle = deserializedCard.Body[0] as InputToggle;
        Assert.NotNull(toggle);
        Assert.Equal("toggle1", toggle.Id);
        Assert.Equal("Subscribe to newsletter", toggle.Title);
        Assert.Equal("true", toggle.Value);
        Assert.Equal("subscribed", toggle.ValueOn);
        Assert.Equal("unsubscribed", toggle.ValueOff);
    }

    [Fact]
    public void Toggle_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "agreement",
                    Label = "Legal Agreement",
                    Title = "I have read and agree to the terms and conditions",
                    IsRequired = true,
                    ErrorMessage = "You must accept to continue",
                    Value = "false",
                    ValueOn = "agreed",
                    ValueOff = "declined",
                    Wrap = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"agreement\"", json);
        Assert.Contains("\"label\": \"Legal Agreement\"", json);
        Assert.Contains("\"title\": \"I have read and agree to the terms and conditions\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"You must accept to continue\"", json);
        Assert.Contains("\"value\": \"false\"", json);
        Assert.Contains("\"valueOn\": \"agreed\"", json);
        Assert.Contains("\"valueOff\": \"declined\"", json);
        Assert.Contains("\"wrap\": true", json);
    }

    [Fact]
    public void Toggle_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle 
                { 
                    Id = "toggle",
                    Title = "Enable feature"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"value\":", json);
        Assert.DoesNotContain("\"valueOn\":", json);
        Assert.DoesNotContain("\"valueOff\":", json);
        Assert.DoesNotContain("\"wrap\":", json);
        Assert.DoesNotContain("\"label\":", json);
        Assert.DoesNotContain("\"errorMessage\":", json);
    }

    [Fact]
    public void Toggle_WithWrapTrue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "longText",
                    Title = "This is a very long title that should wrap to multiple lines when displayed in a narrow container",
                    Wrap = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"wrap\": true", json);
    }

    [Fact]
    public void Toggle_WithValueTrue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "toggle",
                    Title = "Enable",
                    Value = "true"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"true\"", json);
    }

    [Fact]
    public void Toggle_WithValueFalse_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "toggle",
                    Title = "Enable",
                    Value = "false"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"false\"", json);
    }

    [Fact]
    public void Toggle_WithCustomValues_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "mode",
                    Title = "Dark Mode",
                    Value = "enabled",
                    ValueOn = "enabled",
                    ValueOff = "disabled"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"enabled\"", json);
        Assert.Contains("\"valueOn\": \"enabled\"", json);
        Assert.Contains("\"valueOff\": \"disabled\"", json);
    }

    [Fact]
    public void Toggle_EmptyStringValue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "toggle",
                    Title = "Enable",
                    Value = ""
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"\"", json);
    }
}
