import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    InputTextBuilder,
    InputNumberBuilder,
    InputDateBuilder,
    InputTimeBuilder,
    InputToggleBuilder,
    InputChoiceSetBuilder,
    to_json,
    TextInputStyle,
    ChoiceInputStyle,
)


class TestInputTextBuilder:
    def test_builds_input_text_with_all_properties(self):
        input_ = (InputTextBuilder()
                  .with_id('name')
                  .with_label('Your Name')
                  .with_placeholder('Enter name...')
                  .with_value('Alice')
                  .with_max_length(100)
                  .with_is_multiline()
                  .with_style(TextInputStyle.Email)
                  .with_regex('[a-z]+')
                  .with_is_required()
                  .with_error_message('Required')
                  .build())

        assert input_['type'] == 'Input.Text'
        assert input_['id'] == 'name'
        assert input_['label'] == 'Your Name'
        assert input_['placeholder'] == 'Enter name...'
        assert input_['value'] == 'Alice'
        assert input_['maxLength'] == 100
        assert input_['isMultiline'] is True
        assert input_['style'] == TextInputStyle.Email.value
        assert input_['regex'] == '[a-z]+'
        assert input_['isRequired'] is True
        assert input_['errorMessage'] == 'Required'

    def test_type_discriminant_is_input_text(self):
        input_ = InputTextBuilder().with_id('x').build()
        assert input_['type'] == 'Input.Text'


class TestInputNumberBuilder:
    def test_builds_input_number_with_constraints(self):
        input_ = (InputNumberBuilder()
                  .with_id('age')
                  .with_label('Age')
                  .with_min(0)
                  .with_max(120)
                  .with_value(25)
                  .build())

        assert input_['type'] == 'Input.Number'
        assert input_['id'] == 'age'
        assert input_['min'] == 0
        assert input_['max'] == 120
        assert input_['value'] == 25


class TestInputDateBuilder:
    def test_builds_input_date(self):
        input_ = (InputDateBuilder()
                  .with_id('dob')
                  .with_min('2000-01-01')
                  .with_max('2024-12-31')
                  .with_value('2024-01-01')
                  .build())

        assert input_['type'] == 'Input.Date'
        assert input_['min'] == '2000-01-01'
        assert input_['max'] == '2024-12-31'
        assert input_['value'] == '2024-01-01'


class TestInputTimeBuilder:
    def test_builds_input_time(self):
        input_ = (InputTimeBuilder()
                  .with_id('startTime')
                  .with_min('09:00')
                  .with_max('17:00')
                  .with_value('10:00')
                  .build())

        assert input_['type'] == 'Input.Time'
        assert input_['min'] == '09:00'
        assert input_['max'] == '17:00'
        assert input_['value'] == '10:00'


class TestInputToggleBuilder:
    def test_builds_input_toggle(self):
        input_ = (InputToggleBuilder()
                  .with_id('accept')
                  .with_title('Accept Terms')
                  .with_value_on('yes')
                  .with_value_off('no')
                  .with_wrap()
                  .build())

        assert input_['type'] == 'Input.Toggle'
        assert input_['id'] == 'accept'
        assert input_['title'] == 'Accept Terms'
        assert input_['valueOn'] == 'yes'
        assert input_['valueOff'] == 'no'
        assert input_['wrap'] is True


class TestInputChoiceSetBuilder:
    def test_builds_input_choice_set_with_choices(self):
        input_ = (InputChoiceSetBuilder()
                  .with_id('hobbies')
                  .is_multi_select()
                  .with_style(ChoiceInputStyle.Expanded)
                  .add_choice('Reading', 'reading')
                  .add_choice('Gaming', 'gaming')
                  .add_choice('Sports', 'sports')
                  .build())

        assert input_['type'] == 'Input.ChoiceSet'
        assert input_['id'] == 'hobbies'
        assert input_['isMultiSelect'] is True
        assert input_['style'] == ChoiceInputStyle.Expanded.value
        assert len(input_['choices']) == 3
        assert input_['choices'][0]['title'] == 'Reading'
        assert input_['choices'][0]['value'] == 'reading'


class TestAdaptiveCardBuilderInputMethods:
    def test_adds_all_input_types_to_body_in_order(self):
        card = (AdaptiveCardBuilder.create()
                .add_input_text(lambda i: i.with_id('name').with_label('Name'))
                .add_input_number(lambda i: i.with_id('age').with_label('Age'))
                .add_input_date(lambda i: i.with_id('date').with_label('Date'))
                .add_input_time(lambda i: i.with_id('time').with_label('Time'))
                .add_input_toggle(lambda i: i.with_id('toggle').with_title('Accept'))
                .add_input_choice_set(lambda i: i.with_id('choice').add_choice('A', 'a'))
                .build())

        assert len(card['body']) == 6
        assert card['body'][0]['type'] == 'Input.Text'
        assert card['body'][1]['type'] == 'Input.Number'
        assert card['body'][2]['type'] == 'Input.Date'
        assert card['body'][3]['type'] == 'Input.Time'
        assert card['body'][4]['type'] == 'Input.Toggle'
        assert card['body'][5]['type'] == 'Input.ChoiceSet'

    def test_serializes_input_type_discriminants_correctly(self):
        card = (AdaptiveCardBuilder.create()
                .add_input_text(lambda i: i.with_id('t1'))
                .add_input_choice_set(lambda i: i.with_id('cs1').add_choice('A', 'a'))
                .build())
        json_str = to_json(card)
        assert '"type": "Input.Text"' in json_str
        assert '"type": "Input.ChoiceSet"' in json_str
