import {
  AdaptiveCardBuilder,
  TextSize,
  TextWeight,
  TextColor,
  HorizontalAlignment,
  ImageSize,
} from 'fluent-cards';

/** Creates a simple welcome card. */
export function createWelcomeCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Welcome to FluentCards!')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder)
        .withHorizontalAlignment(HorizontalAlignment.Center),
    )
    .addTextBlock((tb) =>
      tb
        .withText('This library helps you create Adaptive Cards using a fluent API.')
        .withWrap(true),
    )
    .addAction((a) => a.openUrl('https://adaptivecards.io').withTitle('Learn More'))
    .build();
}

/** Creates a simple notification card. */
export function createNotificationCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Notification')
        .withSize(TextSize.Medium)
        .withWeight(TextWeight.Bolder)
        .withColor(TextColor.Attention),
    )
    .addTextBlock((tb) =>
      tb
        .withText('You have a new message waiting for your review.')
        .withWrap(true),
    )
    .addAction((a) => a.openUrl('https://example.com/messages').withTitle('View Message'))
    .build();
}

/** Creates a card with an image. */
export function createImageCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addImage((img) =>
      img
        .withUrl('https://adaptivecards.io/content/adaptive-card-50.png')
        .withSize(ImageSize.Large)
        .withHorizontalAlignment(HorizontalAlignment.Center),
    )
    .addTextBlock((tb) =>
      tb
        .withText('Adaptive Cards')
        .withHorizontalAlignment(HorizontalAlignment.Center)
        .withWeight(TextWeight.Bolder),
    )
    .build();
}
