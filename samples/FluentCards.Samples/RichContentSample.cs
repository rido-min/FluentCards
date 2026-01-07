namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating cards with rich content like images, media, and rich text.
/// </summary>
public static class RichContentSample
{
    /// <summary>
    /// Creates a card with rich text formatting.
    /// </summary>
    /// <returns>An Adaptive Card with rich text blocks.</returns>
    public static AdaptiveCard CreateRichTextCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr
                    .WithText("Welcome ")
                    .WithSize(TextSize.Large))
                .AddTextRun(tr => tr
                    .WithText("to FluentCards!")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder)
                    .WithColor(TextColor.Accent))
                .AddTextRun(tr => tr
                    .WithText("\n\nThis demonstrates ")
                    .WithSize(TextSize.Default))
                .AddTextRun(tr => tr
                    .WithText("rich text formatting")
                    .WithWeight(TextWeight.Bolder)
                    .WithColor(TextColor.Good))
                .AddTextRun(tr => tr
                    .WithText(" with multiple text runs.")
                    .WithSize(TextSize.Default)))
            .Build();
    }

    /// <summary>
    /// Creates a card with an image set gallery.
    /// </summary>
    /// <returns>An Adaptive Card with an image set.</returns>
    public static AdaptiveCard CreateImageSetCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Photo Gallery")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddImageSet(imgSet => imgSet
                .WithImageSize(ImageSize.Medium)
                .AddImage(img => img
                    .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png"))
                .AddImage(img => img
                    .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png"))
                .AddImage(img => img
                    .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png")))
            .AddTextBlock(tb => tb
                .WithText("View more photos in the gallery")
                .WithWrap(true))
            .Build();
    }

    /// <summary>
    /// Creates a card with a table.
    /// </summary>
    /// <returns>An Adaptive Card with a table.</returns>
    public static AdaptiveCard CreateTableCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Sales Report")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddTable(table => table
                .AddColumn(new TableColumnDefinition { Width = "2" })
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddColumn(new TableColumnDefinition { Width = "1" })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Product A" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "150" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "$15,000" } } }
                    }
                })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Product B" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "200" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "$20,000" } } }
                    }
                })
                .AddRow(new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "Product C" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "100" } } },
                        new TableCell { Items = new List<AdaptiveElement> { new TextBlock { Text = "$10,000" } } }
                    }
                }))
            .Build();
    }

    /// <summary>
    /// Creates a card with media player.
    /// </summary>
    /// <returns>An Adaptive Card with a media player.</returns>
    public static AdaptiveCard CreateMediaCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Video Tutorial")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddMedia(media => media
                .AddSource("https://example.com/video.mp4", "video/mp4")
                .WithPoster("https://example.com/poster.jpg")
                .WithAltText("Getting started with FluentCards"))
            .AddTextBlock(tb => tb
                .WithText("Watch this tutorial to learn the basics of FluentCards.")
                .WithWrap(true))
            .Build();
    }

    /// <summary>
    /// Creates a comprehensive card combining multiple rich content types.
    /// </summary>
    /// <returns>An Adaptive Card with mixed rich content.</returns>
    public static AdaptiveCard CreateComprehensiveCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Product Launch Announcement")
                .WithSize(TextSize.ExtraLarge)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Accent))
            .AddImage(img => img
                .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png")
                .WithSize(ImageSize.Large)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .AddRichTextBlock(rtb => rtb
                .AddTextRun(tr => tr
                    .WithText("Introducing ")
                    .WithSize(TextSize.Medium))
                .AddTextRun(tr => tr
                    .WithText("FluentCards 2.0")
                    .WithSize(TextSize.Medium)
                    .WithWeight(TextWeight.Bolder)
                    .WithColor(TextColor.Good)))
            .AddFactSet(fs => fs
                .AddFact("Release Date", "January 1, 2025")
                .AddFact("Version", "2.0.0")
                .AddFact("License", "MIT"))
            .AddAction(a => a
                .OpenUrl("https://github.com/rido-min/FluentCards")
                .WithTitle("View on GitHub"))
            .AddAction(a => a
                .Submit("Get Notified")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }
}
