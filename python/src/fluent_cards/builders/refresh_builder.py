from __future__ import annotations
from typing import Callable


class RefreshBuilder:
    def __init__(self):
        self._refresh: dict = {}

    def with_action(self, configure: Callable) -> RefreshBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._refresh['action'] = b.build()
        return self

    def add_user_id(self, user_id: str) -> RefreshBuilder:
        if 'userIds' not in self._refresh:
            self._refresh['userIds'] = []
        self._refresh['userIds'].append(user_id)
        return self

    def with_expires(self, expires: str) -> RefreshBuilder:
        self._refresh['expires'] = expires
        return self

    def build(self) -> dict:
        return self._refresh
