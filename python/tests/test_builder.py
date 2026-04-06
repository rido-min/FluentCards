import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    TextBlockBuilder,
    TextSize,
    TextWeight,
    TextColor,
    HorizontalAlignment,
)


class TestAdaptiveCardBuilder:
    def test_creates_card_with_default_version_and_schema(self):
        card = AdaptiveCardBuilder.create().build()
        assert card['type'] == 'AdaptiveCard'
        assert card['version'] == '1.5'
        assert card.get('$schema')

    def test_with_version_sets_version(self):
        card = AdaptiveCardBuilder.create().with_version('1.6').build()
        assert card['version'] == '1.6'

    def test_with_schema_sets_schema(self):
        card = AdaptiveCardBuilder.create().with_schema('https://example.com/schema.json').build()
        assert card['$schema'] == 'https://example.com/schema.json'

    def test_with_schema_none_removes_schema(self):
        card = AdaptiveCardBuilder.create().with_schema(None).build()
        assert card.get('$schema') is None

    def test_supports_fluent_chaining(self):
        card = (AdaptiveCardBuilder.create()
                .with_version('1.6')
                .with_schema('https://example.com/schema.json')
                .build())
        assert card['version'] == '1.6'
        assert card['$schema'] == 'https://example.com/schema.json'

    def test_add_text_block_adds_to_body(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('Hello, World!'))
                .build())
        assert card.get('body') is not None
        assert len(card['body']) == 1
        assert card['body'][0]['type'] == 'TextBlock'
        assert card['body'][0]['text'] == 'Hello, World!'

    def test_adds_multiple_body_elements_preserving_order(self):
        card = (AdaptiveCardBuilder.create()
                .add_text_block(lambda b: b.with_text('First'))
                .add_text_block(lambda b: b.with_text('Second'))
                .add_text_block(lambda b: b.with_text('Third'))
                .build())
        assert len(card['body']) == 3
        assert card['body'][0]['text'] == 'First'
        assert card['body'][1]['text'] == 'Second'
        assert card['body'][2]['text'] == 'Third'

    def test_with_metadata_sets_web_url(self):
        card = AdaptiveCardBuilder.create().with_metadata('https://example.com').build()
        assert card['metadata']['webUrl'] == 'https://example.com'

    def test_add_action_adds_to_actions_array(self):
        card = (AdaptiveCardBuilder.create()
                .add_action(lambda b: b.open_url('https://example.com').with_title('Visit'))
                .build())
        assert card.get('actions') is not None
        assert len(card['actions']) == 1
        assert card['actions'][0]['type'] == 'Action.OpenUrl'
        assert card['actions'][0]['title'] == 'Visit'


class TestTextBlockBuilder:
    def test_builds_text_block_with_all_properties(self):
        tb = (TextBlockBuilder()
              .with_id('tb1')
              .with_text('Test Text')
              .with_size(TextSize.Large)
              .with_weight(TextWeight.Bolder)
              .with_color(TextColor.Accent)
              .with_wrap(True)
              .with_max_lines(5)
              .with_horizontal_alignment(HorizontalAlignment.Right)
              .build())

        assert tb['type'] == 'TextBlock'
        assert tb['id'] == 'tb1'
        assert tb['text'] == 'Test Text'
        assert tb['size'] == TextSize.Large.value
        assert tb['weight'] == TextWeight.Bolder.value
        assert tb['color'] == TextColor.Accent.value
        assert tb['wrap'] is True
        assert tb['maxLines'] == 5
        assert tb['horizontalAlignment'] == HorizontalAlignment.Right.value

    def test_unset_optional_properties_are_absent(self):
        tb = TextBlockBuilder().with_text('Simple').build()
        assert 'size' not in tb
        assert 'weight' not in tb
        assert 'color' not in tb
        assert 'wrap' not in tb
