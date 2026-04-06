from fluent_cards import (
    AdaptiveCardBuilder,
    TextSize,
    TextWeight,
    TextColor,
    HorizontalAlignment,
    ImageSize,
)


def create_welcome_card():
    """Creates a simple welcome card."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Welcome to FluentCards!')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
            .with_horizontal_alignment(HorizontalAlignment.Center)
        )
        .add_text_block(
            lambda tb: tb.with_text(
                'This library helps you create Adaptive Cards using a fluent API.'
            ).with_wrap(True)
        )
        .add_action(
            lambda a: a.open_url('https://adaptivecards.io').with_title('Learn More')
        )
        .build()
    )


def create_notification_card():
    """Creates a simple notification card."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Notification')
            .with_size(TextSize.Medium)
            .with_weight(TextWeight.Bolder)
            .with_color(TextColor.Attention)
        )
        .add_text_block(
            lambda tb: tb.with_text(
                'You have a new message waiting for your review.'
            ).with_wrap(True)
        )
        .add_action(
            lambda a: a.open_url('https://example.com/messages').with_title(
                'View Message'
            )
        )
        .build()
    )


def create_image_card():
    """Creates a card with an image."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_image(
            lambda img: img.with_url(
                'https://adaptivecards.io/content/adaptive-card-50.png'
            )
            .with_size(ImageSize.Large)
            .with_horizontal_alignment(HorizontalAlignment.Center)
        )
        .add_text_block(
            lambda tb: tb.with_text('Adaptive Cards')
            .with_horizontal_alignment(HorizontalAlignment.Center)
            .with_weight(TextWeight.Bolder)
        )
        .build()
    )
