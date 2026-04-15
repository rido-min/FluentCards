"""Fluent builder for Teams card-level properties (width, mentions)."""
from __future__ import annotations


class TeamsCardPropertiesBuilder:
    """Fluent builder for Teams card-level properties."""

    def __init__(self):
        self._properties: dict = {}

    def with_full_width(self) -> TeamsCardPropertiesBuilder:
        """Sets the card width to Full.

        Returns:
            The builder instance for method chaining.
        """
        self._properties['width'] = 'Full'
        return self

    def add_mention(self, display_name: str, user_id: str) -> TeamsCardPropertiesBuilder:
        """Adds an @mention entity with auto-generated ``<at>displayName</at>`` text.

        Args:
            display_name: The user's display name.
            user_id: The Teams user ID.

        Returns:
            The builder instance for method chaining.
        """
        if 'entities' not in self._properties:
            self._properties['entities'] = []
        self._properties['entities'].append({
            'type': 'mention',
            'text': f'<at>{display_name}</at>',
            'mentioned': {
                'id': user_id,
                'name': display_name,
            },
        })
        return self

    def build(self) -> dict:
        """Builds and returns the configured Teams card properties.

        Returns:
            The configured Teams card properties dictionary.
        """
        return self._properties
