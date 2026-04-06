using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class FallbackTests
{
    [Fact]
    public void ElementWithFallbackDrop_Serialization_ContainsFallbackProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Hello",
                    Fallback = "drop"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"fallback\": \"drop\"", json);
    }

    [Fact]
    public void ElementWithFallbackElement_Serialization_ContainsNestedElement()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Fallback = new TextBlock { Text = "Fallback text" }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"fallback\":", json);
        Assert.Contains("\"type\": \"TextBlock\"", json);
        Assert.Contains("\"text\": \"Fallback text\"", json);
    }

    [Fact]
    public void RoundtripSerialization_WithFallbackDrop_PreservesFallback()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Main content",
                    Fallback = "drop"
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var textBlock = deserializedCard.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("drop", textBlock.Fallback);
    }

    [Fact]
    public void RoundtripSerialization_WithFallbackElement_PreservesNestedElement()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Fallback = new TextBlock
                    {
                        Text = "Fallback text",
                        Weight = TextWeight.Bolder
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var richTextBlock = deserializedCard.Body[0] as RichTextBlock;
        Assert.NotNull(richTextBlock);
        Assert.NotNull(richTextBlock.Fallback);

        var fallbackElement = richTextBlock.Fallback as TextBlock;
        Assert.NotNull(fallbackElement);
        Assert.Equal("Fallback text", fallbackElement.Text);
        Assert.Equal(TextWeight.Bolder, fallbackElement.Weight);
    }

    [Fact]
    public void DeserializeFallbackFromJsonString_ReturnsStringValue()
    {
        // Arrange
        var json = """
        {
          "type": "AdaptiveCard",
          "version": "1.5",
          "body": [
            {
              "type": "TextBlock",
              "text": "Test",
              "fallback": "drop"
            }
          ]
        }
        """;

        // Act
        var card = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        var textBlock = card.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("drop", textBlock.Fallback);
    }

    [Fact]
    public void DeserializeFallbackFromJsonObject_ReturnsElementObject()
    {
        // Arrange
        var json = """
        {
          "type": "AdaptiveCard",
          "version": "1.5",
          "body": [
            {
              "type": "Image",
              "url": "https://example.com/image.png",
              "fallback": {
                "type": "TextBlock",
                "text": "Image not available"
              }
            }
          ]
        }
        """;

        // Act
        var card = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        var image = card.Body[0] as Image;
        Assert.NotNull(image);
        Assert.NotNull(image.Fallback);

        var fallbackElement = image.Fallback as TextBlock;
        Assert.NotNull(fallbackElement);
        Assert.Equal("Image not available", fallbackElement.Text);
    }

    [Fact]
    public void FallbackWithDeeplyNestedElement_Serialization_Works()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Fallback = new Image
                    {
                        Url = "https://example.com/thumbnail.png",
                        Fallback = new TextBlock { Text = "Media unavailable" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"fallback\":", json);
        Assert.Contains("\"type\": \"Image\"", json);
        Assert.Contains("\"type\": \"TextBlock\"", json);
        Assert.Contains("\"text\": \"Media unavailable\"", json);
    }

    [Fact]
    public void ElementWithoutFallback_DoesNotSerializeFallbackProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Simple text" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"fallback\":", json);
    }
}
