from __future__ import annotations
from typing import Callable, Optional
from ..enums import VerticalAlignment


class AdaptiveCardBuilder:
    """Fluent builder for creating Adaptive Card documents."""

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
        """Creates a new AdaptiveCardBuilder instance.

        Returns:
            A new builder instance with default version 1.5.
        """
        return cls()

    def with_version(self, version: str) -> AdaptiveCardBuilder:
        """Sets the Adaptive Cards schema version and updates the schema URL.

        Args:
            version: The schema version string (e.g. '1.5').

        Returns:
            The builder instance for method chaining.
        """
        self._card['version'] = version
        self._card['$schema'] = self.SCHEMA_URLS.get(
            version, 'http://adaptivecards.io/schemas/adaptive-card.json'
        )
        return self

    def with_schema(self, schema: Optional[str]) -> AdaptiveCardBuilder:
        """Overrides the schema URL for the card.

        Args:
            schema: The schema URL to set, or None to clear it.

        Returns:
            The builder instance for method chaining.
        """
        self._card['$schema'] = schema
        return self

    def with_fallback_text(self, fallback_text: str) -> AdaptiveCardBuilder:
        """Sets text displayed when the card cannot be rendered.

        Args:
            fallback_text: The fallback text to display.

        Returns:
            The builder instance for method chaining.
        """
        self._card['fallbackText'] = fallback_text
        return self

    def with_speak(self, speak: str) -> AdaptiveCardBuilder:
        """Sets the text to be spoken when the card is read aloud.

        Args:
            speak: The speech text.

        Returns:
            The builder instance for method chaining.
        """
        self._card['speak'] = speak
        return self

    def with_lang(self, lang: str) -> AdaptiveCardBuilder:
        """Sets the locale for the card content.

        Args:
            lang: The BCP-47 language tag (e.g. 'en-US').

        Returns:
            The builder instance for method chaining.
        """
        self._card['lang'] = lang
        return self

    def with_rtl(self, rtl: bool = True) -> AdaptiveCardBuilder:
        """Sets whether the card content should be laid out right-to-left.

        Args:
            rtl: True to enable right-to-left layout.

        Returns:
            The builder instance for method chaining.
        """
        self._card['rtl'] = rtl
        return self

    def with_min_height(self, min_height: str) -> AdaptiveCardBuilder:
        """Sets the minimum height of the card.

        Args:
            min_height: The minimum height value (e.g. '100px').

        Returns:
            The builder instance for method chaining.
        """
        self._card['minHeight'] = min_height
        return self

    def with_vertical_content_alignment(self, alignment: VerticalAlignment) -> AdaptiveCardBuilder:
        """Sets the vertical alignment of content within the card.

        Args:
            alignment: The vertical alignment value.

        Returns:
            The builder instance for method chaining.
        """
        self._card['verticalContentAlignment'] = alignment.value
        return self

    def with_background_image(self, background_image: dict) -> AdaptiveCardBuilder:
        """Sets the background image for the card.

        Args:
            background_image: A pre-built background image dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._card['backgroundImage'] = background_image
        return self

    def with_select_action(self, action: dict) -> AdaptiveCardBuilder:
        """Sets the action invoked when the card itself is selected.

        Args:
            action: A pre-built action dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._card['selectAction'] = action
        return self

    def with_metadata(self, web_url: str) -> AdaptiveCardBuilder:
        """Sets the metadata web URL for the card.

        Args:
            web_url: The URL to associate with the card's metadata.

        Returns:
            The builder instance for method chaining.
        """
        self._card['metadata'] = {'webUrl': web_url}
        return self

    # ─── Body elements ────────────────────────────────────────────────────────

    def add_text_block(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a TextBlock element to the card body.

        Args:
            configure: A callable that receives a TextBlockBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .text_block_builder import TextBlockBuilder
        b = TextBlockBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_image(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Image element to the card body.

        Args:
            configure: A callable that receives an ImageBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .image_builder import ImageBuilder
        b = ImageBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_container(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a Container element to the card body.

        Args:
            configure: A callable that receives a ContainerBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .container_builder import ContainerBuilder
        b = ContainerBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_column_set(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a ColumnSet element to the card body.

        Args:
            configure: A callable that receives a ColumnSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .column_set_builder import ColumnSetBuilder
        b = ColumnSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_fact_set(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a FactSet element to the card body.

        Args:
            configure: A callable that receives a FactSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .fact_set_builder import FactSetBuilder
        b = FactSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_rich_text_block(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a RichTextBlock element to the card body.

        Args:
            configure: A callable that receives a RichTextBlockBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .rich_text_block_builder import RichTextBlockBuilder
        b = RichTextBlockBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_action_set(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an ActionSet element to the card body.

        Args:
            configure: A callable that receives an ActionSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .action_set_builder import ActionSetBuilder
        b = ActionSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_media(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a Media element to the card body.

        Args:
            configure: A callable that receives a MediaBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .media_builder import MediaBuilder
        b = MediaBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_image_set(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an ImageSet element to the card body.

        Args:
            configure: A callable that receives an ImageSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .image_set_builder import ImageSetBuilder
        b = ImageSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_table(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds a Table element to the card body.

        Args:
            configure: A callable that receives a TableBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .table_builder import TableBuilder
        b = TableBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    # ─── Input elements ───────────────────────────────────────────────────────

    def add_input_text(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.Text element to the card body.

        Args:
            configure: A callable that receives an InputTextBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_text_builder import InputTextBuilder
        b = InputTextBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_number(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.Number element to the card body.

        Args:
            configure: A callable that receives an InputNumberBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_number_builder import InputNumberBuilder
        b = InputNumberBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_date(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.Date element to the card body.

        Args:
            configure: A callable that receives an InputDateBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_date_builder import InputDateBuilder
        b = InputDateBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_time(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.Time element to the card body.

        Args:
            configure: A callable that receives an InputTimeBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_time_builder import InputTimeBuilder
        b = InputTimeBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_toggle(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.Toggle element to the card body.

        Args:
            configure: A callable that receives an InputToggleBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_toggle_builder import InputToggleBuilder
        b = InputToggleBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_input_choice_set(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an Input.ChoiceSet element to the card body.

        Args:
            configure: A callable that receives an InputChoiceSetBuilder to configure the element.

        Returns:
            The builder instance for method chaining.
        """
        from .inputs.input_choice_set_builder import InputChoiceSetBuilder
        b = InputChoiceSetBuilder()
        configure(b)
        self._push_body(b.build())
        return self

    def add_element(self, element: dict) -> AdaptiveCardBuilder:
        """Adds a pre-built element directly to the card body.

        Args:
            element: A pre-built element dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._push_body(element)
        return self

    # ─── Actions ─────────────────────────────────────────────────────────────

    def add_action(self, configure: Callable) -> AdaptiveCardBuilder:
        """Adds an action to the card's actions list.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        if 'actions' not in self._card:
            self._card['actions'] = []
        self._card['actions'].append(b.build())
        return self

    # ─── Advanced configuration ───────────────────────────────────────────────

    def with_refresh(self, configure: Callable) -> AdaptiveCardBuilder:
        """Sets the refresh configuration for the card.

        Args:
            configure: A callable that receives a RefreshBuilder to configure refresh.

        Returns:
            The builder instance for method chaining.
        """
        from .refresh_builder import RefreshBuilder
        b = RefreshBuilder()
        configure(b)
        self._card['refresh'] = b.build()
        return self

    def with_authentication(self, configure: Callable) -> AdaptiveCardBuilder:
        """Sets the authentication configuration for the card.

        Args:
            configure: A callable that receives an AuthenticationBuilder to configure authentication.

        Returns:
            The builder instance for method chaining.
        """
        from .authentication_builder import AuthenticationBuilder
        b = AuthenticationBuilder()
        configure(b)
        self._card['authentication'] = b.build()
        return self

    def build(self) -> dict:
        """Builds and returns the configured Adaptive Card dictionary.

        Returns:
            The configured Adaptive Card dictionary.
        """
        return self._card

    def _push_body(self, element: dict) -> None:
        if 'body' not in self._card:
            self._card['body'] = []
        self._card['body'].append(element)
