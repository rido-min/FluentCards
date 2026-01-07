namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating cards with advanced layouts using containers and columns.
/// </summary>
public static class LayoutCardSample
{
    /// <summary>
    /// Creates a card with a two-column layout.
    /// </summary>
    /// <returns>An Adaptive Card with two-column layout.</returns>
    public static AdaptiveCard CreateTwoColumnCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Product Information")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddImage(img => img
                        .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png")
                        .WithSize(ImageSize.Medium)))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb
                        .WithText("Adaptive Cards SDK")
                        .WithWeight(TextWeight.Bolder))
                    .AddTextBlock(tb => tb
                        .WithText("Create platform-agnostic UI snippets")
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText("$49.99")
                        .WithColor(TextColor.Good)
                        .WithSize(TextSize.Large))))
            .AddAction(a => a
                .Submit("Add to Cart")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }

    /// <summary>
    /// Creates a card with containers and styled sections.
    /// </summary>
    /// <returns>An Adaptive Card with styled containers.</returns>
    public static AdaptiveCard CreateStyledContainerCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Important Notice")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder))
                .AddTextBlock(tb => tb
                    .WithText("This is an emphasized section with important information.")
                    .WithWrap(true)))
            .AddContainer(c => c
                .AddTextBlock(tb => tb
                    .WithText("Regular Section")
                    .WithWeight(TextWeight.Bolder))
                .AddTextBlock(tb => tb
                    .WithText("This is a normal section with regular styling.")
                    .WithWrap(true)))
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Accent)
                .AddTextBlock(tb => tb
                    .WithText("Highlighted Section")
                    .WithWeight(TextWeight.Bolder))
                .AddTextBlock(tb => tb
                    .WithText("This section uses accent styling to stand out.")
                    .WithWrap(true)))
            .Build();
    }

    /// <summary>
    /// Creates a card with a fact set for displaying key-value pairs.
    /// </summary>
    /// <returns>An Adaptive Card with a fact set.</returns>
    public static AdaptiveCard CreateFactSetCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Meeting Details")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddFactSet(fs => fs
                .AddFact("Date", "December 15, 2024")
                .AddFact("Time", "2:00 PM - 3:00 PM")
                .AddFact("Location", "Conference Room A")
                .AddFact("Organizer", "John Smith")
                .AddFact("Attendees", "12 people"))
            .AddAction(a => a
                .OpenUrl("https://example.com/meeting/123")
                .WithTitle("Join Meeting"))
            .Build();
    }

    /// <summary>
    /// Creates a card with nested containers for complex layouts.
    /// </summary>
    /// <returns>An Adaptive Card with nested containers.</returns>
    public static AdaptiveCard CreateNestedContainerCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Dashboard")
                .WithSize(TextSize.ExtraLarge)
                .WithWeight(TextWeight.Bolder))
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Statistics")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder))
                .AddColumnSet(cs => cs
                    .AddColumn(col => col
                        .WithWidth("stretch")
                        .AddContainer(cont => cont
                            .WithStyle(ContainerStyle.Good)
                            .AddTextBlock(tb => tb
                                .WithText("Active Users")
                                .WithWeight(TextWeight.Bolder))
                            .AddTextBlock(tb => tb
                                .WithText("1,234")
                                .WithSize(TextSize.ExtraLarge))))
                    .AddColumn(col => col
                        .WithWidth("stretch")
                        .AddContainer(cont => cont
                            .WithStyle(ContainerStyle.Attention)
                            .AddTextBlock(tb => tb
                                .WithText("Pending Issues")
                                .WithWeight(TextWeight.Bolder))
                            .AddTextBlock(tb => tb
                                .WithText("42")
                                .WithSize(TextSize.ExtraLarge))))))
            .Build();
    }
}
