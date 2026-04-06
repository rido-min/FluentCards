from __future__ import annotations
from typing import Callable
from ..enums import HorizontalAlignment, ImageSize, ImageStyle, Spacing


class ImageBuilder:
    """Fluent builder for creating Image elements."""

    def __init__(self):
        self._image: dict = {'type': 'Image'}

    def with_id(self, id: str) -> ImageBuilder:
        """Sets the unique identifier for the Image.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._image['id'] = id
        return self

    def with_url(self, url: str) -> ImageBuilder:
        """Sets the URL of the image to display.

        Args:
            url: The absolute URL of the image.

        Returns:
            The builder instance for method chaining.
        """
        self._image['url'] = url
        return self

    def with_alt_text(self, alt_text: str) -> ImageBuilder:
        """Sets the alternate text for the image for accessibility.

        Args:
            alt_text: The descriptive text for the image.

        Returns:
            The builder instance for method chaining.
        """
        self._image['altText'] = alt_text
        return self

    def with_size(self, size: ImageSize) -> ImageBuilder:
        """Sets the size of the image.

        Args:
            size: The image size.

        Returns:
            The builder instance for method chaining.
        """
        self._image['size'] = size.value
        return self

    def with_style(self, style: ImageStyle) -> ImageBuilder:
        """Sets the display style of the image.

        Args:
            style: The image style.

        Returns:
            The builder instance for method chaining.
        """
        self._image['style'] = style.value
        return self

    def with_width(self, width: str) -> ImageBuilder:
        """Sets the explicit width of the image.

        Args:
            width: The width value (e.g. '50px').

        Returns:
            The builder instance for method chaining.
        """
        self._image['width'] = width
        return self

    def with_height(self, height: str) -> ImageBuilder:
        """Sets the explicit height of the image.

        Args:
            height: The height value (e.g. '50px').

        Returns:
            The builder instance for method chaining.
        """
        self._image['height'] = height
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> ImageBuilder:
        """Sets the horizontal alignment of the image.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._image['horizontalAlignment'] = alignment.value
        return self

    def with_background_color(self, color: str) -> ImageBuilder:
        """Sets the background color shown behind the image.

        Args:
            color: A CSS color value (e.g. '#ffffff').

        Returns:
            The builder instance for method chaining.
        """
        self._image['backgroundColor'] = color
        return self

    def with_spacing(self, spacing: Spacing) -> ImageBuilder:
        """Sets the spacing before the image.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._image['spacing'] = spacing.value
        return self

    def with_separator(self, separator: bool) -> ImageBuilder:
        """Sets whether a separator line is drawn above the image.

        Args:
            separator: True to draw a separator line above the element.

        Returns:
            The builder instance for method chaining.
        """
        self._image['separator'] = separator
        return self

    def with_select_action(self, configure: Callable) -> ImageBuilder:
        """Sets the action invoked when the image is selected.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._image['selectAction'] = b.build()
        return self

    def build(self) -> dict:
        """Builds and returns the configured Image dictionary.

        Returns:
            The configured Image dictionary.
        """
        return self._image
