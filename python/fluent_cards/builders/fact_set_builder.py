from __future__ import annotations
from typing import Union
from ..enums import Spacing


class FactSetBuilder:
    def __init__(self):
        self._fact_set: dict = {'type': 'FactSet', 'facts': []}

    def with_id(self, id: str) -> FactSetBuilder:
        self._fact_set['id'] = id
        return self

    def with_spacing(self, spacing: Spacing) -> FactSetBuilder:
        self._fact_set['spacing'] = spacing.value
        return self

    def add_fact(self, title_or_fact: Union[str, dict], value: str = None) -> FactSetBuilder:
        if isinstance(title_or_fact, str):
            self._fact_set['facts'].append({'title': title_or_fact, 'value': value})
        else:
            self._fact_set['facts'].append(title_or_fact)
        return self

    def build(self) -> dict:
        return self._fact_set
