from __future__ import annotations


class AuthenticationBuilder:
    """Fluent builder for creating authentication configurations for Adaptive Cards."""

    def __init__(self):
        self._auth: dict = {}

    def with_text(self, text: str) -> AuthenticationBuilder:
        """Sets the instructional text displayed in the sign-in prompt.

        Args:
            text: The sign-in prompt text.

        Returns:
            The builder instance for method chaining.
        """
        self._auth['text'] = text
        return self

    def with_connection_name(self, connection_name: str) -> AuthenticationBuilder:
        """Sets the OAuth connection name for authentication.

        Args:
            connection_name: The name of the OAuth connection.

        Returns:
            The builder instance for method chaining.
        """
        self._auth['connectionName'] = connection_name
        return self

    def with_token_exchange_resource(self, resource: dict) -> AuthenticationBuilder:
        """Sets the token exchange resource for single sign-on.

        Args:
            resource: A pre-built TokenExchangeResource dictionary.

        Returns:
            The builder instance for method chaining.
        """
        self._auth['tokenExchangeResource'] = resource
        return self

    def add_button(self, button: dict) -> AuthenticationBuilder:
        """Adds a sign-in button to the authentication configuration.

        Args:
            button: A pre-built AuthCardButton dictionary.

        Returns:
            The builder instance for method chaining.
        """
        if 'buttons' not in self._auth:
            self._auth['buttons'] = []
        self._auth['buttons'].append(button)
        return self

    def build(self) -> dict:
        """Builds and returns the configured authentication dictionary.

        Returns:
            The configured authentication dictionary.
        """
        return self._auth
