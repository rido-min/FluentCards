import { describe, it } from 'node:test';
import assert from 'node:assert/strict';
import {
  AdaptiveCardBuilder,
  InputTextBuilder,
  InputNumberBuilder,
  InputDateBuilder,
  InputTimeBuilder,
  InputToggleBuilder,
  InputChoiceSetBuilder,
  TextInputStyle,
  ChoiceInputStyle,
  Spacing,
} from 'fluent-cards';
import type {
  InputText,
  InputNumber,
  InputDate,
  InputTime,
  InputToggle,
  InputChoiceSet,
} from 'fluent-cards';

describe('InputText – deep tests', () => {
  it('builds with all properties', () => {
    const input = new InputTextBuilder()
      .withId('txt1')
      .withLabel('Name')
      .withPlaceholder('Enter name')
      .withValue('John')
      .withIsMultiline()
      .withMaxLength(100)
      .withRegex('^[a-zA-Z]+$')
      .withStyle(TextInputStyle.Text)
      .withIsRequired()
      .withErrorMessage('Name is required')
      .withSpacing(Spacing.Large)
      .build();

    assert.equal(input.type, 'Input.Text');
    assert.equal(input.id, 'txt1');
    assert.equal(input.label, 'Name');
    assert.equal(input.placeholder, 'Enter name');
    assert.equal(input.value, 'John');
    assert.equal(input.isMultiline, true);
    assert.equal(input.maxLength, 100);
    assert.equal(input.regex, '^[a-zA-Z]+$');
    assert.equal(input.style, 'text');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Name is required');
    assert.equal(input.spacing, 'large');
  });

  it('supports password style', () => {
    const input = new InputTextBuilder()
      .withId('pwd')
      .withStyle(TextInputStyle.Password)
      .build();
    assert.equal(input.style, 'password');
  });

  it('supports tel style', () => {
    const input = new InputTextBuilder()
      .withId('phone')
      .withStyle(TextInputStyle.Tel)
      .build();
    assert.equal(input.style, 'tel');
  });

  it('supports email style', () => {
    const input = new InputTextBuilder()
      .withId('email')
      .withStyle(TextInputStyle.Email)
      .build();
    assert.equal(input.style, 'email');
  });

  it('supports url style', () => {
    const input = new InputTextBuilder()
      .withId('url')
      .withStyle(TextInputStyle.Url)
      .build();
    assert.equal(input.style, 'url');
  });

  it('supports inlineAction', () => {
    const input = new InputTextBuilder()
      .withId('search')
      .withInlineAction((a) => a.submit('Go').withIconUrl('https://example.com/search.png'))
      .build();

    assert.ok(input.inlineAction);
    assert.equal(input.inlineAction!.type, 'Action.Submit');
    assert.equal(input.inlineAction!.title, 'Go');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((b) =>
        b.withId('txt2').withLabel('Comment').withIsMultiline().withMaxLength(500),
      )
      .build();

    const input = card.body![0] as InputText;
    assert.equal(input.type, 'Input.Text');
    assert.equal(input.id, 'txt2');
    assert.equal(input.isMultiline, true);
    assert.equal(input.maxLength, 500);
  });
});

describe('InputNumber – deep tests', () => {
  it('builds with all properties', () => {
    const input = new InputNumberBuilder()
      .withId('num1')
      .withLabel('Quantity')
      .withPlaceholder('Enter amount')
      .withValue(5)
      .withMin(0)
      .withMax(100)
      .withIsRequired()
      .withErrorMessage('Required')
      .build();

    assert.equal(input.type, 'Input.Number');
    assert.equal(input.id, 'num1');
    assert.equal(input.label, 'Quantity');
    assert.equal(input.placeholder, 'Enter amount');
    assert.equal(input.value, 5);
    assert.equal(input.min, 0);
    assert.equal(input.max, 100);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Required');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputNumber((b) => b.withId('n1').withMin(1).withMax(10))
      .build();

    const input = card.body![0] as InputNumber;
    assert.equal(input.type, 'Input.Number');
    assert.equal(input.min, 1);
    assert.equal(input.max, 10);
  });
});

describe('InputDate – deep tests', () => {
  it('builds with all properties', () => {
    const input = new InputDateBuilder()
      .withId('date1')
      .withLabel('Start Date')
      .withPlaceholder('Select date')
      .withValue('2024-01-15')
      .withMin('2024-01-01')
      .withMax('2024-12-31')
      .withIsRequired()
      .withErrorMessage('Date required')
      .build();

    assert.equal(input.type, 'Input.Date');
    assert.equal(input.id, 'date1');
    assert.equal(input.label, 'Start Date');
    assert.equal(input.placeholder, 'Select date');
    assert.equal(input.value, '2024-01-15');
    assert.equal(input.min, '2024-01-01');
    assert.equal(input.max, '2024-12-31');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Date required');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputDate((b) => b.withId('d1').withMin('2024-06-01').withMax('2024-06-30'))
      .build();

    const input = card.body![0] as InputDate;
    assert.equal(input.type, 'Input.Date');
    assert.equal(input.min, '2024-06-01');
    assert.equal(input.max, '2024-06-30');
  });
});

describe('InputTime – deep tests', () => {
  it('builds with all properties', () => {
    const input = new InputTimeBuilder()
      .withId('time1')
      .withLabel('Meeting Time')
      .withPlaceholder('Select time')
      .withValue('09:00')
      .withMin('08:00')
      .withMax('17:00')
      .withIsRequired()
      .withErrorMessage('Time required')
      .build();

    assert.equal(input.type, 'Input.Time');
    assert.equal(input.id, 'time1');
    assert.equal(input.label, 'Meeting Time');
    assert.equal(input.placeholder, 'Select time');
    assert.equal(input.value, '09:00');
    assert.equal(input.min, '08:00');
    assert.equal(input.max, '17:00');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Time required');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputTime((b) => b.withId('t1').withMin('09:00').withMax('18:00'))
      .build();

    const input = card.body![0] as InputTime;
    assert.equal(input.type, 'Input.Time');
    assert.equal(input.min, '09:00');
    assert.equal(input.max, '18:00');
  });
});

describe('InputToggle – deep tests', () => {
  it('builds with all properties', () => {
    const input = new InputToggleBuilder()
      .withId('toggle1')
      .withTitle('Accept terms')
      .withLabel('Terms and Conditions')
      .withValue('false')
      .withValueOn('accepted')
      .withValueOff('declined')
      .withWrap()
      .withIsRequired()
      .withErrorMessage('Must accept')
      .build();

    assert.equal(input.type, 'Input.Toggle');
    assert.equal(input.id, 'toggle1');
    assert.equal(input.title, 'Accept terms');
    assert.equal(input.label, 'Terms and Conditions');
    assert.equal(input.value, 'false');
    assert.equal(input.valueOn, 'accepted');
    assert.equal(input.valueOff, 'declined');
    assert.equal(input.wrap, true);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Must accept');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputToggle((b) =>
        b.withId('tg1').withTitle('Notify me').withValueOn('yes').withValueOff('no'),
      )
      .build();

    const input = card.body![0] as InputToggle;
    assert.equal(input.type, 'Input.Toggle');
    assert.equal(input.title, 'Notify me');
    assert.equal(input.valueOn, 'yes');
    assert.equal(input.valueOff, 'no');
  });
});

describe('InputChoiceSet – deep tests', () => {
  it('builds with choices and all properties', () => {
    const input = new InputChoiceSetBuilder()
      .withId('cs1')
      .withLabel('Color')
      .withPlaceholder('Pick a color')
      .withValue('red')
      .withStyle(ChoiceInputStyle.Expanded)
      .withIsMultiSelect()
      .withWrap()
      .withIsRequired()
      .withErrorMessage('Selection required')
      .addChoice('Red', 'red')
      .addChoice('Blue', 'blue')
      .addChoice('Green', 'green')
      .build();

    assert.equal(input.type, 'Input.ChoiceSet');
    assert.equal(input.id, 'cs1');
    assert.equal(input.label, 'Color');
    assert.equal(input.placeholder, 'Pick a color');
    assert.equal(input.value, 'red');
    assert.equal(input.style, 'expanded');
    assert.equal(input.isMultiSelect, true);
    assert.equal(input.wrap, true);
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Selection required');
    assert.equal(input.choices!.length, 3);
    assert.deepEqual(input.choices![0], { title: 'Red', value: 'red' });
    assert.deepEqual(input.choices![1], { title: 'Blue', value: 'blue' });
    assert.deepEqual(input.choices![2], { title: 'Green', value: 'green' });
  });

  it('compact style', () => {
    const input = new InputChoiceSetBuilder()
      .withId('cs2')
      .withStyle(ChoiceInputStyle.Compact)
      .addChoice('A', 'a')
      .build();
    assert.equal(input.style, 'compact');
  });

  it('filtered style', () => {
    const input = new InputChoiceSetBuilder()
      .withId('cs3')
      .withStyle(ChoiceInputStyle.Filtered)
      .addChoice('X', 'x')
      .build();
    assert.equal(input.style, 'filtered');
  });

  it('withIsMultiSelect(false) sets isMultiSelect to false', () => {
    const input = new InputChoiceSetBuilder()
      .withId('cs4')
      .withIsMultiSelect(false)
      .build();
    assert.equal(input.isMultiSelect, false);
  });

  it('withChoicesData sets dynamic dataset', () => {
    const input = new InputChoiceSetBuilder()
      .withId('people')
      .withChoicesData('graph.microsoft.com/users')
      .build();

    assert.ok(input['choices.data']);
    assert.equal(input['choices.data']!.type, 'Data.Query');
    assert.equal(input['choices.data']!.dataset, 'graph.microsoft.com/users');
  });

  it('via card builder', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputChoiceSet((b) =>
        b
          .withId('cs5')
          .withIsMultiSelect()
          .addChoice('Yes', 'yes')
          .addChoice('No', 'no'),
      )
      .build();

    const input = card.body![0] as InputChoiceSet;
    assert.equal(input.type, 'Input.ChoiceSet');
    assert.equal(input.isMultiSelect, true);
    assert.equal(input.choices!.length, 2);
  });
});

describe('All inputs – common properties', () => {
  it('InputText has id, label, isRequired, errorMessage', () => {
    const input = new InputTextBuilder()
      .withId('common1')
      .withLabel('Field')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common1');
    assert.equal(input.label, 'Field');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });

  it('InputNumber has id, label, isRequired, errorMessage', () => {
    const input = new InputNumberBuilder()
      .withId('common2')
      .withLabel('Amount')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common2');
    assert.equal(input.label, 'Amount');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });

  it('InputDate has id, label, isRequired, errorMessage', () => {
    const input = new InputDateBuilder()
      .withId('common3')
      .withLabel('Date')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common3');
    assert.equal(input.label, 'Date');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });

  it('InputTime has id, label, isRequired, errorMessage', () => {
    const input = new InputTimeBuilder()
      .withId('common4')
      .withLabel('Time')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common4');
    assert.equal(input.label, 'Time');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });

  it('InputToggle has id, label, isRequired, errorMessage', () => {
    const input = new InputToggleBuilder()
      .withId('common5')
      .withTitle('Toggle')
      .withLabel('Setting')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common5');
    assert.equal(input.label, 'Setting');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });

  it('InputChoiceSet has id, label, isRequired, errorMessage', () => {
    const input = new InputChoiceSetBuilder()
      .withId('common6')
      .withLabel('Choice')
      .withIsRequired()
      .withErrorMessage('Error')
      .build();
    assert.equal(input.id, 'common6');
    assert.equal(input.label, 'Choice');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Error');
  });
});
