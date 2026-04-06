using Xunit;

namespace FluentCards.Tests;

public class ContainerBuilderTests
{
    [Fact]
    public void ContainerBuilder_CreatesContainer()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .WithId("container1")
            .Build();

        // Assert
        Assert.NotNull(container);
        Assert.Equal("container1", container.Id);
        Assert.NotNull(container.Items);
    }

    [Fact]
    public void ContainerBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .WithId("container1")
            .WithStyle(ContainerStyle.Emphasis)
            .WithVerticalContentAlignment(VerticalAlignment.Center)
            .WithBleed(true)
            .WithMinHeight("100px")
            .Build();

        // Assert
        Assert.Equal("container1", container.Id);
        Assert.Equal(ContainerStyle.Emphasis, container.Style);
        Assert.Equal(VerticalAlignment.Center, container.VerticalContentAlignment);
        Assert.True(container.Bleed);
        Assert.Equal("100px", container.MinHeight);
    }

    [Fact]
    public void ContainerBuilder_AddTextBlock_AddsToItems()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .AddTextBlock(tb => tb.WithText("Hello"))
            .AddTextBlock(tb => tb.WithText("World"))
            .Build();

        // Assert
        Assert.NotNull(container.Items);
        Assert.Equal(2, container.Items.Count);
        Assert.Equal("Hello", ((TextBlock)container.Items[0]).Text);
        Assert.Equal("World", ((TextBlock)container.Items[1]).Text);
    }

    [Fact]
    public void ContainerBuilder_AddImage_AddsToItems()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .AddImage(i => i.WithUrl("https://example.com/image.jpg"))
            .Build();

        // Assert
        Assert.NotNull(container.Items);
        Assert.Single(container.Items);
        var image = container.Items[0] as Image;
        Assert.NotNull(image);
        Assert.Equal("https://example.com/image.jpg", image.Url);
    }

    [Fact]
    public void ContainerBuilder_NestedContainers_Work()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .AddContainer(c => c
                .WithId("nested")
                .AddTextBlock(tb => tb.WithText("Nested text")))
            .Build();

        // Assert
        Assert.NotNull(container.Items);
        Assert.Single(container.Items);
        var nested = container.Items[0] as Container;
        Assert.NotNull(nested);
        Assert.Equal("nested", nested.Id);
        Assert.Single(nested.Items!);
    }

    [Fact]
    public void ContainerBuilder_WithBackgroundImage_SetsBackgroundImage()
    {
        // Arrange & Act
        var container = new ContainerBuilder()
            .WithBackgroundImage(bg => bg
                .WithUrl("https://example.com/bg.jpg")
                .WithFillMode(BackgroundImageFillMode.Cover))
            .Build();

        // Assert
        Assert.NotNull(container.BackgroundImage);
        Assert.Equal("https://example.com/bg.jpg", container.BackgroundImage.Url);
        Assert.Equal(BackgroundImageFillMode.Cover, container.BackgroundImage.FillMode);
    }

    [Fact]
    public void AdaptiveCardBuilder_AddContainer_AddsContainerToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb.WithText("In container")))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Single(card.Body);
        var container = card.Body[0] as Container;
        Assert.NotNull(container);
        Assert.Equal(ContainerStyle.Emphasis, container.Style);
        Assert.Single(container.Items!);
    }
}
