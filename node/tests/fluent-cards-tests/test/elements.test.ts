import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  ContainerBuilder,
  ColumnSetBuilder,
  FactSetBuilder,
  RichTextBlockBuilder,
  ActionSetBuilder,
  MediaBuilder,
  ImageSetBuilder,
  TableBuilder,
  toJson,
  ContainerStyle,
  VerticalAlignment,
  HorizontalAlignment,
  ImageSize,
  TextColor,
  TextWeight,
} from 'fluent-cards';
import type { Container, Column, FactSet, RichTextBlock, Table, TableRow, TableCell, TableColumnDefinition } from 'fluent-cards';

describe('ContainerBuilder', () => {
  it('builds a Container with items', () => {
    const container = new ContainerBuilder()
      .withId('c1')
      .withStyle(ContainerStyle.Emphasis)
      .withVerticalContentAlignment(VerticalAlignment.Center)
      .withBleed()
      .withMinHeight('50px')
      .addTextBlock((b) => b.withText('Hello'))
      .addImage((b) => b.withUrl('https://example.com/img.png'))
      .build();

    assert.equal(container.type, 'Container');
    assert.equal(container.id, 'c1');
    assert.equal(container.style, ContainerStyle.Emphasis);
    assert.equal(container.verticalContentAlignment, VerticalAlignment.Center);
    assert.equal(container.bleed, true);
    assert.equal(container.minHeight, '50px');
    assert.equal(container.items!.length, 2);
  });

  it('addElement accepts a pre-built element', () => {
    const img = new (require('fluent-cards').ImageBuilder)().withUrl('https://example.com/x.png').build();
    const container = new ContainerBuilder().addElement(img).build();
    assert.equal(container.items!.length, 1);
    assert.equal(container.items![0].type, 'Image');
  });

  it('is added to card body via addContainer', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c) => c.addTextBlock((b) => b.withText('Nested')))
      .build();

    assert.equal(card.body![0].type, 'Container');
    assert.equal(((card.body![0] as Container).items![0] as any).text, 'Nested');
  });

  it('serializes container style as camelCase', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c) => c.withStyle(ContainerStyle.Accent).addTextBlock((b) => b.withText('x')))
      .build();
    assert.ok(toJson(card).includes('"style": "accent"'));
  });
});

describe('ColumnSetBuilder', () => {
  it('builds columns with the two-argument overload', () => {
    const cs = new ColumnSetBuilder()
      .addColumn('auto', (col) => col.addTextBlock((b) => b.withText('A')))
      .addColumn('stretch', (col) => col.addTextBlock((b) => b.withText('B')))
      .build();

    assert.equal(cs.type, 'ColumnSet');
    assert.equal(cs.columns!.length, 2);
    assert.equal(cs.columns![0].width, 'auto');
    assert.equal(cs.columns![1].width, 'stretch');
  });

  it('builds columns with the single-argument overload', () => {
    const cs = new ColumnSetBuilder()
      .addColumn((col) => col.withWidth('1').addTextBlock((b) => b.withText('Col')))
      .build();

    assert.equal(cs.columns!.length, 1);
    assert.equal(cs.columns![0].width, '1');
  });

  it('serializes columns', () => {
    const card = AdaptiveCardBuilder.create()
      .addColumnSet((cs) =>
        cs
          .addColumn('auto', (col) => col.addTextBlock((b) => b.withText('Left')))
          .addColumn('auto', (col) => col.addTextBlock((b) => b.withText('Right'))),
      )
      .build();

    const json = toJson(card);
    assert.ok(json.includes('"type": "ColumnSet"'));
    assert.ok(json.includes('"type": "Column"'));
    assert.ok(json.includes('"text": "Left"'));
    assert.ok(json.includes('"text": "Right"'));
  });
});

describe('FactSetBuilder', () => {
  it('builds FactSet with string overload', () => {
    const fs = new FactSetBuilder()
      .addFact('Name', 'Alice')
      .addFact('Age', '30')
      .build();

    assert.equal(fs.type, 'FactSet');
    assert.equal(fs.facts!.length, 2);
    assert.equal(fs.facts![0].title, 'Name');
    assert.equal(fs.facts![0].value, 'Alice');
  });

  it('builds FactSet with object overload', () => {
    const fs = new FactSetBuilder()
      .addFact({ title: 'City', value: 'NYC' })
      .build();

    assert.equal(fs.facts![0].title, 'City');
  });
});

describe('RichTextBlockBuilder', () => {
  it('builds inlines with strings and TextRuns', () => {
    const rtb = new RichTextBlockBuilder()
      .addText('Plain text ')
      .addTextRun((r) => r.withText('Bold').withWeight(TextWeight.Bolder).withColor(TextColor.Accent))
      .build();

    assert.equal(rtb.type, 'RichTextBlock');
    assert.equal(rtb.inlines!.length, 2);
    assert.equal(rtb.inlines![0], 'Plain text ');
    const run = rtb.inlines![1] as any;
    assert.equal(run.type, 'TextRun');
    assert.equal(run.text, 'Bold');
    assert.equal(run.weight, TextWeight.Bolder);
    assert.equal(run.color, TextColor.Accent);
  });

  it('serializes RichTextBlock inlines', () => {
    const card = AdaptiveCardBuilder.create()
      .addRichTextBlock((b) =>
        b
          .addText('Hello ')
          .addTextRun((r) => r.withText('World').withWeight(TextWeight.Bolder)),
      )
      .build();
    const json = toJson(card);
    assert.ok(json.includes('"type": "RichTextBlock"'));
    assert.ok(json.includes('"type": "TextRun"'));
    assert.ok(json.includes('"Hello "'));
    assert.ok(json.includes('"weight": "bolder"'));
  });
});

describe('MediaBuilder', () => {
  it('builds a Media element', () => {
    const media = new MediaBuilder()
      .withId('m1')
      .withPoster('https://example.com/poster.jpg')
      .withAltText('A video')
      .addSource('https://example.com/video.mp4', 'video/mp4')
      .addSource('https://example.com/video.webm', 'video/webm')
      .build();

    assert.equal(media.type, 'Media');
    assert.equal(media.poster, 'https://example.com/poster.jpg');
    assert.equal(media.sources!.length, 2);
    assert.equal(media.sources![0].mimeType, 'video/mp4');
  });
});

describe('ImageSetBuilder', () => {
  it('builds an ImageSet with images', () => {
    const is = new ImageSetBuilder()
      .withImageSize(ImageSize.Medium)
      .addImage((b) => b.withUrl('https://example.com/a.png'))
      .addImage((b) => b.withUrl('https://example.com/b.png'))
      .build();

    assert.equal(is.type, 'ImageSet');
    assert.equal(is.imageSize, ImageSize.Medium);
    assert.equal(is.images!.length, 2);
  });
});

describe('TableBuilder', () => {
  it('builds a Table with columns and rows', () => {
    const col: TableColumnDefinition = { width: '1' };
    const cell: TableCell = { type: 'TableCell', items: [{ type: 'TextBlock', text: 'Cell' }] };
    const row: TableRow = { type: 'TableRow', cells: [cell] };

    const table = new TableBuilder()
      .withFirstRowAsHeader()
      .withShowGridLines()
      .addColumn(col)
      .addRow(row)
      .build();

    assert.equal(table.type, 'Table');
    assert.equal(table.firstRowAsHeader, true);
    assert.equal(table.showGridLines, true);
    assert.equal(table.columns!.length, 1);
    assert.equal(table.rows!.length, 1);
  });

  it('serializes Table correctly', () => {
    const card = AdaptiveCardBuilder.create()
      .addTable((t) => {
        t.withFirstRowAsHeader();
        t.addColumn({ width: 'auto' });
        t.addRow({
          type: 'TableRow',
          cells: [{ type: 'TableCell', items: [{ type: 'TextBlock', text: 'Header' }] }],
        });
      })
      .build();

    const json = toJson(card);
    assert.ok(json.includes('"type": "Table"'));
    assert.ok(json.includes('"type": "TableRow"'));
    assert.ok(json.includes('"type": "TableCell"'));
    assert.ok(json.includes('"firstRowAsHeader": true'));
  });
});

describe('ActionSetBuilder', () => {
  it('builds an ActionSet with actions', () => {
    const as = new ActionSetBuilder()
      .withId('as1')
      .addAction((b) => b.openUrl('https://example.com', 'Visit'))
      .build();

    assert.equal(as.type, 'ActionSet');
    assert.equal(as.id, 'as1');
    assert.equal(as.actions!.length, 1);
    assert.equal(as.actions![0].type, 'Action.OpenUrl');
  });
});
