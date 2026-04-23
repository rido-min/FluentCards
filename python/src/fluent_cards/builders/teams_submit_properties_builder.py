"""Fluent builder for Teams submit action properties (feedback control)."""
from __future__ import annotations


class TeamsSubmitPropertiesBuilder:
    """Fluent builder for Teams submit action properties."""

    def __init__(self):
        self._properties: dict = {}

    def with_feedback_hidden(self) -> TeamsSubmitPropertiesBuilder:
        """Hides the feedback UI after the submit action is invoked.

        Returns:
            The builder instance for method chaining.
        """
        self._properties['feedback'] = {'hide': True}
        return self

    def build(self) -> dict:
        """Builds and returns the configured Teams submit action properties.

        Returns:
            The configured Teams submit action properties dictionary.
        """
        return self._properties
