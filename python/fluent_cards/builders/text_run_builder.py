from __future__ import annotations
from typing import Callable
from ..enums import TextColor, TextSize, TextWeight


class TextRunBuilder:
    def __init__(self):
        self._run: dict = {'type': 'TextRun'}

    def with_text(self, text: str) -> TextRunBuilder:
        self._run['text'] = text
        return self

    def with_size(self, size: TextSize) -> TextRunBuilder:
        self._run['size'] = size.value
        return self

    def with_weight(self, weight: TextWeight) -> TextRunBuilder:
        self._run['weight'] = weight.value
        return self

    def with_color(self, color: TextColor) -> TextRunBuilder:
        self._run['color'] = color.value
        return self

    def is_subtle(self, subtle: bool = True) -> TextRunBuilder:
        self._run['isSubtle'] = subtle
        return self

    def is_italic(self, italic: bool = True) -> TextRunBuilder:
        self._run['italic'] = italic
        return self

    def is_strikethrough(self, strikethrough: bool = True) -> TextRunBuilder:
        self._run['strikethrough'] = strikethrough
        return self

    def is_underline(self, underline: bool = True) -> TextRunBuilder:
        self._run['underline'] = underline
        return self

    def is_highlight(self, highlight: bool = True) -> TextRunBuilder:
        self._run['highlight'] = highlight
        return self

    def with_select_action(self, configure: Callable) -> TextRunBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._run['selectAction'] = b.build()
        return self

    def build(self) -> dict:
        return self._run
