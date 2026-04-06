import { AdaptiveCardBuilder } from 'fluent-cards';

/** Creates a people picker card that searches users from Microsoft Graph. */
export function createPeoplePickerCard() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addInputChoiceSet((i) =>
      i
        .withId('people-picker')
        .withLabel('Select users in the whole organization')
        .isMultiSelect()
        .withValue('user1,user2')
        .withChoicesData('graph.microsoft.com/users'),
    )
    .addAction((a) => a.submit('Submit'))
    .build();
}
