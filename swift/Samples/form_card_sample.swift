import FluentCards

func createContactForm() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Contact Us")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addInputText { i in
            i.withID("name")
                .withLabel("Name")
                .withPlaceholder("Enter your name")
                .withIsRequired(true)
                .withErrorMessage("Name is required")
        }
        .addInputText { i in
            i.withID("email")
                .withLabel("Email")
                .withPlaceholder("Enter your email")
                .withStyle(.email)
                .withIsRequired(true)
        }
        .addInputText { i in
            i.withID("message")
                .withLabel("Message")
                .withPlaceholder("How can we help?")
                .withIsMultiline(true)
                .withMaxLength(500)
        }
        .addAction { a in
            a.submit("Send Message").withStyle(.positive)
        }
        .build()
}

func createSurveyForm() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Customer Satisfaction Survey")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addInputChoiceSet { i in
            i.withID("satisfaction")
                .withLabel("How satisfied are you?")
                .addChoice("Very Satisfied", "5")
                .addChoice("Satisfied", "4")
                .addChoice("Neutral", "3")
                .addChoice("Dissatisfied", "2")
                .addChoice("Very Dissatisfied", "1")
                .withIsRequired(true)
        }
        .addInputText { i in
            i.withID("feedback")
                .withLabel("Additional Feedback")
                .withPlaceholder("Tell us more...")
                .withIsMultiline(true)
        }
        .addAction { a in
            a.submit("Submit Survey").withStyle(.positive)
        }
        .build()
}

func createRegistrationForm() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Event Registration")
                .withSize(.large)
                .withWeight(.bolder)
        }
        .addInputText { i in
            i.withID("fullName")
                .withLabel("Full Name")
                .withIsRequired(true)
        }
        .addInputText { i in
            i.withID("email")
                .withLabel("Email Address")
                .withStyle(.email)
                .withIsRequired(true)
        }
        .addInputDate { i in
            i.withID("eventDate")
                .withLabel("Event Date")
        }
        .addInputToggle { i in
            i.withID("newsletter")
                .withTitle("Subscribe to newsletter")
                .withValue("true")
        }
        .addAction { a in
            a.submit("Register").withStyle(.positive)
        }
        .build()
}
