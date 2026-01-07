using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class SerializationTests
{
    [Fact]
    public void EmptyCard_Serialization_ContainsTypeAndVersion()
    {
        // Arrange
        var card = new AdaptiveCard();

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"AdaptiveCard\"", json);
        Assert.Contains("\"version\": \"1.5\"", json);
    }

    [Fact]
    public void CardWithSchema_Serialization_ContainsSchemaProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = "http://adaptivecards.io/schemas/adaptive-card.json"
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"$schema\": \"http://adaptivecards.io/schemas/adaptive-card.json\"", json);
    }

    [Fact]
    public void CardWithSingleTextBlock_Serialization_ContainsBodyArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello, World!" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"body\":", json);
        Assert.Contains("\"text\": \"Hello, World!\"", json);
        Assert.Contains("\"type\": \"TextBlock\"", json);
    }

    [Fact]
    public void TextBlockWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Id = "textBlock1",
                    Text = "Sample Text",
                    Size = TextSize.Large,
                    Weight = TextWeight.Bolder,
                    Color = TextColor.Accent,
                    Wrap = true,
                    MaxLines = 3,
                    HorizontalAlignment = HorizontalAlignment.Center
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"textBlock1\"", json);
        Assert.Contains("\"text\": \"Sample Text\"", json);
        Assert.Contains("\"size\": \"large\"", json);
        Assert.Contains("\"weight\": \"bolder\"", json);
        Assert.Contains("\"color\": \"accent\"", json);
        Assert.Contains("\"wrap\": true", json);
        Assert.Contains("\"maxLines\": 3", json);
        Assert.Contains("\"horizontalAlignment\": \"center\"", json);
    }

    [Fact]
    public void RoundtripSerialization_PreservesCardStructure()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Test Text",
                    Size = TextSize.Medium,
                    Weight = TextWeight.Bolder
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.Equal(originalCard.Version, deserializedCard.Version);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);
        
        var textBlock = deserializedCard.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Test Text", textBlock.Text);
        Assert.Equal(TextSize.Medium, textBlock.Size);
        Assert.Equal(TextWeight.Bolder, textBlock.Weight);
    }

    [Fact]
    public void NullProperties_AreNotSerialized()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Simple Text" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"size\":", json);
        Assert.DoesNotContain("\"weight\":", json);
        Assert.DoesNotContain("\"color\":", json);
        Assert.DoesNotContain("\"wrap\":", json);
        Assert.DoesNotContain("\"maxLines\":", json);
    }

    [Fact]
    public void EnumSerialization_UsesCamelCase()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Test",
                    Size = TextSize.ExtraLarge,
                    Color = TextColor.Attention
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"size\": \"extraLarge\"", json);
        Assert.Contains("\"color\": \"attention\"", json);
    }

    [Fact]
    public void PolymorphicTypeSerialization_IncludesTypeDiscriminator()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Test" }
            },
            Actions = new List<AdaptiveAction>
            {
                new OpenUrlAction { Url = "https://example.com", Title = "Visit" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"TextBlock\"", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
    }
}
