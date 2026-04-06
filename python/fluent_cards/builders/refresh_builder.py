from __future__ import annotations
from typing import Callable


class RefreshBuilder:
    """Fluent builder for creating refresh configurations for Adaptive Cards."""

    def __init__(self):
        self._refresh: dict = {}

    def with_action(self, configure: Callable) -> RefreshBuilder:
        """Sets the action invoked to refresh the card.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._refresh['action'] = b.build()
        return self

    def add_user_id(self, user_id: str) -> RefreshBuilder:
        """Adds a user ID that triggers automatic card refresh when the user views it.

        Args:
            user_id: The user ID to add.

        Returns:
            The builder instance for method chaining.
        """
        if 'userIds' not in self._refresh:
            self._refresh['userIds'] = []
        self._refresh['userIds'].append(user_id)
        return self

    def with_expires(self, expires: str) -> RefreshBuilder:
        """Sets the expiry time for the refresh configuration.

        Args:
            expires: An ISO 8601 datetime string indicating when the card expires.

        Returns:
            The builder instance for method chaining.
        """
        self._refresh['expires'] = expires
        return self

    def build(self) -> dict:
        """Builds and returns the configured refresh dictionary.

        Returns:
            The configured refresh dictionary.
        """
        return self._refresh
