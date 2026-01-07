namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating a basic Adaptive Card with text and an action.
/// </summary>
public static class BasicCardSample
{
    /// <summary>
    /// Creates a simple welcome card.
    /// </summary>
    /// <returns>An Adaptive Card with welcome message and action.</returns>
    public static AdaptiveCard CreateWelcomeCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Welcome to FluentCards!")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .AddTextBlock(tb => tb
                .WithText("This library helps you create Adaptive Cards using a fluent API.")
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl("https://adaptivecards.io")
                .WithTitle("Learn More"))
            .Build();
    }

    /// <summary>
    /// Creates a simple notification card.
    /// </summary>
    /// <returns>An Adaptive Card for notifications.</returns>
    public static AdaptiveCard CreateNotificationCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Notification")
                .WithSize(TextSize.Medium)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Attention))
            .AddTextBlock(tb => tb
                .WithText("You have a new message waiting for you.")
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl("https://example.com/messages")
                .WithTitle("View Messages"))
            .Build();
    }

    /// <summary>
    /// Creates a simple card with an image.
    /// </summary>
    /// <returns>An Adaptive Card with an image.</returns>
    public static AdaptiveCard CreateImageCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddImage(img => img
                .WithUrl("https://adaptivecards.io/content/adaptive-card-50.png")
                .WithSize(ImageSize.Medium)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .AddTextBlock(tb => tb
                .WithText("Adaptive Cards")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .AddTextBlock(tb => tb
                .WithText("Platform-agnostic snippets of UI")
                .WithWrap(true)
                .WithHorizontalAlignment(HorizontalAlignment.Center))
            .Build();
    }
}
