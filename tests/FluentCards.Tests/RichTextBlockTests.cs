using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class RichTextBlockTests
{
    [Fact]
    public void RichTextBlock_WithTextRunObjects_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        new TextRun { Text = "Bold text", Weight = TextWeight.Bolder },
                        new TextRun { Text = " normal text " },
                        new TextRun { Text = "colored text", Color = TextColor.Accent }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"RichTextBlock\"", json);
        Assert.Contains("\"inlines\":", json);
        Assert.Contains("\"type\": \"TextRun\"", json);
        Assert.Contains("Bold text", json);
        Assert.Contains("\"weight\": \"bolder\"", json);
        Assert.Contains("\"color\": \"accent\"", json);
    }

    [Fact]
    public void RichTextBlock_WithPlainStringInlines_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        "Plain string 1",
                        "Plain string 2"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"RichTextBlock\"", json);
        Assert.Contains("\"inlines\":", json);
        Assert.Contains("Plain string 1", json);
        Assert.Contains("Plain string 2", json);
    }

    [Fact]
    public void RichTextBlock_WithMixedInlines_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        "Start with plain text, ",
                        new TextRun { Text = "then formatted", Weight = TextWeight.Bolder, Italic = true },
                        ", and back to plain"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("Start with plain text, ", json);
        Assert.Contains("then formatted", json);
        Assert.Contains("\"weight\": \"bolder\"", json);
        Assert.Contains("\"italic\": true", json);
        Assert.Contains(", and back to plain", json);
    }

    [Fact]
    public void TextRun_WithAllFormattingOptions_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        new TextRun
                        {
                            Text = "Formatted text",
                            Size = TextSize.Large,
                            Weight = TextWeight.Bolder,
                            Color = TextColor.Good,
                            IsSubtle = true,
                            Italic = true,
                            Strikethrough = true,
                            Underline = true,
                            Highlight = true
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"text\": \"Formatted text\"", json);
        Assert.Contains("\"size\": \"large\"", json);
        Assert.Contains("\"weight\": \"bolder\"", json);
        Assert.Contains("\"color\": \"good\"", json);
        Assert.Contains("\"isSubtle\": true", json);
        Assert.Contains("\"italic\": true", json);
        Assert.Contains("\"strikethrough\": true", json);
        Assert.Contains("\"underline\": true", json);
        Assert.Contains("\"highlight\": true", json);
    }

    [Fact]
    public void TextRun_WithSelectAction_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        new TextRun
                        {
                            Text = "Clickable text",
                            SelectAction = new OpenUrlAction
                            {
                                Url = "https://example.com"
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"text\": \"Clickable text\"", json);
        Assert.Contains("\"selectAction\":", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
        Assert.Contains("\"url\": \"https://example.com\"", json);
    }

    [Fact]
    public void RichTextBlock_RoundtripSerialization_PreservesInlineFormatting()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Id = "richText1",
                    Inlines = new List<object>
                    {
                        "Plain text ",
                        new TextRun { Text = "bold", Weight = TextWeight.Bolder },
                        " more plain"
                    },
                    HorizontalAlignment = HorizontalAlignment.Center
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

        var richTextBlock = deserializedCard.Body[0] as RichTextBlock;
        Assert.NotNull(richTextBlock);
        Assert.Equal("richText1", richTextBlock.Id);
        Assert.Equal(HorizontalAlignment.Center, richTextBlock.HorizontalAlignment);
        Assert.NotNull(richTextBlock.Inlines);
        Assert.Equal(3, richTextBlock.Inlines.Count);
    }

    [Fact]
    public void RichTextBlock_WithHorizontalAlignment_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object> { "Test" },
                    HorizontalAlignment = HorizontalAlignment.Right
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"horizontalAlignment\": \"right\"", json);
    }

    [Fact]
    public void TextRun_DefaultValues_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        new TextRun { Text = "Simple text" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"text\": \"Simple text\"", json);
        Assert.DoesNotContain("\"size\":", json);
        Assert.DoesNotContain("\"weight\":", json);
        Assert.DoesNotContain("\"color\":", json);
        Assert.DoesNotContain("\"isSubtle\":", json);
        Assert.DoesNotContain("\"italic\":", json);
    }
}
