import FluentCards

func createWelcomeCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Welcome to FluentCards!")
                .withSize(.large)
                .withWeight(.bolder)
                .withHorizontalAlignment(.center)
        }
        .addTextBlock { tb in
            tb.withText("This library helps you create Adaptive Cards using a fluent API.")
                .withWrap(true)
        }
        .addAction { a in
            a.openURL("https://adaptivecards.io").withTitle("Learn More")
        }
        .build()
}

func createNotificationCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("Notification")
                .withSize(.medium)
                .withWeight(.bolder)
                .withColor(.attention)
        }
        .addTextBlock { tb in
            tb.withText("You have a new message waiting for you.")
                .withWrap(true)
        }
        .addAction { a in
            a.openURL("https://example.com/messages").withTitle("View Messages")
        }
        .build()
}

func createImageCard() -> Card {
    return AdaptiveCardBuilder()
        .withVersion("1.5")
        .addImage { img in
            img.withURL("https://adaptivecards.io/content/adaptive-card-50.png")
                .withSize(.medium)
                .withHorizontalAlignment(.center)
        }
        .addTextBlock { tb in
            tb.withText("Adaptive Cards")
                .withSize(.large)
                .withWeight(.bolder)
                .withHorizontalAlignment(.center)
        }
        .addTextBlock { tb in
            tb.withText("Platform-agnostic snippets of UI")
                .withWrap(true)
                .withHorizontalAlignment(.center)
        }
        .build()
}
