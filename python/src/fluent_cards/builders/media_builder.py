from __future__ import annotations
from typing import Union
from ..enums import Spacing


class MediaBuilder:
    def __init__(self):
        self._media: dict = {'type': 'Media', 'sources': []}

    def with_id(self, id: str) -> MediaBuilder:
        self._media['id'] = id
        return self

    def with_poster(self, poster: str) -> MediaBuilder:
        self._media['poster'] = poster
        return self

    def with_alt_text(self, alt_text: str) -> MediaBuilder:
        self._media['altText'] = alt_text
        return self

    def with_spacing(self, spacing: Spacing) -> MediaBuilder:
        self._media['spacing'] = spacing.value
        return self

    def add_source(self, url_or_source: Union[str, dict], mime_type: str = None) -> MediaBuilder:
        if isinstance(url_or_source, str):
            self._media['sources'].append({'url': url_or_source, 'mimeType': mime_type})
        else:
            self._media['sources'].append(url_or_source)
        return self

    def build(self) -> dict:
        return self._media
