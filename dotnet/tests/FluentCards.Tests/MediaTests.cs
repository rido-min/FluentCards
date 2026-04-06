using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class MediaTests
{
    [Fact]
    public void Media_WithSingleSource_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Sources = new List<MediaSource>
                    {
                        new MediaSource { MimeType = "video/mp4", Url = "https://example.com/video.mp4" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Media\"", json);
        Assert.Contains("\"sources\":", json);
        Assert.Contains("\"mimeType\": \"video/mp4\"", json);
        Assert.Contains("\"url\": \"https://example.com/video.mp4\"", json);
    }

    [Fact]
    public void Media_WithMultipleSources_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Sources = new List<MediaSource>
                    {
                        new MediaSource { MimeType = "video/mp4", Url = "https://example.com/video.mp4" },
                        new MediaSource { MimeType = "video/webm", Url = "https://example.com/video.webm" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"mimeType\": \"video/mp4\"", json);
        Assert.Contains("\"mimeType\": \"video/webm\"", json);
        Assert.Contains("https://example.com/video.mp4", json);
        Assert.Contains("https://example.com/video.webm", json);
    }

    [Fact]
    public void Media_WithPosterImage_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Sources = new List<MediaSource>
                    {
                        new MediaSource { MimeType = "video/mp4", Url = "https://example.com/video.mp4" }
                    },
                    Poster = "https://example.com/poster.jpg",
                    AltText = "Sample video"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"poster\": \"https://example.com/poster.jpg\"", json);
        Assert.Contains("\"altText\": \"Sample video\"", json);
    }

    [Fact]
    public void Media_RoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Id = "media1",
                    Sources = new List<MediaSource>
                    {
                        new MediaSource { MimeType = "audio/mp3", Url = "https://example.com/audio.mp3" }
                    },
                    Poster = "https://example.com/poster.png",
                    AltText = "Audio description"
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

        var media = deserializedCard.Body[0] as Media;
        Assert.NotNull(media);
        Assert.Equal("media1", media.Id);
        Assert.NotNull(media.Sources);
        Assert.Single(media.Sources);
        Assert.Equal("audio/mp3", media.Sources[0].MimeType);
        Assert.Equal("https://example.com/audio.mp3", media.Sources[0].Url);
        Assert.Equal("https://example.com/poster.png", media.Poster);
        Assert.Equal("Audio description", media.AltText);
    }

    [Fact]
    public void Media_WithoutOptionalProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Media
                {
                    Sources = new List<MediaSource>
                    {
                        new MediaSource { MimeType = "video/mp4", Url = "https://example.com/video.mp4" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Media\"", json);
        Assert.DoesNotContain("\"poster\":", json);
        Assert.DoesNotContain("\"altText\":", json);
    }
}
