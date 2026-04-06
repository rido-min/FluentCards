from __future__ import annotations
from typing import Union
from ..enums import Spacing


class MediaBuilder:
    """Fluent builder for creating Media elements."""

    def __init__(self):
        self._media: dict = {'type': 'Media', 'sources': []}

    def with_id(self, id: str) -> MediaBuilder:
        """Sets the unique identifier for the Media element.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._media['id'] = id
        return self

    def with_poster(self, poster: str) -> MediaBuilder:
        """Sets the URL of the poster image displayed before the media plays.

        Args:
            poster: The URL of the poster image.

        Returns:
            The builder instance for method chaining.
        """
        self._media['poster'] = poster
        return self

    def with_alt_text(self, alt_text: str) -> MediaBuilder:
        """Sets the alternate text for the media element for accessibility.

        Args:
            alt_text: The descriptive text for the media.

        Returns:
            The builder instance for method chaining.
        """
        self._media['altText'] = alt_text
        return self

    def with_spacing(self, spacing: Spacing) -> MediaBuilder:
        """Sets the spacing before the media element.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._media['spacing'] = spacing.value
        return self

    def add_source(self, url_or_source: Union[str, dict], mime_type: str = None) -> MediaBuilder:
        """Adds a media source to the element.

        Args:
            url_or_source: The URL of the media source, or a pre-built source dictionary.
            mime_type: The MIME type of the media (e.g. 'video/mp4'), required when
                url_or_source is a URL string.

        Returns:
            The builder instance for method chaining.
        """
        if isinstance(url_or_source, str):
            self._media['sources'].append({'url': url_or_source, 'mimeType': mime_type})
        else:
            self._media['sources'].append(url_or_source)
        return self

    def build(self) -> dict:
        """Builds and returns the configured Media dictionary.

        Returns:
            The configured Media dictionary.
        """
        return self._media
