import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  toJson,
  fromJson,
  TextSize,
  TextWeight,
  TextColor,
  HorizontalAlignment,
  ActionStyle,
  AssociatedInputs,
} from 'fluent-cards';
import type { AdaptiveCard, TextBlock, OpenUrlAction, SubmitAction } from 'fluent-cards';

describe('toJson', () => {
  it('includes type and version', () => {
    const card = AdaptiveCardBuilder.create().build();
    const json = toJson(card);
    assert.ok(json.includes('"type": "AdaptiveCard"'));
    assert.ok(json.includes('"version": "1.5"'));
  });

  it('includes $schema property', () => {
    const json = toJson(AdaptiveCardBuilder.create().build());
    assert.ok(json.includes('"$schema"'));
    assert.ok(json.includes('"https://adaptivecards.io/schemas/1.5.0/adaptive-card.json"'));
  });

  it('omits undefined optional properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Simple Text'))
      .build();
    const json = toJson(card);
    assert.ok(!json.includes('"size"'));
    assert.ok(!json.includes('"weight"'));
    assert.ok(!json.includes('"color"'));
    assert.ok(!json.includes('"wrap"'));
    assert.ok(!json.includes('"maxLines"'));
  });

  it('serializes TextBlock with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) =>
        b
          .withId('textBlock1')
          .withText('Sample Text')
          .withSize(TextSize.Large)
          .withWeight(TextWeight.Bolder)
          .withColor(TextColor.Accent)
          .withWrap(true)
          .withMaxLines(3)
          .withHorizontalAlignment(HorizontalAlignment.Center),
      )
      .build();
    const json = toJson(card);

    assert.ok(json.includes('"id": "textBlock1"'));
    assert.ok(json.includes('"text": "Sample Text"'));
    assert.ok(json.includes('"size": "large"'));
    assert.ok(json.includes('"weight": "bolder"'));
    assert.ok(json.includes('"color": "accent"'));
    assert.ok(json.includes('"wrap": true'));
    assert.ok(json.includes('"maxLines": 3'));
    assert.ok(json.includes('"horizontalAlignment": "center"'));
  });

  it('serializes enum values as camelCase strings', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('x').withSize(TextSize.ExtraLarge).withColor(TextColor.Attention))
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"size": "extraLarge"'));
    assert.ok(json.includes('"color": "attention"'));
  });

  it('includes type discriminator on body elements', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Test'))
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"type": "TextBlock"'));
  });

  it('includes type discriminator on actions', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.openUrl('https://example.com'))
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"type": "Action.OpenUrl"'));
  });

  it('serializes action style as camelCase', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.submit('OK').withStyle(ActionStyle.Positive))
      .addAction((b) => b.submit('Delete').withStyle(ActionStyle.Destructive))
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"style": "positive"'));
    assert.ok(json.includes('"style": "destructive"'));
  });

  it('serializes associatedInputs as camelCase', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.submit('Auto').withAssociatedInputs(AssociatedInputs.Auto))
      .addAction((b) => b.submit('None').withAssociatedInputs(AssociatedInputs.None))
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"associatedInputs": "auto"'));
    assert.ok(json.includes('"associatedInputs": "none"'));
  });

  it('omits action style when not set', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.submit('Submit'))
      .build();
    const json = toJson(card);
    assert.ok(!json.includes('"style"'));
  });

  it('serializes all action base properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) =>
        b
          .submit('Submit')
          .withId('submit1')
          .withIconUrl('https://example.com/icon.png')
          .withStyle(ActionStyle.Positive)
          .withIsEnabled(false)
          .withTooltip('Submit the form'),
      )
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"id": "submit1"'));
    assert.ok(json.includes('"iconUrl": "https://example.com/icon.png"'));
    assert.ok(json.includes('"style": "positive"'));
    assert.ok(json.includes('"isEnabled": false'));
    assert.ok(json.includes('"tooltip": "Submit the form"'));
  });

  it('produces indented output by default', () => {
    const json = toJson(AdaptiveCardBuilder.create().build());
    assert.ok(json.includes('\n'));
    assert.ok(json.includes('  '));
  });

  it('produces compact output when indent=0', () => {
    const json = toJson(AdaptiveCardBuilder.create().build(), 0);
    assert.ok(!json.includes('\n'));
  });
});

describe('fromJson', () => {
  it('parses a valid card', () => {
    const original = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Hello'))
      .build();
    const card = fromJson(toJson(original));
    assert.ok(card);
    assert.equal(card.type, 'AdaptiveCard');
    assert.equal(card.version, '1.5');
  });

  it('returns null for invalid JSON', () => {
    assert.equal(fromJson('not json'), null);
  });

  it('returns null when root type is not AdaptiveCard', () => {
    assert.equal(fromJson('{"type":"Something"}'), null);
  });

  it('preserves body element type discriminants after round-trip', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) => b.withText('Hi'))
      .addImage((b) => b.withUrl('https://example.com/img.png'))
      .build();

    const parsed = fromJson(toJson(card))!;
    assert.equal(parsed.body![0].type, 'TextBlock');
    assert.equal(parsed.body![1].type, 'Image');
  });

  it('preserves action type discriminants after round-trip', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.openUrl('https://example.com'))
      .addAction((b) => b.submit('Submit'))
      .build();

    const parsed = fromJson(toJson(card))!;
    assert.equal(parsed.actions![0].type, 'Action.OpenUrl');
    assert.equal(parsed.actions![1].type, 'Action.Submit');
  });

  it('round-trips TextBlock properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((b) =>
        b.withText('Test').withSize(TextSize.Medium).withWeight(TextWeight.Bolder),
      )
      .build();

    const parsed = fromJson(toJson(card))!;
    const tb = parsed.body![0] as TextBlock;
    assert.equal(tb.text, 'Test');
    assert.equal(tb.size, TextSize.Medium);
    assert.equal(tb.weight, TextWeight.Bolder);
  });

  it('round-trips OpenUrl action URL', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((b) => b.openUrl('https://example.com').withTitle('Visit'))
      .build();

    const parsed = fromJson(toJson(card))!;
    const action = parsed.actions![0] as OpenUrlAction;
    assert.equal(action.url, 'https://example.com');
    assert.equal(action.title, 'Visit');
  });
});
