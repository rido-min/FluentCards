import {
  AdaptiveCardBuilder,
  TextSize,
  TextWeight,
  TextColor,
  ImageSize,
  ActionStyle,
  ContainerStyle,
} from 'fluent-cards';

/** Creates a card with a two-column layout. */
export function createTwoColumnCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Product Information')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addColumnSet((cs) =>
      cs
        .addColumn((col) =>
          col
            .withWidth('auto')
            .addImage((img) =>
              img
                .withUrl('https://adaptivecards.io/content/adaptive-card-50.png')
                .withSize(ImageSize.Medium),
            ),
        )
        .addColumn((col) =>
          col
            .withWidth('stretch')
            .addTextBlock((tb) =>
              tb.withText('Adaptive Cards SDK').withWeight(TextWeight.Bolder),
            )
            .addTextBlock((tb) =>
              tb
                .withText('Create platform-agnostic UI snippets')
                .withWrap(true),
            )
            .addTextBlock((tb) =>
              tb
                .withText('$49.99')
                .withColor(TextColor.Good)
                .withSize(TextSize.Large),
            ),
        ),
    )
    .addAction((a) => a.submit('Add to Cart').withStyle(ActionStyle.Positive))
    .build();
}

/** Creates a card with containers and styled sections. */
export function createStyledContainerCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addContainer((c) =>
      c
        .withStyle(ContainerStyle.Emphasis)
        .addTextBlock((tb) =>
          tb
            .withText('Important Notice')
            .withSize(TextSize.Large)
            .withWeight(TextWeight.Bolder),
        )
        .addTextBlock((tb) =>
          tb
            .withText('This is an emphasized section with important information.')
            .withWrap(true),
        ),
    )
    .addContainer((c) =>
      c
        .addTextBlock((tb) =>
          tb.withText('Regular Section').withWeight(TextWeight.Bolder),
        )
        .addTextBlock((tb) =>
          tb
            .withText('This is a normal section with regular styling.')
            .withWrap(true),
        ),
    )
    .addContainer((c) =>
      c
        .withStyle(ContainerStyle.Accent)
        .addTextBlock((tb) =>
          tb.withText('Highlighted Section').withWeight(TextWeight.Bolder),
        )
        .addTextBlock((tb) =>
          tb
            .withText('This section uses accent styling to stand out.')
            .withWrap(true),
        ),
    )
    .build();
}

/** Creates a card with a fact set for displaying key-value pairs. */
export function createFactSetCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Meeting Details')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addFactSet((fs) =>
      fs
        .addFact('Date', 'December 15, 2024')
        .addFact('Time', '2:00 PM - 3:00 PM')
        .addFact('Location', 'Conference Room A')
        .addFact('Organizer', 'John Smith')
        .addFact('Attendees', '12 people'),
    )
    .addAction((a) =>
      a.openUrl('https://example.com/meeting/123').withTitle('Join Meeting'),
    )
    .build();
}

/** Creates a card with nested containers for complex layouts. */
export function createNestedContainerCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Dashboard')
        .withSize(TextSize.ExtraLarge)
        .withWeight(TextWeight.Bolder),
    )
    .addContainer((c) =>
      c
        .withStyle(ContainerStyle.Emphasis)
        .addTextBlock((tb) =>
          tb
            .withText('Statistics')
            .withSize(TextSize.Large)
            .withWeight(TextWeight.Bolder),
        )
        .addColumnSet((cs) =>
          cs
            .addColumn((col) =>
              col
                .withWidth('stretch')
                .addContainer((cont) =>
                  cont
                    .withStyle(ContainerStyle.Good)
                    .addTextBlock((tb) =>
                      tb
                        .withText('Active Users')
                        .withWeight(TextWeight.Bolder),
                    )
                    .addTextBlock((tb) =>
                      tb.withText('1,234').withSize(TextSize.ExtraLarge),
                    ),
                ),
            )
            .addColumn((col) =>
              col
                .withWidth('stretch')
                .addContainer((cont) =>
                  cont
                    .withStyle(ContainerStyle.Attention)
                    .addTextBlock((tb) =>
                      tb
                        .withText('Pending Issues')
                        .withWeight(TextWeight.Bolder),
                    )
                    .addTextBlock((tb) =>
                      tb.withText('42').withSize(TextSize.ExtraLarge),
                    ),
                ),
            ),
        ),
    )
    .build();
}
