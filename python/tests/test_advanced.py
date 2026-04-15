"""Advanced tests — version handling, authentication, refresh, card-level properties."""

import pytest
from fluent_cards import (
    AdaptiveCardBuilder,
    RefreshBuilder,
    AuthenticationBuilder,
    ActionBuilder,
)


class TestVersionHandling:
    @pytest.mark.parametrize("version", ["1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6"])
    def test_with_version_sets_version(self, version):
        card = AdaptiveCardBuilder.create().with_version(version).build()
        assert card["version"] == version

    @pytest.mark.parametrize("version", ["1.0", "1.1", "1.2", "1.3", "1.4", "1.5", "1.6"])
    def test_with_version_sets_schema_url(self, version):
        card = AdaptiveCardBuilder.create().with_version(version).build()
        schema = card.get("$schema", "")
        assert "adaptivecards.io" in schema or schema != ""

    def test_default_version_is_1_5(self):
        card = AdaptiveCardBuilder.create().build()
        assert card["version"] == "1.5"

    def test_unknown_version_still_sets_version(self):
        card = AdaptiveCardBuilder.create().with_version("9.9").build()
        assert card["version"] == "9.9"

    def test_unknown_version_has_schema(self):
        card = AdaptiveCardBuilder.create().with_version("9.9").build()
        # Should have some schema URL (may be generic)
        assert "$schema" in card

    def test_custom_schema_overrides(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.5")
            .with_schema("https://custom.schema/v1")
            .build()
        )
        assert card["$schema"] == "https://custom.schema/v1"

    def test_schema_none_removes_it(self):
        card = AdaptiveCardBuilder.create().with_schema(None).build()
        assert "$schema" not in card or card.get("$schema") is None


class TestAuthenticationBuilder:
    def test_authentication_text(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(lambda a: a.with_text("Please sign in"))
            .build()
        )
        assert card["authentication"]["text"] == "Please sign in"

    def test_authentication_connection_name(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(lambda a: a.with_connection_name("myConnection"))
            .build()
        )
        assert card["authentication"]["connectionName"] == "myConnection"

    def test_authentication_token_exchange_resource(self):
        resource = {"id": "resource1", "uri": "https://token.example.com", "providerId": "AAD"}
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(lambda a: a.with_token_exchange_resource(resource))
            .build()
        )
        assert card["authentication"]["tokenExchangeResource"] == resource

    def test_authentication_buttons(self):
        btn = {"type": "signin", "title": "Sign In", "value": "https://signin.example.com"}
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(lambda a: a.with_text("Auth").add_button(btn))
            .build()
        )
        assert len(card["authentication"]["buttons"]) == 1
        assert card["authentication"]["buttons"][0] == btn

    def test_authentication_all_properties(self):
        resource = {"id": "r1", "uri": "https://token.example.com", "providerId": "AAD"}
        btn1 = {"type": "signin", "title": "Sign In", "value": "https://login.example.com"}
        btn2 = {"type": "signin", "title": "SSO", "value": "https://sso.example.com"}
        card = (
            AdaptiveCardBuilder.create()
            .with_authentication(
                lambda a: a.with_text("Authenticate")
                .with_connection_name("conn1")
                .with_token_exchange_resource(resource)
                .add_button(btn1)
                .add_button(btn2)
            )
            .build()
        )
        auth = card["authentication"]
        assert auth["text"] == "Authenticate"
        assert auth["connectionName"] == "conn1"
        assert auth["tokenExchangeResource"] == resource
        assert len(auth["buttons"]) == 2

    def test_standalone_authentication_builder(self):
        builder = AuthenticationBuilder()
        result = builder.with_text("Sign in").with_connection_name("c").build()
        assert result["text"] == "Sign in"
        assert result["connectionName"] == "c"


class TestRefreshBuilder:
    def test_refresh_with_action(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_refresh(
                lambda r: r.with_action(lambda a: a.execute("Refresh").with_verb("refresh"))
            )
            .build()
        )
        assert card["refresh"]["action"]["type"] == "Action.Execute"
        assert card["refresh"]["action"]["verb"] == "refresh"

    def test_refresh_with_user_ids(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_refresh(
                lambda r: r.with_action(lambda a: a.execute("Refresh"))
                .add_user_id("user1")
                .add_user_id("user2")
            )
            .build()
        )
        assert card["refresh"]["userIds"] == ["user1", "user2"]

    def test_refresh_with_expires(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_refresh(
                lambda r: r.with_action(lambda a: a.execute("Refresh"))
                .with_expires("2024-12-31T23:59:59Z")
            )
            .build()
        )
        assert card["refresh"]["expires"] == "2024-12-31T23:59:59Z"

    def test_refresh_all_properties(self):
        card = (
            AdaptiveCardBuilder.create()
            .with_refresh(
                lambda r: r.with_action(
                    lambda a: a.execute("Refresh").with_verb("doRefresh").with_data({"key": "val"})
                )
                .add_user_id("u1")
                .add_user_id("u2")
                .with_expires("2025-01-01T00:00:00Z")
            )
            .build()
        )
        refresh = card["refresh"]
        assert refresh["action"]["type"] == "Action.Execute"
        assert refresh["action"]["verb"] == "doRefresh"
        assert refresh["action"]["data"] == {"key": "val"}
        assert refresh["userIds"] == ["u1", "u2"]
        assert refresh["expires"] == "2025-01-01T00:00:00Z"

    def test_standalone_refresh_builder(self):
        builder = RefreshBuilder()
        result = (
            builder.with_action(lambda a: a.execute("R"))
            .add_user_id("u1")
            .build()
        )
        assert result["action"]["type"] == "Action.Execute"
        assert result["userIds"] == ["u1"]


class TestMetadata:
    def test_web_url(self):
        card = AdaptiveCardBuilder.create().with_metadata("https://example.com/page").build()
        assert card["metadata"]["webUrl"] == "https://example.com/page"

    def test_metadata_is_dict(self):
        card = AdaptiveCardBuilder.create().with_metadata("https://example.com").build()
        assert isinstance(card["metadata"], dict)

    def test_metadata_absent_by_default(self):
        card = AdaptiveCardBuilder.create().build()
        assert "metadata" not in card or card.get("metadata") is None


class TestCardLevelProperties:
    def test_fallback_text(self):
        card = AdaptiveCardBuilder.create().with_fallback_text("Cannot render").build()
        assert card["fallbackText"] == "Cannot render"

    def test_speak(self):
        card = AdaptiveCardBuilder.create().with_speak("<speak>Hello</speak>").build()
        assert card["speak"] == "<speak>Hello</speak>"

    def test_lang(self):
        card = AdaptiveCardBuilder.create().with_lang("de-DE").build()
        assert card["lang"] == "de-DE"

    def test_rtl_true(self):
        card = AdaptiveCardBuilder.create().with_rtl(True).build()
        assert card["rtl"] is True

    def test_rtl_false(self):
        card = AdaptiveCardBuilder.create().with_rtl(False).build()
        assert card["rtl"] is False

    def test_min_height(self):
        card = AdaptiveCardBuilder.create().with_min_height("500px").build()
        assert card["minHeight"] == "500px"

    def test_select_action_dict(self):
        action = {"type": "Action.Submit", "title": "Click"}
        card = AdaptiveCardBuilder.create().with_select_action(action).build()
        assert card["selectAction"]["type"] == "Action.Submit"
        assert card["selectAction"]["title"] == "Click"

    def test_select_action_open_url(self):
        action = {"type": "Action.OpenUrl", "url": "https://example.com", "title": "Open"}
        card = AdaptiveCardBuilder.create().with_select_action(action).build()
        assert card["selectAction"]["url"] == "https://example.com"

    def test_vertical_content_alignment(self):
        from fluent_cards import VerticalAlignment
        card = (
            AdaptiveCardBuilder.create()
            .with_vertical_content_alignment(VerticalAlignment.Center)
            .build()
        )
        assert card["verticalContentAlignment"] == "center"

    def test_card_with_all_properties_combined(self):
        from fluent_cards import VerticalAlignment, BackgroundImageFillMode
        card = (
            AdaptiveCardBuilder.create()
            .with_version("1.6")
            .with_fallback_text("FB")
            .with_speak("Speak")
            .with_lang("en")
            .with_rtl(False)
            .with_min_height("100px")
            .with_vertical_content_alignment(VerticalAlignment.Top)
            .with_metadata("https://example.com")
            .with_background_image(lambda bg: bg.with_url("https://bg.example.com/bg.png"))
            .with_select_action({"type": "Action.OpenUrl", "url": "https://example.com"})
            .add_text_block(lambda tb: tb.with_text("Body"))
            .add_action(lambda a: a.open_url("https://example.com").with_title("Go"))
            .with_refresh(
                lambda r: r.with_action(lambda a: a.execute("Refresh"))
            )
            .with_authentication(lambda a: a.with_text("Auth"))
            .build()
        )
        assert card["type"] == "AdaptiveCard"
        assert card["version"] == "1.6"
        assert card["fallbackText"] == "FB"
        assert card["speak"] == "Speak"
        assert card["lang"] == "en"
        assert card["rtl"] is False
        assert card["minHeight"] == "100px"
        assert card["verticalContentAlignment"] == "top"
        assert card["metadata"]["webUrl"] == "https://example.com"
        assert card["backgroundImage"]["url"] == "https://bg.example.com/bg.png"
        assert card["selectAction"]["type"] == "Action.OpenUrl"
        assert len(card["body"]) == 1
        assert len(card["actions"]) == 1
        assert "refresh" in card
        assert "authentication" in card
