// Deno program: Runs all FluentCards samples
// 
// To run all samples: deno run program.ts

import { toJson, toObject, fromJson, validate, TextSize, TextWeight, TextColor, AdaptiveCardBuilder } from 'jsr:@adaptivecards/fluent';
import type { AdaptiveCard } from 'jsr:@adaptivecards/fluent';
// import { toJson, toObject, fromJson, validate, TextSize, TextWeight, TextColor, AdaptiveCardBuilder } from '../../packages/fluent-cards/src/index.ts';
// import type { AdaptiveCard } from '../../packages/fluent-cards/src/index.ts';

import { createWelcomeCard, createNotificationCard, createImageCard } from './basic_card_sample.ts';
import { createContactForm, createSurveyForm, createRegistrationForm } from './form_card_sample.ts';
import {
  createTwoColumnCard,
  createStyledContainerCard,
  createFactSetCard,
  createNestedContainerCard,
} from './layout_card_sample.ts';
import {
  createRichTextCard,
  createImageSetCard,
  createTableCard,
  createMediaCard,
  createComprehensiveCard,
} from './rich_content_sample.ts';
import { createPeoplePickerCard } from './people_picker_sample.ts';
import { createActionSubmitExecuteCard } from './action_submit_execute_sample.ts';
import { runValidationSamples } from './validation_sample.ts';

console.log('=== FluentCards Demo (Deno) ===\n');

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

// Demonstrate toObject — returns a clean native object (no undefined keys)
console.log('\n=== toObject Demo ===');
const obj = toObject(card);
console.log('Type:', typeof obj);
console.log('Has undefined values:', JSON.stringify(obj) !== JSON.stringify(obj)); // always false
console.log('Body elements:', obj.body?.length ?? 0);
console.log('Ready to embed in an API payload without double-serialization.');

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
printSample('People Picker Card', createPeoplePickerCard());
printSample('Action Submit/Execute Card', createActionSubmitExecuteCard());

// Validation samples
runValidationSamples();
