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
  ImageSetBuilder,
  MediaBuilder,
  InputTextBuilder,
  InputNumberBuilder,
  InputDateBuilder,
  InputTimeBuilder,
  InputToggleBuilder,
  InputChoiceSetBuilder,
  RefreshBuilder,
  AuthenticationBuilder,
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
  ActionMode,
  AssociatedInputs,
  ChoiceInputStyle,
  Spacing,
  TextInputStyle,
  InputLabelPosition,
  InputStyle,
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
  ShowCardAction,
  ToggleVisibilityAction,
  ImageSet,
  Media,
  InputText,
  InputNumber,
  InputDate,
  InputTime,
  InputToggle,
  InputChoiceSet,
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
              .withFontType(FontType.Monospace)
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
    const run = rtb.inlines![1] as { type: string; text: string; weight: string; color: string; fontType: string; italic: boolean; underline: boolean };
    assert.equal(run.type, 'TextRun');
    assert.equal(run.text, 'Bold text');
    assert.equal(run.weight, 'bolder');
    assert.equal(run.color, 'accent');
    assert.equal(run.fontType, 'monospace');
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

// ═══════════════════════════════════════════════════════════════════════════════
// Input Elements
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Input.Text', () => {
  it('builds an Input.Text with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((b) =>
        b
          .withId('txt1')
          .withLabel('Name')
          .withPlaceholder('Enter name')
          .withValue('John')
          .withMaxLength(100)
          .withIsMultiline()
          .withStyle(TextInputStyle.Email)
          .withRegex('^[a-z]+$')
          .withIsRequired()
          .withErrorMessage('Required field')
          .withSpacing(Spacing.Medium)
          .withSeparator()
          .withIsVisible(true)
          .withHeight('stretch')
          .withLabelPosition(InputLabelPosition.Above)
          .withLabelWidth('100px')
          .withInputStyle(InputStyle.RevealOnHover)
          .withInlineAction((a) => a.openUrl('https://example.com', 'Go')),
      )
      .build();

    const input = card.body![0] as InputText;
    assert.equal(input.type, 'Input.Text');
    assert.equal(input.id, 'txt1');
    assert.equal(input.label, 'Name');
    assert.equal(input.placeholder, 'Enter name');
    assert.equal(input.value, 'John');
    assert.equal(input.maxLength, 100);
    assert.equal(input.isMultiline, true);
    assert.equal(input.style, 'email');
    assert.equal(input.regex, '^[a-z]+$');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Required field');
    assert.equal(input.spacing, 'medium');
    assert.equal(input.separator, true);
    assert.equal(input.isVisible, true);
    assert.equal(input.height, 'stretch');
    assert.equal(input.labelPosition, 'above');
    assert.equal(input.labelWidth, '100px');
    assert.equal(input.inputStyle, 'revealOnHover');
    assert.ok(input.inlineAction);
    assert.equal(input.inlineAction!.type, 'Action.OpenUrl');
  });

  it('omits unset Input.Text optional properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((b) => b.withId('txt2'))
      .build();

    const input = card.body![0] as InputText;
    assert.equal(input.type, 'Input.Text');
    assert.equal(input.id, 'txt2');
    assert.equal(input.label, undefined);
    assert.equal(input.placeholder, undefined);
    assert.equal(input.value, undefined);
    assert.equal(input.maxLength, undefined);
    assert.equal(input.isMultiline, undefined);
    assert.equal(input.style, undefined);
    assert.equal(input.regex, undefined);
    assert.equal(input.isRequired, undefined);
    assert.equal(input.errorMessage, undefined);
    assert.equal(input.inlineAction, undefined);
  });
});

describe('Schema conformance – Input.Number', () => {
  it('builds an Input.Number with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputNumber((b) =>
        b
          .withId('num1')
          .withLabel('Age')
          .withPlaceholder('Enter age')
          .withValue(25)
          .withMin(0)
          .withMax(120)
          .withIsRequired()
          .withErrorMessage('Invalid age')
          .withSpacing(Spacing.Large)
          .withSeparator()
          .withLabelPosition(InputLabelPosition.Inline)
          .withLabelWidth('80px')
          .withInputStyle(InputStyle.Default),
      )
      .build();

    const input = card.body![0] as InputNumber;
    assert.equal(input.type, 'Input.Number');
    assert.equal(input.id, 'num1');
    assert.equal(input.label, 'Age');
    assert.equal(input.placeholder, 'Enter age');
    assert.equal(input.value, 25);
    assert.equal(input.min, 0);
    assert.equal(input.max, 120);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Invalid age');
    assert.equal(input.spacing, 'large');
    assert.equal(input.separator, true);
    assert.equal(input.labelPosition, 'inline');
    assert.equal(input.labelWidth, '80px');
    assert.equal(input.inputStyle, 'default');
  });

  it('omits unset Input.Number optional properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputNumber((b) => b.withId('num2'))
      .build();

    const input = card.body![0] as InputNumber;
    assert.equal(input.type, 'Input.Number');
    assert.equal(input.id, 'num2');
    assert.equal(input.label, undefined);
    assert.equal(input.placeholder, undefined);
    assert.equal(input.value, undefined);
    assert.equal(input.min, undefined);
    assert.equal(input.max, undefined);
  });
});

describe('Schema conformance – Input.Date', () => {
  it('builds an Input.Date with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputDate((b) =>
        b
          .withId('date1')
          .withLabel('Birthday')
          .withPlaceholder('YYYY-MM-DD')
          .withValue('2000-01-15')
          .withMin('1900-01-01')
          .withMax('2030-12-31')
          .withIsRequired()
          .withErrorMessage('Invalid date')
          .withSpacing(Spacing.Small)
          .withSeparator()
          .withLabelPosition(InputLabelPosition.Above)
          .withLabelWidth('120px')
          .withInputStyle(InputStyle.RevealOnHover),
      )
      .build();

    const input = card.body![0] as InputDate;
    assert.equal(input.type, 'Input.Date');
    assert.equal(input.id, 'date1');
    assert.equal(input.label, 'Birthday');
    assert.equal(input.placeholder, 'YYYY-MM-DD');
    assert.equal(input.value, '2000-01-15');
    assert.equal(input.min, '1900-01-01');
    assert.equal(input.max, '2030-12-31');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Invalid date');
    assert.equal(input.spacing, 'small');
    assert.equal(input.separator, true);
  });
});

describe('Schema conformance – Input.Time', () => {
  it('builds an Input.Time with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputTime((b) =>
        b
          .withId('time1')
          .withLabel('Meeting time')
          .withPlaceholder('HH:MM')
          .withValue('14:30')
          .withMin('08:00')
          .withMax('18:00')
          .withIsRequired()
          .withErrorMessage('Invalid time')
          .withSpacing(Spacing.ExtraLarge)
          .withSeparator()
          .withLabelPosition(InputLabelPosition.Inline)
          .withLabelWidth('90px')
          .withInputStyle(InputStyle.Default),
      )
      .build();

    const input = card.body![0] as InputTime;
    assert.equal(input.type, 'Input.Time');
    assert.equal(input.id, 'time1');
    assert.equal(input.label, 'Meeting time');
    assert.equal(input.placeholder, 'HH:MM');
    assert.equal(input.value, '14:30');
    assert.equal(input.min, '08:00');
    assert.equal(input.max, '18:00');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Invalid time');
    assert.equal(input.spacing, 'extraLarge');
    assert.equal(input.separator, true);
  });
});

describe('Schema conformance – Input.Toggle', () => {
  it('builds an Input.Toggle with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputToggle((b) =>
        b
          .withId('toggle1')
          .withTitle('Accept terms')
          .withLabel('Terms and conditions')
          .withValue('true')
          .withValueOn('yes')
          .withValueOff('no')
          .withWrap()
          .withIsRequired()
          .withErrorMessage('Must accept')
          .withSpacing(Spacing.Medium)
          .withSeparator()
          .withLabelPosition(InputLabelPosition.Above)
          .withLabelWidth('150px')
          .withInputStyle(InputStyle.RevealOnHover),
      )
      .build();

    const input = card.body![0] as InputToggle;
    assert.equal(input.type, 'Input.Toggle');
    assert.equal(input.id, 'toggle1');
    assert.equal(input.title, 'Accept terms');
    assert.equal(input.label, 'Terms and conditions');
    assert.equal(input.value, 'true');
    assert.equal(input.valueOn, 'yes');
    assert.equal(input.valueOff, 'no');
    assert.equal(input.wrap, true);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Must accept');
    assert.equal(input.spacing, 'medium');
    assert.equal(input.separator, true);
  });

  it('omits unset Input.Toggle optional properties (except title)', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputToggle((b) => b.withId('toggle2').withTitle('Check'))
      .build();

    const input = card.body![0] as InputToggle;
    assert.equal(input.type, 'Input.Toggle');
    assert.equal(input.id, 'toggle2');
    assert.equal(input.title, 'Check');
    assert.equal(input.value, undefined);
    assert.equal(input.valueOn, undefined);
    assert.equal(input.valueOff, undefined);
    assert.equal(input.wrap, undefined);
  });
});

describe('Schema conformance – Input.ChoiceSet', () => {
  it('builds an Input.ChoiceSet with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputChoiceSet((b) =>
        b
          .withId('choice1')
          .withLabel('Favorite color')
          .withPlaceholder('Pick one')
          .withValue('red')
          .withStyle(ChoiceInputStyle.Expanded)
          .withIsMultiSelect()
          .withWrap()
          .withIsRequired()
          .withErrorMessage('Must choose')
          .withSpacing(Spacing.Large)
          .withSeparator()
          .withLabelPosition(InputLabelPosition.Above)
          .withLabelWidth('100px')
          .withInputStyle(InputStyle.Default)
          .addChoice('Red', 'red')
          .addChoice('Blue', 'blue')
          .addChoice('Green', 'green'),
      )
      .build();

    const input = card.body![0] as InputChoiceSet;
    assert.equal(input.type, 'Input.ChoiceSet');
    assert.equal(input.id, 'choice1');
    assert.equal(input.label, 'Favorite color');
    assert.equal(input.placeholder, 'Pick one');
    assert.equal(input.value, 'red');
    assert.equal(input.style, 'expanded');
    assert.equal(input.isMultiSelect, true);
    assert.equal(input.wrap, true);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Must choose');
    assert.equal(input.choices!.length, 3);
    assert.deepEqual(input.choices![0], { title: 'Red', value: 'red' });
    assert.deepEqual(input.choices![1], { title: 'Blue', value: 'blue' });
    assert.deepEqual(input.choices![2], { title: 'Green', value: 'green' });
  });

  it('builds an Input.ChoiceSet with choices.data (dynamic dataset)', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputChoiceSet((b) =>
        b
          .withId('people1')
          .withStyle(ChoiceInputStyle.Filtered)
          .withChoicesData('graph.microsoft.com/users'),
      )
      .build();

    const input = card.body![0] as InputChoiceSet;
    assert.equal(input.type, 'Input.ChoiceSet');
    assert.equal(input.style, 'filtered');
    assert.ok(input['choices.data']);
    assert.equal(input['choices.data']!.type, 'Data.Query');
    assert.equal(input['choices.data']!.dataset, 'graph.microsoft.com/users');
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// ImageSet
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – ImageSet', () => {
  it('builds an ImageSet with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addImageSet((b) =>
        b
          .withId('is1')
          .withImageSize(ImageSize.Medium)
          .withSpacing(Spacing.Large)
          .withSeparator()
          .withIsVisible(true)
          .withHeight('auto')
          .addImage((img) => img.withUrl('https://example.com/1.png').withAltText('Pic 1'))
          .addImage((img) => img.withUrl('https://example.com/2.png').withAltText('Pic 2')),
      )
      .build();

    const imageSet = card.body![0] as ImageSet;
    assert.equal(imageSet.type, 'ImageSet');
    assert.equal(imageSet.id, 'is1');
    assert.equal(imageSet.imageSize, 'medium');
    assert.equal(imageSet.spacing, 'large');
    assert.equal(imageSet.separator, true);
    assert.equal(imageSet.isVisible, true);
    assert.equal(imageSet.height, 'auto');
    assert.equal(imageSet.images!.length, 2);
    assert.equal(imageSet.images![0].url, 'https://example.com/1.png');
    assert.equal(imageSet.images![1].url, 'https://example.com/2.png');
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// Media
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Media', () => {
  it('builds a Media element with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addMedia((b) =>
        b
          .withId('media1')
          .withPoster('https://example.com/poster.png')
          .withAltText('A video')
          .withSpacing(Spacing.Medium)
          .withSeparator()
          .withIsVisible(true)
          .withHeight('auto')
          .addSource('https://example.com/video.mp4', 'video/mp4')
          .addSource('https://example.com/video.webm', 'video/webm')
          .addCaptionSource('https://example.com/video.vtt', 'text/vtt', 'English'),
      )
      .build();

    const media = card.body![0] as Media;
    assert.equal(media.type, 'Media');
    assert.equal(media.id, 'media1');
    assert.equal(media.poster, 'https://example.com/poster.png');
    assert.equal(media.altText, 'A video');
    assert.equal(media.spacing, 'medium');
    assert.equal(media.separator, true);
    assert.equal(media.isVisible, true);
    assert.equal(media.height, 'auto');
    assert.equal(media.sources!.length, 2);
    assert.deepEqual(media.sources![0], { url: 'https://example.com/video.mp4', mimeType: 'video/mp4' });
    assert.deepEqual(media.sources![1], { url: 'https://example.com/video.webm', mimeType: 'video/webm' });
    assert.equal(media.captionSources!.length, 1);
    assert.deepEqual(media.captionSources![0], { url: 'https://example.com/video.vtt', mimeType: 'text/vtt', label: 'English' });
    assert.equal('type' in media.captionSources![0], false);
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// Action Details
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Action.OpenUrl details', () => {
  it('builds Action.OpenUrl with all base action properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((a) =>
        a
          .openUrl('https://example.com', 'Open Link')
          .withId('openUrl1')
          .withIconUrl('https://example.com/icon.png')
          .withStyle(ActionStyle.Positive)
          .withIsEnabled(true)
          .withTooltip('Opens example.com')
          .withMode(ActionMode.Primary),
      )
      .build();

    const action = card.actions![0] as OpenUrlAction;
    assert.equal(action.type, 'Action.OpenUrl');
    assert.equal(action.url, 'https://example.com');
    assert.equal(action.title, 'Open Link');
    assert.equal(action.id, 'openUrl1');
    assert.equal(action.iconUrl, 'https://example.com/icon.png');
    assert.equal(action.style, 'positive');
    assert.equal(action.isEnabled, true);
    assert.equal(action.tooltip, 'Opens example.com');
    assert.equal(action.mode, 'primary');
  });
});

describe('Schema conformance – Action.Submit details', () => {
  it('builds Action.Submit with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((a) =>
        a
          .submit('Send')
          .withId('submit1')
          .withData({ action: 'save', id: 42 })
          .withAssociatedInputs(AssociatedInputs.Auto)
          .withIconUrl('https://example.com/send.png')
          .withStyle(ActionStyle.Destructive)
          .withIsEnabled(false)
          .withTooltip('Submit the form')
          .withMode(ActionMode.Secondary),
      )
      .build();

    const action = card.actions![0] as SubmitAction;
    assert.equal(action.type, 'Action.Submit');
    assert.equal(action.title, 'Send');
    assert.equal(action.id, 'submit1');
    assert.deepEqual(action.data, { action: 'save', id: 42 });
    assert.equal(action.associatedInputs, 'auto');
    assert.equal(action.iconUrl, 'https://example.com/send.png');
    assert.equal(action.style, 'destructive');
    assert.equal(action.isEnabled, false);
    assert.equal(action.tooltip, 'Submit the form');
    assert.equal(action.mode, 'secondary');
  });
});

describe('Schema conformance – Action.Execute details', () => {
  it('builds Action.Execute with all properties', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((a) =>
        a
          .execute('Run')
          .withId('exec1')
          .withVerb('doStuff')
          .withData({ key: 'value' })
          .withAssociatedInputs(AssociatedInputs.None)
          .withIconUrl('https://example.com/exec.png')
          .withStyle(ActionStyle.Positive)
          .withIsEnabled(true)
          .withTooltip('Execute an action')
          .withMode(ActionMode.Primary),
      )
      .build();

    const action = card.actions![0] as ExecuteAction;
    assert.equal(action.type, 'Action.Execute');
    assert.equal(action.title, 'Run');
    assert.equal(action.id, 'exec1');
    assert.equal(action.verb, 'doStuff');
    assert.deepEqual(action.data, { key: 'value' });
    assert.equal(action.associatedInputs, 'none');
    assert.equal(action.iconUrl, 'https://example.com/exec.png');
    assert.equal(action.style, 'positive');
    assert.equal(action.isEnabled, true);
    assert.equal(action.tooltip, 'Execute an action');
    assert.equal(action.mode, 'primary');
  });
});

describe('Schema conformance – Action.ShowCard details', () => {
  it('builds Action.ShowCard with an embedded card', () => {
    const innerCard = AdaptiveCardBuilder.create()
      .addTextBlock((tb) => tb.withText('Inner card'))
      .build();

    const card = AdaptiveCardBuilder.create()
      .addAction((a) =>
        a
          .showCard('Show Details')
          .withId('showCard1')
          .withCard(innerCard)
          .withStyle(ActionStyle.Default)
          .withMode(ActionMode.Secondary),
      )
      .build();

    const action = card.actions![0] as ShowCardAction;
    assert.equal(action.type, 'Action.ShowCard');
    assert.equal(action.title, 'Show Details');
    assert.equal(action.id, 'showCard1');
    assert.ok(action.card);
    assert.equal(action.card!.body![0].type, 'TextBlock');
    assert.equal(action.style, 'default');
    assert.equal(action.mode, 'secondary');
  });
});

describe('Schema conformance – Action.ToggleVisibility details', () => {
  it('builds Action.ToggleVisibility with target elements', () => {
    const card = AdaptiveCardBuilder.create()
      .addAction((a) =>
        a
          .toggleVisibility('Toggle')
          .withId('toggle1')
          .addTargetElement('elem1')
          .addTargetElement('elem2', true)
          .addTargetElement('elem3', false)
          .withStyle(ActionStyle.Positive)
          .withMode(ActionMode.Primary),
      )
      .build();

    const action = card.actions![0] as ToggleVisibilityAction;
    assert.equal(action.type, 'Action.ToggleVisibility');
    assert.equal(action.title, 'Toggle');
    assert.equal(action.id, 'toggle1');
    assert.equal(action.targetElements!.length, 3);
    assert.equal(action.style, 'positive');
    assert.equal(action.mode, 'primary');
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// Enum Value Tests
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Enums', () => {
  it('TextSize has all spec values', () => {
    const values: string[] = Object.values(TextSize);
    assert.ok(values.includes('small'));
    assert.ok(values.includes('default'));
    assert.ok(values.includes('medium'));
    assert.ok(values.includes('large'));
    assert.ok(values.includes('extraLarge'));
    assert.equal(values.length, 5);
  });

  it('TextWeight has all spec values', () => {
    const values: string[] = Object.values(TextWeight);
    assert.ok(values.includes('lighter'));
    assert.ok(values.includes('default'));
    assert.ok(values.includes('bolder'));
    assert.equal(values.length, 3);
  });

  it('TextColor has all spec values', () => {
    const values: string[] = Object.values(TextColor);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('dark'));
    assert.ok(values.includes('light'));
    assert.ok(values.includes('accent'));
    assert.ok(values.includes('good'));
    assert.ok(values.includes('attention'));
    assert.ok(values.includes('warning'));
    assert.equal(values.length, 7);
  });

  it('FontType has all spec values', () => {
    const values: string[] = Object.values(FontType);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('monospace'));
    assert.equal(values.length, 2);
  });

  it('TextBlockStyle has all spec values', () => {
    const values: string[] = Object.values(TextBlockStyle);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('heading'));
    assert.equal(values.length, 2);
  });

  it('HorizontalAlignment has all spec values', () => {
    const values: string[] = Object.values(HorizontalAlignment);
    assert.ok(values.includes('left'));
    assert.ok(values.includes('center'));
    assert.ok(values.includes('right'));
    assert.equal(values.length, 3);
  });

  it('VerticalAlignment has all spec values', () => {
    const values: string[] = Object.values(VerticalAlignment);
    assert.ok(values.includes('top'));
    assert.ok(values.includes('center'));
    assert.ok(values.includes('bottom'));
    assert.equal(values.length, 3);
  });

  it('ImageSize has all spec values', () => {
    const values: string[] = Object.values(ImageSize);
    assert.ok(values.includes('auto'));
    assert.ok(values.includes('stretch'));
    assert.ok(values.includes('small'));
    assert.ok(values.includes('medium'));
    assert.ok(values.includes('large'));
    assert.equal(values.length, 5);
  });

  it('ImageStyle has all spec values', () => {
    const values: string[] = Object.values(ImageStyle);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('person'));
    assert.equal(values.length, 2);
  });

  it('ContainerStyle has all spec values', () => {
    const values: string[] = Object.values(ContainerStyle);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('emphasis'));
    assert.ok(values.includes('good'));
    assert.ok(values.includes('attention'));
    assert.ok(values.includes('warning'));
    assert.ok(values.includes('accent'));
    assert.equal(values.length, 6);
  });

  it('ActionStyle has all spec values', () => {
    const values: string[] = Object.values(ActionStyle);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('positive'));
    assert.ok(values.includes('destructive'));
    assert.equal(values.length, 3);
  });

  it('ChoiceInputStyle has all spec values', () => {
    const values: string[] = Object.values(ChoiceInputStyle);
    assert.ok(values.includes('compact'));
    assert.ok(values.includes('expanded'));
    assert.ok(values.includes('filtered'));
    assert.equal(values.length, 3);
  });

  it('AssociatedInputs has all spec values', () => {
    const values: string[] = Object.values(AssociatedInputs);
    assert.ok(values.includes('auto'));
    assert.ok(values.includes('none'));
    assert.equal(values.length, 2);
  });

  it('Spacing has all spec values', () => {
    const values: string[] = Object.values(Spacing);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('none'));
    assert.ok(values.includes('small'));
    assert.ok(values.includes('medium'));
    assert.ok(values.includes('large'));
    assert.ok(values.includes('extraLarge'));
    assert.ok(values.includes('padding'));
    assert.equal(values.length, 7);
  });

  it('BackgroundImageFillMode has all spec values', () => {
    const values: string[] = Object.values(BackgroundImageFillMode);
    assert.ok(values.includes('cover'));
    assert.ok(values.includes('repeatHorizontally'));
    assert.ok(values.includes('repeatVertically'));
    assert.ok(values.includes('repeat'));
    assert.equal(values.length, 4);
  });

  it('ActionMode has all spec values', () => {
    const values: string[] = Object.values(ActionMode);
    assert.ok(values.includes('primary'));
    assert.ok(values.includes('secondary'));
    assert.equal(values.length, 2);
  });

  it('TextInputStyle has all spec values', () => {
    const values: string[] = Object.values(TextInputStyle);
    assert.ok(values.includes('text'));
    assert.ok(values.includes('tel'));
    assert.ok(values.includes('url'));
    assert.ok(values.includes('email'));
    assert.ok(values.includes('password'));
    assert.equal(values.length, 5);
  });

  it('InputLabelPosition has all spec values', () => {
    const values: string[] = Object.values(InputLabelPosition);
    assert.ok(values.includes('inline'));
    assert.ok(values.includes('above'));
    assert.equal(values.length, 2);
  });

  it('InputStyle has all spec values', () => {
    const values: string[] = Object.values(InputStyle);
    assert.ok(values.includes('default'));
    assert.ok(values.includes('revealOnHover'));
    assert.equal(values.length, 2);
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// Advanced Features
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Authentication', () => {
  it('builds a card with authentication configuration', () => {
    const card = AdaptiveCardBuilder.create()
      .withAuthentication((auth) =>
        auth
          .withText('Please sign in')
          .withConnectionName('myConnection')
          .withTokenExchangeResource({
            id: 'ter1',
            uri: 'https://example.com/token',
            providerId: 'provider1',
          })
          .addButton({
            type: 'signin',
            title: 'Sign In',
            image: 'https://example.com/signin.png',
            value: 'https://example.com/auth',
          }),
      )
      .build();

    assert.ok(card.authentication);
    assert.equal(card.authentication!.text, 'Please sign in');
    assert.equal(card.authentication!.connectionName, 'myConnection');
    assert.ok(card.authentication!.tokenExchangeResource);
    assert.equal(card.authentication!.tokenExchangeResource!.id, 'ter1');
    assert.equal(card.authentication!.tokenExchangeResource!.uri, 'https://example.com/token');
    assert.equal(card.authentication!.tokenExchangeResource!.providerId, 'provider1');
    assert.equal(card.authentication!.buttons!.length, 1);
    assert.equal(card.authentication!.buttons![0].type, 'signin');
    assert.equal(card.authentication!.buttons![0].title, 'Sign In');
    assert.equal(card.authentication!.buttons![0].image, 'https://example.com/signin.png');
    assert.equal(card.authentication!.buttons![0].value, 'https://example.com/auth');
  });
});

describe('Schema conformance – Refresh', () => {
  it('builds a card with refresh configuration', () => {
    const card = AdaptiveCardBuilder.create()
      .withRefresh((r) =>
        r
          .withAction((a) => a.execute('Refresh').withVerb('refresh'))
          .addUserId('user1')
          .addUserId('user2')
          .withExpires('2030-01-01T00:00:00Z'),
      )
      .build();

    assert.ok(card.refresh);
    assert.ok(card.refresh!.action);
    assert.equal((card.refresh!.action as ExecuteAction).type, 'Action.Execute');
    assert.equal((card.refresh!.action as ExecuteAction).verb, 'refresh');
    assert.deepEqual(card.refresh!.userIds, ['user1', 'user2']);
    assert.equal(card.refresh!.expires, '2030-01-01T00:00:00Z');
  });
});

// ═══════════════════════════════════════════════════════════════════════════════
// Common Properties on Elements
// ═══════════════════════════════════════════════════════════════════════════════

describe('Schema conformance – Common element properties', () => {
  it('TextBlock supports separator, spacing, isVisible, fallback', () => {
    const tb = new TextBlockBuilder()
      .withText('Test')
      .withSpacing(Spacing.Large)
      .withSeparator(true)
      .withIsVisible(false)
      .withFallback('drop')
      .build();

    assert.equal(tb.spacing, 'large');
    assert.equal(tb.separator, true);
    assert.equal(tb.isVisible, false);
    assert.equal(tb.fallback, 'drop');
  });

  it('Container supports separator, spacing, isVisible, fallback', () => {
    const card = AdaptiveCardBuilder.create()
      .addContainer((c) =>
        c
          .withSpacing(Spacing.ExtraLarge)
          .withSeparator(true)
          .withIsVisible(false)
          .withHeight('stretch')
          .withFallback('drop'),
      )
      .build();

    const container = card.body![0] as Container;
    assert.equal(container.spacing, 'extraLarge');
    assert.equal(container.separator, true);
    assert.equal(container.isVisible, false);
    assert.equal(container.height, 'stretch');
    assert.equal(container.fallback, 'drop');
  });

  it('ColumnSet supports separator, spacing, isVisible, fallback', () => {
    const card = AdaptiveCardBuilder.create()
      .addColumnSet((cs) =>
        cs
          .withSpacing(Spacing.Small)
          .withSeparator(true)
          .withIsVisible(false)
          .withHeight('auto')
          .withFallback('drop'),
      )
      .build();

    const colSet = card.body![0] as ColumnSet;
    assert.equal(colSet.spacing, 'small');
    assert.equal(colSet.separator, true);
    assert.equal(colSet.isVisible, false);
    assert.equal(colSet.height, 'auto');
    assert.equal(colSet.fallback, 'drop');
  });

  it('Image supports separator, spacing, isVisible, fallback, backgroundColor', () => {
    const card = AdaptiveCardBuilder.create()
      .addImage((img) =>
        img
          .withUrl('https://example.com/pic.png')
          .withSpacing(Spacing.Medium)
          .withSeparator(true)
          .withIsVisible(false)
          .withBackgroundColor('#FF0000')
          .withFallback('drop'),
      )
      .build();

    const image = card.body![0] as Image;
    assert.equal(image.spacing, 'medium');
    assert.equal(image.separator, true);
    assert.equal(image.isVisible, false);
    assert.equal(image.backgroundColor, '#FF0000');
    assert.equal(image.fallback, 'drop');
  });

  it('RichTextBlock supports separator, spacing, isVisible, fallback', () => {
    const card = AdaptiveCardBuilder.create()
      .addRichTextBlock((rtb) =>
        rtb
          .addText('Test')
          .withSpacing(Spacing.Padding)
          .withSeparator()
          .withIsVisible(true)
          .withHeight('auto')
          .withFallback('drop'),
      )
      .build();

    const rtb = card.body![0] as RichTextBlock;
    assert.equal(rtb.spacing, 'padding');
    assert.equal(rtb.separator, true);
    assert.equal(rtb.isVisible, true);
    assert.equal(rtb.height, 'auto');
    assert.equal(rtb.fallback, 'drop');
  });

  it('FactSet supports separator, spacing, isVisible', () => {
    const card = AdaptiveCardBuilder.create()
      .addFactSet((fs) =>
        fs
          .addFact('Key', 'Value')
          .withSpacing(Spacing.None)
          .withSeparator()
          .withIsVisible(false)
          .withHeight('auto')
          .withFallback('drop'),
      )
      .build();

    const factSet = card.body![0] as FactSet;
    assert.equal(factSet.spacing, 'none');
    assert.equal(factSet.separator, true);
    assert.equal(factSet.isVisible, false);
    assert.equal(factSet.height, 'auto');
    assert.equal(factSet.fallback, 'drop');
  });

  it('ActionSet supports separator, spacing, isVisible', () => {
    const card = AdaptiveCardBuilder.create()
      .addActionSet((as) =>
        as
          .addAction((a) => a.openUrl('https://example.com'))
          .withSpacing(Spacing.Large)
          .withSeparator()
          .withIsVisible(true)
          .withHeight('auto')
          .withFallback('drop'),
      )
      .build();

    const actionSet = card.body![0] as ActionSet;
    assert.equal(actionSet.spacing, 'large');
    assert.equal(actionSet.separator, true);
    assert.equal(actionSet.isVisible, true);
    assert.equal(actionSet.height, 'auto');
    assert.equal(actionSet.fallback, 'drop');
  });

  it('Input.Text supports common base properties (spacing, separator, isVisible, fallback)', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((b) =>
        b
          .withId('txt-common')
          .withSpacing(Spacing.Medium)
          .withSeparator()
          .withIsVisible(false)
          .withHeight('stretch')
          .withFallback('drop'),
      )
      .build();

    const input = card.body![0] as InputText;
    assert.equal(input.spacing, 'medium');
    assert.equal(input.separator, true);
    assert.equal(input.isVisible, false);
    assert.equal(input.height, 'stretch');
    assert.equal(input.fallback, 'drop');
  });
});
