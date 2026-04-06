from __future__ import annotations
from typing import Callable, Union
from ..enums import ImageSize, Spacing


class ImageSetBuilder:
    def __init__(self):
        self._image_set: dict = {'type': 'ImageSet', 'images': []}

    def with_id(self, id: str) -> ImageSetBuilder:
        self._image_set['id'] = id
        return self

    def with_image_size(self, size: ImageSize) -> ImageSetBuilder:
        self._image_set['imageSize'] = size.value
        return self

    def with_spacing(self, spacing: Spacing) -> ImageSetBuilder:
        self._image_set['spacing'] = spacing.value
        return self

    def add_image(self, configure_or_image: Union[Callable, dict]) -> ImageSetBuilder:
        from .image_builder import ImageBuilder
        if callable(configure_or_image):
            b = ImageBuilder()
            configure_or_image(b)
            self._image_set['images'].append(b.build())
        else:
            self._image_set['images'].append(configure_or_image)
        return self

    def build(self) -> dict:
        return self._image_set
