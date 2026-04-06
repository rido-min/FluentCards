using Xunit;

namespace FluentCards.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void EmptyBodyArray_Serialization_ProducesEmptyArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>()
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"body\": []", json);
    }

    [Fact]
    public void SpecialCharactersInText_Serialization_EscapesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello \"World\"\nNew Line\tTab\u2713 Unicode" }
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
        Assert.Equal("Hello \"World\"\nNew Line\tTab\u2713 Unicode", textBlock.Text);
    }

    [Fact]
    public void VeryLongText_Serialization_HandlesLargeStrings()
    {
        // Arrange
        var longText = new string('A', 10000);
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = longText }
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
        Assert.Equal(10000, textBlock.Text.Length);
        Assert.Equal(longText, textBlock.Text);
    }

    [Fact]
    public void DeserializationOfUnknownProperties_DoesNotThrow()
    {
        // Arrange
        var jsonWithUnknownProps = @"{
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""unknownProperty"": ""some value"",
            ""body"": [
                {
                    ""type"": ""TextBlock"",
                    ""text"": ""Hello"",
                    ""unknownTextBlockProp"": ""another value""
                }
            ]
        }";

        // Act & Assert
        var exception = Record.Exception(() => AdaptiveCardExtensions.FromJson(jsonWithUnknownProps));
        Assert.Null(exception);
        
        var card = AdaptiveCardExtensions.FromJson(jsonWithUnknownProps);
        Assert.NotNull(card);
        Assert.NotNull(card.Body);
        Assert.Single(card.Body);
    }

    [Fact]
    public void NullBodyAndActions_Serialization_DoesNotIncludeProperties()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = null,
            Actions = null
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"body\"", json);
        Assert.DoesNotContain("\"actions\"", json);
    }

    [Fact]
    public void CardWithNullSchema_Serialization_DoesNotIncludeSchema()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Schema = null
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"$schema\"", json);
    }

    [Fact]
    public void MultipleActions_Serialization_PreservesOrder()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a.OpenUrl("https://first.com").WithTitle("First"))
            .AddAction(a => a.OpenUrl("https://second.com").WithTitle("Second"))
            .AddAction(a => a.OpenUrl("https://third.com").WithTitle("Third"))
            .Build();

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard?.Actions);
        Assert.Equal(3, deserializedCard.Actions.Count);
        Assert.Equal("First", deserializedCard.Actions[0].Title);
        Assert.Equal("Second", deserializedCard.Actions[1].Title);
        Assert.Equal("Third", deserializedCard.Actions[2].Title);
    }
}
