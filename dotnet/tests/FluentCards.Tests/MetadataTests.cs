using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class MetadataTests
{
    [Fact]
    public void CardWithMetadata_Serialization_ContainsMetadataProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Metadata = new CardMetadata
            {
                WebUrl = "https://example.com/card"
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"metadata\":", json);
        Assert.Contains("\"webUrl\": \"https://example.com/card\"", json);
    }

    [Fact]
    public void MetadataWithWebUrl_Serialization_ContainsWebUrlProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Metadata = new CardMetadata
            {
                WebUrl = "https://adaptivecards.io"
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"webUrl\": \"https://adaptivecards.io\"", json);
    }

    [Fact]
    public void RoundtripSerialization_WithMetadata_PreservesMetadata()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Metadata = new CardMetadata
            {
                WebUrl = "https://github.com/microsoft/adaptivecards"
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Metadata);
        Assert.Equal("https://github.com/microsoft/adaptivecards", deserializedCard.Metadata.WebUrl);
    }

    [Fact]
    public void CardWithoutMetadata_DoesNotSerializeMetadataProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"metadata\":", json);
    }

    [Fact]
    public void MetadataWithVeryLongUrl_Serialization_Works()
    {
        // Arrange
        var longUrl = "https://example.com/very/long/path/to/resource/" + new string('a', 500);
        var card = new AdaptiveCard
        {
            Metadata = new CardMetadata
            {
                WebUrl = longUrl
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.Contains("\"webUrl\":", json);
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Metadata);
        Assert.Equal(longUrl, deserializedCard.Metadata.WebUrl);
    }

    [Fact]
    public void EmptyMetadataObject_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Metadata = new CardMetadata()
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Metadata);
        Assert.Null(deserializedCard.Metadata.WebUrl);
    }

    [Fact]
    public void MetadataWithSpecialCharactersInUrl_Serialization_Works()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Metadata = new CardMetadata
            {
                WebUrl = "https://example.com/path?query=value&other=test#fragment"
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Metadata);
        Assert.Equal("https://example.com/path?query=value&other=test#fragment", deserializedCard.Metadata.WebUrl);
    }
}
