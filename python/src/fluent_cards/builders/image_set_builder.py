from __future__ import annotations
from typing import Callable, Union
from ..enums import ImageSize, Spacing


class ImageSetBuilder:
    """Fluent builder for creating ImageSet elements."""

    def __init__(self):
        self._image_set: dict = {'type': 'ImageSet', 'images': []}

    def with_id(self, id: str) -> ImageSetBuilder:
        """Sets the unique identifier for the ImageSet.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._image_set['id'] = id
        return self

    def with_image_size(self, size: ImageSize) -> ImageSetBuilder:
        """Sets the size applied to all images in the set.

        Args:
            size: The image size.

        Returns:
            The builder instance for method chaining.
        """
        self._image_set['imageSize'] = size.value
        return self

    def with_spacing(self, spacing: Spacing) -> ImageSetBuilder:
        """Sets the spacing before the image set.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._image_set['spacing'] = spacing.value
        return self

    def add_image(self, configure_or_image: Union[Callable, dict]) -> ImageSetBuilder:
        """Adds an image to the image set.

        Args:
            configure_or_image: Either a callable that receives an ImageBuilder to configure
                the image, or a pre-built image dictionary.

        Returns:
            The builder instance for method chaining.
        """
        from .image_builder import ImageBuilder
        if callable(configure_or_image):
            b = ImageBuilder()
            configure_or_image(b)
            self._image_set['images'].append(b.build())
        else:
            self._image_set['images'].append(configure_or_image)
        return self

    def build(self) -> dict:
        """Builds and returns the configured ImageSet dictionary.

        Returns:
            The configured ImageSet dictionary.
        """
        return self._image_set
