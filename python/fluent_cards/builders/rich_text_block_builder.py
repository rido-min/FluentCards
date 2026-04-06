from __future__ import annotations
from typing import Callable
from ..enums import HorizontalAlignment, Spacing


class RichTextBlockBuilder:
    def __init__(self):
        self._rich_text: dict = {'type': 'RichTextBlock', 'inlines': []}

    def with_id(self, id: str) -> RichTextBlockBuilder:
        self._rich_text['id'] = id
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> RichTextBlockBuilder:
        self._rich_text['horizontalAlignment'] = alignment.value
        return self

    def with_spacing(self, spacing: Spacing) -> RichTextBlockBuilder:
        self._rich_text['spacing'] = spacing.value
        return self

    def add_text(self, text: str) -> RichTextBlockBuilder:
        self._rich_text['inlines'].append(text)
        return self

    def add_text_run(self, configure: Callable) -> RichTextBlockBuilder:
        from .text_run_builder import TextRunBuilder
        b = TextRunBuilder()
        configure(b)
        self._rich_text['inlines'].append(b.build())
        return self

    def build(self) -> dict:
        return self._rich_text
