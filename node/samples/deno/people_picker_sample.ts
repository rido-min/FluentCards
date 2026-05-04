// Deno sample: People picker card using dynamic datasets
// 
// To run: deno run people_picker_sample.ts

import { AdaptiveCardBuilder } from 'jsr:@adaptivecards/fluent';

/** Creates a people picker card that searches users from Microsoft Graph. */
export function createPeoplePickerCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addInputChoiceSet((i) =>
      i
        .withId('people-picker')
        .withLabel('Select users in the whole organization')
        .withIsMultiSelect()
        .withValue('user1,user2')
        .withChoicesData('graph.microsoft.com/users'),
    )
    .addAction((a) => a.submit('Submit'))
    .build();
}

// Run when invoked directly
if (import.meta.main) {
  console.log('=== People Picker Sample ===\n');
  console.log('People Picker Card:', JSON.stringify(createPeoplePickerCard(), null, 2));
}
