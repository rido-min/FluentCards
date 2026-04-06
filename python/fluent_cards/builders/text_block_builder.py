from __future__ import annotations
from typing import Callable
from ..enums import TextSize, TextWeight, TextColor, HorizontalAlignment, FontType, TextBlockStyle, Spacing


class TextBlockBuilder:
    """Fluent builder for creating TextBlock elements."""

    def __init__(self):
        self._block: dict = {'type': 'TextBlock', 'text': ''}

    def with_id(self, id: str) -> TextBlockBuilder:
        """Sets the unique identifier for the TextBlock.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._block['id'] = id
        return self

    def with_text(self, text: str) -> TextBlockBuilder:
        """Sets the text to display.

        Args:
            text: The text content.

        Returns:
            The builder instance for method chaining.
        """
        self._block['text'] = text
        return self

    def with_size(self, size: TextSize) -> TextBlockBuilder:
        """Sets the size of the text.

        Args:
            size: The text size.

        Returns:
            The builder instance for method chaining.
        """
        self._block['size'] = size.value
        return self

    def with_weight(self, weight: TextWeight) -> TextBlockBuilder:
        """Sets the weight (boldness) of the text.

        Args:
            weight: The text weight.

        Returns:
            The builder instance for method chaining.
        """
        self._block['weight'] = weight.value
        return self

    def with_color(self, color: TextColor) -> TextBlockBuilder:
        """Sets the color of the text.

        Args:
            color: The text color.

        Returns:
            The builder instance for method chaining.
        """
        self._block['color'] = color.value
        return self

    def with_is_subtle(self, is_subtle: bool = True) -> TextBlockBuilder:
        """Sets whether the text displays with subtle styling.

        Args:
            is_subtle: True for subtle styling.

        Returns:
            The builder instance for method chaining.
        """
        self._block['isSubtle'] = is_subtle
        return self

    def with_wrap(self, wrap: bool) -> TextBlockBuilder:
        """Sets whether the text should wrap.

        Args:
            wrap: True to allow text wrapping, False to clip text.

        Returns:
            The builder instance for method chaining.
        """
        self._block['wrap'] = wrap
        return self

    def with_max_lines(self, max_lines: int) -> TextBlockBuilder:
        """Sets the maximum number of lines to display.

        Args:
            max_lines: The maximum number of lines.

        Returns:
            The builder instance for method chaining.
        """
        self._block['maxLines'] = max_lines
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> TextBlockBuilder:
        """Sets the horizontal alignment of the text.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._block['horizontalAlignment'] = alignment.value
        return self

    def with_font_type(self, font_type: FontType) -> TextBlockBuilder:
        """Sets the font type for the text.

        Args:
            font_type: The font type.

        Returns:
            The builder instance for method chaining.
        """
        self._block['fontType'] = font_type.value
        return self

    def with_style(self, style: TextBlockStyle) -> TextBlockBuilder:
        """Sets the style of the text block.

        Args:
            style: The text block style.

        Returns:
            The builder instance for method chaining.
        """
        self._block['style'] = style.value
        return self

    def with_spacing(self, spacing: Spacing) -> TextBlockBuilder:
        """Sets the spacing before the text block.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._block['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> TextBlockBuilder:
        """Sets whether a separator line is drawn above the text block.

        Args:
            separator: True to draw a separator line above the element.

        Returns:
            The builder instance for method chaining.
        """
        self._block['separator'] = separator
        return self

    def with_is_visible(self, is_visible: bool) -> TextBlockBuilder:
        """Sets whether the text block is visible.

        Args:
            is_visible: False to hide the element initially.

        Returns:
            The builder instance for method chaining.
        """
        self._block['isVisible'] = is_visible
        return self

    def with_select_action(self, action: dict) -> TextBlockBuilder:
        """Sets the action invoked when the text block is selected.

        Args:
            action: A pre-built action dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._block['selectAction'] = action
        return self

    def build(self) -> dict:
        """Builds and returns the configured TextBlock dictionary.

        Returns:
            The configured TextBlock dictionary.
        """
        return self._block
