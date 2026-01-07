using Xunit;

namespace FluentCards.Tests;

public class BuilderTests
{
    [Fact]
    public void Builder_CreatesValidCard()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .Build();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("AdaptiveCard", card.Type);
        Assert.Equal("1.5", card.Version);
    }

    [Fact]
    public void Builder_FluentChaining_WorksCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithSchema("https://example.com/schema.json")
            .Build();

        // Assert
        Assert.Equal("1.6", card.Version);
        Assert.Equal("https://example.com/schema.json", card.Schema);
    }

    [Fact]
    public void Builder_WithTextBlock_AddsTextBlockToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb
                .WithText("Hello, World!"))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Single(card.Body);
        var textBlock = card.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Hello, World!", textBlock.Text);
    }

    [Fact]
    public void Builder_WithMultipleElements_AddsAllToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("First"))
            .AddTextBlock(tb => tb.WithText("Second"))
            .AddTextBlock(tb => tb.WithText("Third"))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Equal(3, card.Body.Count);
        Assert.Equal("First", ((TextBlock)card.Body[0]).Text);
        Assert.Equal("Second", ((TextBlock)card.Body[1]).Text);
        Assert.Equal("Third", ((TextBlock)card.Body[2]).Text);
    }

    [Fact]
    public void TextBlockBuilder_AllProperties_SetCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb
                .WithId("tb1")
                .WithText("Test Text")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Accent)
                .WithWrap(true)
                .WithMaxLines(5)
                .WithHorizontalAlignment(HorizontalAlignment.Right))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        var textBlock = card.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("tb1", textBlock.Id);
        Assert.Equal("Test Text", textBlock.Text);
        Assert.Equal(TextSize.Large, textBlock.Size);
        Assert.Equal(TextWeight.Bolder, textBlock.Weight);
        Assert.Equal(TextColor.Accent, textBlock.Color);
        Assert.True(textBlock.Wrap);
        Assert.Equal(5, textBlock.MaxLines);
        Assert.Equal(HorizontalAlignment.Right, textBlock.HorizontalAlignment);
    }

    [Fact]
    public void ActionBuilder_OpenUrl_CreatesOpenUrlAction()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .OpenUrl("https://example.com")
                .WithTitle("Visit Website"))
            .Build();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);
        var action = card.Actions[0] as OpenUrlAction;
        Assert.NotNull(action);
        Assert.Equal("https://example.com", action.Url);
        Assert.Equal("Visit Website", action.Title);
    }

    [Fact]
    public void TextBlockBuilder_Build_ReturnsTextBlock()
    {
        // Arrange
        var builder = new TextBlockBuilder();

        // Act
        var textBlock = builder
            .WithText("Sample")
            .WithSize(TextSize.Medium)
            .Build();

        // Assert
        Assert.NotNull(textBlock);
        Assert.Equal("Sample", textBlock.Text);
        Assert.Equal(TextSize.Medium, textBlock.Size);
    }
}
