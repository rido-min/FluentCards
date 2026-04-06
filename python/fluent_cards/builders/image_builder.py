from __future__ import annotations
from typing import Callable
from ..enums import HorizontalAlignment, ImageSize, ImageStyle, Spacing


class ImageBuilder:
    def __init__(self):
        self._image: dict = {'type': 'Image'}

    def with_id(self, id: str) -> ImageBuilder:
        self._image['id'] = id
        return self

    def with_url(self, url: str) -> ImageBuilder:
        self._image['url'] = url
        return self

    def with_alt_text(self, alt_text: str) -> ImageBuilder:
        self._image['altText'] = alt_text
        return self

    def with_size(self, size: ImageSize) -> ImageBuilder:
        self._image['size'] = size.value
        return self

    def with_style(self, style: ImageStyle) -> ImageBuilder:
        self._image['style'] = style.value
        return self

    def with_width(self, width: str) -> ImageBuilder:
        self._image['width'] = width
        return self

    def with_height(self, height: str) -> ImageBuilder:
        self._image['height'] = height
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> ImageBuilder:
        self._image['horizontalAlignment'] = alignment.value
        return self

    def with_background_color(self, color: str) -> ImageBuilder:
        self._image['backgroundColor'] = color
        return self

    def with_spacing(self, spacing: Spacing) -> ImageBuilder:
        self._image['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ImageBuilder:
        self._image['separator'] = separator
        return self

    def with_select_action(self, configure: Callable) -> ImageBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._image['selectAction'] = b.build()
        return self

    def build(self) -> dict:
        return self._image
