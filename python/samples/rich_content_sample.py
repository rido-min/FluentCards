from fluent_cards import (
    AdaptiveCardBuilder,
    TextSize,
    TextWeight,
    TextColor,
    ImageSize,
    HorizontalAlignment,
    ActionStyle,
)


def create_rich_text_card():
    """Creates a card with rich text formatting."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_rich_text_block(
            lambda rtb: rtb.add_text_run(
                lambda tr: tr.with_text('Welcome ').with_size(TextSize.Large)
            )
            .add_text_run(
                lambda tr: tr.with_text('to FluentCards!')
                .with_size(TextSize.Large)
                .with_weight(TextWeight.Bolder)
                .with_color(TextColor.Accent)
            )
            .add_text_run(
                lambda tr: tr.with_text('\n\nThis demonstrates ').with_size(
                    TextSize.Default
                )
            )
            .add_text_run(
                lambda tr: tr.with_text('rich text formatting')
                .with_weight(TextWeight.Bolder)
                .with_color(TextColor.Good)
            )
            .add_text_run(
                lambda tr: tr.with_text(' with multiple text runs.').with_size(
                    TextSize.Default
                )
            )
        )
        .build()
    )


def create_image_set_card():
    """Creates a card with an image set gallery."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Photo Gallery')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_image_set(
            lambda img_set: img_set.with_image_size(ImageSize.Medium)
            .add_image(
                lambda img: img.with_url(
                    'https://adaptivecards.io/content/adaptive-card-50.png'
                )
            )
            .add_image(
                lambda img: img.with_url(
                    'https://adaptivecards.io/content/adaptive-card-50.png'
                )
            )
            .add_image(
                lambda img: img.with_url(
                    'https://adaptivecards.io/content/adaptive-card-50.png'
                )
            )
        )
        .add_text_block(
            lambda tb: tb.with_text('View more photos in the gallery').with_wrap(True)
        )
        .build()
    )


def create_table_card():
    """Creates a card with a table."""
    columns = [{'width': '2'}, {'width': '1'}, {'width': '1'}]

    rows = [
        {
            'type': 'TableRow',
            'cells': [
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Product A'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '150'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '$15,000'}]},
            ],
        },
        {
            'type': 'TableRow',
            'cells': [
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Product B'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '200'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '$20,000'}]},
            ],
        },
        {
            'type': 'TableRow',
            'cells': [
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': 'Product C'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '100'}]},
                {'type': 'TableCell', 'items': [{'type': 'TextBlock', 'text': '$10,000'}]},
            ],
        },
    ]

    def build_table(table):
        for col in columns:
            table.add_column(col)
        for row in rows:
            table.add_row(row)

    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Sales Report')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_table(build_table)
        .build()
    )


def create_media_card():
    """Creates a card with media player."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Video Tutorial')
            .with_size(TextSize.Large)
            .with_weight(TextWeight.Bolder)
        )
        .add_media(
            lambda media: media.add_source(
                'https://example.com/video.mp4', 'video/mp4'
            )
            .with_poster('https://example.com/poster.jpg')
            .with_alt_text('Getting started with FluentCards')
        )
        .add_text_block(
            lambda tb: tb.with_text(
                'Watch this tutorial to learn the basics of FluentCards.'
            ).with_wrap(True)
        )
        .build()
    )


def create_comprehensive_card():
    """Creates a comprehensive card combining multiple rich content types."""
    return (
        AdaptiveCardBuilder.create()
        .with_version('1.5')
        .add_text_block(
            lambda tb: tb.with_text('Product Launch Announcement')
            .with_size(TextSize.ExtraLarge)
            .with_weight(TextWeight.Bolder)
            .with_color(TextColor.Accent)
        )
        .add_image(
            lambda img: img.with_url(
                'https://adaptivecards.io/content/adaptive-card-50.png'
            )
            .with_size(ImageSize.Large)
            .with_horizontal_alignment(HorizontalAlignment.Center)
        )
        .add_rich_text_block(
            lambda rtb: rtb.add_text_run(
                lambda tr: tr.with_text('Introducing ').with_size(TextSize.Medium)
            ).add_text_run(
                lambda tr: tr.with_text('FluentCards 2.0')
                .with_size(TextSize.Medium)
                .with_weight(TextWeight.Bolder)
                .with_color(TextColor.Good)
            )
        )
        .add_fact_set(
            lambda fs: fs.add_fact('Release Date', 'January 1, 2025')
            .add_fact('Version', '2.0.0')
            .add_fact('License', 'MIT')
        )
        .add_action(
            lambda a: a.open_url('https://github.com/rido-min/FluentCards').with_title(
                'View on GitHub'
            )
        )
        .add_action(
            lambda a: a.submit('Get Notified').with_style(ActionStyle.Positive)
        )
        .build()
    )
