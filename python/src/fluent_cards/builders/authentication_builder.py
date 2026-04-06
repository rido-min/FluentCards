from __future__ import annotations


class AuthenticationBuilder:
    def __init__(self):
        self._auth: dict = {}

    def with_text(self, text: str) -> AuthenticationBuilder:
        self._auth['text'] = text
        return self

    def with_connection_name(self, connection_name: str) -> AuthenticationBuilder:
        self._auth['connectionName'] = connection_name
        return self

    def with_token_exchange_resource(self, resource: dict) -> AuthenticationBuilder:
        self._auth['tokenExchangeResource'] = resource
        return self

    def add_button(self, button: dict) -> AuthenticationBuilder:
        if 'buttons' not in self._auth:
            self._auth['buttons'] = []
        self._auth['buttons'].append(button)
        return self

    def build(self) -> dict:
        return self._auth
