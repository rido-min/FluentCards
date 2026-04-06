from __future__ import annotations
from typing import Union
from ...enums import ChoiceInputStyle, Spacing


class InputChoiceSetBuilder:
    def __init__(self):
        self._input: dict = {'type': 'Input.ChoiceSet', 'id': '', 'choices': []}

    def with_id(self, id: str) -> InputChoiceSetBuilder:
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputChoiceSetBuilder:
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputChoiceSetBuilder:
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputChoiceSetBuilder:
        self._input['value'] = value
        return self

    def with_style(self, style: ChoiceInputStyle) -> InputChoiceSetBuilder:
        self._input['style'] = style.value
        return self

    def is_multi_select(self, is_multi_select: bool = True) -> InputChoiceSetBuilder:
        self._input['isMultiSelect'] = is_multi_select
        return self

    def with_wrap(self, wrap: bool = True) -> InputChoiceSetBuilder:
        self._input['wrap'] = wrap
        return self

    def with_is_required(self, is_required: bool = True) -> InputChoiceSetBuilder:
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputChoiceSetBuilder:
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputChoiceSetBuilder:
        self._input['spacing'] = spacing.value
        return self

    def add_choice(self, title_or_choice: Union[str, dict], value: str = None) -> InputChoiceSetBuilder:
        if isinstance(title_or_choice, str):
            self._input['choices'].append({'title': title_or_choice, 'value': value})
        else:
            self._input['choices'].append(title_or_choice)
        return self

    def build(self) -> dict:
        return self._input
