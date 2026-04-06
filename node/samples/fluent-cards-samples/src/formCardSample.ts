import {
  AdaptiveCardBuilder,
  TextSize,
  TextWeight,
  TextInputStyle,
  ActionStyle,
  ChoiceInputStyle,
} from 'fluent-cards';

/** Creates a contact form card. */
export function createContactForm() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Contact Us')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addInputText((i) =>
      i
        .withId('name')
        .withLabel('Name')
        .withPlaceholder('Enter your name')
        .withIsRequired()
        .withErrorMessage('Name is required'),
    )
    .addInputText((i) =>
      i
        .withId('email')
        .withLabel('Email')
        .withPlaceholder('Enter your email')
        .withStyle(TextInputStyle.Email)
        .withIsRequired(),
    )
    .addInputText((i) =>
      i
        .withId('message')
        .withLabel('Message')
        .withPlaceholder('How can we help?')
        .withIsMultiline()
        .withMaxLength(500),
    )
    .addAction((a) => a.submit('Send Message').withStyle(ActionStyle.Positive))
    .build();
}

/** Creates a survey form card. */
export function createSurveyForm() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Customer Satisfaction Survey')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addInputChoiceSet((i) =>
      i
        .withId('satisfaction')
        .withLabel('How satisfied are you?')
        .addChoice('Very Satisfied', '5')
        .addChoice('Satisfied', '4')
        .addChoice('Neutral', '3')
        .addChoice('Dissatisfied', '2')
        .addChoice('Very Dissatisfied', '1')
        .withIsRequired(),
    )
    .addInputText((i) =>
      i
        .withId('feedback')
        .withLabel('Additional Feedback')
        .withPlaceholder('Tell us more...')
        .withIsMultiline(),
    )
    .addAction((a) => a.submit('Submit Survey').withStyle(ActionStyle.Positive))
    .build();
}

/** Creates a registration form card. */
export function createRegistrationForm() {
  return AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb
        .withText('Event Registration')
        .withSize(TextSize.Large)
        .withWeight(TextWeight.Bolder),
    )
    .addInputText((i) =>
      i.withId('fullName').withLabel('Full Name').withIsRequired(),
    )
    .addInputText((i) =>
      i
        .withId('email')
        .withLabel('Email Address')
        .withStyle(TextInputStyle.Email)
        .withIsRequired(),
    )
    .addInputDate((i) => i.withId('eventDate').withLabel('Event Date'))
    .addInputToggle((i) =>
      i
        .withId('newsletter')
        .withTitle('Subscribe to newsletter')
        .withValue('true'),
    )
    .addAction((a) => a.submit('Register').withStyle(ActionStyle.Positive))
    .build();
}
