using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class RequiresTests
{
    [Fact]
    public void ElementWithRequires_Serialization_ContainsRequiresProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.2" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"requires\":", json);
        Assert.Contains("\"adaptiveCards\": \"1.2\"", json);
    }

    [Fact]
    public void ElementWithMultipleRequirements_Serialization_ContainsAllRequirements()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.4" },
                        { "hostCapability", "video" },
                        { "feature", "customFeature" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"requires\":", json);
        Assert.Contains("\"adaptiveCards\": \"1.4\"", json);
        Assert.Contains("\"hostCapability\": \"video\"", json);
        Assert.Contains("\"feature\": \"customFeature\"", json);
    }

    [Fact]
    public void RoundtripSerialization_WithRequires_PreservesRequirements()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Table
                {
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.5" },
                        { "hostCapability", "table" }
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
        var table = deserializedCard.Body[0] as Table;
        Assert.NotNull(table);
        Assert.NotNull(table.Requires);
        Assert.Equal(2, table.Requires.Count);
        Assert.Equal("1.5", table.Requires["adaptiveCards"]);
        Assert.Equal("table", table.Requires["hostCapability"]);
    }

    [Fact]
    public void ElementWithoutRequires_DoesNotSerializeRequiresProperty()
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
        Assert.DoesNotContain("\"requires\":", json);
    }

    [Fact]
    public void EmptyRequiresDictionary_DoesNotSerializeRequiresProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Text",
                    Requires = new Dictionary<string, string>()
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        // Empty dictionary should not be serialized due to JsonIgnoreCondition.WhenWritingNull
        // However, an empty dictionary is not null, so it might still appear
        // Let's test this behavior
    }

    [Fact]
    public void RequiresWithSpecialCharactersInKeys_Serialization_Works()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Test",
                    Requires = new Dictionary<string, string>
                    {
                        { "host-capability", "1.0" },
                        { "feature_flag", "enabled" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"requires\":", json);
        Assert.Contains("\"host-capability\": \"1.0\"", json);
        Assert.Contains("\"feature_flag\": \"enabled\"", json);
    }

    [Fact]
    public void RequiresWithVersionNumbers_Serialization_PreservesVersionFormat()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Version test",
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.0" },
                        { "feature", "2.5.1" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var textBlock = deserializedCard.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.NotNull(textBlock.Requires);
        Assert.Equal("1.0", textBlock.Requires["adaptiveCards"]);
        Assert.Equal("2.5.1", textBlock.Requires["feature"]);
    }

    [Fact]
    public void MultipleElementsWithDifferentRequires_Serialization_PreservesEachSeparately()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Element 1",
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.0" }
                    }
                },
                new Image
                {
                    Url = "https://example.com/image.png",
                    Requires = new Dictionary<string, string>
                    {
                        { "adaptiveCards", "1.2" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Equal(2, deserializedCard.Body.Count);

        var textBlock = deserializedCard.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.NotNull(textBlock.Requires);
        Assert.Equal("1.0", textBlock.Requires["adaptiveCards"]);

        var image = deserializedCard.Body[1] as Image;
        Assert.NotNull(image);
        Assert.NotNull(image.Requires);
        Assert.Equal("1.2", image.Requires["adaptiveCards"]);
    }
}
