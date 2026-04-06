from __future__ import annotations
from ...enums import Spacing


class InputTimeBuilder:
    def __init__(self):
        self._input: dict = {'type': 'Input.Time', 'id': ''}

    def with_id(self, id: str) -> InputTimeBuilder:
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputTimeBuilder:
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputTimeBuilder:
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputTimeBuilder:
        self._input['value'] = value
        return self

    def with_min(self, min: str) -> InputTimeBuilder:
        self._input['min'] = min
        return self

    def with_max(self, max: str) -> InputTimeBuilder:
        self._input['max'] = max
        return self

    def with_is_required(self, is_required: bool = True) -> InputTimeBuilder:
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputTimeBuilder:
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputTimeBuilder:
        self._input['spacing'] = spacing.value
        return self

    def build(self) -> dict:
        return self._input
