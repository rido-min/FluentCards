"""Fluent builder for Teams-specific action data payloads."""
from __future__ import annotations
from typing import Any


class TeamsDataBuilder:
    """Fluent builder for Teams action data with msteams and custom properties."""

    def __init__(self):
        self._msteams: dict | None = None
        self._properties: dict[str, Any] = {}

    def with_task_fetch(self) -> TeamsDataBuilder:
        """Sets the msteams object to ``{'type': 'task/fetch'}``.

        Returns:
            The builder instance for method chaining.
        """
        self._msteams = {'type': 'task/fetch'}
        return self

    def with_msteams(self, value: dict) -> TeamsDataBuilder:
        """Sets the msteams object from a dictionary.

        Args:
            value: The msteams object. Must be a dict.

        Returns:
            The builder instance for method chaining.

        Raises:
            TypeError: If value is not a dict.
        """
        if not isinstance(value, dict):
            raise TypeError('The msteams value must be a dict.')
        self._msteams = dict(value)
        return self

    def with_property(self, key: str, value: Any) -> TeamsDataBuilder:
        """Adds a custom property to the data payload.

        Args:
            key: The property name. Cannot be 'msteams'.
            value: The property value.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If key is 'msteams' (case-insensitive).
        """
        if key.lower() == 'msteams':
            raise ValueError(
                "Cannot use 'msteams' as a property key. "
                "Use with_msteams() or with_task_fetch() instead."
            )
        self._properties[key] = value
        return self

    def build(self) -> dict:
        """Builds and returns the data payload as a dictionary.

        Returns:
            A dict containing msteams and custom properties.
        """
        result: dict[str, Any] = {}
        if self._msteams is not None:
            result['msteams'] = self._msteams
        result.update(self._properties)
        return result
