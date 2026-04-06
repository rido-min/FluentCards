import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    to_json,
    from_json,
    TextSize,
    TextWeight,
    TextColor,
    HorizontalAlignment,
    ActionStyle,
    AssociatedInputs,
)


class TestToJson:
    def test_includes_type_and_version(self):
        card = AdaptiveCardBuilder.create().build()
        json_str = to_json(card)
        assert '"type": "AdaptiveCard"' in json_str
        assert '"version": "1.5"' in json_str

    def test_includes_schema_property(self):
        json_str = to_json(AdaptiveCardBuilder.create().build())
        assert '"$schema"' in json_str
        assert '"https://adaptivecards.io/schemas/1.5.0/adaptive-card.json"' in json_str

    def test_omits_none_optional_properties(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('Simple Text'))
                .build())
        json_str = to_json(card)
        assert '"size"' not in json_str
        assert '"weight"' not in json_str
        assert '"color"' not in json_str
        assert '"wrap"' not in json_str
        assert '"maxLines"' not in json_str

    def test_serializes_text_block_with_all_properties(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b
                                .with_id('textBlock1')
                                .with_text('Sample Text')
                                .with_size(TextSize.Large)
                                .with_weight(TextWeight.Bolder)
                                .with_color(TextColor.Accent)
                                .with_wrap(True)
                                .with_max_lines(3)
                                .with_horizontal_alignment(HorizontalAlignment.Center))
                .build())
        json_str = to_json(card)

        assert '"id": "textBlock1"' in json_str
        assert '"text": "Sample Text"' in json_str
        assert '"size": "large"' in json_str
        assert '"weight": "bolder"' in json_str
        assert '"color": "accent"' in json_str
        assert '"wrap": true' in json_str
        assert '"maxLines": 3' in json_str
        assert '"horizontalAlignment": "center"' in json_str

    def test_serializes_enum_values_as_camel_case_strings(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('x')
                                .with_size(TextSize.ExtraLarge)
                                .with_color(TextColor.Attention))
                .build())
        json_str = to_json(card)
        assert '"size": "extraLarge"' in json_str
        assert '"color": "attention"' in json_str

    def test_includes_type_discriminator_on_body_elements(self):
        card = AdaptiveCardBuilder.create().add_text_block(lambda b: b.with_text('Test')).build()
        assert '"type": "TextBlock"' in to_json(card)

    def test_includes_type_discriminator_on_actions(self):
        card = AdaptiveCardBuilder.create().add_action(lambda b: b.open_url('https://example.com')).build()
        assert '"type": "Action.OpenUrl"' in to_json(card)

    def test_serializes_action_style_as_camel_case(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.submit('OK').with_style(ActionStyle.Positive))
                .add_action(lambda b: b.submit('Delete').with_style(ActionStyle.Destructive))
                .build())
        json_str = to_json(card)
        assert '"style": "positive"' in json_str
        assert '"style": "destructive"' in json_str

    def test_serializes_associated_inputs_as_camel_case(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.submit('Auto').with_associated_inputs(AssociatedInputs.Auto))
                .add_action(lambda b: b.submit('None').with_associated_inputs(AssociatedInputs.None_))
                .build())
        json_str = to_json(card)
        assert '"associatedInputs": "auto"' in json_str
        assert '"associatedInputs": "none"' in json_str

    def test_omits_action_style_when_not_set(self):
        card = AdaptiveCardBuilder.create().add_action(lambda b: b.submit('Submit')).build()
        assert '"style"' not in to_json(card)

    def test_serializes_all_action_base_properties(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b
                            .submit('Submit')
                            .with_id('submit1')
                            .with_icon_url('https://example.com/icon.png')
                            .with_style(ActionStyle.Positive)
                            .with_is_enabled(False)
                            .with_tooltip('Submit the form'))
                .build())
        json_str = to_json(card)
        assert '"id": "submit1"' in json_str
        assert '"iconUrl": "https://example.com/icon.png"' in json_str
        assert '"style": "positive"' in json_str
        assert '"isEnabled": false' in json_str
        assert '"tooltip": "Submit the form"' in json_str

    def test_produces_indented_output_by_default(self):
        json_str = to_json(AdaptiveCardBuilder.create().build())
        assert '\n' in json_str
        assert '  ' in json_str

    def test_produces_compact_output_when_indent_0(self):
        json_str = to_json(AdaptiveCardBuilder.create().build(), 0)
        assert '\n' not in json_str


class TestFromJson:
    def test_parses_valid_card(self):
        original = (AdaptiveCardBuilder.create()
                    .add_text_block(lambda b: b.with_text('Hello'))
                    .build())
        card = from_json(to_json(original))
        assert card is not None
        assert card['type'] == 'AdaptiveCard'
        assert card['version'] == '1.5'

    def test_returns_none_for_invalid_json(self):
        assert from_json('not json') is None

    def test_returns_none_when_root_type_is_not_adaptive_card(self):
        assert from_json('{"type":"Something"}') is None

    def test_preserves_body_element_type_discriminants_after_round_trip(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('Hi'))
                .add_image(lambda b: b.with_url('https://example.com/img.png'))
                .build())
        parsed = from_json(to_json(card))
        assert parsed['body'][0]['type'] == 'TextBlock'
        assert parsed['body'][1]['type'] == 'Image'

    def test_preserves_action_type_discriminants_after_round_trip(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.open_url('https://example.com'))
                .add_action(lambda b: b.submit('Submit'))
                .build())
        parsed = from_json(to_json(card))
        assert parsed['actions'][0]['type'] == 'Action.OpenUrl'
        assert parsed['actions'][1]['type'] == 'Action.Submit'

    def test_round_trips_text_block_properties(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('Test')
                                .with_size(TextSize.Medium)
                                .with_weight(TextWeight.Bolder))
                .build())
        parsed = from_json(to_json(card))
        tb = parsed['body'][0]
        assert tb['text'] == 'Test'
        assert tb['size'] == TextSize.Medium.value
        assert tb['weight'] == TextWeight.Bolder.value

    def test_round_trips_open_url_action_url(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.open_url('https://example.com').with_title('Visit'))
                .build())
        parsed = from_json(to_json(card))
        action = parsed['actions'][0]
        assert action['url'] == 'https://example.com'
        assert action['title'] == 'Visit'
