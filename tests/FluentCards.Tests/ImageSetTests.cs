using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ImageSetTests
{
    [Fact]
    public void ImageSet_WithImages_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ImageSet
                {
                    Images = new List<Image>
                    {
                        new Image { Url = "https://example.com/image1.jpg" },
                        new Image { Url = "https://example.com/image2.jpg" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"ImageSet\"", json);
        Assert.Contains("\"images\":", json);
        Assert.Contains("https://example.com/image1.jpg", json);
        Assert.Contains("https://example.com/image2.jpg", json);
    }

    [Fact]
    public void ImageSet_WithImageSize_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ImageSet
                {
                    Images = new List<Image>
                    {
                        new Image { Url = "https://example.com/image1.jpg" }
                    },
                    ImageSize = ImageSize.Medium
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"imageSize\": \"medium\"", json);
    }

    [Fact]
    public void ImageSet_RoundtripSerialization_PreservesImageOrder()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ImageSet
                {
                    Id = "imageSet1",
                    Images = new List<Image>
                    {
                        new Image { Url = "https://example.com/1.jpg", AltText = "First" },
                        new Image { Url = "https://example.com/2.jpg", AltText = "Second" },
                        new Image { Url = "https://example.com/3.jpg", AltText = "Third" }
                    },
                    ImageSize = ImageSize.Large
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

        var imageSet = deserializedCard.Body[0] as ImageSet;
        Assert.NotNull(imageSet);
        Assert.Equal("imageSet1", imageSet.Id);
        Assert.Equal(ImageSize.Large, imageSet.ImageSize);
        Assert.NotNull(imageSet.Images);
        Assert.Equal(3, imageSet.Images.Count);
        Assert.Equal("https://example.com/1.jpg", imageSet.Images[0].Url);
        Assert.Equal("First", imageSet.Images[0].AltText);
        Assert.Equal("https://example.com/2.jpg", imageSet.Images[1].Url);
        Assert.Equal("Second", imageSet.Images[1].AltText);
        Assert.Equal("https://example.com/3.jpg", imageSet.Images[2].Url);
        Assert.Equal("Third", imageSet.Images[2].AltText);
    }

    [Fact]
    public void ImageSet_WithEmptyImages_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ImageSet
                {
                    Images = new List<Image>()
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var imageSet = deserializedCard.Body![0] as ImageSet;
        Assert.NotNull(imageSet);
        Assert.NotNull(imageSet.Images);
        Assert.Empty(imageSet.Images);
    }

    [Fact]
    public void ImageSet_AllImageSizeValues_SerializeCorrectly()
    {
        // Test all ImageSize enum values
        var sizes = new[]
        {
            (ImageSize.Auto, "auto"),
            (ImageSize.Stretch, "stretch"),
            (ImageSize.Small, "small"),
            (ImageSize.Medium, "medium"),
            (ImageSize.Large, "large")
        };

        foreach (var (size, expected) in sizes)
        {
            // Arrange
            var card = new AdaptiveCard
            {
                Body = new List<AdaptiveElement>
                {
                    new ImageSet
                    {
                        Images = new List<Image> { new Image { Url = "test.jpg" } },
                        ImageSize = size
                    }
                }
            };

            // Act
            var json = card.ToJson();

            // Assert
            Assert.Contains($"\"imageSize\": \"{expected}\"", json);
        }
    }

    [Fact]
    public void Image_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ImageSet
                {
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Id = "img1",
                            Url = "https://example.com/image.jpg",
                            AltText = "Sample image",
                            Size = ImageSize.Medium,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Width = "100px",
                            Height = "100px"
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"img1\"", json);
        Assert.Contains("\"url\": \"https://example.com/image.jpg\"", json);
        Assert.Contains("\"altText\": \"Sample image\"", json);
        Assert.Contains("\"size\": \"medium\"", json);
        Assert.Contains("\"horizontalAlignment\": \"center\"", json);
        Assert.Contains("\"width\": \"100px\"", json);
        Assert.Contains("\"height\": \"100px\"", json);
    }
}
