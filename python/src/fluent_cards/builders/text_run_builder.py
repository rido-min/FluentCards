from __future__ import annotations
from typing import Callable
from ..enums import TextColor, TextSize, TextWeight


class TextRunBuilder:
    """Fluent builder for creating TextRun inlines within a RichTextBlock."""

    def __init__(self):
        self._run: dict = {'type': 'TextRun'}

    def with_text(self, text: str) -> TextRunBuilder:
        """Sets the text content of the run.

        Args:
            text: The text content.

        Returns:
            The builder instance for method chaining.
        """
        self._run['text'] = text
        return self

    def with_size(self, size: TextSize) -> TextRunBuilder:
        """Sets the size of the text run.

        Args:
            size: The text size.

        Returns:
            The builder instance for method chaining.
        """
        self._run['size'] = size.value
        return self

    def with_weight(self, weight: TextWeight) -> TextRunBuilder:
        """Sets the weight of the text run.

        Args:
            weight: The text weight.

        Returns:
            The builder instance for method chaining.
        """
        self._run['weight'] = weight.value
        return self

    def with_color(self, color: TextColor) -> TextRunBuilder:
        """Sets the color of the text run.

        Args:
            color: The text color.

        Returns:
            The builder instance for method chaining.
        """
        self._run['color'] = color.value
        return self

    def is_subtle(self, subtle: bool = True) -> TextRunBuilder:
        """Sets whether the text run displays with subtle styling.

        Args:
            subtle: True for subtle styling.

        Returns:
            The builder instance for method chaining.
        """
        self._run['isSubtle'] = subtle
        return self

    def is_italic(self, italic: bool = True) -> TextRunBuilder:
        """Sets whether the text run is italicized.

        Args:
            italic: True to italicize the text.

        Returns:
            The builder instance for method chaining.
        """
        self._run['italic'] = italic
        return self

    def is_strikethrough(self, strikethrough: bool = True) -> TextRunBuilder:
        """Sets whether the text run displays with strikethrough formatting.

        Args:
            strikethrough: True to apply strikethrough.

        Returns:
            The builder instance for method chaining.
        """
        self._run['strikethrough'] = strikethrough
        return self

    def is_underline(self, underline: bool = True) -> TextRunBuilder:
        """Sets whether the text run displays with underline formatting.

        Args:
            underline: True to apply underline.

        Returns:
            The builder instance for method chaining.
        """
        self._run['underline'] = underline
        return self

    def is_highlight(self, highlight: bool = True) -> TextRunBuilder:
        """Sets whether the text run displays with highlight formatting.

        Args:
            highlight: True to apply highlight.

        Returns:
            The builder instance for method chaining.
        """
        self._run['highlight'] = highlight
        return self

    def with_select_action(self, configure: Callable) -> TextRunBuilder:
        """Sets the action invoked when the text run is selected.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._run['selectAction'] = b.build()
        return self

    def build(self) -> dict:
        """Builds and returns the configured TextRun dictionary.

        Returns:
            The configured TextRun dictionary.
        """
        return self._run
