from __future__ import annotations
from typing import Optional, Union
from ..enums import Spacing


class FactSetBuilder:
    """Fluent builder for creating FactSet elements."""

    def __init__(self):
        self._fact_set: dict = {'type': 'FactSet', 'facts': []}

    def with_id(self, id: str) -> FactSetBuilder:
        """Sets the unique identifier for the FactSet.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._fact_set['id'] = id
        return self

    def with_spacing(self, spacing: Spacing) -> FactSetBuilder:
        """Sets the spacing before the fact set.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._fact_set['spacing'] = spacing.value
        return self

    def with_is_visible(self, is_visible: bool) -> FactSetBuilder:
        """Sets whether the element is visible."""
        self._fact_set['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> FactSetBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._fact_set['separator'] = separator
        return self

    def with_height(self, height: str) -> FactSetBuilder:
        """Sets the height of the element."""
        self._fact_set['height'] = height
        return self

    def with_fallback(self, fallback) -> FactSetBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._fact_set['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> FactSetBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._fact_set:
            self._fact_set['requires'] = {}
        self._fact_set['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> FactSetBuilder:
        """Sets right-to-left text direction."""
        self._fact_set['rtl'] = rtl
        return self

    def add_fact(self, title_or_fact: Union[str, dict], value: Optional[str] = None) -> FactSetBuilder:
        """Adds a fact to the fact set.

        Args:
            title_or_fact: The title string for the fact, or a pre-built fact dictionary.
            value: The value string for the fact, required when title_or_fact is a string.

        Returns:
            The builder instance for method chaining.
        """
        if isinstance(title_or_fact, str):
            self._fact_set['facts'].append({'title': title_or_fact, 'value': value})
        else:
            self._fact_set['facts'].append(title_or_fact)
        return self

    def build(self) -> dict:
        """Builds and returns the configured FactSet dictionary.

        Returns:
            The configured FactSet dictionary.
        """
        return self._fact_set
