namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating a form with input elements.
/// </summary>
public static class FormCardSample
{
    /// <summary>
    /// Creates a contact form card.
    /// </summary>
    /// <returns>An Adaptive Card with a contact form.</returns>
    public static AdaptiveCard CreateContactForm()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Contact Us")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddInputText(i => i
                .WithId("name")
                .WithLabel("Name")
                .WithPlaceholder("Enter your name")
                .IsRequired(true)
                .WithErrorMessage("Name is required"))
            .AddInputText(i => i
                .WithId("email")
                .WithLabel("Email")
                .WithPlaceholder("Enter your email")
                .WithStyle(TextInputStyle.Email)
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("message")
                .WithLabel("Message")
                .WithPlaceholder("How can we help?")
                .IsMultiline(true)
                .WithMaxLength(500))
            .AddAction(a => a
                .Submit("Send Message")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }

    /// <summary>
    /// Creates a survey form card.
    /// </summary>
    /// <returns>An Adaptive Card with a survey form.</returns>
    public static AdaptiveCard CreateSurveyForm()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Customer Satisfaction Survey")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddInputChoiceSet(i => i
                .WithId("satisfaction")
                .WithLabel("How satisfied are you?")
                .AddChoice("Very Satisfied", "5")
                .AddChoice("Satisfied", "4")
                .AddChoice("Neutral", "3")
                .AddChoice("Dissatisfied", "2")
                .AddChoice("Very Dissatisfied", "1")
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("feedback")
                .WithLabel("Additional Feedback")
                .WithPlaceholder("Tell us more...")
                .IsMultiline(true))
            .AddAction(a => a
                .Submit("Submit Survey")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }

    /// <summary>
    /// Creates a registration form card.
    /// </summary>
    /// <returns>An Adaptive Card with a registration form.</returns>
    public static AdaptiveCard CreateRegistrationForm()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Event Registration")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddInputText(i => i
                .WithId("fullName")
                .WithLabel("Full Name")
                .IsRequired(true))
            .AddInputText(i => i
                .WithId("email")
                .WithLabel("Email Address")
                .WithStyle(TextInputStyle.Email)
                .IsRequired(true))
            .AddInputDate(i => i
                .WithId("eventDate")
                .WithLabel("Event Date"))
            .AddInputToggle(i => i
                .WithId("newsletter")
                .WithTitle("Subscribe to newsletter")
                .WithValue("true"))
            .AddAction(a => a
                .Submit("Register")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }
}
