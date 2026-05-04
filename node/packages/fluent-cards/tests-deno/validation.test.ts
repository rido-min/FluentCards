// ─────────────────────────────────────────────────────────────────────────────
// Deno test suite for validation: validate, validateAndThrow, error handling
// ─────────────────────────────────────────────────────────────────────────────
//
// Run tests: deno test tests-deno/
// Run this file: deno test tests-deno/validation.test.ts

import { assertEquals, assertExists, assertThrows } from 'jsr:@std/assert@^1';
import type { AdaptiveCard } from '../src/index.ts';
import {
  AdaptiveCardBuilder,
  validate,
  validateAndThrow,
  AdaptiveCardValidationError,
  ValidationSeverity,
} from '../src/index.ts';

Deno.test('validate - returns no issues for valid card', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText('Hello, Deno!'))
    .build();

  const issues = validate(card);
  assertEquals(issues.length, 0);
});

Deno.test('validate - reports MISSING_VERSION error when version is empty', () => {
  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };
  const issues = validate(card);

  const err = issues.find((i) => i.code === 'MISSING_VERSION');
  assertExists(err);
  assertEquals(err!.severity, ValidationSeverity.Error);
  assertEquals(err!.path, 'version');
  assertExists(err!.message);
});

Deno.test('validate - reports MISSING_SCHEMA warning when $schema is absent', () => {
  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '1.5', '$schema': undefined };
  card.body = [{ type: 'TextBlock', text: 'Hi' }];
  const issues = validate(card);

  const warn = issues.find((i) => i.code === 'MISSING_SCHEMA');
  assertExists(warn);
  assertEquals(warn!.severity, ValidationSeverity.Warning);
  assertEquals(warn!.path, '$schema');
});

Deno.test('validate - reports EMPTY_CARD warning when body and actions are both absent', () => {
  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '1.5' };
  const issues = validate(card);

  const warn = issues.find((i) => i.code === 'EMPTY_CARD');
  assertExists(warn);
  assertEquals(warn!.severity, ValidationSeverity.Warning);
});

Deno.test('validate - reports MISSING_TEXT error for blank TextBlock', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText(''))
    .build();

  const issues = validate(card);
  const err = issues.find((i) => i.code === 'MISSING_TEXT');
  assertExists(err);
  assertEquals(err!.severity, ValidationSeverity.Error);
  assertEquals(err!.path, 'body[0].text');
});

Deno.test('validate - reports MISSING_IMAGE_URL error for Image without url', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addImage((img) => img.withUrl(''))
    .build();

  const issues = validate(card);
  const err = issues.find((i) => i.code === 'MISSING_IMAGE_URL');
  assertExists(err);
  assertEquals(err!.severity, ValidationSeverity.Error);
  assertEquals(err!.path, 'body[0].url');
});

Deno.test('validate - reports MISSING_ACTION_URL error for OpenUrl without url', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText('Test'))
    .build();
  card.actions = [{ type: 'Action.OpenUrl', url: '' }];

  const issues = validate(card);
  const err = issues.find((i) => i.code === 'MISSING_ACTION_URL');
  assertExists(err);
  assertEquals(err!.severity, ValidationSeverity.Error);
  assertEquals(err!.path, 'actions[0].url');
});

Deno.test('validate - reports MISSING_INPUT_ID error when input lacks id', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addInputText((input) => input.withPlaceholder('Enter text'))
    .build();

  const issues = validate(card);
  const err = issues.find((i) => i.code === 'MISSING_INPUT_ID');
  assertExists(err);
  assertEquals(err!.severity, ValidationSeverity.Error);
  assertEquals(err!.path, 'body[0].id');
});

Deno.test('validateAndThrow - succeeds on valid card', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText('Valid Card'))
    .build();

  // Should not throw
  validateAndThrow(card);
  assertEquals(true, true); // Confirm we reached here
});

Deno.test('validateAndThrow - throws AdaptiveCardValidationError on invalid card', () => {
  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };

  assertThrows(
    () => {
      validateAndThrow(card);
    },
    AdaptiveCardValidationError,
    'Adaptive Card validation failed',
  );
});

Deno.test('validateAndThrow - error contains validation issues', () => {
  const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };

  try {
    validateAndThrow(card);
    // Should not reach here
    assertEquals(true, false, 'Expected exception was not thrown');
  } catch (err) {
    if (err instanceof AdaptiveCardValidationError) {
      assertExists(err.errors);
      assertEquals(err.errors.length > 0, true);
      assertEquals(err.errors.some((e) => e.code === 'MISSING_VERSION'), true);
    } else {
      throw err;
    }
  }
});

Deno.test('validate - multiple validation errors reported together', () => {
  const card: AdaptiveCard = {
    type: 'AdaptiveCard',
    version: '',
    body: [
      { type: 'TextBlock', text: '' },
      { type: 'Image', url: '' },
    ],
  };

  const issues = validate(card);
  assertEquals(issues.length >= 3, true); // MISSING_VERSION + MISSING_TEXT + MISSING_IMAGE_URL
  assertEquals(issues.some((i) => i.code === 'MISSING_VERSION'), true);
  assertEquals(issues.some((i) => i.code === 'MISSING_TEXT'), true);
  assertEquals(issues.some((i) => i.code === 'MISSING_IMAGE_URL'), true);
});

Deno.test('validate - path property correctly identifies location', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText(''))
    .addTextBlock((tb) => tb.withText('Valid'))
    .addTextBlock((tb) => tb.withText(''))
    .build();

  const issues = validate(card);
  const textErrors = issues.filter((i) => i.code === 'MISSING_TEXT');
  assertEquals(textErrors.length, 2);
  assertEquals(textErrors[0].path, 'body[0].text');
  assertEquals(textErrors[1].path, 'body[2].text');
});
