from __future__ import annotations
from typing import Callable, TYPE_CHECKING
from ..enums import Spacing

if TYPE_CHECKING:
    from .action_builder import ActionBuilder


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

    def with_is_visible(self, is_visible: bool) -> ActionSetBuilder:
        """Sets whether the element is visible."""
        self._action_set['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> ActionSetBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._action_set['separator'] = separator
        return self

    def with_height(self, height: str) -> ActionSetBuilder:
        """Sets the height of the element."""
        self._action_set['height'] = height
        return self

    def with_fallback(self, fallback) -> ActionSetBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._action_set['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> ActionSetBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._action_set:
            self._action_set['requires'] = {}
        self._action_set['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> ActionSetBuilder:
        """Sets right-to-left text direction."""
        self._action_set['rtl'] = rtl
        return self

    def add_action(self, configure: Callable[[ActionBuilder], None]) -> ActionSetBuilder:
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
