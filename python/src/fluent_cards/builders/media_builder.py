from __future__ import annotations
from typing import Union
from urllib.parse import urlparse
from ..enums import Spacing


def _is_absolute_url(value: str) -> bool:
    """Returns whether the value is an absolute URL with scheme and host."""
    parsed = urlparse(value)
    return bool(parsed.scheme and parsed.netloc)


def _looks_like_mime_type(value: str) -> bool:
    """Returns whether the value heuristically matches a MIME type pattern."""
    if value.count('/') != 1 or '://' in value:
        return False
    major, minor = value.split('/', 1)
    return bool(major and minor)


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

    def with_is_visible(self, is_visible: bool) -> MediaBuilder:
        """Sets whether the element is visible."""
        self._media['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> MediaBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._media['separator'] = separator
        return self

    def with_height(self, height: str) -> MediaBuilder:
        """Sets the height of the element."""
        self._media['height'] = height
        return self

    def with_fallback(self, fallback) -> MediaBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._media['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> MediaBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._media:
            self._media['requires'] = {}
        self._media['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> MediaBuilder:
        """Sets right-to-left text direction."""
        self._media['rtl'] = rtl
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

    def add_caption_source(
        self,
        url_or_source: Union[str, dict],
        mime_type: str = None,
        label: str = None
    ) -> MediaBuilder:
        """Adds a caption source to the media element.

        Args:
            url_or_source: The URL of the caption file, or a pre-built caption source
                dictionary.
            mime_type: The MIME type of the caption (e.g. 'text/vtt'), required when
                url_or_source is a URL string.
            label: The label for the caption source when url_or_source is a URL string.

        Returns:
            The builder instance for method chaining.
        """
        self._media.setdefault('captionSources', [])

        if isinstance(url_or_source, str):
            url = url_or_source

            # Backward compatibility: support legacy order
            # add_caption_source(mime_type, url, label).
            if (
                mime_type is not None
                and _looks_like_mime_type(url_or_source)
                and _is_absolute_url(mime_type)
            ):
                url, mime_type = mime_type, url_or_source

            self._media['captionSources'].append({
                'type': 'CaptionSource',
                'mimeType': mime_type,
                'url': url,
                'label': label,
            })
        else:
            if mime_type is not None or label is not None:
                raise ValueError(
                    'When url_or_source is a dict, mime_type and label must be None'
                )
            self._media['captionSources'].append(url_or_source)
        return self

    def build(self) -> dict:
        """Builds and returns the configured Media dictionary.

        Returns:
            The configured Media dictionary.
        """
        return self._media
