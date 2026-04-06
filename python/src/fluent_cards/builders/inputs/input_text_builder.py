from __future__ import annotations
from typing import Callable
from ...enums import TextInputStyle, Spacing


class InputTextBuilder:
    def __init__(self):
        self._input: dict = {'type': 'Input.Text', 'id': ''}

    def with_id(self, id: str) -> InputTextBuilder:
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputTextBuilder:
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputTextBuilder:
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputTextBuilder:
        self._input['value'] = value
        return self

    def with_max_length(self, max_length: int) -> InputTextBuilder:
        self._input['maxLength'] = max_length
        return self

    def with_is_multiline(self, is_multiline: bool = True) -> InputTextBuilder:
        self._input['isMultiline'] = is_multiline
        return self

    def with_style(self, style: TextInputStyle) -> InputTextBuilder:
        self._input['style'] = style.value
        return self

    def with_regex(self, regex: str) -> InputTextBuilder:
        self._input['regex'] = regex
        return self

    def with_is_required(self, is_required: bool = True) -> InputTextBuilder:
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputTextBuilder:
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputTextBuilder:
        self._input['spacing'] = spacing.value
        return self

    def with_inline_action(self, configure: Callable) -> InputTextBuilder:
        from ..action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._input['inlineAction'] = b.build()
        return self

    def build(self) -> dict:
        return self._input
