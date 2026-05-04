// ─────────────────────────────────────────────────────────────────────────────
// Deno test suite for serialization: toJson, toObject, fromJson, round-trips
// ─────────────────────────────────────────────────────────────────────────────
//
// Run tests: deno test tests-deno/
// Run this file: deno test tests-deno/serialization.test.ts

import { assertEquals, assertExists } from 'jsr:@std/assert@^1';
import type { AdaptiveCard } from '../src/index.ts';
import {
  AdaptiveCardBuilder,
  toJson,
  toObject,
  fromJson,
  TextSize,
  TextWeight,
  TextColor,
  ActionStyle,
  AssociatedInputs,
} from '../src/index.ts';

Deno.test('toJson - includes type and version', () => {
  const card = AdaptiveCardBuilder.create().build();
  const json = toJson(card);
  assertEquals(json.includes('"type": "AdaptiveCard"'), true);
  assertEquals(json.includes('"version": "1.5"'), true);
});

Deno.test('toJson - includes $schema property', () => {
  const card = AdaptiveCardBuilder.create().build();
  const json = toJson(card);
  assertEquals(json.includes('"$schema"'), true);
  assertEquals(json.includes('https://adaptivecards.io/schemas/1.5.0/adaptive-card.json'), true);
});

Deno.test('toJson - omits undefined optional properties', () => {
  const card = AdaptiveCardBuilder.create()
    .addTextBlock((tb) => tb.withText('Simple Text'))
    .build();
  const json = toJson(card);
  assertEquals(json.includes('"size"'), false);
  assertEquals(json.includes('"weight"'), false);
  assertEquals(json.includes('"color"'), false);
  assertEquals(json.includes('"wrap"'), false);
  assertEquals(json.includes('"maxLines"'), false);
});

Deno.test('toJson - serializes enum values as camelCase strings', () => {
  const card = AdaptiveCardBuilder.create()
    .addTextBlock((tb) =>
      tb
        .withText('Test')
        .withSize(TextSize.ExtraLarge)
        .withWeight(TextWeight.Bolder)
        .withColor(TextColor.Attention),
    )
    .build();
  const json = toJson(card);
  assertEquals(json.includes('"size": "extraLarge"'), true);
  assertEquals(json.includes('"weight": "bolder"'), true);
  assertEquals(json.includes('"color": "attention"'), true);
});

Deno.test('toJson - serializes action enum values as camelCase', () => {
  const card = AdaptiveCardBuilder.create()
    .addAction((a) => a.submit('OK').withStyle(ActionStyle.Positive).withAssociatedInputs(AssociatedInputs.Auto))
    .build();
  const json = toJson(card);
  assertEquals(json.includes('"style": "positive"'), true);
  assertEquals(json.includes('"associatedInputs": "auto"'), true);
});

Deno.test('toJson - produces indented output by default', () => {
  const card = AdaptiveCardBuilder.create().build();
  const json = toJson(card);
  assertEquals(json.includes('\n'), true);
  assertEquals(json.includes('  '), true);
});

Deno.test('toObject - returns plain JavaScript object', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addTextBlock((tb) => tb.withText('Hello'))
    .build();
  const obj = toObject(card);

  assertEquals(typeof obj, 'object');
  assertEquals(obj.type, 'AdaptiveCard');
  assertEquals(obj.version, '1.6');
  assertExists(obj.body);
  assertEquals(obj.body!.length, 1);
});

Deno.test('toObject - strips undefined properties', () => {
  const card = AdaptiveCardBuilder.create()
    .addTextBlock((tb) => tb.withText('Simple'))
    .build();
  const obj = toObject(card);

  const body0 = obj.body![0];
  assertEquals(body0.type, 'TextBlock');
  if (body0.type === 'TextBlock') {
    assertEquals('size' in body0, false);
    assertEquals('weight' in body0, false);
    assertEquals('color' in body0, false);
  }
});

Deno.test('toObject - preserves enum values as camelCase strings', () => {
  const card = AdaptiveCardBuilder.create()
    .addTextBlock((tb) => tb.withText('Test').withSize(TextSize.Large).withColor(TextColor.Accent))
    .build();
  const obj = toObject(card);

  const body0 = obj.body![0];
  if (body0.type === 'TextBlock') {
    assertEquals(body0.size, 'large');
    assertEquals(body0.color, 'accent');
  }
});

Deno.test('fromJson - parses valid card', () => {
  const original = AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addTextBlock((tb) => tb.withText('Hello World'))
    .build();
  const json = toJson(original);
  const parsed = fromJson(json);

  assertExists(parsed);
  assertEquals(parsed!.type, 'AdaptiveCard');
  assertEquals(parsed!.version, '1.6');
  assertExists(parsed!.body);
  assertEquals(parsed!.body!.length, 1);
});

Deno.test('fromJson - round-trip preserves card structure', () => {
  const original = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) =>
      tb.withText('Round trip test').withSize(TextSize.Large).withWeight(TextWeight.Bolder).withWrap(true),
    )
    .addImage((img) => img.withUrl('https://example.com/img.png').withAltText('Test Image'))
    .addAction((a) => a.openUrl('https://example.com').withTitle('Visit'))
    .build();

  const json = toJson(original);
  const parsed = fromJson(json);

  assertExists(parsed);
  assertEquals(parsed!.type, original.type);
  assertEquals(parsed!.version, original.version);
  assertEquals(parsed!.body!.length, original.body!.length);
  assertEquals(parsed!.actions!.length, original.actions!.length);
});

Deno.test('fromJson and toJson equivalence - complex card', () => {
  const original = AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addContainer((c) =>
      c
        .addTextBlock((tb) => tb.withText('Title').withSize(TextSize.ExtraLarge))
        .addFactSet((fs) => fs.addFact('Key1', 'Value1').addFact('Key2', 'Value2')),
    )
    .addInputText((input) => input.withId('textInput1').withLabel('Name').withIsRequired(true))
    .addAction((a) => a.submit('Submit').withStyle(ActionStyle.Positive))
    .build();

  const json1 = toJson(original);
  const parsed = fromJson(json1);
  assertExists(parsed);
  const json2 = toJson(parsed!);

  // Round-trip JSON should be structurally equivalent
  const obj1 = JSON.parse(json1);
  const obj2 = JSON.parse(json2);
  assertEquals(obj1.type, obj2.type);
  assertEquals(obj1.version, obj2.version);
  assertEquals(obj1.body.length, obj2.body.length);
});

Deno.test('toObject equivalence with fromJson(toJson())', () => {
  const card = AdaptiveCardBuilder.create()
    .addTextBlock((tb) => tb.withText('Test').withSize(TextSize.Medium).withWrap(true))
    .build();

  const viaObject = toObject(card);
  const viaJson = fromJson(toJson(card));

  assertExists(viaJson);
  assertEquals(viaObject.type, viaJson!.type);
  assertEquals(viaObject.version, viaJson!.version);
  assertEquals(viaObject.body!.length, viaJson!.body!.length);
});
