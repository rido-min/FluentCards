import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  TextBlockBuilder,
  TextSize,
  TextWeight,
  TextColor,
  HorizontalAlignment,
} from 'fluent-cards';

describe('AdaptiveCardBuilder', () => {
  it('creates a card with default version and schema', () => {
    const card = AdaptiveCardBuilder.create().build();
    assert.equal(card.type, 'AdaptiveCard');
    assert.equal(card.version, '1.5');
    assert.ok(card['$schema']);
  });

  it('withVersion sets the version', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.6').build();
    assert.equal(card.version, '1.6');
  });

  it('withSchema sets the schema', () => {
    const card = AdaptiveCardBuilder.create().withSchema('https://example.com/schema.json').build();
    assert.equal(card['$schema'], 'https://example.com/schema.json');
  });

  it('withSchema(undefined) removes the schema', () => {
    const card = AdaptiveCardBuilder.create().withSchema(undefined).build();
    assert.equal(card['$schema'], undefined);
  });

  it('supports fluent chaining', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.6')
      .withSchema('https://example.com/schema.json')
      .build();
    assert.equal(card.version, '1.6');
    assert.equal(card['$schema'], 'https://example.com/schema.json');
  });

  it('addTextBlock adds a TextBlock to the body', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Hello, World!'))
      .build();

    assert.ok(card.body);
    assert.equal(card.body.length, 1);
    assert.equal(card.body[0].type, 'TextBlock');
    if (card.body[0].type === 'TextBlock') {
      assert.equal(card.body[0].text, 'Hello, World!');
    }
  });

  it('adds multiple body elements preserving order', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('First'))
      .addTextBlock((b) => b.withText('Second'))
      .addTextBlock((b) => b.withText('Third'))
      .build();

    assert.equal(card.body!.length, 3);
    assert.equal((card.body![0] as any).text, 'First');
    assert.equal((card.body![1] as any).text, 'Second');
    assert.equal((card.body![2] as any).text, 'Third');
  });

  it('withMetadata sets the metadata webUrl', () => {
    const card = AdaptiveCardBuilder.create().withMetadata('https://example.com').build();
    assert.equal(card.metadata?.webUrl, 'https://example.com');
  });

  it('addAction adds to the actions array', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.openUrl('https://example.com').withTitle('Visit'))
      .build();

    assert.ok(card.actions);
    assert.equal(card.actions.length, 1);
    assert.equal(card.actions[0].type, 'Action.OpenUrl');
    assert.equal(card.actions[0].title, 'Visit');
  });
});

describe('TextBlockBuilder', () => {
  it('builds a TextBlock with all properties', () => {
    const tb = new TextBlockBuilder()
      .withId('tb1')
      .withText('Test Text')
      .withSize(TextSize.Large)
      .withWeight(TextWeight.Bolder)
      .withColor(TextColor.Accent)
      .withWrap(true)
      .withMaxLines(5)
      .withHorizontalAlignment(HorizontalAlignment.Right)
      .build();

    assert.equal(tb.type, 'TextBlock');
    assert.equal(tb.id, 'tb1');
    assert.equal(tb.text, 'Test Text');
    assert.equal(tb.size, TextSize.Large);
    assert.equal(tb.weight, TextWeight.Bolder);
    assert.equal(tb.color, TextColor.Accent);
    assert.equal(tb.wrap, true);
    assert.equal(tb.maxLines, 5);
    assert.equal(tb.horizontalAlignment, HorizontalAlignment.Right);
  });

  it('unset optional properties are undefined', () => {
    const tb = new TextBlockBuilder().withText('Simple').build();
    assert.equal(tb.size, undefined);
    assert.equal(tb.weight, undefined);
    assert.equal(tb.color, undefined);
    assert.equal(tb.wrap, undefined);
  });
});
