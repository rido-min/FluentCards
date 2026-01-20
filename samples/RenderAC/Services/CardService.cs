using FluentCards;

namespace FluentCards.WebRenderer.Backend.Services;

public interface ICardService
{
    string GetWelcomeCard();
    string GetFormCard();
    string GetLayoutCard();
    string GetProductCard();
    object HandleCardSubmission(string cardType, Dictionary<string, object> data);
}

public class CardService : ICardService
{
    public string GetWelcomeCard()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Welcome to FluentCards!")
                .WithSize(TextSize.ExtraLarge)
                .WithWeight(TextWeight.Bolder)
                .WithColor(TextColor.Accent))
            .AddTextBlock(tb => tb
                .WithText("This is a demonstration of Adaptive Cards rendered in a browser using FluentCards library.")
                .WithWrap(true)
                .WithColor(TextColor.Default))
            .AddTextBlock(tb => tb
                .WithText("Try different card types from the menu above.")
                .WithSize(TextSize.Medium)
                .WithWeight(TextWeight.Default))
            .AddAction(a => a
                .OpenUrl("https://github.com/rido-min/FluentCards")
                .WithTitle("View on GitHub")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .OpenUrl("https://adaptivecards.io")
                .WithTitle("Learn More"))
            .Build();

        return card.ToJson();
    }

    public string GetFormCard()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Contact Form")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddTextBlock(tb => tb
                .WithText("Please fill out the form below and we'll get back to you soon.")
                .WithWrap(true))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Full Name")
                .WithPlaceholder("Enter your name")
                .IsRequired(true)
                .WithErrorMessage("Name is required"))
            .AddInputText(i => i
                .WithId("email")
                .WithLabel("Email Address")
                .WithPlaceholder("Enter your email")
                .WithStyle(TextInputStyle.Email)
                .IsRequired(true)
                .WithErrorMessage("Valid email is required"))
            .AddInputChoiceSet(i => i
                .WithId("reason")
                .WithLabel("Contact Reason")
                .AddChoice("General Inquiry", "general")
                .AddChoice("Support Request", "support")
                .AddChoice("Feedback", "feedback")
                .AddChoice("Bug Report", "bug")
                .WithValue("general")
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("message")
                .WithLabel("Message")
                .WithPlaceholder("Tell us more...")
                .IsMultiline(true)
                .WithMaxLength(500)
                .IsRequired(true))
            .AddAction(a => a
                .Submit("Send Message")
                .WithStyle(ActionStyle.Positive))
            .Build();

        return card.ToJson();
    }

    public string GetLayoutCard()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Advanced Layout Demo")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddImage(img => img
                        .WithUrl("https://picsum.photos/seed/adaptive1/100/100.jpg")
                        .WithSize(ImageSize.Medium)
                        .WithStyle(ImageStyle.Default)))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb
                        .WithText("Column Layouts")
                        .WithWeight(TextWeight.Bolder)
                        .WithSize(TextSize.Medium))
                    .AddTextBlock(tb => tb
                        .WithText("This demonstrates how to create responsive column layouts with images and text.")
                        .WithWrap(true))))
            .AddTextBlock(tb => tb.WithText("Layout Type: ColumnSet + FactSet"))
            .AddTextBlock(tb => tb.WithText("Responsive: Yes"))
            .AddTextBlock(tb => tb.WithText("Elements: 3"))
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Container with Emphasis")
                    .WithWeight(TextWeight.Bolder))
                .AddTextBlock(tb => tb
                    .WithText("This container has a different background style to create visual separation.")
                    .WithWrap(true)))
            .AddAction(a => a
                .OpenUrl("https://adaptivecards.io/samples/")
                .WithTitle("More Samples"))
            .Build();

        return card.ToJson();
    }

    public string GetProductCard()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("auto")
                    .AddImage(img => img
                        .WithUrl("https://picsum.photos/seed/product/120/120.jpg")
                        .WithSize(ImageSize.Large)
                        .WithStyle(ImageStyle.Default)))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb
                        .WithText("Premium Product")
                        .WithWeight(TextWeight.Bolder)
                        .WithSize(TextSize.Large))
                    .AddTextBlock(tb => tb
                        .WithText("This is an amazing product that will solve all your problems.")
                        .WithWrap(true)
                        .WithColor(TextColor.Default))
                    .AddTextBlock(tb => tb.WithText("Price: $99.99"))
                    .AddTextBlock(tb => tb.WithText("Rating: ⭐⭐⭐⭐⭐"))
                    .AddTextBlock(tb => tb.WithText("In Stock: Yes"))))
            .AddTextBlock(tb => tb
                .WithText("Key Features")
                .WithWeight(TextWeight.Bolder)
                .WithSize(TextSize.Medium))
            .AddColumnSet(cs => cs
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb.WithText("✓ High Quality"))
                    .AddTextBlock(tb => tb.WithText("✓ Fast Shipping"))
                    .AddTextBlock(tb => tb.WithText("✓ 24/7 Support")))
                .AddColumn(col => col
                    .WithWidth("stretch")
                    .AddTextBlock(tb => tb.WithText("✓ Money Back"))
                    .AddTextBlock(tb => tb.WithText("✓ Easy Returns"))
                    .AddTextBlock(tb => tb.WithText("✓ Premium Materials"))))
            .AddAction(a => a
                .Submit("Add to Cart")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .OpenUrl("https://example.com/details")
                .WithTitle("View Details"))
            .Build();

        return card.ToJson();
    }

    public object HandleCardSubmission(string cardType, Dictionary<string, object> data)
    {
        return new
        {
            success = true,
            message = $"Form submitted successfully for {cardType}",
            timestamp = DateTime.UtcNow,
            data = data
        };
    }
}