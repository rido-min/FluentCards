from __future__ import annotations
from typing import Union
from ...enums import ChoiceInputStyle, Spacing


class InputChoiceSetBuilder:
    """Fluent builder for creating Input.ChoiceSet elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.ChoiceSet', 'id': '', 'choices': []}

    def with_id(self, id: str) -> InputChoiceSetBuilder:
        """Sets the unique identifier for the Input.ChoiceSet element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputChoiceSetBuilder:
        """Sets the label displayed above the choice set.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputChoiceSetBuilder:
        """Sets the placeholder text shown in compact style when no choice is selected.

        Args:
            placeholder: The placeholder text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputChoiceSetBuilder:
        """Sets the initially selected value.

        Args:
            value: The default selected choice value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_style(self, style: ChoiceInputStyle) -> InputChoiceSetBuilder:
        """Sets the display style of the choice set.

        Args:
            style: The choice input style (compact, expanded, or filtered).

        Returns:
            The builder instance for method chaining.
        """
        self._input['style'] = style.value
        return self

    def is_multi_select(self, is_multi_select: bool = True) -> InputChoiceSetBuilder:
        """Sets whether multiple choices can be selected.

        Args:
            is_multi_select: True to allow multiple selections.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isMultiSelect'] = is_multi_select
        return self

    def with_wrap(self, wrap: bool = True) -> InputChoiceSetBuilder:
        """Sets whether choice text wraps in expanded style.

        Args:
            wrap: True to allow text wrapping.

        Returns:
            The builder instance for method chaining.
        """
        self._input['wrap'] = wrap
        return self

    def with_is_required(self, is_required: bool = True) -> InputChoiceSetBuilder:
        """Sets whether a selection is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputChoiceSetBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputChoiceSetBuilder:
        """Sets the spacing before the choice set.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def add_choice(self, title_or_choice: Union[str, dict], value: str = None) -> InputChoiceSetBuilder:
        """Adds a choice to the choice set.

        Args:
            title_or_choice: The display title string, or a pre-built choice dictionary.
            value: The submitted value for the choice, required when title_or_choice is a string.

        Returns:
            The builder instance for method chaining.
        """
        if isinstance(title_or_choice, str):
            self._input['choices'].append({'title': title_or_choice, 'value': value})
        else:
            self._input['choices'].append(title_or_choice)
        return self

    def with_choices_data(self, dataset: str) -> InputChoiceSetBuilder:
        """Sets a dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+).

        Args:
            dataset: The dataset identifier, e.g. 'graph.microsoft.com/users'.

        Returns:
            The builder instance for method chaining.
        """
        self._input['choices.data'] = {'type': 'Data.Query', 'dataset': dataset}
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.ChoiceSet dictionary.

        Returns:
            The configured Input.ChoiceSet dictionary.
        """
        return self._input
