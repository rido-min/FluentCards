from __future__ import annotations
from typing import Callable
from ..enums import Spacing


class ActionSetBuilder:
    """Fluent builder for creating ActionSet elements."""

    def __init__(self):
        self._action_set: dict = {'type': 'ActionSet', 'actions': []}

    def with_id(self, id: str) -> ActionSetBuilder:
        """Sets the unique identifier for the ActionSet.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._action_set['id'] = id
        return self

    def with_spacing(self, spacing: Spacing) -> ActionSetBuilder:
        """Sets the spacing before the action set.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._action_set['spacing'] = spacing.value
        return self

    def add_action(self, configure: Callable) -> ActionSetBuilder:
        """Adds an action to the action set.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._action_set['actions'].append(b.build())
        return self

    def build(self) -> dict:
        """Builds and returns the configured ActionSet dictionary.

        Returns:
            The configured ActionSet dictionary.
        """
        return self._action_set
