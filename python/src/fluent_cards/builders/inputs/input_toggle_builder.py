from __future__ import annotations
from ...enums import Spacing


class InputToggleBuilder:
    def __init__(self):
        self._input: dict = {'type': 'Input.Toggle', 'id': '', 'title': ''}

    def with_id(self, id: str) -> InputToggleBuilder:
        self._input['id'] = id
        return self

    def with_title(self, title: str) -> InputToggleBuilder:
        self._input['title'] = title
        return self

    def with_label(self, label: str) -> InputToggleBuilder:
        self._input['label'] = label
        return self

    def with_value(self, value: str) -> InputToggleBuilder:
        self._input['value'] = value
        return self

    def with_value_on(self, value_on: str) -> InputToggleBuilder:
        self._input['valueOn'] = value_on
        return self

    def with_value_off(self, value_off: str) -> InputToggleBuilder:
        self._input['valueOff'] = value_off
        return self

    def with_wrap(self, wrap: bool = True) -> InputToggleBuilder:
        self._input['wrap'] = wrap
        return self

    def with_is_required(self, is_required: bool = True) -> InputToggleBuilder:
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputToggleBuilder:
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputToggleBuilder:
        self._input['spacing'] = spacing.value
        return self

    def build(self) -> dict:
        return self._input
