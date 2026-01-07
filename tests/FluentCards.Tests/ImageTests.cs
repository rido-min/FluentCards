using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ImageTests
{
    [Fact]
    public void ImageWithUrlOnly_Serialization_ContainsMinimalFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image { Url = "https://example.com/image.jpg" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Image\"", json);
        Assert.Contains("\"url\": \"https://example.com/image.jpg\"", json);
        Assert.DoesNotContain("\"altText\":", json);
        Assert.DoesNotContain("\"size\":", json);
    }

    [Fact]
    public void ImageWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Id = "image1",
                    Url = "https://example.com/image.jpg",
                    AltText = "Example Image",
                    Size = ImageSize.Large,
                    Style = ImageStyle.Person,
                    Width = "50px",
                    Height = "50px",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    BackgroundColor = "#FFFFFF"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"image1\"", json);
        Assert.Contains("\"url\": \"https://example.com/image.jpg\"", json);
        Assert.Contains("\"altText\": \"Example Image\"", json);
        Assert.Contains("\"size\": \"large\"", json);
        Assert.Contains("\"style\": \"person\"", json);
        Assert.Contains("\"width\": \"50px\"", json);
        Assert.Contains("\"height\": \"50px\"", json);
        Assert.Contains("\"horizontalAlignment\": \"center\"", json);
        Assert.Contains("\"backgroundColor\": \"#FFFFFF\"", json);
    }

    [Fact]
    public void ImageWithSelectAction_Serialization_ContainsAction()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Url = "https://example.com/image.jpg",
                    SelectAction = new OpenUrlAction
                    {
                        Url = "https://example.com/target",
                        Title = "Click me"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"selectAction\":", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
        Assert.Contains("\"url\": \"https://example.com/target\"", json);
    }

    [Fact]
    public void ImageRoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Id = "img1",
                    Url = "https://example.com/test.png",
                    AltText = "Test Image",
                    Size = ImageSize.Medium,
                    Style = ImageStyle.Person,
                    Width = "100px",
                    HorizontalAlignment = HorizontalAlignment.Right
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

        var image = deserializedCard.Body[0] as Image;
        Assert.NotNull(image);
        Assert.Equal("img1", image.Id);
        Assert.Equal("https://example.com/test.png", image.Url);
        Assert.Equal("Test Image", image.AltText);
        Assert.Equal(ImageSize.Medium, image.Size);
        Assert.Equal(ImageStyle.Person, image.Style);
        Assert.Equal("100px", image.Width);
        Assert.Equal(HorizontalAlignment.Right, image.HorizontalAlignment);
    }

    [Fact]
    public void ImageEnumSerialization_UsesCamelCase()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Url = "https://example.com/image.jpg",
                    Size = ImageSize.Large,
                    Style = ImageStyle.Default
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        // Should use camelCase for enum values
        Assert.Contains("\"size\": \"large\"", json);
        Assert.DoesNotContain("\"size\": \"Large\"", json);
    }

    [Fact]
    public void ImageWithSpecialCharactersInAltText_Serialization_EscapesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Url = "https://example.com/image.jpg",
                    AltText = "Image with \"quotes\" and\nnewlines"
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var image = deserializedCard.Body[0] as Image;
        Assert.NotNull(image);
        Assert.Equal("Image with \"quotes\" and\nnewlines", image.AltText);
    }

    [Fact]
    public void ImageWithVeryLongUrl_Serialization_HandlesLargeStrings()
    {
        // Arrange
        var longUrl = "https://example.com/" + new string('a', 1000);
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image { Url = longUrl }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var image = deserializedCard.Body[0] as Image;
        Assert.NotNull(image);
        Assert.Equal(longUrl, image.Url);
    }

    [Fact]
    public void ImageWithCommonProperties_Serialization_IncludesBaseProperties()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Image
                {
                    Url = "https://example.com/image.jpg",
                    IsVisible = false,
                    Spacing = Spacing.Large,
                    Separator = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isVisible\": false", json);
        Assert.Contains("\"spacing\": \"large\"", json);
        Assert.Contains("\"separator\": true", json);
    }
}
