"""Serialization edge-case tests."""

import json
import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    to_json,
    from_json,
    TextSize,
    TextWeight,
)


class TestNoneOmission:
    def test_none_optionals_omitted_from_json(self):
        card = AdaptiveCardBuilder.create().build()
        json_str = to_json(card)
        parsed = json.loads(json_str)
        for value in parsed.values():
            assert value is not None

    def test_card_with_unset_optionals_has_no_none_in_json(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("Hello"))
            .build()
        )
        json_str = to_json(card)
        assert "null" not in json_str

    def test_nested_none_stripped(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("Text"))
            .build()
        )
        # Manually inject a None to verify stripping
        card["body"][0]["fakeField"] = None
        json_str = to_json(card)
        parsed = json.loads(json_str)
        assert "fakeField" not in parsed["body"][0]


class TestRoundTrip:
    def test_build_to_json_from_json_structural_equality(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.5")
            .add_text_block(lambda tb: tb.with_text("Hello").with_size(TextSize.Large))
            .add_text_block(lambda tb: tb.with_text("World").with_weight(TextWeight.Bolder))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored is not None
        assert restored["type"] == card["type"]
        assert restored["version"] == card["version"]
        assert len(restored["body"]) == len(card["body"])
        assert restored["body"][0]["text"] == "Hello"
        assert restored["body"][1]["text"] == "World"

    def test_round_trip_with_actions(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_action(lambda a: a.open_url("https://example.com").with_title("Open"))
            .add_action(lambda a: a.submit("Submit").with_data({"key": "val"}))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored is not None
        assert len(restored["actions"]) == 2
        assert restored["actions"][0]["url"] == "https://example.com"
        assert restored["actions"][1]["data"]["key"] == "val"

    def test_round_trip_preserves_nested_structure(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_container(
                lambda c: c.add_text_block(lambda tb: tb.with_text("Nested"))
            )
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored["body"][0]["type"] == "Container"
        assert restored["body"][0]["items"][0]["text"] == "Nested"

    def test_round_trip_complex_card(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.6")
            .with_fallback_text("Fallback")
            .with_speak("Speak")
            .with_lang("en")
            .with_metadata("https://example.com")
            .add_text_block(lambda tb: tb.with_text("Title").with_size(TextSize.ExtraLarge))
            .add_column_set(
                lambda cs: cs.add_column(
                    lambda col: col.with_width("1").add_text_block(lambda tb: tb.with_text("C1"))
                )
            )
            .add_action(lambda a: a.open_url("https://example.com").with_title("Go"))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored is not None
        assert restored["version"] == "1.6"
        assert restored["fallbackText"] == "Fallback"
        assert restored["metadata"]["webUrl"] == "https://example.com"


class TestUnicodeAndEdgeCases:
    def test_unicode_text(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("こんにちは世界 🌍"))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored["body"][0]["text"] == "こんにちは世界 🌍"

    def test_emoji_in_text(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("🎉🎊🎈"))
            .build()
        )
        json_str = to_json(card)
        parsed = json.loads(json_str)
        assert parsed["body"][0]["text"] == "🎉🎊🎈"

    def test_very_long_text(self):
        long_text = "A" * 10000
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text(long_text))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored["body"][0]["text"] == long_text

    def test_empty_body(self):
        card = AdaptiveCardBuilder.create().build()
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored is not None
        assert restored["type"] == "AdaptiveCard"

    def test_special_characters_in_text(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text('He said "hello" & <goodbye>'))
            .build()
        )
        json_str = to_json(card)
        restored = from_json(json_str)
        assert restored["body"][0]["text"] == 'He said "hello" & <goodbye>'


class TestFromJsonErrorHandling:
    def test_invalid_json_returns_none(self):
        result = from_json("{not valid json")
        assert result is None

    def test_empty_string_returns_none(self):
        result = from_json("")
        assert result is None

    def test_valid_json_not_adaptive_card_returns_none(self):
        result = from_json('{"type": "SomethingElse", "version": "1.0"}')
        assert result is None

    def test_json_array_returns_none(self):
        result = from_json('[1, 2, 3]')
        assert result is None

    def test_json_string_returns_none(self):
        result = from_json('"just a string"')
        assert result is None

    def test_json_number_returns_none(self):
        result = from_json("42")
        assert result is None

    def test_json_null_returns_none(self):
        result = from_json("null")
        assert result is None

    def test_valid_json_missing_type_returns_none(self):
        result = from_json('{"version": "1.5"}')
        assert result is None


class TestCompactSerialization:
    def test_compact_json_no_whitespace(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("Hello"))
            .build()
        )
        compact = to_json(card, indent=0)
        assert "\n" not in compact
        # Should still be valid JSON
        parsed = json.loads(compact)
        assert parsed["type"] == "AdaptiveCard"

    def test_compact_vs_indented_parse_equal(self):
        card = (
            AdaptiveCardBuilder.create()
            .add_text_block(lambda tb: tb.with_text("Test"))
            .add_action(lambda a: a.open_url("https://example.com").with_title("Go"))
            .build()
        )
        compact = to_json(card, indent=0)
        indented = to_json(card, indent=2)
        assert json.loads(compact) == json.loads(indented)

    def test_default_indent_is_2(self):
        card = AdaptiveCardBuilder.create().add_text_block(lambda tb: tb.with_text("Hi")).build()
        json_str = to_json(card)
        # Default indent=2 should produce newlines
        assert "\n" in json_str
