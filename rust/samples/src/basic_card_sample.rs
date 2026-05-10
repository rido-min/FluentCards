use fluent_cards::*;

pub fn create_welcome_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Welcome to FluentCards!")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder)
                .with_horizontal_alignment(HorizontalAlignment::Center);
        })
        .add_text_block(|tb| {
            tb.with_text("This library helps you create Adaptive Cards using a fluent API.")
                .with_wrap(true);
        })
        .add_action(|a| {
            a.open_url("https://adaptivecards.io")
                .with_title("Learn More");
        })
        .build()
}

pub fn create_notification_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Notification")
                .with_size(TextSize::Medium)
                .with_weight(TextWeight::Bolder)
                .with_color(TextColor::Attention);
        })
        .add_text_block(|tb| {
            tb.with_text("You have a new message waiting for you.")
                .with_wrap(true);
        })
        .add_action(|a| {
            a.open_url("https://example.com/messages")
                .with_title("View Messages");
        })
        .build()
}

pub fn create_image_card() -> Card {
    AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_image(|img| {
            img.with_url("https://adaptivecards.io/content/adaptive-card-50.png")
                .with_size(ImageSize::Medium)
                .with_horizontal_alignment(HorizontalAlignment::Center);
        })
        .add_text_block(|tb| {
            tb.with_text("Adaptive Cards")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder)
                .with_horizontal_alignment(HorizontalAlignment::Center);
        })
        .add_text_block(|tb| {
            tb.with_text("Platform-agnostic snippets of UI")
                .with_wrap(true)
                .with_horizontal_alignment(HorizontalAlignment::Center);
        })
        .build()
}
