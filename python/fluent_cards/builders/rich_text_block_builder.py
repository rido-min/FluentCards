from __future__ import annotations
from typing import Callable
from ..enums import HorizontalAlignment, Spacing


class RichTextBlockBuilder:
    """Fluent builder for creating RichTextBlock elements."""

    def __init__(self):
        self._rich_text: dict = {'type': 'RichTextBlock', 'inlines': []}

    def with_id(self, id: str) -> RichTextBlockBuilder:
        """Sets the unique identifier for the RichTextBlock.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._rich_text['id'] = id
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> RichTextBlockBuilder:
        """Sets the horizontal alignment of the rich text block.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._rich_text['horizontalAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> RichTextBlockBuilder:
        """Sets the spacing before the rich text block.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._rich_text['spacing'] = spacing.value
        return self

    def add_text(self, text: str) -> RichTextBlockBuilder:
        """Adds a plain text inline string to the rich text block.

        Args:
            text: The text content to add.

        Returns:
            The builder instance for method chaining.
        """
        self._rich_text['inlines'].append(text)
        return self

    def add_text_run(self, configure: Callable) -> RichTextBlockBuilder:
        """Adds a formatted TextRun inline to the rich text block.

        Args:
            configure: A callable that receives a TextRunBuilder to configure the inline.

        Returns:
            The builder instance for method chaining.
        """
        from .text_run_builder import TextRunBuilder
        b = TextRunBuilder()
        configure(b)
        self._rich_text['inlines'].append(b.build())
        return self

    def build(self) -> dict:
        """Builds and returns the configured RichTextBlock dictionary.

        Returns:
            The configured RichTextBlock dictionary.
        """
        return self._rich_text
