from __future__ import annotations
from ..enums import BackgroundImageFillMode, HorizontalAlignment, VerticalAlignment


class BackgroundImageBuilder:
    def __init__(self):
        self._bg: dict = {}

    def with_url(self, url: str) -> BackgroundImageBuilder:
        self._bg['url'] = url
        return self

    def with_fill_mode(self, fill_mode: BackgroundImageFillMode) -> BackgroundImageBuilder:
        self._bg['fillMode'] = fill_mode.value
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> BackgroundImageBuilder:
        self._bg['horizontalAlignment'] = alignment.value
        return self

    def with_vertical_alignment(self, alignment: VerticalAlignment) -> BackgroundImageBuilder:
        self._bg['verticalAlignment'] = alignment.value
        return self

    def build(self) -> dict:
        return self._bg
