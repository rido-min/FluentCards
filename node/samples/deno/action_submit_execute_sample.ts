// Deno sample: Action.Submit and Action.Execute with custom verbs and data
// 
// To run: deno run action_submit_execute_sample.ts

import { AdaptiveCardBuilder, TextSize, TextWeight } from 'jsr:@adaptivecards/fluent';
// import { AdaptiveCardBuilder, TextSize, TextWeight } from '../../packages/fluent-cards/src/index.ts';

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

// Run when invoked directly
if (import.meta.main) {
  console.log('=== Action Submit/Execute Sample ===\n');
  console.log('Action Card:', JSON.stringify(createActionSubmitExecuteCard(), null, 2));
}
