import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  InputTextBuilder,
  validate,
  validateAndThrow,
  AdaptiveCardValidationError,
  ValidationSeverity,
  toJson,
} from 'fluent-cards';
import type { AdaptiveCard } from 'fluent-cards';

describe('validate', () => {
  it('returns no issues for a valid card', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText('Hello World'))
      .build();

    const issues = validate(card);
    assert.equal(issues.length, 0);
  });

  it('reports MISSING_VERSION error when version is empty', () => {
    const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };
    const issues = validate(card);

    const err = issues.find((i) => i.code === 'MISSING_VERSION');
    assert.ok(err);
    assert.equal(err!.severity, ValidationSeverity.Error);
    assert.equal(err!.path, 'version');
  });

  it('reports MISSING_SCHEMA warning when $schema is absent', () => {
    const card: AdaptiveCard = { type: 'AdaptiveCard', version: '1.5', '$schema': undefined };
    card.body = [{ type: 'TextBlock', text: 'Hi' }];
    const issues = validate(card);

    const warn = issues.find((i) => i.code === 'MISSING_SCHEMA');
    assert.ok(warn);
    assert.equal(warn!.severity, ValidationSeverity.Warning);
    assert.equal(warn!.path, '$schema');
  });

  it('reports EMPTY_CARD warning when body and actions are both absent', () => {
    const card: AdaptiveCard = { type: 'AdaptiveCard', version: '1.5' };
    const issues = validate(card);

    assert.ok(issues.some((i) => i.code === 'EMPTY_CARD' && i.severity === ValidationSeverity.Warning));
  });

  it('reports EMPTY_TEXT warning for blank TextBlock', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText(''))
      .build();

    const issues = validate(card);
    const warn = issues.find((i) => i.code === 'EMPTY_TEXT');
    assert.ok(warn);
    assert.equal(warn!.severity, ValidationSeverity.Warning);
    assert.equal(warn!.path, 'body[0].text');
  });

  it('reports MISSING_IMAGE_URL error for Image without url', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addImage((b) => b.withUrl(''))
      .build();

    const issues = validate(card);
    const err = issues.find((i) => i.code === 'MISSING_IMAGE_URL');
    assert.ok(err);
    assert.equal(err!.severity, ValidationSeverity.Error);
    assert.equal(err!.path, 'body[0].url');
  });

  it('reports INVALID_IMAGE_URL warning for a relative url', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addImage((b) => b.withUrl('not-a-url'))
      .build();

    const issues = validate(card);
    const warn = issues.find((i) => i.code === 'INVALID_IMAGE_URL');
    assert.ok(warn);
    assert.equal(warn!.severity, ValidationSeverity.Warning);
    assert.equal(warn!.path, 'body[0].url');
  });

  it('reports MISSING_INPUT_ID error for input without id', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addInputText((b) => b.withPlaceholder('Enter text'))
      .build();

    const issues = validate(card);
    const err = issues.find((i) => i.code === 'MISSING_INPUT_ID');
    assert.ok(err);
    assert.equal(err!.severity, ValidationSeverity.Error);
    assert.equal(err!.path, 'body[0].id');
  });

  it('reports TOO_MANY_ACTIONS warning when more than 5 actions', () => {
    const builder = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText('Test'));
    for (let i = 0; i < 6; i++) {
      builder.addAction((a) => a.openUrl(`https://example.com/${i}`).withTitle(`Action ${i}`));
    }
    const issues = validate(builder.build());

    const warn = issues.find((i) => i.code === 'TOO_MANY_ACTIONS');
    assert.ok(warn);
    assert.equal(warn!.severity, ValidationSeverity.Warning);
    assert.equal(warn!.path, 'actions');
  });

  it('reports MISSING_ACTION_URL error for OpenUrl without url', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText('Test'))
      .build();
    card.actions = [{ type: 'Action.OpenUrl', url: '' }];

    const issues = validate(card);
    const err = issues.find((i) => i.code === 'MISSING_ACTION_URL');
    assert.ok(err);
    assert.equal(err!.path, 'actions[0].url');
  });

  it('reports INVALID_ACTION_URL warning for a relative OpenUrl', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText('Test'))
      .addAction((a) => a.openUrl('not-a-url').withTitle('Test'))
      .build();

    const issues = validate(card);
    const warn = issues.find((i) => i.code === 'INVALID_ACTION_URL');
    assert.ok(warn);
    assert.equal(warn!.path, 'actions[0].url');
  });

  it('reports UNKNOWN_VERSION warning for an unrecognised version', () => {
    const card: AdaptiveCard = {
      type: 'AdaptiveCard',
      version: '9.9',
      body: [{ type: 'TextBlock', text: 'Test' }],
    };
    const issues = validate(card);

    const warn = issues.find((i) => i.code === 'UNKNOWN_VERSION');
    assert.ok(warn);
    assert.equal(warn!.severity, ValidationSeverity.Warning);
  });

  it('validates nested container items', () => {
    const inputNoId = new InputTextBuilder().withPlaceholder('Test').build();
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addContainer((c) =>
        c.addTextBlock((b) => b.withText('')).addElement(inputNoId),
      )
      .build();

    const issues = validate(card);
    assert.ok(issues.some((i) => i.code === 'EMPTY_TEXT' && i.path === 'body[0].items[0].text'));
    assert.ok(issues.some((i) => i.code === 'MISSING_INPUT_ID' && i.path === 'body[0].items[1].id'));
  });

  it('validates nested columnset items', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addColumnSet((cs) =>
        cs.addColumn((col) => col.addImage((img) => img.withUrl(''))),
      )
      .build();

    const issues = validate(card);
    const err = issues.find((i) => i.code === 'MISSING_IMAGE_URL');
    assert.ok(err);
    assert.equal(err!.path, 'body[0].columns[0].items[0].url');
  });
});

describe('validateAndThrow', () => {
  it('throws AdaptiveCardValidationError when errors exist', () => {
    const card: AdaptiveCard = { type: 'AdaptiveCard', version: '' };
    assert.throws(() => validateAndThrow(card), AdaptiveCardValidationError);
  });

  it('does not throw for warnings-only cards', () => {
    const card: AdaptiveCard = {
      type: 'AdaptiveCard',
      version: '1.5',
      '$schema': undefined,
      body: [{ type: 'TextBlock', text: 'Test' }],
    };
    assert.doesNotThrow(() => validateAndThrow(card));
  });

  it('does not throw for a fully valid card', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.5')
      .addTextBlock((b) => b.withText('Hello World'))
      .build();
    assert.doesNotThrow(() => validateAndThrow(card));
  });
});

describe('AdaptiveCardValidationError', () => {
  it('formats a single error correctly', () => {
    const err = new AdaptiveCardValidationError([
      { severity: ValidationSeverity.Error, code: 'TEST', path: 'test.path', message: 'Test error message' },
    ]);
    assert.equal(err.message, 'Adaptive Card validation failed: Test error message');
  });

  it('formats multiple errors correctly', () => {
    const err = new AdaptiveCardValidationError([
      { severity: ValidationSeverity.Error, code: 'T1', path: 'path1', message: 'Error 1' },
      { severity: ValidationSeverity.Error, code: 'T2', path: 'path2', message: 'Error 2' },
    ]);
    assert.ok(err.message.includes('Adaptive Card validation failed with 2 errors'));
    assert.ok(err.message.includes('[path1] Error 1'));
    assert.ok(err.message.includes('[path2] Error 2'));
  });

  it('exposes the errors array', () => {
    const errors = [
      { severity: ValidationSeverity.Error, code: 'T1', path: 'p', message: 'M' },
    ];
    const ex = new AdaptiveCardValidationError(errors);
    assert.deepEqual(ex.errors, errors);
  });
});
