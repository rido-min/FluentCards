import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  TextBlockBuilder,
  ImageBuilder,
  ContainerBuilder,
  ColumnSetBuilder,
  FactSetBuilder,
  RichTextBlockBuilder,
  TableBuilder,
  ActionSetBuilder,
  TextSize,
  TextWeight,
  TextColor,
  FontType,
  TextBlockStyle,
  HorizontalAlignment,
  VerticalAlignment,
  ImageSize,
  ImageStyle,
  ContainerStyle,
  ActionStyle,
  BackgroundImageFillMode,
  toJson,
} from 'fluent-cards';
import type {
  TextBlock,
  Image,
  Container,
  ColumnSet,
  FactSet,
  RichTextBlock,
  Table,
  ActionSet,
  OpenUrlAction,
  SubmitAction,
  ExecuteAction,
} from 'fluent-cards';

describe('Schema conformance – TextBlock', () => {
  it('builds a TextBlock with all properties', () => {
    const tb = new TextBlockBuilder()
      .withId('tb1')
      .withText('Hello')
      .withSize(TextSize.ExtraLarge)
      .withWeight(TextWeight.Bolder)
      .withColor(TextColor.Accent)
      .withFontType(FontType.Monospace)
      .withIsSubtle()
      .withStyle(TextBlockStyle.Heading)
      .withWrap(true)
      .withMaxLines(3)
      .withHorizontalAlignment(HorizontalAlignment.Center)
      .build();

    assert.equal(tb.type, 'TextBlock');
    assert.equal(tb.id, 'tb1');
    assert.equal(tb.text, 'Hello');
    assert.equal(tb.size, 'extraLarge');
    assert.equal(tb.weight, 'bolder');
    assert.equal(tb.color, 'accent');
    assert.equal(tb.fontType, 'monospace');
    assert.equal(tb.isSubtle, true);
    assert.equal(tb.style, 'heading');
    assert.equal(tb.wrap, true);
    assert.equal(tb.maxLines, 3);
    assert.equal(tb.horizontalAlignment, 'center');
  });

  it('omits unset TextBlock optional properties', () => {
    const tb = new TextBlockBuilder().withText('Simple').build();
    assert.equal(tb.type, 'TextBlock');
    assert.equal(tb.text, 'Simple');
    assert.equal(tb.size, undefined);
    assert.equal(tb.weight, undefined);
    assert.equal(tb.color, undefined);
    assert.equal(tb.fontType, undefined);
    assert.equal(tb.isSubtle, undefined);
    assert.equal(tb.style, undefined);
    assert.equal(tb.wrap, undefined);
    assert.equal(tb.maxLines, undefined);
    assert.equal(tb.horizontalAlignment, undefined);
  });

  it('TextBlock with selectAction', () => {
    const card = AdaptiveCardBuilder.create()
      .addTextBlock((tb) =>
        tb.withText('Clickable').withSelectAction(
          { type: 'Action.OpenUrl', url: 'https://example.com' },
        ),
      )
      .build();

    const tb = card.body![0] as TextBlock;
    assert.equal(tb.selectAction?.type, 'Action.OpenUrl');
    assert.equal((tb.selectAction as OpenUrlAction).url, 'https://example.com');
  });
});

describe('Schema conformance – Image', () => {
  it('builds an Image with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addImage((b) =>
        b
          .withId('img1')
          .withUrl('https://example.com/image.png')
          .withAltText('An image')
          .withSize(ImageSize.Large)
          .withStyle(ImageStyle.Person)
          .withWidth('100px')
          .withHeight('100px')
          .withHorizontalAlignment(HorizontalAlignment.Right)
          .withSelectAction((a) => a.openUrl('https://example.com')),
      )
      .build();

    const img = card.body![0] as Image;
    assert.equal(img.type, 'Image');
    assert.equal(img.id, 'img1');
    assert.equal(img.url, 'https://example.com/image.png');
    assert.equal(img.altText, 'An image');
    assert.equal(img.size, 'large');
    assert.equal(img.style, 'person');
    assert.equal(img.width, '100px');
    assert.equal(img.height, '100px');
    assert.equal(img.horizontalAlignment, 'right');
    assert.ok(img.selectAction);
    assert.equal(img.selectAction!.type, 'Action.OpenUrl');
  });

  it('omits unset Image optional properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addImage((b) => b.withUrl('https://example.com/pic.png'))
      .build();

    const img = card.body![0] as Image;
    assert.equal(img.type, 'Image');
    assert.equal(img.url, 'https://example.com/pic.png');
    assert.equal(img.altText, undefined);
    assert.equal(img.size, undefined);
    assert.equal(img.style, undefined);
  });
});

describe('Schema conformance – Container', () => {
  it('builds a Container with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c) =>
        c
          .withId('c1')
          .withStyle(ContainerStyle.Emphasis)
          .withBleed()
          .withMinHeight('200px')
          .withVerticalContentAlignment(VerticalAlignment.Center)
          .withBackgroundImage((bg) =>
            bg.withUrl('https://example.com/bg.png').withFillMode(BackgroundImageFillMode.Cover),
          )
          .withSelectAction((a) => a.submit('Select'))
          .addTextBlock((tb) => tb.withText('Inside container')),
      )
      .build();

    const container = card.body![0] as Container;
    assert.equal(container.type, 'Container');
    assert.equal(container.id, 'c1');
    assert.equal(container.style, 'emphasis');
    assert.equal(container.bleed, true);
    assert.equal(container.minHeight, '200px');
    assert.equal(container.verticalContentAlignment, 'center');
    assert.ok(container.backgroundImage);
    assert.equal(container.backgroundImage!.url, 'https://example.com/bg.png');
    assert.equal(container.backgroundImage!.fillMode, 'cover');
    assert.ok(container.selectAction);
    assert.equal(container.selectAction!.type, 'Action.Submit');
    assert.equal(container.items!.length, 1);
    assert.equal(container.items![0].type, 'TextBlock');
  });
});

describe('Schema conformance – ColumnSet', () => {
  it('builds a ColumnSet with columns having width, items, and style', () => {
    const card = AdaptiveCardBuilder.create()
      .addColumnSet((cs) =>
        cs
          .withId('cs1')
          .withStyle(ContainerStyle.Accent)
          .addColumn('1', (col) =>
            col
              .withStyle(ContainerStyle.Good)
              .addTextBlock((tb) => tb.withText('Col 1')),
          )
          .addColumn('2', (col) =>
            col.addTextBlock((tb) => tb.withText('Col 2')),
          ),
      )
      .build();

    const colSet = card.body![0] as ColumnSet;
    assert.equal(colSet.type, 'ColumnSet');
    assert.equal(colSet.id, 'cs1');
    assert.equal(colSet.style, 'accent');
    assert.equal(colSet.columns!.length, 2);
    assert.equal(colSet.columns![0].width, '1');
    assert.equal(colSet.columns![0].style, 'good');
    assert.equal(colSet.columns![0].items!.length, 1);
    assert.equal(colSet.columns![1].width, '2');
  });
});

describe('Schema conformance – FactSet', () => {
  it('builds a FactSet with facts array', () => {
    const card = AdaptiveCardBuilder.create()
      .addFactSet((fs) =>
        fs
          .withId('fs1')
          .addFact('Name', 'John')
          .addFact('Age', '30')
          .addFact('Location', 'Seattle'),
      )
      .build();

    const factSet = card.body![0] as FactSet;
    assert.equal(factSet.type, 'FactSet');
    assert.equal(factSet.id, 'fs1');
    assert.equal(factSet.facts!.length, 3);
    assert.deepEqual(factSet.facts![0], { title: 'Name', value: 'John' });
    assert.deepEqual(factSet.facts![1], { title: 'Age', value: '30' });
    assert.deepEqual(factSet.facts![2], { title: 'Location', value: 'Seattle' });
  });
});

describe('Schema conformance – RichTextBlock', () => {
  it('builds a RichTextBlock with string and TextRun inlines', () => {
    const card = AdaptiveCardBuilder.create()
      .addRichTextBlock((rtb) =>
        rtb
          .withId('rtb1')
          .withHorizontalAlignment(HorizontalAlignment.Center)
          .addText('Plain text ')
          .addTextRun((tr) =>
            tr
              .withText('Bold text')
              .withWeight(TextWeight.Bolder)
              .withColor(TextColor.Accent)
              .isItalic()
              .isUnderline(),
          ),
      )
      .build();

    const rtb = card.body![0] as RichTextBlock;
    assert.equal(rtb.type, 'RichTextBlock');
    assert.equal(rtb.id, 'rtb1');
    assert.equal(rtb.horizontalAlignment, 'center');
    assert.equal(rtb.inlines!.length, 2);
    assert.equal(rtb.inlines![0], 'Plain text ');
    const run = rtb.inlines![1] as { type: string; text: string; weight: string; color: string; italic: boolean; underline: boolean };
    assert.equal(run.type, 'TextRun');
    assert.equal(run.text, 'Bold text');
    assert.equal(run.weight, 'bolder');
    assert.equal(run.color, 'accent');
    assert.equal(run.italic, true);
    assert.equal(run.underline, true);
  });
});

describe('Schema conformance – Table', () => {
  it('builds a Table with columns, rows, and cells', () => {
    const card = AdaptiveCardBuilder.create()
      .addTable((t) =>
        t
          .withId('t1')
          .withFirstRowAsHeader()
          .withShowGridLines()
          .withGridStyle(ContainerStyle.Default)
          .withHorizontalCellContentAlignment(HorizontalAlignment.Center)
          .withVerticalCellContentAlignment(VerticalAlignment.Center)
          .addColumn({ width: '1' })
          .addColumn({ width: '2' })
          .addRow({
            type: 'TableRow',
            cells: [
              { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Header 1' }] },
              { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Header 2' }] },
            ],
          })
          .addRow({
            type: 'TableRow',
            cells: [
              { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Cell 1' }] },
              { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Cell 2' }] },
            ],
          }),
      )
      .build();

    const table = card.body![0] as Table;
    assert.equal(table.type, 'Table');
    assert.equal(table.id, 't1');
    assert.equal(table.firstRowAsHeader, true);
    assert.equal(table.showGridLines, true);
    assert.equal(table.gridStyle, 'default');
    assert.equal(table.horizontalCellContentAlignment, 'center');
    assert.equal(table.verticalCellContentAlignment, 'center');
    assert.equal(table.columns!.length, 2);
    assert.equal(table.columns![0].width, '1');
    assert.equal(table.columns![1].width, '2');
    assert.equal(table.rows!.length, 2);
    assert.equal(table.rows![0].cells!.length, 2);
    assert.equal(table.rows![1].cells!.length, 2);
  });
});

describe('Schema conformance – ActionSet', () => {
  it('builds an ActionSet with mixed action types', () => {
    const card = AdaptiveCardBuilder.create()
      .addActionSet((as) =>
        as
          .withId('as1')
          .addAction((a) => a.openUrl('https://example.com', 'Open'))
          .addAction((a) => a.submit('Submit').withData({ key: 'value' }))
          .addAction((a) => a.execute('Execute').withVerb('doIt')),
      )
      .build();

    const actionSet = card.body![0] as ActionSet;
    assert.equal(actionSet.type, 'ActionSet');
    assert.equal(actionSet.id, 'as1');
    assert.equal(actionSet.actions!.length, 3);
    assert.equal(actionSet.actions![0].type, 'Action.OpenUrl');
    assert.equal(actionSet.actions![1].type, 'Action.Submit');
    assert.equal(actionSet.actions![2].type, 'Action.Execute');
    const submitAction = actionSet.actions![1] as SubmitAction;
    assert.deepEqual(submitAction.data, { key: 'value' });
    const execAction = actionSet.actions![2] as ExecuteAction;
    assert.equal(execAction.verb, 'doIt');
  });
});

describe('Schema conformance – Card-level properties', () => {
  it('sets version and auto-maps schema URL', () => {
    const card = AdaptiveCardBuilder.create().withVersion('1.6').build();
    assert.equal(card.version, '1.6');
    assert.equal(card['$schema'], 'https://adaptivecards.io/schemas/1.6.0/adaptive-card.json');
  });

  it('sets fallbackText', () => {
    const card = AdaptiveCardBuilder.create().withFallbackText('Cannot render').build();
    assert.equal(card.fallbackText, 'Cannot render');
  });

  it('sets speak', () => {
    const card = AdaptiveCardBuilder.create().withSpeak('This card says hello').build();
    assert.equal(card.speak, 'This card says hello');
  });

  it('sets lang', () => {
    const card = AdaptiveCardBuilder.create().withLang('en-US').build();
    assert.equal(card.lang, 'en-US');
  });

  it('sets rtl', () => {
    const card = AdaptiveCardBuilder.create().withRtl().build();
    assert.equal(card.rtl, true);
  });

  it('sets minHeight', () => {
    const card = AdaptiveCardBuilder.create().withMinHeight('300px').build();
    assert.equal(card.minHeight, '300px');
  });

  it('sets metadata webUrl', () => {
    const card = AdaptiveCardBuilder.create().withMetadata('https://example.com/card').build();
    assert.deepEqual(card.metadata, { webUrl: 'https://example.com/card' });
  });

  it('sets backgroundImage', () => {
    const card = AdaptiveCardBuilder.create()
      .withBackgroundImage((bg) =>
        bg
          .withUrl('https://example.com/bg.png')
          .withFillMode(BackgroundImageFillMode.RepeatHorizontally)
          .withHorizontalAlignment(HorizontalAlignment.Left)
          .withVerticalAlignment(VerticalAlignment.Top),
      )
      .build();

    assert.ok(card.backgroundImage);
    assert.equal(card.backgroundImage!.url, 'https://example.com/bg.png');
    assert.equal(card.backgroundImage!.fillMode, 'repeatHorizontally');
    assert.equal(card.backgroundImage!.horizontalAlignment, 'left');
    assert.equal(card.backgroundImage!.verticalAlignment, 'top');
  });

  it('sets verticalContentAlignment', () => {
    const card = AdaptiveCardBuilder.create()
      .withVerticalContentAlignment(VerticalAlignment.Bottom)
      .build();
    assert.equal(card.verticalContentAlignment, 'bottom');
  });

  it('builds a card with all card-level properties together', () => {
    const card = AdaptiveCardBuilder.create()
      .withVersion('1.6')
      .withFallbackText('Fallback')
      .withSpeak('Speak text')
      .withLang('fr-FR')
      .withRtl()
      .withMinHeight('500px')
      .withMetadata('https://example.com')
      .addTextBlock((tb) => tb.withText('Body'))
      .addAction((a) => a.openUrl('https://example.com', 'Link'))
      .build();

    assert.equal(card.type, 'AdaptiveCard');
    assert.equal(card.version, '1.6');
    assert.equal(card.fallbackText, 'Fallback');
    assert.equal(card.speak, 'Speak text');
    assert.equal(card.lang, 'fr-FR');
    assert.equal(card.rtl, true);
    assert.equal(card.minHeight, '500px');
    assert.deepEqual(card.metadata, { webUrl: 'https://example.com' });
    assert.equal(card.body!.length, 1);
    assert.equal(card.actions!.length, 1);
  });
});
