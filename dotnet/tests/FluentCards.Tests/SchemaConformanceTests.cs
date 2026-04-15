using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

/// <summary>
/// Tests that verify the library covers the entire Adaptive Cards 1.6.0 specification.
/// These tests serve as a regression safety net — if a property is removed or an enum 
/// value is missing, the test suite catches it.
/// </summary>
public class SchemaConformanceTests
{
    #region Element Type Coverage Tests

    [Fact]
    public void TextBlock_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb
                .WithText("test")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Accent)
                .WithIsSubtle(true)
                .WithWrap(true)
                .WithMaxLines(3)
                .WithFontType(FontType.Monospace)
                .WithHorizontalAlignment(HorizontalAlignment.Center)
                .WithStyle(TextBlockStyle.Heading)
                .WithId("tb1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Large)
                .WithHeight("stretch")
                .WithFallback("drop")
                .WithRequires("featureKey", "1.5")
                .WithRtl(true)
                .WithSelectAction(a => a.OpenUrl("https://example.com"))
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var textBlock = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("TextBlock", textBlock.GetProperty("type").GetString());
        Assert.True(textBlock.TryGetProperty("text", out _));
        Assert.True(textBlock.TryGetProperty("size", out _));
        Assert.True(textBlock.TryGetProperty("weight", out _));
        Assert.True(textBlock.TryGetProperty("color", out _));
        Assert.True(textBlock.TryGetProperty("isSubtle", out _));
        Assert.True(textBlock.TryGetProperty("wrap", out _));
        Assert.True(textBlock.TryGetProperty("maxLines", out _));
        Assert.True(textBlock.TryGetProperty("fontType", out _));
        Assert.True(textBlock.TryGetProperty("horizontalAlignment", out _));
        Assert.True(textBlock.TryGetProperty("style", out _));
        Assert.True(textBlock.TryGetProperty("id", out _));
        Assert.True(textBlock.TryGetProperty("isVisible", out _));
        Assert.True(textBlock.TryGetProperty("separator", out _));
        Assert.True(textBlock.TryGetProperty("spacing", out _));
        Assert.True(textBlock.TryGetProperty("height", out _));
        Assert.True(textBlock.TryGetProperty("fallback", out _));
        Assert.True(textBlock.TryGetProperty("requires", out _));
        Assert.True(textBlock.TryGetProperty("rtl", out _));
        Assert.True(textBlock.TryGetProperty("selectAction", out _));
    }

    [Fact]
    public void Image_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddImage(img => img
                .WithUrl("https://example.com/image.png")
                .WithAltText("Alt text")
                .WithSize(ImageSize.Large)
                .WithStyle(ImageStyle.Person)
                .WithHorizontalAlignment(HorizontalAlignment.Center)
                .WithBackgroundColor("#FFFFFF")
                .WithWidth("100px")
                .WithHeight("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
                .WithFallback("drop")
                .WithRequires("featureKey", "1.0")
                .WithRtl(true)
                .WithSelectAction(a => a.OpenUrl("https://example.com"))
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var image = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Image", image.GetProperty("type").GetString());
        Assert.True(image.TryGetProperty("url", out _));
        Assert.True(image.TryGetProperty("altText", out _));
        Assert.True(image.TryGetProperty("size", out _));
        Assert.True(image.TryGetProperty("style", out _));
        Assert.True(image.TryGetProperty("horizontalAlignment", out _));
        Assert.True(image.TryGetProperty("backgroundColor", out _));
        Assert.True(image.TryGetProperty("width", out _));
        Assert.True(image.TryGetProperty("height", out _));
        Assert.True(image.TryGetProperty("isVisible", out _));
        Assert.True(image.TryGetProperty("separator", out _));
        Assert.True(image.TryGetProperty("spacing", out _));
        Assert.True(image.TryGetProperty("fallback", out _));
        Assert.True(image.TryGetProperty("requires", out _));
        Assert.True(image.TryGetProperty("rtl", out _));
        Assert.True(image.TryGetProperty("selectAction", out _));
    }

    [Fact]
    public void Media_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddMedia(m => m
                .AddSource("https://example.com/video.mp4", "video/mp4")
                .WithPoster("https://example.com/poster.png")
                .WithAltText("Video description")
                .AddCaptionSource("https://example.com/captions.vtt", "vtt", "English")
                .WithId("media1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var media = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Media", media.GetProperty("type").GetString());
        Assert.True(media.TryGetProperty("sources", out _));
        Assert.True(media.TryGetProperty("poster", out _));
        Assert.True(media.TryGetProperty("altText", out _));
        Assert.True(media.TryGetProperty("captionSources", out _));
        Assert.True(media.TryGetProperty("id", out _));
        Assert.True(media.TryGetProperty("isVisible", out _));
        Assert.True(media.TryGetProperty("separator", out _));
        Assert.True(media.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void ImageSet_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddImageSet(imgSet => imgSet
                .AddImage(img => img.WithUrl("https://example.com/1.png"))
                .WithImageSize(ImageSize.Medium)
                .WithId("imgset1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Large)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var imageSet = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("ImageSet", imageSet.GetProperty("type").GetString());
        Assert.True(imageSet.TryGetProperty("images", out _));
        Assert.True(imageSet.TryGetProperty("imageSize", out _));
        Assert.True(imageSet.TryGetProperty("id", out _));
        Assert.True(imageSet.TryGetProperty("isVisible", out _));
        Assert.True(imageSet.TryGetProperty("separator", out _));
        Assert.True(imageSet.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void RichTextBlock_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr.WithText("Bold text").WithWeight(TextWeight.Bolder))
                .AddText("plain text")
                .WithHorizontalAlignment(HorizontalAlignment.Right)
                .WithId("rtb1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var richTextBlock = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("RichTextBlock", richTextBlock.GetProperty("type").GetString());
        Assert.True(richTextBlock.TryGetProperty("inlines", out _));
        Assert.True(richTextBlock.TryGetProperty("horizontalAlignment", out _));
        Assert.True(richTextBlock.TryGetProperty("id", out _));
        Assert.True(richTextBlock.TryGetProperty("isVisible", out _));
        Assert.True(richTextBlock.TryGetProperty("separator", out _));
        Assert.True(richTextBlock.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void FactSet_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddFactSet(fs => fs
                .AddFact("Key1", "Value1")
                .AddFact("Key2", "Value2")
                .WithId("fs1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Small)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var factSet = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("FactSet", factSet.GetProperty("type").GetString());
        Assert.True(factSet.TryGetProperty("facts", out _));
        Assert.True(factSet.TryGetProperty("id", out _));
        Assert.True(factSet.TryGetProperty("isVisible", out _));
        Assert.True(factSet.TryGetProperty("separator", out _));
        Assert.True(factSet.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void ActionSet_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddActionSet(acts => acts
                .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Link"))
                .WithId("acts1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Large)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var actionSet = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("ActionSet", actionSet.GetProperty("type").GetString());
        Assert.True(actionSet.TryGetProperty("actions", out _));
        Assert.True(actionSet.TryGetProperty("id", out _));
        Assert.True(actionSet.TryGetProperty("isVisible", out _));
        Assert.True(actionSet.TryGetProperty("separator", out _));
        Assert.True(actionSet.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void Container_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddContainer(c => c
                .AddTextBlock(tb => tb.WithText("Inside container"))
                .WithStyle(ContainerStyle.Emphasis)
                .WithVerticalContentAlignment(VerticalAlignment.Center)
                .WithBleed(true)
                .WithMinHeight("100px")
                .WithBackgroundImage(bg => bg.WithUrl("https://example.com/bg.png"))
                .WithSelectAction(a => a.OpenUrl("https://example.com"))
                .WithId("cont1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.ExtraLarge)
                .WithRtl(true)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var container = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Container", container.GetProperty("type").GetString());
        Assert.True(container.TryGetProperty("items", out _));
        Assert.True(container.TryGetProperty("style", out _));
        Assert.True(container.TryGetProperty("verticalContentAlignment", out _));
        Assert.True(container.TryGetProperty("bleed", out _));
        Assert.True(container.TryGetProperty("minHeight", out _));
        Assert.True(container.TryGetProperty("backgroundImage", out _));
        Assert.True(container.TryGetProperty("selectAction", out _));
        Assert.True(container.TryGetProperty("id", out _));
        Assert.True(container.TryGetProperty("isVisible", out _));
        Assert.True(container.TryGetProperty("separator", out _));
        Assert.True(container.TryGetProperty("spacing", out _));
        Assert.True(container.TryGetProperty("rtl", out _));
    }

    [Fact]
    public void ColumnSet_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .AddTextBlock(tb => tb.WithText("Column 1"))
                    .WithWidth("auto"))
                .WithSelectAction(a => a.OpenUrl("https://example.com"))
                .WithStyle(ContainerStyle.Accent)
                .WithId("cs1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var columnSet = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("ColumnSet", columnSet.GetProperty("type").GetString());
        Assert.True(columnSet.TryGetProperty("columns", out _));
        Assert.True(columnSet.TryGetProperty("selectAction", out _));
        Assert.True(columnSet.TryGetProperty("style", out _));
        Assert.True(columnSet.TryGetProperty("id", out _));
        Assert.True(columnSet.TryGetProperty("isVisible", out _));
        Assert.True(columnSet.TryGetProperty("separator", out _));
        Assert.True(columnSet.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void Column_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .AddTextBlock(tb => tb.WithText("Column content"))
                    .WithWidth("stretch")
                    .WithStyle(ContainerStyle.Emphasis)
                    .WithVerticalContentAlignment(VerticalAlignment.Bottom)
                    .WithBleed(true)
                    .WithMinHeight("50px")
                    .WithBackgroundImage(bg => bg.WithUrl("https://example.com/bg.png"))
                    .WithSelectAction(a => a.OpenUrl("https://example.com"))
                    .WithSeparator(true)
                    .WithSpacing(Spacing.Large)
                    .WithId("col1")
                    .WithIsVisible(false)
                    .WithRtl(true)
                )
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var column = doc.RootElement.GetProperty("body")[0].GetProperty("columns")[0];

        Assert.Equal("Column", column.GetProperty("type").GetString());
        Assert.True(column.TryGetProperty("items", out _));
        Assert.True(column.TryGetProperty("width", out _));
        Assert.True(column.TryGetProperty("style", out _));
        Assert.True(column.TryGetProperty("verticalContentAlignment", out _));
        Assert.True(column.TryGetProperty("bleed", out _));
        Assert.True(column.TryGetProperty("minHeight", out _));
        Assert.True(column.TryGetProperty("backgroundImage", out _));
        Assert.True(column.TryGetProperty("selectAction", out _));
        Assert.True(column.TryGetProperty("separator", out _));
        Assert.True(column.TryGetProperty("spacing", out _));
        Assert.True(column.TryGetProperty("id", out _));
        Assert.True(column.TryGetProperty("isVisible", out _));
        Assert.True(column.TryGetProperty("rtl", out _));
    }

    [Fact]
    public void Table_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTable(t => t
                .AddColumn(new TableColumnDefinition { Width = "100" })
                .AddRow(new TableRow 
                { 
                    Cells = new List<TableCell> 
                    { 
                        new TableCell 
                        { 
                            Items = new List<AdaptiveElement> 
                            { 
                                new TextBlock { Text = "Cell" } 
                            } 
                        } 
                    } 
                })
                .WithFirstRowAsHeader(true)
                .WithShowGridLines(true)
                .WithGridStyle(ContainerStyle.Emphasis)
                .WithHorizontalCellContentAlignment(HorizontalAlignment.Center)
                .WithVerticalCellContentAlignment(VerticalAlignment.Center)
                .WithId("table1")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var table = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Table", table.GetProperty("type").GetString());
        Assert.True(table.TryGetProperty("columns", out _));
        Assert.True(table.TryGetProperty("rows", out _));
        Assert.True(table.TryGetProperty("firstRowAsHeader", out _));
        Assert.True(table.TryGetProperty("showGridLines", out _));
        Assert.True(table.TryGetProperty("gridStyle", out _));
        Assert.True(table.TryGetProperty("horizontalCellContentAlignment", out _));
        Assert.True(table.TryGetProperty("verticalCellContentAlignment", out _));
        Assert.True(table.TryGetProperty("id", out _));
        Assert.True(table.TryGetProperty("isVisible", out _));
        Assert.True(table.TryGetProperty("separator", out _));
        Assert.True(table.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputText_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputText(it => it
                .WithId("input1")
                .WithPlaceholder("Enter text")
                .WithValue("default value")
                .WithMaxLength(100)
                .IsMultiline(true)
                .WithStyle(TextInputStyle.Email)
                .WithInlineAction(a => a.Submit().WithTitle("Submit"))
                .IsRequired(true)
                .WithErrorMessage("This field is required")
                .WithLabel("Text input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputText = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.Text", inputText.GetProperty("type").GetString());
        Assert.True(inputText.TryGetProperty("id", out _));
        Assert.True(inputText.TryGetProperty("placeholder", out _));
        Assert.True(inputText.TryGetProperty("value", out _));
        Assert.True(inputText.TryGetProperty("maxLength", out _));
        Assert.True(inputText.TryGetProperty("isMultiline", out _));
        Assert.True(inputText.TryGetProperty("style", out _));
        Assert.True(inputText.TryGetProperty("inlineAction", out _));
        Assert.True(inputText.TryGetProperty("isRequired", out _));
        Assert.True(inputText.TryGetProperty("errorMessage", out _));
        Assert.True(inputText.TryGetProperty("label", out _));
        Assert.True(inputText.TryGetProperty("labelPosition", out _));
        Assert.True(inputText.TryGetProperty("labelWidth", out _));
        Assert.True(inputText.TryGetProperty("isVisible", out _));
        Assert.True(inputText.TryGetProperty("separator", out _));
        Assert.True(inputText.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputNumber_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputNumber(inp => inp
                .WithId("num1")
                .WithPlaceholder("Enter number")
                .WithValue(42)
                .WithMin(0)
                .WithMax(100)
                .IsRequired(true)
                .WithErrorMessage("Number required")
                .WithLabel("Number input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Small)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputNumber = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.Number", inputNumber.GetProperty("type").GetString());
        Assert.True(inputNumber.TryGetProperty("id", out _));
        Assert.True(inputNumber.TryGetProperty("placeholder", out _));
        Assert.True(inputNumber.TryGetProperty("value", out _));
        Assert.True(inputNumber.TryGetProperty("min", out _));
        Assert.True(inputNumber.TryGetProperty("max", out _));
        Assert.True(inputNumber.TryGetProperty("isRequired", out _));
        Assert.True(inputNumber.TryGetProperty("errorMessage", out _));
        Assert.True(inputNumber.TryGetProperty("label", out _));
        Assert.True(inputNumber.TryGetProperty("labelPosition", out _));
        Assert.True(inputNumber.TryGetProperty("labelWidth", out _));
        Assert.True(inputNumber.TryGetProperty("isVisible", out _));
        Assert.True(inputNumber.TryGetProperty("separator", out _));
        Assert.True(inputNumber.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputDate_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputDate(id => id
                .WithId("date1")
                .WithPlaceholder("Select date")
                .WithValue("2024-01-15")
                .WithMin("2024-01-01")
                .WithMax("2024-12-31")
                .IsRequired(true)
                .WithErrorMessage("Date required")
                .WithLabel("Date input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputDate = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.Date", inputDate.GetProperty("type").GetString());
        Assert.True(inputDate.TryGetProperty("id", out _));
        Assert.True(inputDate.TryGetProperty("placeholder", out _));
        Assert.True(inputDate.TryGetProperty("value", out _));
        Assert.True(inputDate.TryGetProperty("min", out _));
        Assert.True(inputDate.TryGetProperty("max", out _));
        Assert.True(inputDate.TryGetProperty("isRequired", out _));
        Assert.True(inputDate.TryGetProperty("errorMessage", out _));
        Assert.True(inputDate.TryGetProperty("label", out _));
        Assert.True(inputDate.TryGetProperty("labelPosition", out _));
        Assert.True(inputDate.TryGetProperty("labelWidth", out _));
        Assert.True(inputDate.TryGetProperty("isVisible", out _));
        Assert.True(inputDate.TryGetProperty("separator", out _));
        Assert.True(inputDate.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputTime_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputTime(it => it
                .WithId("time1")
                .WithPlaceholder("Select time")
                .WithValue("14:30")
                .WithMin("09:00")
                .WithMax("17:00")
                .IsRequired(true)
                .WithErrorMessage("Time required")
                .WithLabel("Time input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Large)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputTime = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.Time", inputTime.GetProperty("type").GetString());
        Assert.True(inputTime.TryGetProperty("id", out _));
        Assert.True(inputTime.TryGetProperty("placeholder", out _));
        Assert.True(inputTime.TryGetProperty("value", out _));
        Assert.True(inputTime.TryGetProperty("min", out _));
        Assert.True(inputTime.TryGetProperty("max", out _));
        Assert.True(inputTime.TryGetProperty("isRequired", out _));
        Assert.True(inputTime.TryGetProperty("errorMessage", out _));
        Assert.True(inputTime.TryGetProperty("label", out _));
        Assert.True(inputTime.TryGetProperty("labelPosition", out _));
        Assert.True(inputTime.TryGetProperty("labelWidth", out _));
        Assert.True(inputTime.TryGetProperty("isVisible", out _));
        Assert.True(inputTime.TryGetProperty("separator", out _));
        Assert.True(inputTime.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputToggle_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputToggle(it => it
                .WithId("toggle1")
                .WithTitle("Accept terms")
                .WithValue("true")
                .WithValueOn("yes")
                .WithValueOff("no")
                .WithWrap(true)
                .IsRequired(true)
                .WithErrorMessage("You must accept")
                .WithLabel("Toggle input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Small)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputToggle = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.Toggle", inputToggle.GetProperty("type").GetString());
        Assert.True(inputToggle.TryGetProperty("id", out _));
        Assert.True(inputToggle.TryGetProperty("title", out _));
        Assert.True(inputToggle.TryGetProperty("value", out _));
        Assert.True(inputToggle.TryGetProperty("valueOn", out _));
        Assert.True(inputToggle.TryGetProperty("valueOff", out _));
        Assert.True(inputToggle.TryGetProperty("wrap", out _));
        Assert.True(inputToggle.TryGetProperty("isRequired", out _));
        Assert.True(inputToggle.TryGetProperty("errorMessage", out _));
        Assert.True(inputToggle.TryGetProperty("label", out _));
        Assert.True(inputToggle.TryGetProperty("labelPosition", out _));
        Assert.True(inputToggle.TryGetProperty("labelWidth", out _));
        Assert.True(inputToggle.TryGetProperty("isVisible", out _));
        Assert.True(inputToggle.TryGetProperty("separator", out _));
        Assert.True(inputToggle.TryGetProperty("spacing", out _));
    }

    [Fact]
    public void InputChoiceSet_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputChoiceSet(ics => ics
                .WithId("choice1")
                .AddChoice("Option 1", "1")
                .AddChoice("Option 2", "2")
                .IsMultiSelect(true)
                .WithStyle(ChoiceInputStyle.Expanded)
                .WithValue("1")
                .WithPlaceholder("Choose")
                .WithWrap(true)
                .WithChoicesData(dq =>
                {
                    dq.Dataset = "dynamicChoices";
                    dq.Count = 10;
                    dq.Skip = 0;
                })
                .IsRequired(true)
                .WithErrorMessage("Choice required")
                .WithLabel("Choice input")
                .WithLabelPosition(InputLabelPosition.Above)
                .WithLabelWidth("100px")
                .WithIsVisible(false)
                .WithSeparator(true)
                .WithSpacing(Spacing.Medium)
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var inputChoiceSet = doc.RootElement.GetProperty("body")[0];

        Assert.Equal("Input.ChoiceSet", inputChoiceSet.GetProperty("type").GetString());
        Assert.True(inputChoiceSet.TryGetProperty("id", out _));
        Assert.True(inputChoiceSet.TryGetProperty("choices", out _));
        Assert.True(inputChoiceSet.TryGetProperty("isMultiSelect", out _));
        Assert.True(inputChoiceSet.TryGetProperty("style", out _));
        Assert.True(inputChoiceSet.TryGetProperty("value", out _));
        Assert.True(inputChoiceSet.TryGetProperty("placeholder", out _));
        Assert.True(inputChoiceSet.TryGetProperty("wrap", out _));
        Assert.True(inputChoiceSet.TryGetProperty("choices.data", out _));
        Assert.True(inputChoiceSet.TryGetProperty("isRequired", out _));
        Assert.True(inputChoiceSet.TryGetProperty("errorMessage", out _));
        Assert.True(inputChoiceSet.TryGetProperty("label", out _));
        Assert.True(inputChoiceSet.TryGetProperty("labelPosition", out _));
        Assert.True(inputChoiceSet.TryGetProperty("labelWidth", out _));
        Assert.True(inputChoiceSet.TryGetProperty("isVisible", out _));
        Assert.True(inputChoiceSet.TryGetProperty("separator", out _));
        Assert.True(inputChoiceSet.TryGetProperty("spacing", out _));
    }

    #endregion

    #region Action Type Coverage Tests

    [Fact]
    public void ExecuteAction_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute()
                .WithVerb("doSomething")
                .WithData("{\"key\":\"value\"}")
                .WithAssociatedInputs(AssociatedInputs.None)
                .WithTitle("Execute")
                .WithIconUrl("https://example.com/icon.png")
                .WithId("exec1")
                .WithStyle(ActionStyle.Positive)
                .WithTooltip("Execute action")
                .WithIsEnabled(false)
                .WithMode(ActionMode.Secondary)
                .WithRequires("featureKey", "1.4")
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.Equal("Action.Execute", action.GetProperty("type").GetString());
        Assert.True(action.TryGetProperty("verb", out _));
        Assert.True(action.TryGetProperty("data", out _));
        Assert.True(action.TryGetProperty("associatedInputs", out _));
        Assert.True(action.TryGetProperty("title", out _));
        Assert.True(action.TryGetProperty("iconUrl", out _));
        Assert.True(action.TryGetProperty("id", out _));
        Assert.True(action.TryGetProperty("style", out _));
        Assert.True(action.TryGetProperty("tooltip", out _));
        Assert.True(action.TryGetProperty("isEnabled", out _));
        Assert.True(action.TryGetProperty("mode", out _));
        Assert.True(action.TryGetProperty("requires", out _));
    }

    [Fact]
    public void SubmitAction_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit()
                .WithData("{\"submitKey\":\"submitValue\"}")
                .WithAssociatedInputs(AssociatedInputs.Auto)
                .WithTitle("Submit")
                .WithIconUrl("https://example.com/submit.png")
                .WithId("submit1")
                .WithStyle(ActionStyle.Positive)
                .WithTooltip("Submit form")
                .WithIsEnabled(false)
                .WithMode(ActionMode.Primary)
                .WithRequires("featureKey", "1.0")
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.Equal("Action.Submit", action.GetProperty("type").GetString());
        Assert.True(action.TryGetProperty("data", out _));
        Assert.True(action.TryGetProperty("associatedInputs", out _));
        Assert.True(action.TryGetProperty("title", out _));
        Assert.True(action.TryGetProperty("iconUrl", out _));
        Assert.True(action.TryGetProperty("id", out _));
        Assert.True(action.TryGetProperty("style", out _));
        Assert.True(action.TryGetProperty("tooltip", out _));
        Assert.True(action.TryGetProperty("isEnabled", out _));
        Assert.True(action.TryGetProperty("mode", out _));
        Assert.True(action.TryGetProperty("requires", out _));
    }

    [Fact]
    public void OpenUrlAction_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .OpenUrl("https://example.com")
                .WithTitle("Open Link")
                .WithIconUrl("https://example.com/link.png")
                .WithId("open1")
                .WithStyle(ActionStyle.Destructive)
                .WithTooltip("Opens external link")
                .WithIsEnabled(false)
                .WithMode(ActionMode.Secondary)
                .WithRequires("featureKey", "1.0")
            )
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.Equal("Action.OpenUrl", action.GetProperty("type").GetString());
        Assert.True(action.TryGetProperty("url", out _));
        Assert.True(action.TryGetProperty("title", out _));
        Assert.True(action.TryGetProperty("iconUrl", out _));
        Assert.True(action.TryGetProperty("id", out _));
        Assert.True(action.TryGetProperty("style", out _));
        Assert.True(action.TryGetProperty("tooltip", out _));
        Assert.True(action.TryGetProperty("isEnabled", out _));
        Assert.True(action.TryGetProperty("mode", out _));
        Assert.True(action.TryGetProperty("requires", out _));
    }

    [Fact]
    public void ShowCardAction_Supports_All_Schema_Properties()
    {
        var nestedCard = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Shown card"))
            .Build();
            
        var card = AdaptiveCardBuilder.Create()
            .Build();
        
        card.Actions = new List<AdaptiveAction>
        {
            new ShowCardAction
            {
                Card = nestedCard,
                Title = "Show Card",
                IconUrl = "https://example.com/show.png",
                Id = "show1",
                Style = ActionStyle.Positive,
                Tooltip = "Shows a card",
                IsEnabled = false,
                Mode = ActionMode.Primary,
                Requires = new Dictionary<string, string> { { "featureKey", "1.1" } }
            }
        };

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.Equal("Action.ShowCard", action.GetProperty("type").GetString());
        Assert.True(action.TryGetProperty("card", out _));
        Assert.True(action.TryGetProperty("title", out _));
        Assert.True(action.TryGetProperty("iconUrl", out _));
        Assert.True(action.TryGetProperty("id", out _));
        Assert.True(action.TryGetProperty("style", out _));
        Assert.True(action.TryGetProperty("tooltip", out _));
        Assert.True(action.TryGetProperty("isEnabled", out _));
        Assert.True(action.TryGetProperty("mode", out _));
        Assert.True(action.TryGetProperty("requires", out _));
    }

    [Fact]
    public void ToggleVisibilityAction_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Target").WithId("target1"))
            .Build();
            
        card.Actions = new List<AdaptiveAction>
        {
            new ToggleVisibilityAction
            {
                TargetElements = new List<object> 
                { 
                    new TargetElement { ElementId = "target1", IsVisible = true } 
                },
                Title = "Toggle",
                IconUrl = "https://example.com/toggle.png",
                Id = "toggle1",
                Style = ActionStyle.Destructive,
                Tooltip = "Toggle visibility",
                IsEnabled = false,
                Mode = ActionMode.Secondary,
                Requires = new Dictionary<string, string> { { "featureKey", "1.2" } }
            }
        };

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var action = doc.RootElement.GetProperty("actions")[0];

        Assert.Equal("Action.ToggleVisibility", action.GetProperty("type").GetString());
        Assert.True(action.TryGetProperty("targetElements", out _));
        Assert.True(action.TryGetProperty("title", out _));
        Assert.True(action.TryGetProperty("iconUrl", out _));
        Assert.True(action.TryGetProperty("id", out _));
        Assert.True(action.TryGetProperty("style", out _));
        Assert.True(action.TryGetProperty("tooltip", out _));
        Assert.True(action.TryGetProperty("isEnabled", out _));
        Assert.True(action.TryGetProperty("mode", out _));
        Assert.True(action.TryGetProperty("requires", out _));
    }

    #endregion

    #region Enum Value Coverage Tests

    [Fact]
    public void TextSize_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<TextSize>();
        Assert.Contains("Default", values);
        Assert.Contains("Small", values);
        Assert.Contains("Medium", values);
        Assert.Contains("Large", values);
        Assert.Contains("ExtraLarge", values);
        Assert.Equal(5, values.Length);
    }

    [Fact]
    public void TextWeight_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<TextWeight>();
        Assert.Contains("Default", values);
        Assert.Contains("Lighter", values);
        Assert.Contains("Bolder", values);
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void TextColor_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<TextColor>();
        Assert.Contains("Default", values);
        Assert.Contains("Dark", values);
        Assert.Contains("Light", values);
        Assert.Contains("Accent", values);
        Assert.Contains("Good", values);
        Assert.Contains("Warning", values);
        Assert.Contains("Attention", values);
        Assert.Equal(7, values.Length);
    }

    [Fact]
    public void FontType_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<FontType>();
        Assert.Contains("Default", values);
        Assert.Contains("Monospace", values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void TextBlockStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<TextBlockStyle>();
        Assert.Contains("Default", values);
        Assert.Contains("Heading", values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void HorizontalAlignment_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<HorizontalAlignment>();
        Assert.Contains("Left", values);
        Assert.Contains("Center", values);
        Assert.Contains("Right", values);
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void VerticalAlignment_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<VerticalAlignment>();
        Assert.Contains("Top", values);
        Assert.Contains("Center", values);
        Assert.Contains("Bottom", values);
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void ImageSize_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ImageSize>();
        Assert.Contains("Auto", values);
        Assert.Contains("Stretch", values);
        Assert.Contains("Small", values);
        Assert.Contains("Medium", values);
        Assert.Contains("Large", values);
        Assert.Equal(5, values.Length);
    }

    [Fact]
    public void ImageStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ImageStyle>();
        Assert.Contains("Default", values);
        Assert.Contains("Person", values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void ContainerStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ContainerStyle>();
        Assert.Contains("Default", values);
        Assert.Contains("Emphasis", values);
        Assert.Contains("Good", values);
        Assert.Contains("Attention", values);
        Assert.Contains("Warning", values);
        Assert.Contains("Accent", values);
        Assert.Equal(6, values.Length);
    }

    [Fact]
    public void Spacing_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<Spacing>();
        Assert.Contains("Default", values);
        Assert.Contains("None", values);
        Assert.Contains("Small", values);
        Assert.Contains("Medium", values);
        Assert.Contains("Large", values);
        Assert.Contains("ExtraLarge", values);
        Assert.Contains("Padding", values);
        Assert.Equal(7, values.Length);
    }

    [Fact]
    public void ActionStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ActionStyle>();
        Assert.Contains("Default", values);
        Assert.Contains("Positive", values);
        Assert.Contains("Destructive", values);
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void ActionMode_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ActionMode>();
        Assert.Contains("Primary", values);
        Assert.Contains("Secondary", values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void AssociatedInputs_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<AssociatedInputs>();
        Assert.Contains("Auto", values);
        Assert.Contains("None", values);
        Assert.Equal(2, values.Length);
    }

    [Fact]
    public void ChoiceInputStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<ChoiceInputStyle>();
        Assert.Contains("Compact", values);
        Assert.Contains("Expanded", values);
        Assert.Contains("Filtered", values);
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void TextInputStyle_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<TextInputStyle>();
        Assert.Contains("Text", values);
        Assert.Contains("Tel", values);
        Assert.Contains("Url", values);
        Assert.Contains("Email", values);
        Assert.Contains("Password", values);
        Assert.Equal(5, values.Length);
    }

    [Fact]
    public void InputLabelPosition_Has_All_Schema_Values()
    {
        var values = Enum.GetNames<InputLabelPosition>();
        Assert.Contains("Inline", values);
        Assert.Contains("Above", values);
        Assert.Equal(2, values.Length);
    }

    #endregion

    #region AdaptiveCard Top-Level Properties Tests

    [Fact]
    public void AdaptiveCard_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithSchema("https://adaptivecards.io/schemas/adaptive-card.json")
            .AddTextBlock(tb => tb.WithText("Body"))
            .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Action"))
            .WithSelectAction(a => a.OpenUrl("https://example.com"))
            .WithFallbackText("Fallback text")
            .WithBackgroundImage(bg => bg.WithUrl("https://example.com/bg.png"))
            .WithMetadata("https://example.com/card")
            .WithMinHeight("200px")
            .WithRtl(true)
            .WithSpeak("Speak this")
            .WithLang("en")
            .WithVerticalContentAlignment(VerticalAlignment.Center)
            .WithRefresh(r => r
                .WithAction(a => a.Execute().WithVerb("refresh"))
                .AddUserId("user1")
                .WithExpires("2024-12-31T23:59:59Z"))
            .WithAuthentication(auth => auth
                .WithText("Sign in")
                .WithConnectionName("myConnection")
                .AddButton(new AuthCardButton { Title = "Sign in", Value = "signin" }))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        Assert.Equal("AdaptiveCard", root.GetProperty("type").GetString());
        Assert.True(root.TryGetProperty("version", out _));
        Assert.True(root.TryGetProperty("$schema", out _));
        Assert.True(root.TryGetProperty("body", out _));
        Assert.True(root.TryGetProperty("actions", out _));
        Assert.True(root.TryGetProperty("selectAction", out _));
        Assert.True(root.TryGetProperty("fallbackText", out _));
        Assert.True(root.TryGetProperty("backgroundImage", out _));
        Assert.True(root.TryGetProperty("metadata", out _));
        Assert.True(root.TryGetProperty("minHeight", out _));
        Assert.True(root.TryGetProperty("rtl", out _));
        Assert.True(root.TryGetProperty("speak", out _));
        Assert.True(root.TryGetProperty("lang", out _));
        Assert.True(root.TryGetProperty("verticalContentAlignment", out _));
        Assert.True(root.TryGetProperty("refresh", out _));
        Assert.True(root.TryGetProperty("authentication", out _));
    }

    #endregion

    #region Advanced Features Tests

    [Fact]
    public void Refresh_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithRefresh(r => r
                .WithAction(a => a.Execute().WithVerb("refreshAction"))
                .AddUserId("user1")
                .AddUserId("user2")
                .WithExpires("2024-12-31T23:59:59Z"))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var refresh = doc.RootElement.GetProperty("refresh");

        Assert.True(refresh.TryGetProperty("action", out _));
        Assert.True(refresh.TryGetProperty("userIds", out _));
        Assert.True(refresh.TryGetProperty("expires", out _));
    }

    [Fact]
    public void Authentication_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithAuthentication(auth => auth
                .WithText("Please sign in")
                .WithConnectionName("authConnection")
                .WithTokenExchangeResource(new TokenExchangeResource
                {
                    Id = "resourceId",
                    Uri = "https://example.com/token",
                    ProviderId = "providerId"
                })
                .AddButton(new AuthCardButton { Title = "Sign in with Google", Value = "google" })
                .AddButton(new AuthCardButton { Title = "Sign in with Microsoft", Value = "microsoft" }))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var authentication = doc.RootElement.GetProperty("authentication");

        Assert.True(authentication.TryGetProperty("text", out _));
        Assert.True(authentication.TryGetProperty("connectionName", out _));
        Assert.True(authentication.TryGetProperty("tokenExchangeResource", out _));
        Assert.True(authentication.TryGetProperty("buttons", out _));
    }

    [Fact]
    public void Metadata_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithMetadata("https://example.com/metadata")
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var metadata = doc.RootElement.GetProperty("metadata");

        Assert.True(metadata.TryGetProperty("webUrl", out _));
    }

    [Fact]
    public void CaptionSource_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddMedia(m => m
                .AddSource("https://example.com/video.mp4", "video/mp4")
                .AddCaptionSource("English", "https://example.com/captions.vtt", "vtt"))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var captionSource = doc.RootElement.GetProperty("body")[0]
            .GetProperty("captionSources")[0];

        Assert.True(captionSource.TryGetProperty("mimeType", out _));
        Assert.True(captionSource.TryGetProperty("url", out _));
        Assert.True(captionSource.TryGetProperty("label", out _));
    }

    [Fact]
    public void DataQuery_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddInputChoiceSet(ics => ics
                .WithId("dynamicChoices")
                .WithChoicesData(dq =>
                {
                    dq.Dataset = "peopleDataset";
                    dq.Count = 50;
                    dq.Skip = 10;
                }))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var dataQuery = doc.RootElement.GetProperty("body")[0]
            .GetProperty("choices.data");

        Assert.True(dataQuery.TryGetProperty("dataset", out _));
        Assert.True(dataQuery.TryGetProperty("count", out _));
        Assert.True(dataQuery.TryGetProperty("skip", out _));
    }

    [Fact]
    public void BackgroundImage_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithBackgroundImage(bg => bg
                .WithUrl("https://example.com/background.png")
                .WithFillMode(BackgroundImageFillMode.Cover)
                .WithHorizontalAlignment(HorizontalAlignment.Center)
                .WithVerticalAlignment(VerticalAlignment.Center))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var backgroundImage = doc.RootElement.GetProperty("backgroundImage");

        Assert.True(backgroundImage.TryGetProperty("url", out _));
        Assert.True(backgroundImage.TryGetProperty("fillMode", out _));
        Assert.True(backgroundImage.TryGetProperty("horizontalAlignment", out _));
        Assert.True(backgroundImage.TryGetProperty("verticalAlignment", out _));
    }

    [Fact]
    public void TokenExchangeResource_Supports_All_Schema_Properties()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithAuthentication(auth => auth
                .WithText("Sign in")
                .WithConnectionName("conn")
                .WithTokenExchangeResource(new TokenExchangeResource
                {
                    Id = "tokenId",
                    Uri = "https://example.com/token",
                    ProviderId = "provider123"
                }))
            .Build();

        var json = card.ToJson();
        var doc = JsonDocument.Parse(json);
        var tokenExchange = doc.RootElement.GetProperty("authentication")
            .GetProperty("tokenExchangeResource");

        Assert.True(tokenExchange.TryGetProperty("id", out _));
        Assert.True(tokenExchange.TryGetProperty("uri", out _));
        Assert.True(tokenExchange.TryGetProperty("providerId", out _));
    }

    #endregion
}
