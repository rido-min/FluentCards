using Xunit;

namespace FluentCards.Tests;

public class RichContentBuilderTests
{
    [Fact]
    public void FactSetBuilder_CreatesFactSet()
    {
        // Arrange & Act
        var factSet = new FactSetBuilder()
            .AddFact("Title", "Value")
            .AddFact("Another", "Fact")
            .Build();

        // Assert
        Assert.NotNull(factSet);
        Assert.NotNull(factSet.Facts);
        Assert.Equal(2, factSet.Facts.Count);
        Assert.Equal("Title", factSet.Facts[0].Title);
        Assert.Equal("Value", factSet.Facts[0].Value);
    }

    [Fact]
    public void TextRunBuilder_CreatesTextRun()
    {
        // Arrange & Act
        var textRun = new TextRunBuilder()
            .WithText("Bold text")
            .WithWeight(TextWeight.Bolder)
            .WithSize(TextSize.Large)
            .WithColor(TextColor.Accent)
            .Build();

        // Assert
        Assert.NotNull(textRun);
        Assert.Equal("Bold text", textRun.Text);
        Assert.Equal(TextWeight.Bolder, textRun.Weight);
        Assert.Equal(TextSize.Large, textRun.Size);
        Assert.Equal(TextColor.Accent, textRun.Color);
    }

    [Fact]
    public void TextRunBuilder_FormattingProperties_SetCorrectly()
    {
        // Arrange & Act
        var textRun = new TextRunBuilder()
            .WithText("Formatted")
            .IsSubtle(true)
            .IsItalic(true)
            .IsStrikethrough(true)
            .IsUnderline(true)
            .IsHighlight(true)
            .Build();

        // Assert
        Assert.True(textRun.IsSubtle);
        Assert.True(textRun.Italic);
        Assert.True(textRun.Strikethrough);
        Assert.True(textRun.Underline);
        Assert.True(textRun.Highlight);
    }

    [Fact]
    public void RichTextBlockBuilder_CreatesRichTextBlock()
    {
        // Arrange & Act
        var richText = new RichTextBlockBuilder()
            .WithId("rich1")
            .AddText("Plain text ")
            .AddTextRun(tr => tr
                .WithText("bold text")
                .WithWeight(TextWeight.Bolder))
            .AddText(" more plain")
            .WithHorizontalAlignment(HorizontalAlignment.Center)
            .Build();

        // Assert
        Assert.NotNull(richText);
        Assert.Equal("rich1", richText.Id);
        Assert.NotNull(richText.Inlines);
        Assert.Equal(3, richText.Inlines.Count);
        Assert.Equal("Plain text ", richText.Inlines[0]);
        Assert.IsType<TextRun>(richText.Inlines[1]);
        Assert.Equal(HorizontalAlignment.Center, richText.HorizontalAlignment);
    }

    [Fact]
    public void ActionSetBuilder_CreatesActionSet()
    {
        // Arrange & Act
        var actionSet = new ActionSetBuilder()
            .WithId("actions1")
            .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Visit"))
            .AddAction(a => a.Submit("Submit"))
            .Build();

        // Assert
        Assert.NotNull(actionSet);
        Assert.Equal("actions1", actionSet.Id);
        Assert.NotNull(actionSet.Actions);
        Assert.Equal(2, actionSet.Actions.Count);
        Assert.IsType<OpenUrlAction>(actionSet.Actions[0]);
        Assert.IsType<SubmitAction>(actionSet.Actions[1]);
    }

    [Fact]
    public void MediaBuilder_CreatesMedia()
    {
        // Arrange & Act
        var media = new MediaBuilder()
            .WithId("video1")
            .WithPoster("https://example.com/poster.jpg")
            .WithAltText("Video description")
            .AddSource("https://example.com/video.mp4", "video/mp4")
            .AddSource("https://example.com/video.webm", "video/webm")
            .Build();

        // Assert
        Assert.NotNull(media);
        Assert.Equal("video1", media.Id);
        Assert.Equal("https://example.com/poster.jpg", media.Poster);
        Assert.Equal("Video description", media.AltText);
        Assert.NotNull(media.Sources);
        Assert.Equal(2, media.Sources.Count);
        Assert.Equal("video/mp4", media.Sources[0].MimeType);
    }

    [Fact]
    public void ImageSetBuilder_CreatesImageSet()
    {
        // Arrange & Act
        var imageSet = new ImageSetBuilder()
            .WithId("images1")
            .WithImageSize(ImageSize.Medium)
            .AddImage(i => i.WithUrl("https://example.com/1.jpg"))
            .AddImage(i => i.WithUrl("https://example.com/2.jpg"))
            .AddImage(i => i.WithUrl("https://example.com/3.jpg"))
            .Build();

        // Assert
        Assert.NotNull(imageSet);
        Assert.Equal("images1", imageSet.Id);
        Assert.Equal(ImageSize.Medium, imageSet.ImageSize);
        Assert.NotNull(imageSet.Images);
        Assert.Equal(3, imageSet.Images.Count);
    }

    [Fact]
    public void TableBuilder_CreatesTable()
    {
        // Arrange & Act
        var table = new TableBuilder()
            .WithId("table1")
            .WithFirstRowAsHeader(true)
            .WithShowGridLines(true)
            .WithGridStyle(ContainerStyle.Emphasis)
            .WithHorizontalCellContentAlignment(HorizontalAlignment.Center)
            .WithVerticalCellContentAlignment(VerticalAlignment.Center)
            .Build();

        // Assert
        Assert.NotNull(table);
        Assert.Equal("table1", table.Id);
        Assert.True(table.FirstRowAsHeader);
        Assert.True(table.ShowGridLines);
        Assert.Equal(ContainerStyle.Emphasis, table.GridStyle);
        Assert.Equal(HorizontalAlignment.Center, table.HorizontalCellContentAlignment);
        Assert.Equal(VerticalAlignment.Center, table.VerticalCellContentAlignment);
    }

    [Fact]
    public void AdaptiveCardBuilder_AddRichContent_AddsToBody()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddFactSet(fs => fs.AddFact("Key", "Value"))
            .AddRichTextBlock(rtb => rtb.AddText("Text"))
            .AddActionSet(actionSet => actionSet.AddAction(a => a.Submit("Submit")))
            .AddMedia(m => m.AddSource("url", "type"))
            .AddImageSet(imageSet => imageSet.AddImage(i => i.WithUrl("url")))
            .AddTable(t => t.WithId("table"))
            .Build();

        // Assert
        Assert.NotNull(card.Body);
        Assert.Equal(6, card.Body.Count);
        Assert.IsType<FactSet>(card.Body[0]);
        Assert.IsType<RichTextBlock>(card.Body[1]);
        Assert.IsType<ActionSet>(card.Body[2]);
        Assert.IsType<Media>(card.Body[3]);
        Assert.IsType<ImageSet>(card.Body[4]);
        Assert.IsType<Table>(card.Body[5]);
    }
}
