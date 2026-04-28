import pytest
from fluent_cards import (
    ActionBuilder,
    AdaptiveCardBuilder,
    to_json,
    ActionStyle,
    AssociatedInputs,
)


class TestActionBuilder:
    def test_builds_open_url_action(self):
        action = ActionBuilder().open_url('https://example.com', 'Visit').build()
        assert action['type'] == 'Action.OpenUrl'
        assert action['url'] == 'https://example.com'
        assert action['title'] == 'Visit'

    def test_builds_submit_action(self):
        action = ActionBuilder().submit('Send').build()
        assert action['type'] == 'Action.Submit'
        assert action['title'] == 'Send'

    def test_builds_show_card_action_with_nested_card(self):
        nested = (AdaptiveCardBuilder.create()
                  .add_text_block(lambda b: b.with_text('Nested'))
                  .build())
        action = ActionBuilder().show_card('More').with_card(nested).build()
        assert action['type'] == 'Action.ShowCard'
        assert action['title'] == 'More'
        assert action.get('card') is not None
        assert action['card']['body'][0]['type'] == 'TextBlock'

    def test_builds_toggle_visibility_with_string_targets(self):
        action = (ActionBuilder()
                  .toggle_visibility('Toggle')
                  .add_target_element('el1')
                  .add_target_element('el2')
                  .build())
        assert action['type'] == 'Action.ToggleVisibility'
        assert action['targetElements'] == ['el1', 'el2']

    def test_builds_toggle_visibility_with_object_targets(self):
        action = (ActionBuilder()
                  .toggle_visibility()
                  .add_target_element('el1', True)
                  .add_target_element('el2', False)
                  .build())
        assert action['targetElements'] == [
            {'elementId': 'el1', 'isVisible': True},
            {'elementId': 'el2', 'isVisible': False},
        ]

    def test_builds_execute_action_with_verb(self):
        action = (ActionBuilder()
                  .execute('Run')
                  .with_verb('doAction')
                  .with_data({'key': 'value'})
                  .build())
        assert action['type'] == 'Action.Execute'
        assert action['title'] == 'Run'
        assert action['verb'] == 'doAction'
        assert action['data'] == {'key': 'value'}

    def test_sets_common_properties_via_modifiers(self):
        action = (ActionBuilder()
                  .submit()
                  .with_id('a1')
                  .with_title('Submit')
                  .with_icon_url('https://example.com/icon.png')
                  .with_style(ActionStyle.Positive)
                  .with_is_enabled(False)
                  .with_tooltip('Click me')
                  .build())
        assert action['id'] == 'a1'
        assert action['title'] == 'Submit'
        assert action['iconUrl'] == 'https://example.com/icon.png'
        assert action['style'] == ActionStyle.Positive.value
        assert action['isEnabled'] is False
        assert action['tooltip'] == 'Click me'

    def test_sets_associated_inputs_on_submit(self):
        action = (ActionBuilder()
                  .submit()
                  .with_associated_inputs(AssociatedInputs.None_)
                  .build())
        assert action['associatedInputs'] == AssociatedInputs.None_.value

    def test_throws_when_no_action_type_selected(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().build()

    def test_with_id_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_id('a1')

    def test_with_title_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_title('Submit')

    def test_with_icon_url_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_icon_url('https://example.com/icon.png')

    def test_with_style_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_style(ActionStyle.Positive)

    def test_with_is_enabled_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_is_enabled(False)

    def test_with_tooltip_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_tooltip('tip')

    def test_with_data_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_data({'key': 'value'})

    def test_with_verb_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_verb('doAction')

    def test_with_card_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().with_card({})

    def test_add_target_element_raises_when_no_type_set(self):
        with pytest.raises(ValueError, match='No action type specified'):
            ActionBuilder().add_target_element('el1')

    # ── Issue #66: with_data() raises on incompatible types ───────────────

    def test_with_data_raises_on_open_url(self):
        with pytest.raises(ValueError, match='with_data\\(\\) is only available on Submit or Execute'):
            ActionBuilder().open_url('https://example.com').with_data({'k': 'v'})

    def test_with_data_raises_on_show_card(self):
        with pytest.raises(ValueError, match='with_data\\(\\) is only available on Submit or Execute'):
            ActionBuilder().show_card().with_data({'k': 'v'})

    def test_with_data_raises_on_toggle_visibility(self):
        with pytest.raises(ValueError, match='with_data\\(\\) is only available on Submit or Execute'):
            ActionBuilder().toggle_visibility().with_data({'k': 'v'})

    def test_with_data_works_on_submit(self):
        action = ActionBuilder().submit().with_data({'k': 'v'}).build()
        assert action['data'] == {'k': 'v'}

    def test_with_data_works_on_execute(self):
        action = ActionBuilder().execute().with_data({'k': 'v'}).build()
        assert action['data'] == {'k': 'v'}

    # ── Issue #66: with_verb() raises on incompatible types ────────────────

    def test_with_verb_raises_on_open_url(self):
        with pytest.raises(ValueError, match='with_verb\\(\\) is only available on Execute'):
            ActionBuilder().open_url('https://example.com').with_verb('doIt')

    def test_with_verb_raises_on_submit(self):
        with pytest.raises(ValueError, match='with_verb\\(\\) is only available on Execute'):
            ActionBuilder().submit().with_verb('doIt')

    def test_with_verb_raises_on_show_card(self):
        with pytest.raises(ValueError, match='with_verb\\(\\) is only available on Execute'):
            ActionBuilder().show_card().with_verb('doIt')

    def test_with_verb_raises_on_toggle_visibility(self):
        with pytest.raises(ValueError, match='with_verb\\(\\) is only available on Execute'):
            ActionBuilder().toggle_visibility().with_verb('doIt')

    def test_with_verb_works_on_execute(self):
        action = ActionBuilder().execute().with_verb('doIt').build()
        assert action['verb'] == 'doIt'

    # ── Issue #66: with_associated_inputs() raises on incompatible types ───

    def test_with_associated_inputs_raises_on_open_url(self):
        with pytest.raises(ValueError, match='with_associated_inputs\\(\\) is only available on Submit or Execute'):
            ActionBuilder().open_url('https://example.com').with_associated_inputs(AssociatedInputs.None_)

    def test_with_associated_inputs_raises_on_show_card(self):
        with pytest.raises(ValueError, match='with_associated_inputs\\(\\) is only available on Submit or Execute'):
            ActionBuilder().show_card().with_associated_inputs(AssociatedInputs.None_)

    def test_with_associated_inputs_raises_on_toggle_visibility(self):
        with pytest.raises(ValueError, match='with_associated_inputs\\(\\) is only available on Submit or Execute'):
            ActionBuilder().toggle_visibility().with_associated_inputs(AssociatedInputs.None_)

    def test_with_associated_inputs_works_on_submit(self):
        action = ActionBuilder().submit().with_associated_inputs(AssociatedInputs.None_).build()
        assert action['associatedInputs'] == AssociatedInputs.None_.value

    def test_with_associated_inputs_works_on_execute(self):
        action = ActionBuilder().execute().with_associated_inputs(AssociatedInputs.Auto).build()
        assert action['associatedInputs'] == AssociatedInputs.Auto.value

    def test_serializes_show_card_with_nested_actions(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.show_card('Show').with_card(
                    AdaptiveCardBuilder.create()
                    .add_text_block(lambda tb: tb.with_text('Detail'))
                    .add_action(lambda a: a.submit('Submit'))
                    .build()
                ))
                .build())
        json_str = to_json(card)
        assert '"type": "Action.ShowCard"' in json_str
        assert '"type": "Action.Submit"' in json_str
        assert '"text": "Detail"' in json_str
