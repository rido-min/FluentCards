from __future__ import annotations
from typing import Callable, Union, TYPE_CHECKING
from ..enums import ImageSize, Spacing

if TYPE_CHECKING:
    from .image_builder import ImageBuilder


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

    def with_is_visible(self, is_visible: bool) -> ImageSetBuilder:
        """Sets whether the element is visible."""
        self._image_set['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> ImageSetBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._image_set['separator'] = separator
        return self

    def with_height(self, height: str) -> ImageSetBuilder:
        """Sets the height of the element."""
        self._image_set['height'] = height
        return self

    def with_fallback(self, fallback) -> ImageSetBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._image_set['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> ImageSetBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._image_set:
            self._image_set['requires'] = {}
        self._image_set['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> ImageSetBuilder:
        """Sets right-to-left text direction."""
        self._image_set['rtl'] = rtl
        return self

    def add_image(self, configure_or_image: Union[Callable[[ImageBuilder], None], dict]) -> ImageSetBuilder:
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
