"""Tests for Microsoft Teams features (card-level, action-level, data-level)."""
import json
import pytest

from fluent_cards import (
    AdaptiveCardBuilder,
    TeamsCardPropertiesBuilder,
    TeamsDataBuilder,
    TeamsSubmitPropertiesBuilder,
    validate,
    to_json,
    from_json,
    ValidationSeverity,
)


# ── Card-level msteams (TeamsCardPropertiesBuilder) ──────────────────────────

class TestTeamsCardProperties:

    def test_full_width_card(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hello'))
            .with_teams_card(lambda tc: tc.with_full_width())
            .build())
        assert card['msteams'] == {'width': 'Full'}

    def test_card_with_mention(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi <at>John</at>'))
            .with_teams_card(lambda tc: tc.add_mention('John', 'user-123'))
            .build())
        msteams = card['msteams']
        assert len(msteams['entities']) == 1
        entity = msteams['entities'][0]
        assert entity['type'] == 'mention'
        assert entity['text'] == '<at>John</at>'
        assert entity['mentioned']['id'] == 'user-123'
        assert entity['mentioned']['name'] == 'John'

    def test_full_width_with_mentions(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi <at>Alice</at> and <at>Bob</at>'))
            .with_teams_card(lambda tc: tc
                .with_full_width()
                .add_mention('Alice', 'user-1')
                .add_mention('Bob', 'user-2'))
            .build())
        msteams = card['msteams']
        assert msteams['width'] == 'Full'
        assert len(msteams['entities']) == 2

    def test_with_teams_card_raw(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi'))
            .with_teams_card_raw({'width': 'Full', 'custom': True})
            .build())
        assert card['msteams'] == {'width': 'Full', 'custom': True}

    def test_teams_card_typed_then_raw_conflict(self):
        builder = (AdaptiveCardBuilder.create()
            .with_teams_card(lambda tc: tc.with_full_width()))
        with pytest.raises(ValueError, match='Cannot use both'):
            builder.with_teams_card_raw({'width': 'Full'})

    def test_teams_card_raw_then_typed_conflict(self):
        builder = (AdaptiveCardBuilder.create()
            .with_teams_card_raw({'width': 'Full'}))
        with pytest.raises(ValueError, match='Cannot use both'):
            builder.with_teams_card(lambda tc: tc.with_full_width())


# ── Action-level msteams (Submit feedback) ───────────────────────────────────

class TestTeamsSubmitProperties:

    def test_submit_with_feedback_hidden(self):
        card = (AdaptiveCardBuilder.create()
            .add_action_set(lambda a: a
                .add_action(lambda act: act
                    .submit('Click me')
                    .with_teams_submit_feedback(lambda f: f.with_feedback_hidden())))
            .build())
        action = card['body'][0]['actions'][0]
        assert action['msteams'] == {'feedback': {'hide': True}}

    def test_submit_with_teams_submit_raw(self):
        card = (AdaptiveCardBuilder.create()
            .add_action_set(lambda a: a
                .add_action(lambda act: act
                    .submit('Click me')
                    .with_teams_submit_raw({'feedback': {'hide': True}})))
            .build())
        action = card['body'][0]['actions'][0]
        assert action['msteams'] == {'feedback': {'hide': True}}

    def test_teams_submit_typed_then_raw_conflict(self):
        def configure(act):
            act.submit('Click me')
            act.with_teams_submit_feedback(lambda f: f.with_feedback_hidden())
            act.with_teams_submit_raw({'feedback': {'hide': True}})
        with pytest.raises(ValueError, match='Cannot use both'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())

    def test_teams_submit_raw_then_typed_conflict(self):
        def configure(act):
            act.submit('Click me')
            act.with_teams_submit_raw({'feedback': {'hide': True}})
            act.with_teams_submit_feedback(lambda f: f.with_feedback_hidden())
        with pytest.raises(ValueError, match='Cannot use both'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())


# ── Data-level Teams (task/fetch, msteams in data) ───────────────────────────

class TestTeamsData:

    def test_task_fetch_shorthand(self):
        card = (AdaptiveCardBuilder.create()
            .add_action_set(lambda a: a
                .add_action(lambda act: act
                    .submit('Open')
                    .with_teams_task_fetch()))
            .build())
        action = card['body'][0]['actions'][0]
        assert action['data'] == {'msteams': {'type': 'task/fetch'}}

    def test_teams_data_with_custom_properties(self):
        card = (AdaptiveCardBuilder.create()
            .add_action_set(lambda a: a
                .add_action(lambda act: act
                    .submit('Open')
                    .with_teams_data(lambda d: d
                        .with_task_fetch()
                        .with_property('customKey', 'customValue')
                        .with_property('count', 42))))
            .build())
        action = card['body'][0]['actions'][0]
        assert action['data'] == {
            'msteams': {'type': 'task/fetch'},
            'customKey': 'customValue',
            'count': 42,
        }

    def test_teams_data_with_custom_msteams(self):
        builder = TeamsDataBuilder()
        builder.with_msteams({'type': 'task/submit', 'custom': True})
        builder.with_property('key', 'value')
        result = builder.build()
        assert result['msteams'] == {'type': 'task/submit', 'custom': True}
        assert result['key'] == 'value'

    def test_teams_data_msteams_property_rejected(self):
        builder = TeamsDataBuilder()
        with pytest.raises(ValueError, match='msteams'):
            builder.with_property('msteams', {'type': 'task/fetch'})

    def test_teams_data_msteams_property_case_insensitive(self):
        builder = TeamsDataBuilder()
        with pytest.raises(ValueError, match='msteams'):
            builder.with_property('MSTEAMS', 'value')


# ── Submit-only gating ───────────────────────────────────────────────────────

class TestSubmitOnlyGating:

    def test_teams_task_fetch_on_execute_throws(self):
        def configure(act):
            act.execute('Run')
            act.with_teams_task_fetch()
        with pytest.raises(ValueError, match='Submit'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())

    def test_teams_data_on_open_url_throws(self):
        def configure(act):
            act.open_url('Open', 'https://example.com')
            act.with_teams_data(lambda d: d.with_task_fetch())
        with pytest.raises(ValueError, match='Submit'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())

    def test_teams_submit_feedback_on_toggle_throws(self):
        def configure(act):
            act.toggle_visibility('Toggle')
            act.with_teams_submit_feedback(lambda f: f.with_feedback_hidden())
        with pytest.raises(ValueError, match='Submit'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())

    def test_teams_submit_raw_on_execute_throws(self):
        def configure(act):
            act.execute('Run')
            act.with_teams_submit_raw({'feedback': {'hide': True}})
        with pytest.raises(ValueError, match='Submit'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())


# ── Data conflict detection ──────────────────────────────────────────────────

class TestDataConflict:

    def test_with_data_then_teams_data_conflict(self):
        def configure(act):
            act.submit('Click')
            act.with_data({'key': 'value'})
            act.with_teams_data(lambda d: d.with_task_fetch())
        with pytest.raises(ValueError, match='Cannot use both'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())

    def test_teams_data_then_with_data_conflict(self):
        def configure(act):
            act.submit('Click')
            act.with_teams_data(lambda d: d.with_task_fetch())
            act.with_data({'key': 'value'})
        with pytest.raises(ValueError, match='Cannot use both'):
            (AdaptiveCardBuilder.create()
                .add_action_set(lambda a: a.add_action(configure))
                .build())


# ── Mention validation ───────────────────────────────────────────────────────

class TestMentionValidation:

    def test_matched_mentions_pass(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi <at>John</at>'))
            .with_teams_card(lambda tc: tc.add_mention('John', 'user-123'))
            .build())
        issues = validate(card)
        mention_issues = [i for i in issues if 'mention' in i.code.lower() or 'at_token' in i.code.lower()]
        assert len(mention_issues) == 0

    def test_orphaned_mention_entity(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hello world'))
            .with_teams_card(lambda tc: tc.add_mention('John', 'user-123'))
            .build())
        issues = validate(card)
        orphaned = [i for i in issues if i.code == 'ORPHANED_MENTION_ENTITY']
        assert len(orphaned) == 1
        assert orphaned[0].severity == ValidationSeverity.Warning
        assert 'entities[0]' in orphaned[0].path

    def test_orphaned_at_token(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi <at>Unknown</at>'))
            .with_teams_card(lambda tc: tc.add_mention('John', 'user-123'))
            .build())
        issues = validate(card)
        at_issues = [i for i in issues if i.code == 'ORPHANED_AT_TOKEN']
        assert len(at_issues) == 1


# ── Serialization round-trip ─────────────────────────────────────────────────

class TestTeamsSerialization:

    def test_full_teams_card_round_trip(self):
        card = (AdaptiveCardBuilder.create()
            .add_text_block(lambda t: t.with_text('Hi <at>Alice</at>'))
            .with_teams_card(lambda tc: tc
                .with_full_width()
                .add_mention('Alice', 'user-1'))
            .add_action_set(lambda a: a
                .add_action(lambda act: act
                    .submit('Submit')
                    .with_teams_task_fetch()
                    .with_teams_submit_feedback(lambda f: f.with_feedback_hidden())))
            .build())

        json_str = to_json(card)
        parsed = json.loads(json_str)
        assert parsed['msteams']['width'] == 'Full'
        assert len(parsed['msteams']['entities']) == 1
        action = parsed['body'][1]['actions'][0]
        assert action['data'] == {'msteams': {'type': 'task/fetch'}}
        assert action['msteams'] == {'feedback': {'hide': True}}

    def test_from_json_preserves_msteams(self):
        original = {
            'type': 'AdaptiveCard',
            'version': '1.5',
            '$schema': 'http://adaptivecards.io/schemas/adaptive-card.json',
            'msteams': {'width': 'Full'},
            'body': [{'type': 'TextBlock', 'text': 'Hello'}],
        }
        json_str = json.dumps(original)
        restored = from_json(json_str)
        assert restored['msteams'] == {'width': 'Full'}
