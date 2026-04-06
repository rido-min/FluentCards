from __future__ import annotations
from ...enums import Spacing


class InputNumberBuilder:
    def __init__(self):
        self._input: dict = {'type': 'Input.Number', 'id': ''}

    def with_id(self, id: str) -> InputNumberBuilder:
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputNumberBuilder:
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputNumberBuilder:
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: float) -> InputNumberBuilder:
        self._input['value'] = value
        return self

    def with_min(self, min: float) -> InputNumberBuilder:
        self._input['min'] = min
        return self

    def with_max(self, max: float) -> InputNumberBuilder:
        self._input['max'] = max
        return self

    def with_is_required(self, is_required: bool = True) -> InputNumberBuilder:
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputNumberBuilder:
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputNumberBuilder:
        self._input['spacing'] = spacing.value
        return self

    def build(self) -> dict:
        return self._input
