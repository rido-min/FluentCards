from __future__ import annotations
from typing import Callable
from ..enums import Spacing


class ActionSetBuilder:
    def __init__(self):
        self._action_set: dict = {'type': 'ActionSet', 'actions': []}

    def with_id(self, id: str) -> ActionSetBuilder:
        self._action_set['id'] = id
        return self

    def with_spacing(self, spacing: Spacing) -> ActionSetBuilder:
        self._action_set['spacing'] = spacing.value
        return self

    def add_action(self, configure: Callable) -> ActionSetBuilder:
        from .action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._action_set['actions'].append(b.build())
        return self

    def build(self) -> dict:
        return self._action_set
