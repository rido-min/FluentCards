from __future__ import annotations
from typing import Callable
from ..enums import TextSize, TextWeight, TextColor, HorizontalAlignment, FontType, TextBlockStyle, Spacing


class TextBlockBuilder:
    def __init__(self):
        self._block: dict = {'type': 'TextBlock', 'text': ''}

    def with_id(self, id: str) -> TextBlockBuilder:
        self._block['id'] = id
        return self

    def with_text(self, text: str) -> TextBlockBuilder:
        self._block['text'] = text
        return self

    def with_size(self, size: TextSize) -> TextBlockBuilder:
        self._block['size'] = size.value
        return self

    def with_weight(self, weight: TextWeight) -> TextBlockBuilder:
        self._block['weight'] = weight.value
        return self

    def with_color(self, color: TextColor) -> TextBlockBuilder:
        self._block['color'] = color.value
        return self

    def with_is_subtle(self, is_subtle: bool = True) -> TextBlockBuilder:
        self._block['isSubtle'] = is_subtle
        return self

    def with_wrap(self, wrap: bool) -> TextBlockBuilder:
        self._block['wrap'] = wrap
        return self

    def with_max_lines(self, max_lines: int) -> TextBlockBuilder:
        self._block['maxLines'] = max_lines
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> TextBlockBuilder:
        self._block['horizontalAlignment'] = alignment.value
        return self

    def with_font_type(self, font_type: FontType) -> TextBlockBuilder:
        self._block['fontType'] = font_type.value
        return self

    def with_style(self, style: TextBlockStyle) -> TextBlockBuilder:
        self._block['style'] = style.value
        return self

    def with_spacing(self, spacing: Spacing) -> TextBlockBuilder:
        self._block['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> TextBlockBuilder:
        self._block['separator'] = separator
        return self

    def with_is_visible(self, is_visible: bool) -> TextBlockBuilder:
        self._block['isVisible'] = is_visible
        return self

    def with_select_action(self, action: dict) -> TextBlockBuilder:
        self._block['selectAction'] = action
        return self

    def build(self) -> dict:
        return self._block
