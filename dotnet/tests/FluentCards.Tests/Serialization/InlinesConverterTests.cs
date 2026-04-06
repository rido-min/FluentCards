using System.Text.Json;
using FluentCards.Serialization;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class InlinesConverterTests
{
    [Fact]
    public void Deserialize_ArrayWithOnlyStrings_ReturnsStringList()
    {
        // Arrange
        var json = @"{
            ""type"": ""RichTextBlock"",
            ""inlines"": [""Plain text "", ""more text"", "" and more""]
        }";

        // Act
        var block = JsonSerializer.Deserialize<RichTextBlock>(json, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.NotNull(block);
        Assert.NotNull(block.Inlines);
        Assert.Equal(3, block.Inlines.Count);
        Assert.All(block.Inlines, item => Assert.IsType<string>(item));
        Assert.Equal("Plain text ", block.Inlines[0]);
        Assert.Equal("more text", block.Inlines[1]);
        Assert.Equal(" and more", block.Inlines[2]);
    }

    [Fact]
    public void Deserialize_ArrayWithOnlyTextRuns_ReturnsTextRunList()
    {
        // Arrange
        var json = @"{
            ""type"": ""RichTextBlock"",
            ""inlines"": [
                {""type"": ""TextRun"", ""text"": ""Bold"", ""weight"": ""bolder""},
                {""type"": ""TextRun"", ""text"": ""Italic"", ""italic"": true}
            ]
        }";

        // Act
        var block = JsonSerializer.Deserialize<RichTextBlock>(json, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.NotNull(block);
        Assert.NotNull(block.Inlines);
        Assert.Equal(2, block.Inlines.Count);
        Assert.All(block.Inlines, item => Assert.IsType<TextRun>(item));
        
        var run1 = block.Inlines[0] as TextRun;
        var run2 = block.Inlines[1] as TextRun;
        Assert.Equal("Bold", run1?.Text);
        Assert.Equal(TextWeight.Bolder, run1?.Weight);
        Assert.Equal("Italic", run2?.Text);
        Assert.True(run2?.Italic);
    }

    [Fact]
    public void Deserialize_MixedArray_ReturnsMixedList()
    {
        // Arrange
        var json = @"{
            ""type"": ""RichTextBlock"",
            ""inlines"": [
                ""Plain text "",
                {""type"": ""TextRun"", ""text"": ""bold"", ""weight"": ""bolder""},
                "" more plain text""
            ]
        }";

        // Act
        var block = JsonSerializer.Deserialize<RichTextBlock>(json, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.NotNull(block);
        Assert.NotNull(block.Inlines);
        Assert.Equal(3, block.Inlines.Count);
        Assert.IsType<string>(block.Inlines[0]);
        Assert.IsType<TextRun>(block.Inlines[1]);
        Assert.IsType<string>(block.Inlines[2]);
        
        Assert.Equal("Plain text ", block.Inlines[0]);
        var run = block.Inlines[1] as TextRun;
        Assert.Equal("bold", run?.Text);
        Assert.Equal(" more plain text", block.Inlines[2]);
    }

    [Fact]
    public void Serialize_MixedArray_PreservesTypes()
    {
        // Arrange
        var block = new RichTextBlock
        {
            Inlines = new List<object>
            {
                "Plain ",
                new TextRun { Text = "Bold", Weight = TextWeight.Bolder },
                " text"
            }
        };

        // Act
        var json = JsonSerializer.Serialize(block, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.Contains("\"Plain \"", json);
        Assert.Contains("\"text\":\"Bold\"", json.Replace(" ", ""));
        Assert.Contains("\"weight\":\"bolder\"", json.Replace(" ", ""));
        Assert.Contains("\" text\"", json);
    }

    [Fact]
    public void Serialize_TextRunFormattingProperties_Preserved()
    {
        // Arrange
        var block = new RichTextBlock
        {
            Inlines = new List<object>
            {
                new TextRun
                {
                    Text = "Formatted",
                    Size = TextSize.Large,
                    Color = TextColor.Accent,
                    Weight = TextWeight.Bolder,
                    Italic = true,
                    Underline = true,
                    Strikethrough = true
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(block, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.Contains("\"size\":\"large\"", json.Replace(" ", ""));
        Assert.Contains("\"color\":\"accent\"", json.Replace(" ", ""));
        Assert.Contains("\"weight\":\"bolder\"", json.Replace(" ", ""));
        Assert.Contains("\"italic\":true", json.Replace(" ", ""));
        Assert.Contains("\"underline\":true", json.Replace(" ", ""));
        Assert.Contains("\"strikethrough\":true", json.Replace(" ", ""));
    }

    [Fact]
    public void Serialize_EmptyArray_SerializesCorrectly()
    {
        // Arrange
        var block = new RichTextBlock
        {
            Inlines = new List<object>()
        };

        // Act
        var json = JsonSerializer.Serialize(block, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.Contains("\"inlines\":[]", json.Replace(" ", ""));
    }

    [Fact]
    public void Serialize_NullValue_OmittedFromOutput()
    {
        // Arrange
        var block = new RichTextBlock
        {
            Inlines = null
        };

        // Act
        var json = JsonSerializer.Serialize(block, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        // Null values are omitted by default
        Assert.DoesNotContain("inlines", json);
    }

    [Fact]
    public void Roundtrip_MixedInlines_PreservesStructure()
    {
        // Arrange
        var original = new RichTextBlock
        {
            Inlines = new List<object>
            {
                "Start ",
                new TextRun { Text = "middle", Weight = TextWeight.Bolder, Color = TextColor.Accent },
                " end"
            }
        };

        // Act
        var json = JsonSerializer.Serialize(original, FluentCardsJsonContext.Default.RichTextBlock);
        var deserialized = JsonSerializer.Deserialize<RichTextBlock>(json, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Inlines);
        Assert.Equal(3, deserialized.Inlines.Count);
        Assert.Equal("Start ", deserialized.Inlines[0]);
        
        var run = deserialized.Inlines[1] as TextRun;
        Assert.NotNull(run);
        Assert.Equal("middle", run.Text);
        Assert.Equal(TextWeight.Bolder, run.Weight);
        Assert.Equal(TextColor.Accent, run.Color);
        
        Assert.Equal(" end", deserialized.Inlines[2]);
    }

    [Fact]
    public void Deserialize_TextRunWithSelectAction_PreservesAction()
    {
        // Arrange
        var json = @"{
            ""type"": ""RichTextBlock"",
            ""inlines"": [
                {
                    ""type"": ""TextRun"",
                    ""text"": ""Click me"",
                    ""selectAction"": {
                        ""type"": ""Action.OpenUrl"",
                        ""url"": ""https://example.com""
                    }
                }
            ]
        }";

        // Act
        var block = JsonSerializer.Deserialize<RichTextBlock>(json, FluentCardsJsonContext.Default.RichTextBlock);

        // Assert
        Assert.NotNull(block);
        Assert.NotNull(block.Inlines);
        Assert.Single(block.Inlines);
        
        var run = block.Inlines[0] as TextRun;
        Assert.NotNull(run);
        Assert.Equal("Click me", run.Text);
        Assert.NotNull(run.SelectAction);
        Assert.IsType<OpenUrlAction>(run.SelectAction);
        Assert.Equal("https://example.com", ((OpenUrlAction)run.SelectAction).Url);
    }
}
