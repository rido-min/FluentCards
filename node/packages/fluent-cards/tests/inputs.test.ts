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
  toJson,
  TextInputStyle,
  ChoiceInputStyle,
} from 'fluent-cards';
import type { InputText, InputNumber, InputDate, InputTime, InputToggle, InputChoiceSet } from 'fluent-cards';

describe('InputTextBuilder', () => {
  it('builds an InputText with all properties', () => {
    const input = new InputTextBuilder()
      .withId('name')
      .withLabel('Your Name')
      .withPlaceholder('Enter name...')
      .withValue('Alice')
      .withMaxLength(100)
      .withIsMultiline()
      .withStyle(TextInputStyle.Email)
      .withRegex('[a-z]+')
      .withIsRequired()
      .withErrorMessage('Required')
      .build();

    assert.equal(input.type, 'Input.Text');
    assert.equal(input.id, 'name');
    assert.equal(input.label, 'Your Name');
    assert.equal(input.placeholder, 'Enter name...');
    assert.equal(input.value, 'Alice');
    assert.equal(input.maxLength, 100);
    assert.equal(input.isMultiline, true);
    assert.equal(input.style, TextInputStyle.Email);
    assert.equal(input.regex, '[a-z]+');
    assert.equal(input.isRequired, true);
    assert.equal(input.errorMessage, 'Required');
  });

  it('type discriminant is Input.Text', () => {
    const input = new InputTextBuilder().withId('x').build();
    assert.equal(input.type, 'Input.Text');
  });
});

describe('InputNumberBuilder', () => {
  it('builds an InputNumber with constraints', () => {
    const input = new InputNumberBuilder()
      .withId('age')
      .withLabel('Age')
      .withMin(0)
      .withMax(120)
      .withValue(25)
      .build();

    assert.equal(input.type, 'Input.Number');
    assert.equal(input.id, 'age');
    assert.equal(input.min, 0);
    assert.equal(input.max, 120);
    assert.equal(input.value, 25);
  });
});

describe('InputDateBuilder', () => {
  it('builds an InputDate', () => {
    const input = new InputDateBuilder()
      .withId('dob')
      .withMin('2000-01-01')
      .withMax('2024-12-31')
      .withValue('2024-01-01')
      .build();

    assert.equal(input.type, 'Input.Date');
    assert.equal(input.min, '2000-01-01');
    assert.equal(input.max, '2024-12-31');
    assert.equal(input.value, '2024-01-01');
  });
});

describe('InputTimeBuilder', () => {
  it('builds an InputTime', () => {
    const input = new InputTimeBuilder()
      .withId('startTime')
      .withMin('09:00')
      .withMax('17:00')
      .withValue('10:00')
      .build();

    assert.equal(input.type, 'Input.Time');
    assert.equal(input.min, '09:00');
    assert.equal(input.max, '17:00');
    assert.equal(input.value, '10:00');
  });
});

describe('InputToggleBuilder', () => {
  it('builds an InputToggle', () => {
    const input = new InputToggleBuilder()
      .withId('accept')
      .withTitle('Accept Terms')
      .withValueOn('yes')
      .withValueOff('no')
      .withWrap()
      .build();

    assert.equal(input.type, 'Input.Toggle');
    assert.equal(input.id, 'accept');
    assert.equal(input.title, 'Accept Terms');
    assert.equal(input.valueOn, 'yes');
    assert.equal(input.valueOff, 'no');
    assert.equal(input.wrap, true);
  });
});

describe('InputChoiceSetBuilder', () => {
  it('builds an InputChoiceSet with choices', () => {
    const input = new InputChoiceSetBuilder()
      .withId('hobbies')
      .isMultiSelect()
      .withStyle(ChoiceInputStyle.Expanded)
      .addChoice('Reading', 'reading')
      .addChoice('Gaming', 'gaming')
      .addChoice('Sports', 'sports')
      .build();

    assert.equal(input.type, 'Input.ChoiceSet');
    assert.equal(input.id, 'hobbies');
    assert.equal(input.isMultiSelect, true);
    assert.equal(input.style, ChoiceInputStyle.Expanded);
    assert.equal(input.choices!.length, 3);
    assert.equal(input.choices![0].title, 'Reading');
    assert.equal(input.choices![0].value, 'reading');
  });

  it('sets choices.data with a Data.Query payload', () => {
    const input = new InputChoiceSetBuilder()
      .withId('people-picker')
      .withChoicesData('graph.microsoft.com/users')
      .build();

    const choicesData = input['choices.data'];
    assert.ok(choicesData, 'choices.data should be present');
    assert.equal(choicesData!.type, 'Data.Query');
    assert.equal(choicesData!.dataset, 'graph.microsoft.com/users');
  });
});

describe('AdaptiveCardBuilder input methods', () => {
  it('adds all input types to body in order', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((i) => i.withId('name').withLabel('Name'))
      .addInputNumber((i) => i.withId('age').withLabel('Age'))
      .addInputDate((i) => i.withId('date').withLabel('Date'))
      .addInputTime((i) => i.withId('time').withLabel('Time'))
      .addInputToggle((i) => i.withId('toggle').withTitle('Accept'))
      .addInputChoiceSet((i) => i.withId('choice').addChoice('A', 'a'))
      .build();

    assert.equal(card.body!.length, 6);
    assert.equal(card.body![0].type, 'Input.Text');
    assert.equal(card.body![1].type, 'Input.Number');
    assert.equal(card.body![2].type, 'Input.Date');
    assert.equal(card.body![3].type, 'Input.Time');
    assert.equal(card.body![4].type, 'Input.Toggle');
    assert.equal(card.body![5].type, 'Input.ChoiceSet');
  });

  it('serializes input type discriminants correctly', () => {
    const card = AdaptiveCardBuilder.create()
      .addInputText((i) => i.withId('t1'))
      .addInputChoiceSet((i) => i.withId('cs1').addChoice('A', 'a'))
      .build();

    const json = toJson(card);
    assert.ok(json.includes('"type": "Input.Text"'));
    assert.ok(json.includes('"type": "Input.ChoiceSet"'));
  });
});
