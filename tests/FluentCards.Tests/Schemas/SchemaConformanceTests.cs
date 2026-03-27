using Xunit;

namespace FluentCards.Tests.Schemas;

public class SchemaConformanceTests
{
    [Fact]
    public void MinimalCard_ConformsToSchema()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Hello World"))
            .Build();

        // Assert
        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void CardWithAllCardProperties_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithFallbackText("Your client doesn't support this card.")
            .WithMinHeight("200px")
            .WithLang("en")
            .WithSpeak("Hello World")
            .WithVerticalContentAlignment(VerticalAlignment.Center)
            .AddTextBlock(tb => tb.WithText("Hello"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void TextBlock_WithAllProperties_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .AddTextBlock(tb => tb
                .WithId("tb1")
                .WithText("Hello World")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Accent)
                .WithFontType(FontType.Monospace)
                .WithIsSubtle(true)
                .WithStyle(TextBlockStyle.Heading)
                .WithWrap(true)
                .WithMaxLines(3)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void Image_WithAllProperties_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImage(img => img
                .WithUrl("https://example.com/image.png")
                .WithAltText("Sample image")
                .WithSize(ImageSize.Medium)
                .WithStyle(ImageStyle.Person)
                .WithWidth("100px")
                .WithHeight("100px")
                .WithHorizontalAlignment(HorizontalAlignment.Center)
                .WithBackgroundColor("#FF0000"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void Container_WithItems_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithId("container1")
                .WithStyle(ContainerStyle.Emphasis)
                .WithBleed(true)
                .WithMinHeight("100px")
                .AddTextBlock(tb => tb.WithText("Inside container")))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ColumnSet_WithColumns_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .WithId("cs1")
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddTextBlock(tb => tb.WithText("Col 1")))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb.WithText("Col 2"))))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void FactSet_WithFacts_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddFactSet(fs => fs
                .AddFact("Name", "John Doe")
                .AddFact("Age", "30"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void RichTextBlock_WithTextRuns_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr
                    .WithText("Bold text")
                    .WithWeight(TextWeight.Bolder))
                .AddTextRun(tr => tr
                    .WithText(" and italic")
                    .IsItalic(true)))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void Table_WithRowsAndCells_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTable(t => t
                .WithFirstRowAsHeader(true)
                .WithShowGridLines(true)
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Header 1" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Header 2" } } }
                    }
                })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Cell 1" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Cell 2" } } }
                    }
                }))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ImageSet_WithImages_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImageSet(imgSet => imgSet
                .WithImageSize(ImageSize.Medium)
                .AddImage(img => img
                    .WithUrl("https://example.com/img1.png"))
                .AddImage(img => img
                    .WithUrl("https://example.com/img2.png")))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void Media_WithSources_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddMedia(m => m
                .WithPoster("https://example.com/poster.png")
                .WithAltText("Sample video")
                .AddSource("https://example.com/video.mp4", "video/mp4"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ActionSet_WithActions_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddActionSet(acs => acs
                .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Open")))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputText_WithAllProperties_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Name")
                .WithPlaceholder("Enter your name")
                .IsRequired(true)
                .WithErrorMessage("Name is required")
                .WithMaxLength(100)
                .WithStyle(TextInputStyle.Email)
                .WithRegex("^[^@]+@[^@]+$"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputNumber_WithMinMax_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputNumber(i => i
                .WithId("age")
                .WithLabel("Age")
                .WithMin(0)
                .WithMax(150)
                .WithPlaceholder("Enter age"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputDate_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputDate(i => i
                .WithId("dob")
                .WithLabel("Date of Birth")
                .WithMin("1900-01-01")
                .WithMax("2030-12-31"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputTime_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputTime(i => i
                .WithId("time")
                .WithLabel("Meeting Time")
                .WithMin("08:00")
                .WithMax("17:00"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputToggle_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputToggle(i => i
                .WithId("agree")
                .WithTitle("I agree")
                .WithLabel("Agreement"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputChoiceSet_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddInputChoiceSet(i => i
                .WithId("color")
                .WithLabel("Favorite Color")
                .WithStyle(ChoiceInputStyle.Expanded)
                .AddChoice("Red", "red")
                .AddChoice("Blue", "blue")
                .AddChoice("Green", "green"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void InputChoiceSet_WithChoicesData_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .AddInputChoiceSet(i => i
                .WithId("people-picker")
                .WithLabel("Select users")
                .IsMultiSelect()
                .WithValue("user1,user2")
                .WithChoicesData("graph.microsoft.com/users"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void AllActionTypes_ConformToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Actions"))
            .AddAction(a => a.OpenUrl("https://example.com").WithTitle("Open URL"))
            .AddAction(a => a.Submit("Submit"))
            .AddAction(a => a.Execute("Execute").WithTitle("Execute"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ShowCardAction_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Show card test"))
            .AddAction(a => a.ShowCard("Details"))
            .Build();

        // Manually set up the ShowCard with a nested card
        var showCard = (ShowCardAction)card.Actions![0];
        showCard.Card = AdaptiveCardBuilder.Create()
            .AddTextBlock(tb => tb.WithText("Nested content"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ToggleVisibilityAction_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb.WithText("Toggle test").WithId("target"))
            .AddAction(a => a.ToggleVisibility("Toggle").WithTitle("Toggle"))
            .Build();

        // Manually set target elements
        var toggleAction = (ToggleVisibilityAction)card.Actions![0];
        toggleAction.TargetElements = new List<object> { "target" };

        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void WithVersionEnum_V1_6_ProducesSchemaValidCard()
    {
        // Arrange & Act — use the enum overload to verify it sets
        // both version and $schema to values the schema accepts.
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_6)
            .AddTextBlock(tb => tb.WithText("Built with enum API"))
            .Build();

        // Assert
        SchemaValidator.AssertValid(card);
    }

    [Fact]
    public void ComplexCard_WithMultipleElements_ConformsToSchema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .WithLang("en")
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddImage(img => img
                        .WithUrl("https://example.com/avatar.png")
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb
                        .WithText("John Doe")
                        .WithWeight(TextWeight.Bolder))
                    .AddTextBlock(tb => tb
                        .WithText("Software Engineer")
                        .WithIsSubtle(true))))
            .AddFactSet(fs => fs
                .AddFact("Email", "john@example.com")
                .AddFact("Phone", "+1 (555) 123-4567"))
            .AddInputText(i => i
                .WithId("message")
                .WithLabel("Message")
                .IsMultiline(true)
                .WithPlaceholder("Type your message"))
            .AddAction(a => a.Submit("Send").WithStyle(ActionStyle.Positive))
            .AddAction(a => a.OpenUrl("https://example.com").WithTitle("View Profile"))
            .Build();

        SchemaValidator.AssertValid(card);
    }

    // ── Multi-Version Conformance Tests ──────────────────────────────────────

    [Fact]
    public void V1_2Card_WithCoreElements_ConformsToV1_2Schema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_2)
            .AddTextBlock(tb => tb.WithText("Hello from 1.2"))
            .AddImage(img => img.WithUrl("https://example.com/photo.png").WithAltText("Photo"))
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddTextBlock(tb => tb.WithText("Left")))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb.WithText("Right"))))
            .AddFactSet(fs => fs
                .AddFact("Key", "Value"))
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr.WithText("Rich text")))
            .AddAction(a => a.ToggleVisibility("Toggle").WithTitle("Toggle"))
            .Build();

        // Manually set target elements for the toggle action
        var toggleAction = (ToggleVisibilityAction)card.Actions![0];
        toggleAction.TargetElements = new List<object> { "target-id" };

        SchemaValidator.AssertValid(card, AdaptiveCardVersion.V1_2);
    }

    [Fact]
    public void V1_3Card_WithV1_3Features_ConformsToV1_3Schema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_3)
            .AddTextBlock(tb => tb.WithText("Version 1.3 card"))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Your Name")
                .IsRequired(true)
                .WithErrorMessage("Name is required"))
            .AddAction(a => a.Submit("Send"))
            .Build();

        SchemaValidator.AssertValid(card, AdaptiveCardVersion.V1_3);
    }

    [Fact]
    public void V1_4Card_WithExecuteAction_ConformsToV1_4Schema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_4)
            .AddTextBlock(tb => tb.WithText("Execute action card"))
            .AddAction(a => a.Execute("Run").WithTitle("Run"))
            .Build();

        SchemaValidator.AssertValid(card, AdaptiveCardVersion.V1_4);
    }

    [Fact]
    public void V1_5Card_WithTable_ConformsToV1_5Schema()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .AddTable(t => t
                .WithFirstRowAsHeader(true)
                .WithShowGridLines(true)
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddColumn(new TableColumnDefinition { Width = "2" })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new() { Items = new List<AdaptiveElement> { new TextBlock { Text = "Name" } } },
                        new() { Items = new List<AdaptiveElement> { new TextBlock { Text = "Role" } } }
                    }
                })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new() { Items = new List<AdaptiveElement> { new TextBlock { Text = "Alice" } } },
                        new() { Items = new List<AdaptiveElement> { new TextBlock { Text = "Engineer" } } }
                    }
                }))
            .Build();

        SchemaValidator.AssertValid(card, AdaptiveCardVersion.V1_5);
    }

    [Theory]
    [InlineData(AdaptiveCardVersion.V1_2)]
    [InlineData(AdaptiveCardVersion.V1_3)]
    [InlineData(AdaptiveCardVersion.V1_4)]
    [InlineData(AdaptiveCardVersion.V1_5)]
    [InlineData(AdaptiveCardVersion.V1_6)]
    public void MinimalCard_ConformsToMatchingSchema(AdaptiveCardVersion version)
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(version)
            .AddTextBlock(tb => tb.WithText("Hello"))
            .Build();

        SchemaValidator.AssertValid(card, version);
    }

    [Theory]
    [InlineData(AdaptiveCardVersion.V1_0)]
    [InlineData(AdaptiveCardVersion.V1_1)]
    public void UnsupportedVersion_ThrowsArgumentException(AdaptiveCardVersion version)
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.2")
            .AddTextBlock(tb => tb.WithText("Hello"))
            .Build();

        Assert.Throws<ArgumentException>(() => SchemaValidator.AssertValid(card, version));
    }
}
