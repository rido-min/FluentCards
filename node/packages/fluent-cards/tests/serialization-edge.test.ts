import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  toJson,
  fromJson,
  TextSize,
  TextWeight,
  TextColor,
  ContainerStyle,
  HorizontalAlignment,
} from 'fluent-cards';

describe('Serialization edge cases', () => {
  it('omits undefined optional properties from JSON', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((tb) => tb.withText('Hello'))
      .build();

    const json = toJson(card);
    const parsed = JSON.parse(json);

    assert.equal(parsed.type, 'AdaptiveCard');
    assert.equal(parsed.version, '1.5');
    assert.ok(parsed['$schema']);
    assert.equal(parsed.fallbackText, undefined);
    assert.equal(parsed.speak, undefined);
    assert.equal(parsed.lang, undefined);
    assert.equal(parsed.rtl, undefined);
    assert.equal(parsed.minHeight, undefined);
    assert.equal(parsed.metadata, undefined);
    assert.equal(parsed.backgroundImage, undefined);
    assert.equal(parsed.refresh, undefined);
    assert.equal(parsed.authentication, undefined);

    // Verify the JSON string does not contain the keys
    assert.ok(!json.includes('"fallbackText"'));
    assert.ok(!json.includes('"speak"'));
    assert.ok(!json.includes('"lang"'));
    assert.ok(!json.includes('"rtl"'));
    assert.ok(!json.includes('"minHeight"'));
    assert.ok(!json.includes('"metadata"'));
    assert.ok(!json.includes('"backgroundImage"'));
    assert.ok(!json.includes('"refresh"'));
    assert.ok(!json.includes('"authentication"'));
  });

  it('round-trip: build → toJson → fromJson preserves structure', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.6')
      .withFallbackText('Fallback')
      .withLang('en-US')
      .addTextBlock((tb) =>
        tb.withText('Hello').withSize(TextSize.Large).withWeight(TextWeight.Bolder),
      )
      .addTextBlock((tb) => tb.withText('World').withColor(TextColor.Accent))
      .addAction((a) => a.openUrl('https://example.com', 'Visit'))
      .build();

    const json = toJson(card);
    const restored = fromJson(json);

    assert.ok(restored);
    assert.equal(restored!.type, 'AdaptiveCard');
    assert.equal(restored!.version, '1.6');
    assert.equal(restored!.fallbackText, 'Fallback');
    assert.equal(restored!.lang, 'en-US');
    assert.equal(restored!.body!.length, 2);
    assert.equal(restored!.actions!.length, 1);
    assert.deepEqual(restored, card);
  });

  it('round-trip with container and columns', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c) =>
        c.withStyle(ContainerStyle.Emphasis).addTextBlock((tb) => tb.withText('Inside')),
      )
      .addColumnSet((cs) =>
        cs
          .addColumn('1', (col) => col.addTextBlock((tb) => tb.withText('Col A')))
          .addColumn('2', (col) => col.addTextBlock((tb) => tb.withText('Col B'))),
      )
      .build();

    const json = toJson(card);
    const restored = fromJson(json);
    assert.deepEqual(restored, card);
  });

  it('handles Unicode content in text blocks', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((tb) => tb.withText('日本語テキスト 🌍🎉'))
      .addTextBlock((tb) => tb.withText('العربية'))
      .addTextBlock((tb) => tb.withText('Ñoño café résumé naïve'))
      .build();

    const json = toJson(card);
    const restored = fromJson(json);

    assert.ok(restored);
    const body = restored!.body!;
    assert.equal((body[0] as any).text, '日本語テキスト 🌍🎉');
    assert.equal((body[1] as any).text, 'العربية');
    assert.equal((body[2] as any).text, 'Ñoño café résumé naïve');
  });

  it('handles very long text content', () => {
    const longText = 'A'.repeat(10_000);
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((tb) => tb.withText(longText))
      .build();

    const json = toJson(card);
    const restored = fromJson(json);
    assert.ok(restored);
    assert.equal((restored!.body![0] as any).text, longText);
  });

  it('handles empty body array', () => {
    const card = AdaptiveCardBuilder.create().build();
    const json = toJson(card);
    const restored = fromJson(json);

    assert.ok(restored);
    assert.equal(restored!.type, 'AdaptiveCard');
    // body may be undefined or empty array
    const bodyLen = restored!.body?.length ?? 0;
    assert.equal(bodyLen, 0);
  });

  it('handles card with no actions', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((tb) => tb.withText('No actions'))
      .build();

    const json = toJson(card);
    assert.ok(!json.includes('"actions"'));

    const restored = fromJson(json);
    assert.ok(restored);
    assert.equal(restored!.actions, undefined);
  });

  it('handles nested containers 3+ levels deep', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c1) =>
        c1
          .withId('level1')
          .addContainer((c2) =>
            c2
              .withId('level2')
              .addContainer((c3) =>
                c3
                  .withId('level3')
                  .addTextBlock((tb) => tb.withText('Deep inside')),
              ),
          ),
      )
      .build();

    const json = toJson(card);
    const restored = fromJson(json);
    assert.deepEqual(restored, card);

    // Verify nesting
    const level1 = restored!.body![0] as any;
    assert.equal(level1.id, 'level1');
    const level2 = level1.items[0];
    assert.equal(level2.id, 'level2');
    const level3 = level2.items[0];
    assert.equal(level3.id, 'level3');
    assert.equal(level3.items[0].text, 'Deep inside');
  });

  it('fromJson returns null for invalid JSON', () => {
    assert.equal(fromJson('not json'), null);
  });

  it('fromJson returns null for empty string', () => {
    assert.equal(fromJson(''), null);
  });

  it('fromJson returns null for non-AdaptiveCard object', () => {
    assert.equal(fromJson('{"type": "Other"}'), null);
  });

  it('toJson with custom indent', () => {
    const card = AdaptiveCardBuilder.create().build();
    const json4 = toJson(card, 4);
    assert.ok(json4.includes('    "type"'));
    const json0 = toJson(card, 0);
    assert.ok(!json0.includes('  '));
  });
});
