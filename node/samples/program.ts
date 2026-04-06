import {
  AdaptiveCardBuilder,
  toJson,
  fromJson,
  validate,
  TextSize,
  TextWeight,
  TextColor,
} from 'fluent-cards';
import type { AdaptiveCard } from 'fluent-cards';

import { createWelcomeCard, createNotificationCard, createImageCard } from './basicCardSample.js';
import { createContactForm, createSurveyForm, createRegistrationForm } from './formCardSample.js';
import {
  createTwoColumnCard,
  createStyledContainerCard,
  createFactSetCard,
  createNestedContainerCard,
} from './layoutCardSample.js';
import {
  createRichTextCard,
  createImageSetCard,
  createTableCard,
  createMediaCard,
  createComprehensiveCard,
} from './richContentSample.js';

console.log('=== FluentCards Demo ===\n');

// Create a card using the fluent builder pattern
const card = AdaptiveCardBuilder.create()
  .withVersion('1.5')
  .addTextBlock((tb) =>
    tb
      .withText('Hello, FluentCards!')
      .withSize(TextSize.Large)
      .withWeight(TextWeight.Bolder)
      .withWrap(true),
  )
  .addTextBlock((tb) =>
    tb.withText('This card was built with a fluent interface.').withColor(TextColor.Accent),
  )
  .addAction((a) => a.openUrl('https://adaptivecards.io').withTitle('Learn More'))
  .build();

// Serialize to JSON
const json = toJson(card);
console.log(json);

// Demonstrate roundtrip serialization
console.log('\n=== Roundtrip Test ===');
const deserialized = fromJson(json);
if (deserialized) {
  console.log('✓ Successfully deserialized card');
  console.log(`  Version: ${deserialized.version}`);
  console.log(`  Body elements: ${deserialized.body?.length ?? 0}`);
  console.log(`  Actions: ${deserialized.actions?.length ?? 0}`);
}

// Demonstrate validation
console.log('\n=== Validation Test ===');
const issues = validate(card);
if (issues.length === 0) {
  console.log('✓ Card is valid!');
} else {
  console.log(`⚠ Found ${issues.length} validation issue(s):`);
  for (const issue of issues) {
    console.log(`  [${issue.severity}] ${issue.path}: ${issue.message}`);
  }
}

// Demonstrate validation with invalid card
console.log('\n=== Validation with Invalid Card ===');
const invalidCard: AdaptiveCard = { type: 'AdaptiveCard', version: '' };
const invalidIssues = validate(invalidCard);
console.log(`Found ${invalidIssues.length} validation issue(s):`);
for (const issue of invalidIssues) {
  console.log(`  [${issue.severity}] ${issue.code} at '${issue.path}': ${issue.message}`);
}

// Run all samples and print their JSON
function printSample(name: string, card: object) {
  console.log(`\n=== ${name} ===`);
  console.log(toJson(card as AdaptiveCard));
}

printSample('Welcome Card', createWelcomeCard());
printSample('Notification Card', createNotificationCard());
printSample('Image Card', createImageCard());
printSample('Contact Form', createContactForm());
printSample('Survey Form', createSurveyForm());
printSample('Registration Form', createRegistrationForm());
printSample('Two Column Card', createTwoColumnCard());
printSample('Styled Container Card', createStyledContainerCard());
printSample('Fact Set Card', createFactSetCard());
printSample('Nested Container Card', createNestedContainerCard());
printSample('Rich Text Card', createRichTextCard());
printSample('Image Set Card', createImageSetCard());
printSample('Table Card', createTableCard());
printSample('Media Card', createMediaCard());
printSample('Comprehensive Card', createComprehensiveCard());
