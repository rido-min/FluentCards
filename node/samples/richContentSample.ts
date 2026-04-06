import {
  AdaptiveCardBuilder,
  TextSize,
  TextWeight,
  TextColor,
  ImageSize,
  HorizontalAlignment,
  ActionStyle,
} from 'fluent-cards';
import type { TableColumnDefinition, TableRow } from 'fluent-cards';

/** Creates a card with rich text formatting. */
export function createRichTextCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addRichTextBlock((rtb) =>
      rtb
        .addTextRun((tr) => tr.withText('Welcome ').withSize(TextSize.Large))
        .addTextRun((tr) =>
          tr
            .withText('to FluentCards!')
            .withSize(TextSize.Large)
            .withWeight(TextWeight.Bolder)
            .withColor(TextColor.Accent),
        )
        .addTextRun((tr) =>
          tr
            .withText('\n\nThis demonstrates ')
            .withSize(TextSize.Default),
        )
        .addTextRun((tr) =>
          tr
            .withText('rich text formatting')
            .withWeight(TextWeight.Bolder)
            .withColor(TextColor.Good),
        )
        .addTextRun((tr) =>
          tr.withText(' with multiple text runs.').withSize(TextSize.Default),
        ),
    )
    .build();
}

/** Creates a card with an image set gallery. */
export function createImageSetCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Photo Gallery')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addImageSet((imgSet) =>
      imgSet
        .withImageSize(ImageSize.Medium)
        .addImage((img) =>
          img.withUrl('https://adaptivecards.io/content/adaptive-card-50.png'),
        )
        .addImage((img) =>
          img.withUrl('https://adaptivecards.io/content/adaptive-card-50.png'),
        )
        .addImage((img) =>
          img.withUrl('https://adaptivecards.io/content/adaptive-card-50.png'),
        ),
    )
    .addTextBlock((tb) =>
      tb.withText('View more photos in the gallery').withWrap(true),
    )
    .build();
}

/** Creates a card with a table. */
export function createTableCard() {
  const columns: TableColumnDefinition[] = [
    { width: '2' },
    { width: '1' },
    { width: '1' },
  ];

  const rows: TableRow[] = [
    {
      type: 'TableRow',
      cells: [
        { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Product A' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '150' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '$15,000' }] },
      ],
    },
    {
      type: 'TableRow',
      cells: [
        { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Product B' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '200' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '$20,000' }] },
      ],
    },
    {
      type: 'TableRow',
      cells: [
        { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Product C' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '100' }] },
        { type: 'TableCell', items: [{ type: 'TextBlock', text: '$10,000' }] },
      ],
    },
  ];

  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Sales Report')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addTable((table) => {
      for (const col of columns) table.addColumn(col);
      for (const row of rows) table.addRow(row);
    })
    .build();
}

/** Creates a card with media player. */
export function createMediaCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Video Tutorial')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addMedia((media) =>
      media
        .addSource('https://example.com/video.mp4', 'video/mp4')
        .withPoster('https://example.com/poster.jpg')
        .withAltText('Getting started with FluentCards'),
    )
    .addTextBlock((tb) =>
      tb
        .withText('Watch this tutorial to learn the basics of FluentCards.')
        .withWrap(true),
    )
    .build();
}

/** Creates a comprehensive card combining multiple rich content types. */
export function createComprehensiveCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Product Launch Announcement')
        .withSize(TextSize.ExtraLarge)
        .withWeight(TextWeight.Bolder)
        .withColor(TextColor.Accent),
    )
    .addImage((img) =>
      img
        .withUrl('https://adaptivecards.io/content/adaptive-card-50.png')
        .withSize(ImageSize.Large)
        .withHorizontalAlignment(HorizontalAlignment.Center),
    )
    .addRichTextBlock((rtb) =>
      rtb
        .addTextRun((tr) =>
          tr.withText('Introducing ').withSize(TextSize.Medium),
        )
        .addTextRun((tr) =>
          tr
            .withText('FluentCards 2.0')
            .withSize(TextSize.Medium)
            .withWeight(TextWeight.Bolder)
            .withColor(TextColor.Good),
        ),
    )
    .addFactSet((fs) =>
      fs
        .addFact('Release Date', 'January 1, 2025')
        .addFact('Version', '2.0.0')
        .addFact('License', 'MIT'),
    )
    .addAction((a) =>
      a
        .openUrl('https://github.com/rido-min/FluentCards')
        .withTitle('View on GitHub'),
    )
    .addAction((a) =>
      a.submit('Get Notified').withStyle(ActionStyle.Positive),
    )
    .build();
}
