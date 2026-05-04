// ─────────────────────────────────────────────────────────────────────────────
// Deno test suite for builder fluent chaining and element builders
// ─────────────────────────────────────────────────────────────────────────────
//
// Run tests: deno test tests-deno/
// Run this file: deno test tests-deno/builder.test.ts

import { assertEquals, assertExists } from 'jsr:@std/assert@^1';
import {
  AdaptiveCardBuilder,
  TextBlockBuilder,
  ImageBuilder,
  ContainerBuilder,
  ColumnSetBuilder,
  ColumnBuilder,
  FactSetBuilder,
  RichTextBlockBuilder,
  TextRunBuilder,
  ActionSetBuilder,
  InputTextBuilder,
  TextSize,
  TextWeight,
  TextColor,
  HorizontalAlignment,
  ImageSize,
  ImageStyle,
  ContainerStyle,
  Spacing,
  ActionStyle,
} from '../src/index.ts';

Deno.test('AdaptiveCardBuilder - creates card with defaults', () => {
  const card = AdaptiveCardBuilder.create().build();
  assertEquals(card.type, 'AdaptiveCard');
  assertEquals(card.version, '1.5');
  assertExists(card['$schema']);
});

Deno.test('AdaptiveCardBuilder - withVersion sets version', () => {
  const card = AdaptiveCardBuilder.create().withVersion('1.6').build();
  assertEquals(card.version, '1.6');
});

Deno.test('AdaptiveCardBuilder - fluent chaining with multiple body elements', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.5')
    .addTextBlock((tb) => tb.withText('First'))
    .addTextBlock((tb) => tb.withText('Second'))
    .addImage((img) => img.withUrl('https://example.com/image.png'))
    .build();

  assertEquals(card.type, 'AdaptiveCard');
  assertEquals(card.version, '1.5');
  assertExists(card.body);
  assertEquals(card.body!.length, 3);
  assertEquals(card.body![0].type, 'TextBlock');
  assertEquals(card.body![1].type, 'TextBlock');
  assertEquals(card.body![2].type, 'Image');
});

Deno.test('AdaptiveCardBuilder - addAction adds to actions array', () => {
  const card = AdaptiveCardBuilder.create()
    .addAction((a) => a.openUrl('https://example.com').withTitle('Visit'))
    .build();

  assertExists(card.actions);
  assertEquals(card.actions!.length, 1);
  assertEquals(card.actions![0].type, 'Action.OpenUrl');
  assertEquals(card.actions![0].title, 'Visit');
});

Deno.test('TextBlockBuilder - builds with all properties', () => {
  const tb = new TextBlockBuilder()
    .withId('tb1')
    .withText('Hello, Deno!')
    .withSize(TextSize.Large)
    .withWeight(TextWeight.Bolder)
    .withColor(TextColor.Accent)
    .withWrap(true)
    .withMaxLines(3)
    .withHorizontalAlignment(HorizontalAlignment.Center)
    .build();

  assertEquals(tb.type, 'TextBlock');
  assertEquals(tb.id, 'tb1');
  assertEquals(tb.text, 'Hello, Deno!');
  assertEquals(tb.size, TextSize.Large);
  assertEquals(tb.weight, TextWeight.Bolder);
  assertEquals(tb.color, TextColor.Accent);
  assertEquals(tb.wrap, true);
  assertEquals(tb.maxLines, 3);
  assertEquals(tb.horizontalAlignment, HorizontalAlignment.Center);
});

Deno.test('ImageBuilder - builds with core properties', () => {
  const img = new ImageBuilder()
    .withUrl('https://example.com/logo.png')
    .withAltText('Company Logo')
    .withSize(ImageSize.Large)
    .withStyle(ImageStyle.Person)
    .withHorizontalAlignment(HorizontalAlignment.Center)
    .build();

  assertEquals(img.type, 'Image');
  assertEquals(img.url, 'https://example.com/logo.png');
  assertEquals(img.altText, 'Company Logo');
  assertEquals(img.size, ImageSize.Large);
  assertEquals(img.style, ImageStyle.Person);
  assertEquals(img.horizontalAlignment, HorizontalAlignment.Center);
});

Deno.test('ContainerBuilder - nested element builder with lambda', () => {
  const card = AdaptiveCardBuilder.create()
    .addContainer((container) =>
      container
        .withStyle(ContainerStyle.Emphasis)
        .addTextBlock((tb) => tb.withText('Inside container'))
        .addImage((img) => img.withUrl('https://example.com/pic.png')),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'Container');
  if (card.body![0].type === 'Container') {
    assertEquals(card.body![0].style, ContainerStyle.Emphasis);
    assertExists(card.body![0].items);
    assertEquals(card.body![0].items!.length, 2);
    assertEquals(card.body![0].items![0].type, 'TextBlock');
    assertEquals(card.body![0].items![1].type, 'Image');
  }
});

Deno.test('ColumnSetBuilder - builds columns with nested content', () => {
  const card = AdaptiveCardBuilder.create()
    .addColumnSet((cs) =>
      cs
        .addColumn((col) => col.withWidth('auto').addTextBlock((tb) => tb.withText('Left')))
        .addColumn((col) => col.withWidth('stretch').addTextBlock((tb) => tb.withText('Right'))),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'ColumnSet');
  if (card.body![0].type === 'ColumnSet') {
    assertExists(card.body![0].columns);
    assertEquals(card.body![0].columns!.length, 2);
  }
});

Deno.test('FactSetBuilder - builds facts array', () => {
  const card = AdaptiveCardBuilder.create()
    .addFactSet((fs) =>
      fs.addFact('Name', 'Alice').addFact('Role', 'Engineer').addFact('Team', 'Platform'),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'FactSet');
  if (card.body![0].type === 'FactSet') {
    assertExists(card.body![0].facts);
    assertEquals(card.body![0].facts!.length, 3);
    assertEquals(card.body![0].facts![0].title, 'Name');
    assertEquals(card.body![0].facts![0].value, 'Alice');
  }
});

Deno.test('RichTextBlockBuilder - builds with TextRuns', () => {
  const card = AdaptiveCardBuilder.create()
    .addRichTextBlock((rtb) =>
      rtb
        .addTextRun((tr) => tr.withText('Bold text').withWeight(TextWeight.Bolder))
        .addTextRun((tr) => tr.withText(' and normal text')),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'RichTextBlock');
  if (card.body![0].type === 'RichTextBlock') {
    assertExists(card.body![0].inlines);
    assertEquals(card.body![0].inlines!.length, 2);
    // TextRun inlines (not plain strings)
    const inline0 = card.body![0].inlines![0];
    const inline1 = card.body![0].inlines![1];
    if (typeof inline0 === 'object') {
      assertEquals(inline0.type, 'TextRun');
    }
    if (typeof inline1 === 'object') {
      assertEquals(inline1.type, 'TextRun');
    }
  }
});

Deno.test('ActionSetBuilder - builds multiple actions with properties', () => {
  const card = AdaptiveCardBuilder.create()
    .addActionSet((as) =>
      as
        .addAction((a) => a.openUrl('https://example.com').withTitle('Visit').withStyle(ActionStyle.Positive))
        .addAction((a) => a.submit('Delete').withTitle('Delete').withStyle(ActionStyle.Destructive)),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'ActionSet');
  if (card.body![0].type === 'ActionSet') {
    assertExists(card.body![0].actions);
    assertEquals(card.body![0].actions!.length, 2);
    assertEquals(card.body![0].actions![0].type, 'Action.OpenUrl');
    assertEquals(card.body![0].actions![1].type, 'Action.Submit');
  }
});

Deno.test('InputTextBuilder - builds with label and placeholder', () => {
  const card = AdaptiveCardBuilder.create()
    .addInputText((input) =>
      input.withId('name').withLabel('Full Name').withPlaceholder('Enter your name').withIsRequired(true),
    )
    .build();

  assertExists(card.body);
  assertEquals(card.body!.length, 1);
  assertEquals(card.body![0].type, 'Input.Text');
  if (card.body![0].type === 'Input.Text') {
    assertEquals(card.body![0].id, 'name');
    assertEquals(card.body![0].label, 'Full Name');
    assertEquals(card.body![0].placeholder, 'Enter your name');
    assertEquals(card.body![0].isRequired, true);
  }
});

Deno.test('Multiple builders - complex card structure', () => {
  const card = AdaptiveCardBuilder.create()
    .withVersion('1.6')
    .addTextBlock((tb) => tb.withText('Card Title').withSize(TextSize.ExtraLarge).withWeight(TextWeight.Bolder))
    .addContainer((c) =>
      c
        .withStyle(ContainerStyle.Emphasis)
        .withSpacing(Spacing.Medium)
        .addFactSet((fs) => fs.addFact('Status', 'Active').addFact('Priority', 'High')),
    )
    .addInputText((input) => input.withId('comments').withLabel('Comments').withIsMultiline(true))
    .addAction((a) => a.submit('Submit').withTitle('Submit Form').withStyle(ActionStyle.Positive))
    .build();

  assertEquals(card.version, '1.6');
  assertExists(card.body);
  assertEquals(card.body!.length, 3);
  assertEquals(card.body![0].type, 'TextBlock');
  assertEquals(card.body![1].type, 'Container');
  assertEquals(card.body![2].type, 'Input.Text');
  assertExists(card.actions);
  assertEquals(card.actions!.length, 1);
});
