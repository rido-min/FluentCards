using Xunit;

namespace FluentCards.Tests;

public class ImageBuilderTests
{
    [Fact]
    public void ImageBuilder_CreatesImage()
    {
        // Arrange & Act
        var image = new ImageBuilder()
            .WithUrl("https://example.com/image.jpg")
            .Build();

        // Assert
        Assert.NotNull(image);
        Assert.Equal("https://example.com/image.jpg", image.Url);
    }

    [Fact]
    public void ImageBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var image = new ImageBuilder()
            .WithUrl("https://example.com/image.jpg")
            .WithAltText("Sample image")
            .WithSize(ImageSize.Large)
            .WithStyle(ImageStyle.Person)
            .WithWidth("100px")
            .WithHeight("100px")
            .WithHorizontalAlignment(HorizontalAlignment.Center)
            .WithBackgroundColor("#FF0000")
            .Build();

        // Assert
        Assert.Equal("https://example.com/image.jpg", image.Url);
        Assert.Equal("Sample image", image.AltText);
        Assert.Equal(ImageSize.Large, image.Size);
        Assert.Equal(ImageStyle.Person, image.Style);
        Assert.Equal("100px", image.Width);
        Assert.Equal("100px", image.Height);
        Assert.Equal(HorizontalAlignment.Center, image.HorizontalAlignment);
        Assert.Equal("#FF0000", image.BackgroundColor);
    }

    [Fact]
    public void ImageBuilder_WithSelectAction_SetsAction()
    {
        // Arrange & Act
        var image = new ImageBuilder()
            .WithUrl("https://example.com/image.jpg")
            .WithSelectAction(a => a.OpenUrl("https://example.com"))
            .Build();

        // Assert
        Assert.NotNull(image.SelectAction);
        var action = image.SelectAction as OpenUrlAction;
        Assert.NotNull(action);
        Assert.Equal("https://example.com", action.Url);
    }

    [Fact]
    public void AdaptiveCardBuilder_AddImage_AddsImageToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddImage(i => i
                .WithUrl("https://example.com/image.jpg")
                .WithAltText("Test"))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Single(card.Body);
        var image = card.Body[0] as Image;
        Assert.NotNull(image);
        Assert.Equal("https://example.com/image.jpg", image.Url);
        Assert.Equal("Test", image.AltText);
    }
}
