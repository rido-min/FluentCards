from __future__ import annotations
from ..enums import BackgroundImageFillMode, HorizontalAlignment, VerticalAlignment


class BackgroundImageBuilder:
    """Fluent builder for creating BackgroundImage configurations."""

    def __init__(self):
        self._bg: dict = {}

    def with_url(self, url: str) -> BackgroundImageBuilder:
        """Sets the URL of the background image.

        Args:
            url: The absolute URL of the image.

        Returns:
            The builder instance for method chaining.
        """
        self._bg['url'] = url
        return self

    def with_fill_mode(self, fill_mode: BackgroundImageFillMode) -> BackgroundImageBuilder:
        """Sets the fill mode for the background image.

        Args:
            fill_mode: The fill mode controlling how the image is tiled or scaled.

        Returns:
            The builder instance for method chaining.
        """
        self._bg['fillMode'] = fill_mode.value
        return self

    def with_horizontal_alignment(self, alignment: HorizontalAlignment) -> BackgroundImageBuilder:
        """Sets the horizontal alignment of the background image.

        Args:
            alignment: The horizontal alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._bg['horizontalAlignment'] = alignment.value
        return self

    def with_vertical_alignment(self, alignment: VerticalAlignment) -> BackgroundImageBuilder:
        """Sets the vertical alignment of the background image.

        Args:
            alignment: The vertical alignment.

        Returns:
            The builder instance for method chaining.
        """
        self._bg['verticalAlignment'] = alignment.value
        return self

    def build(self) -> dict:
        """Builds and returns the configured BackgroundImage dictionary.

        Returns:
            The configured BackgroundImage dictionary.
        """
        return self._bg
