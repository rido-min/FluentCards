using Xunit;

namespace FluentCards.Tests;

public class TextBlockBuilderTests
{
    [Fact]
    public void WithSelectAction_SetsOpenUrlSelectAction()
    {
        // Arrange & Act
        var textBlock = new TextBlockBuilder()
            .WithText("Click me")
            .WithSelectAction(a => a.OpenUrl("https://example.com"))
            .Build();

        // Assert
        Assert.NotNull(textBlock.SelectAction);
        var openUrl = Assert.IsType<OpenUrlAction>(textBlock.SelectAction);
        Assert.Equal("https://example.com", openUrl.Url);
    }

    [Fact]
    public void WithSpacing_SetsSpacing()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithSpacing(Spacing.Large)
            .Build();

        Assert.Equal(Spacing.Large, textBlock.Spacing);
    }

    [Fact]
    public void WithSeparator_SetsSeparatorTrue()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithSeparator()
            .Build();

        Assert.True(textBlock.Separator);
    }

    [Fact]
    public void WithSeparator_SetsSeparatorFalse()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithSeparator(false)
            .Build();

        Assert.False(textBlock.Separator);
    }

    [Fact]
    public void WithIsVisible_SetsIsVisible()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithIsVisible(false)
            .Build();

        Assert.False(textBlock.IsVisible);
    }

    [Fact]
    public void WithHeight_SetsHeight()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithHeight("stretch")
            .Build();

        Assert.Equal("stretch", textBlock.Height);
    }

    [Fact]
    public void WithFallback_SetsFallback()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithFallback("drop")
            .Build();

        Assert.Equal("drop", textBlock.Fallback);
    }

    [Fact]
    public void WithRequires_SetsRequiresDictionary()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithRequires("adaptiveCards", "1.5")
            .Build();

        Assert.NotNull(textBlock.Requires);
        Assert.Equal("1.5", textBlock.Requires["adaptiveCards"]);
    }

    [Fact]
    public void WithRequires_MultipleCalls_AddsAllEntries()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithRequires("adaptiveCards", "1.5")
            .WithRequires("myFeature", "1.0")
            .Build();

        Assert.NotNull(textBlock.Requires);
        Assert.Equal(2, textBlock.Requires.Count);
        Assert.Equal("1.5", textBlock.Requires["adaptiveCards"]);
        Assert.Equal("1.0", textBlock.Requires["myFeature"]);
    }

    [Fact]
    public void WithRtl_SetsRtlTrue()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithRtl()
            .Build();

        Assert.True(textBlock.Rtl);
    }

    [Fact]
    public void WithRtl_SetsRtlFalse()
    {
        var textBlock = new TextBlockBuilder()
            .WithText("Test")
            .WithRtl(false)
            .Build();

        Assert.False(textBlock.Rtl);
    }

    [Fact]
    public void FluentChaining_AllProperties_WorkCorrectly()
    {
        var textBlock = new TextBlockBuilder()
            .WithId("tb1")
            .WithText("Hello")
            .WithSize(TextSize.Large)
            .WithWeight(TextWeight.Bolder)
            .WithColor(TextColor.Accent)
            .WithWrap(true)
            .WithSpacing(Spacing.Medium)
            .WithSeparator()
            .WithIsVisible(true)
            .WithHeight("auto")
            .WithRtl()
            .WithSelectAction(a => a.OpenUrl("https://example.com"))
            .Build();

        Assert.Equal("tb1", textBlock.Id);
        Assert.Equal("Hello", textBlock.Text);
        Assert.Equal(TextSize.Large, textBlock.Size);
        Assert.Equal(TextWeight.Bolder, textBlock.Weight);
        Assert.Equal(TextColor.Accent, textBlock.Color);
        Assert.True(textBlock.Wrap);
        Assert.Equal(Spacing.Medium, textBlock.Spacing);
        Assert.True(textBlock.Separator);
        Assert.True(textBlock.IsVisible);
        Assert.Equal("auto", textBlock.Height);
        Assert.True(textBlock.Rtl);
        Assert.NotNull(textBlock.SelectAction);
    }
}

public class ActionBuilderThrowTests
{
    [Fact]
    public void WithId_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithId("id1"));
    }

    [Fact]
    public void WithTitle_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithTitle("title"));
    }

    [Fact]
    public void WithIconUrl_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithIconUrl("https://example.com/icon.png"));
    }

    [Fact]
    public void WithStyle_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithStyle(ActionStyle.Positive));
    }

    [Fact]
    public void WithIsEnabled_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithIsEnabled(false));
    }

    [Fact]
    public void WithTooltip_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithTooltip("tooltip"));
    }

    [Fact]
    public void WithFallback_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithFallback("drop"));
    }

    [Fact]
    public void WithMode_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithMode(ActionMode.Secondary));
    }

    [Fact]
    public void WithRequires_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithRequires("adaptiveCards", "1.5"));
    }

    [Fact]
    public void WithData_JsonElement_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        using var doc = System.Text.Json.JsonDocument.Parse("{}");
        Assert.Throws<InvalidOperationException>(() => builder.WithData(doc.RootElement));
    }

    [Fact]
    public void WithData_String_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithData("{}"));
    }

    [Fact]
    public void WithAssociatedInputs_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithAssociatedInputs(AssociatedInputs.Auto));
    }

    [Fact]
    public void WithVerb_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.WithVerb("doStuff"));
    }

    [Fact]
    public void WithId_AfterTypeSet_DoesNotThrow()
    {
        var action = new ActionBuilder()
            .OpenUrl("https://example.com")
            .WithId("action1")
            .Build();

        Assert.Equal("action1", action.Id);
    }

    [Fact]
    public void Build_BeforeTypeSet_ThrowsInvalidOperationException()
    {
        var builder = new ActionBuilder();
        Assert.Throws<InvalidOperationException>(() => builder.Build());
    }
}

public class TextBlockValidationTests
{
    [Fact]
    public void Validate_TextBlockWithShowCardSelectAction_ReturnsError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Clickable text")
                .WithSelectAction(a => a.ShowCard()))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        var error = Assert.Single(issues, i => i.Code == "INVALID_SELECT_ACTION");
        Assert.Equal(ValidationSeverity.Error, error.Severity);
        Assert.Equal("body[0].selectAction", error.Path);
    }

    [Fact]
    public void Validate_TextBlockWithOpenUrlSelectAction_NoSelectActionError()
    {
        // Arrange
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Clickable text")
                .WithSelectAction(a => a.OpenUrl("https://example.com")))
            .Build();

        // Act
        var issues = AdaptiveCardValidator.Validate(card);

        // Assert
        Assert.DoesNotContain(issues, i => i.Code == "INVALID_SELECT_ACTION");
    }
}
