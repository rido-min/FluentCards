from fluent_cards import (
    AdaptiveCardBuilder,
    TextSize,
    TextWeight,
    TextColor,
    ImageSize,
    ActionStyle,
    ContainerStyle,
)


def create_two_column_card():
    """Creates a card with a two-column layout."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Product Information')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_column_set(
            lambda cs: cs.add_column(
                lambda col: col.with_width('auto').add_image(
                    lambda img: img.with_url(
                        'https://adaptivecards.io/content/adaptive-card-50.png'
                    ).with_size(ImageSize.Medium)
                )
            ).add_column(
                lambda col: col.with_width('stretch')
                .add_text_block(
                    lambda tb: tb.with_text('Adaptive Cards SDK').with_weight(
                        TextWeight.Bolder
                    )
                )
                .add_text_block(
                    lambda tb: tb.with_text(
                        'Create platform-agnostic UI snippets'
                    ).with_wrap(True)
                )
                .add_text_block(
                    lambda tb: tb.with_text('$49.99')
                    .with_color(TextColor.Good)
                    .with_size(TextSize.Large)
                )
            )
        )
        .add_action(
            lambda a: a.submit('Add to Cart').with_style(ActionStyle.Positive)
        )
        .build()
    )


def create_styled_container_card():
    """Creates a card with containers and styled sections."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_container(
            lambda c: c.with_style(ContainerStyle.Emphasis)
            .add_text_block(
                lambda tb: tb.with_text('Important Notice')
                .with_size(TextSize.Large)
                .with_weight(TextWeight.Bolder)
            )
            .add_text_block(
                lambda tb: tb.with_text(
                    'This is an emphasized section with important information.'
                ).with_wrap(True)
            )
        )
        .add_container(
            lambda c: c.add_text_block(
                lambda tb: tb.with_text('Regular Section').with_weight(
                    TextWeight.Bolder
                )
            ).add_text_block(
                lambda tb: tb.with_text(
                    'This is a normal section with regular styling.'
                ).with_wrap(True)
            )
        )
        .add_container(
            lambda c: c.with_style(ContainerStyle.Accent)
            .add_text_block(
                lambda tb: tb.with_text('Highlighted Section').with_weight(
                    TextWeight.Bolder
                )
            )
            .add_text_block(
                lambda tb: tb.with_text(
                    'This section uses accent styling to stand out.'
                ).with_wrap(True)
            )
        )
        .build()
    )


def create_fact_set_card():
    """Creates a card with a fact set for displaying key-value pairs."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Meeting Details')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_fact_set(
            lambda fs: fs.add_fact('Date', 'December 15, 2024')
            .add_fact('Time', '2:00 PM - 3:00 PM')
            .add_fact('Location', 'Conference Room A')
            .add_fact('Organizer', 'John Smith')
            .add_fact('Attendees', '12 people')
        )
        .add_action(
            lambda a: a.open_url('https://example.com/meeting/123').with_title(
                'Join Meeting'
            )
        )
        .build()
    )


def create_nested_container_card():
    """Creates a card with nested containers for complex layouts."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Dashboard')
            .with_size(TextSize.ExtraLarge)
            .with_weight(TextWeight.Bolder)
        )
        .add_container(
            lambda c: c.with_style(ContainerStyle.Emphasis)
            .add_text_block(
                lambda tb: tb.with_text('Statistics')
                .with_size(TextSize.Large)
                .with_weight(TextWeight.Bolder)
            )
            .add_column_set(
                lambda cs: cs.add_column(
                    lambda col: col.with_width('stretch').add_container(
                        lambda cont: cont.with_style(ContainerStyle.Good)
                        .add_text_block(
                            lambda tb: tb.with_text('Active Users').with_weight(
                                TextWeight.Bolder
                            )
                        )
                        .add_text_block(
                            lambda tb: tb.with_text('1,234').with_size(
                                TextSize.ExtraLarge
                            )
                        )
                    )
                ).add_column(
                    lambda col: col.with_width('stretch').add_container(
                        lambda cont: cont.with_style(ContainerStyle.Attention)
                        .add_text_block(
                            lambda tb: tb.with_text('Pending Issues').with_weight(
                                TextWeight.Bolder
                            )
                        )
                        .add_text_block(
                            lambda tb: tb.with_text('42').with_size(
                                TextSize.ExtraLarge
                            )
                        )
                    )
                )
            )
        )
        .build()
    )
