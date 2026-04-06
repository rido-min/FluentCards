from __future__ import annotations
from typing import Callable, Optional
from ..enums import VerticalAlignment


class AdaptiveCardBuilder:
    SCHEMA_URLS: dict[str, str] = {
        '1.0': 'https://adaptivecards.io/schemas/1.0.0/adaptive-card.json',
        '1.1': 'https://adaptivecards.io/schemas/1.1.0/adaptive-card.json',
        '1.2': 'https://adaptivecards.io/schemas/1.2.0/adaptive-card.json',
        '1.3': 'https://adaptivecards.io/schemas/1.3.0/adaptive-card.json',
        '1.4': 'https://adaptivecards.io/schemas/1.4.0/adaptive-card.json',
        '1.5': 'https://adaptivecards.io/schemas/1.5.0/adaptive-card.json',
        '1.6': 'https://adaptivecards.io/schemas/1.6.0/adaptive-card.json',
    }

    def __init__(self):
        self._card: dict = {
            'type': 'AdaptiveCard',
            'version': '1.5',
            '$schema': self.SCHEMA_URLS['1.5'],
        }

    @classmethod
    def create(cls) -> AdaptiveCardBuilder:
        return cls()

    def with_version(self, version: str) -> AdaptiveCardBuilder:
        self._card['version'] = version
        self._card['$schema'] = self.SCHEMA_URLS.get(
            version, 'http://adaptivecards.io/schemas/adaptive-card.json'
        )
        return self

    def with_schema(self, schema: Optional[str]) -> AdaptiveCardBuilder:
        self._card['$schema'] = schema
        return self

    def with_fallback_text(self, fallback_text: str) -> AdaptiveCardBuilder:
        self._card['fallbackText'] = fallback_text
        return self

    def with_speak(self, speak: str) -> AdaptiveCardBuilder:
        self._card['speak'] = speak
        return self

    def with_lang(self, lang: str) -> AdaptiveCardBuilder:
        self._card['lang'] = lang
        return self

    def with_rtl(self, rtl: bool = True) -> AdaptiveCardBuilder:
        self._card['rtl'] = rtl
        return self

    def with_min_height(self, min_height: str) -> AdaptiveCardBuilder:
        self._card['minHeight'] = min_height
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> AdaptiveCardBuilder:
        self._card['verticalContentAlignment'] = alignment.value
        return self

    def with_background_image(self, background_image: dict) -> AdaptiveCardBuilder:
        self._card['backgroundImage'] = background_image
        return self

    def with_select_action(self, action: dict) -> AdaptiveCardBuilder:
        self._card['selectAction'] = action
        return self

    def with_metadata(self, web_url: str) -> AdaptiveCardBuilder:
        self._card['metadata'] = {'webUrl': web_url}
        return self

    # ─── Body elements ────────────────────────────────────────────────────────

    def add_text_block(self, configure: Callable) -> AdaptiveCardBuilder:
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_image(self, configure: Callable) -> AdaptiveCardBuilder:
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_container(self, configure: Callable) -> AdaptiveCardBuilder:
        from .container_builder import ContainerBuilder
        b = ContainerBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_column_set(self, configure: Callable) -> AdaptiveCardBuilder:
        from .column_set_builder import ColumnSetBuilder
        b = ColumnSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_fact_set(self, configure: Callable) -> AdaptiveCardBuilder:
        from .fact_set_builder import FactSetBuilder
        b = FactSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_rich_text_block(self, configure: Callable) -> AdaptiveCardBuilder:
        from .rich_text_block_builder import RichTextBlockBuilder
        b = RichTextBlockBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_action_set(self, configure: Callable) -> AdaptiveCardBuilder:
        from .action_set_builder import ActionSetBuilder
        b = ActionSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_media(self, configure: Callable) -> AdaptiveCardBuilder:
        from .media_builder import MediaBuilder
        b = MediaBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_image_set(self, configure: Callable) -> AdaptiveCardBuilder:
        from .image_set_builder import ImageSetBuilder
        b = ImageSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_table(self, configure: Callable) -> AdaptiveCardBuilder:
        from .table_builder import TableBuilder
        b = TableBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    # ─── Input elements ───────────────────────────────────────────────────────

    def add_input_text(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_text_builder import InputTextBuilder
        b = InputTextBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_number(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_number_builder import InputNumberBuilder
        b = InputNumberBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_date(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_date_builder import InputDateBuilder
        b = InputDateBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_time(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_time_builder import InputTimeBuilder
        b = InputTimeBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_toggle(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_toggle_builder import InputToggleBuilder
        b = InputToggleBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_choice_set(self, configure: Callable) -> AdaptiveCardBuilder:
        from .inputs.input_choice_set_builder import InputChoiceSetBuilder
        b = InputChoiceSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_element(self, element: dict) -> AdaptiveCardBuilder:
        """Add a pre-built element directly to the card body."""
        self._push_body(element)
        return self

    # ─── Actions ─────────────────────────────────────────────────────────────

    def add_action(self, configure: Callable) -> AdaptiveCardBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        if 'actions' not in self._card:
            self._card['actions'] = []
        self._card['actions'].append(b.build())
        return self

    # ─── Advanced configuration ───────────────────────────────────────────────

    def with_refresh(self, configure: Callable) -> AdaptiveCardBuilder:
        from .refresh_builder import RefreshBuilder
        b = RefreshBuilder()
        configure(b)
        self._card['refresh'] = b.build()
        return self

    def with_authentication(self, configure: Callable) -> AdaptiveCardBuilder:
        from .authentication_builder import AuthenticationBuilder
        b = AuthenticationBuilder()
        configure(b)
        self._card['authentication'] = b.build()
        return self

    def build(self) -> dict:
        return self._card

    def _push_body(self, element: dict) -> None:
        if 'body' not in self._card:
            self._card['body'] = []
        self._card['body'].append(element)
