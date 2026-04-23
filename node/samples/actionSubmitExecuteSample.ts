import { AdaptiveCardBuilder, TextSize, TextWeight } from 'fluent-cards';

/** Creates a card with Action.Execute and Action.Submit actions and custom verbs/data. */
export function createActionSubmitExecuteCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.4')
    .addTextBlock((tb) =>
      tb
        .withText('welcome to ac 11')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addTextBlock((tb) => tb.withText('click the buttons below'))
    .addAction((a) =>
      a
        .execute('Test AC Action')
        .withData({ message: 'button clicked !!' })
        .withVerb('testAction'),
    )
    .addAction((a) =>
      a
        .submit('Open Task Module')
        .withData({ msteams: { type: 'task/fetch' } }),
    )
    .addAction((a) => a.execute('Request File Upload').withVerb('requestFileUpload'))
    .build();
}
