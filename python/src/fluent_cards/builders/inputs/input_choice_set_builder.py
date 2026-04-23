from __future__ import annotations
from typing import Union
from ...enums import ChoiceInputStyle, Spacing, InputLabelPosition, InputStyle


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

    def with_is_visible(self, is_visible: bool) -> InputChoiceSetBuilder:
        """Sets whether the element is visible."""
        self._input['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> InputChoiceSetBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._input['separator'] = separator
        return self

    def with_height(self, height: str) -> InputChoiceSetBuilder:
        """Sets the height of the element."""
        self._input['height'] = height
        return self

    def with_fallback(self, fallback) -> InputChoiceSetBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._input['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> InputChoiceSetBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._input:
            self._input['requires'] = {}
        self._input['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> InputChoiceSetBuilder:
        """Sets right-to-left text direction."""
        self._input['rtl'] = rtl
        return self

    def with_label_position(self, position: InputLabelPosition) -> InputChoiceSetBuilder:
        """Sets the position of the label relative to the input."""
        self._input['labelPosition'] = position.value
        return self

    def with_label_width(self, width: str) -> InputChoiceSetBuilder:
        """Sets the width of the label."""
        self._input['labelWidth'] = width
        return self

    def with_input_style(self, style: InputStyle) -> InputChoiceSetBuilder:
        """Sets the visual style of the input."""
        self._input['inputStyle'] = style.value
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
