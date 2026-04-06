from __future__ import annotations
from ...enums import Spacing


class InputNumberBuilder:
    """Fluent builder for creating Input.Number elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.Number', 'id': ''}

    def with_id(self, id: str) -> InputNumberBuilder:
        """Sets the unique identifier for the Input.Number element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputNumberBuilder:
        """Sets the label displayed above the input field.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputNumberBuilder:
        """Sets the placeholder text shown when the input is empty.

        Args:
            placeholder: The placeholder text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: float) -> InputNumberBuilder:
        """Sets the initial value of the number input.

        Args:
            value: The default numeric value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_min(self, min: float) -> InputNumberBuilder:
        """Sets the minimum allowed value.

        Args:
            min: The minimum numeric value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['min'] = min
        return self

    def with_max(self, max: float) -> InputNumberBuilder:
        """Sets the maximum allowed value.

        Args:
            max: The maximum numeric value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['max'] = max
        return self

    def with_is_required(self, is_required: bool = True) -> InputNumberBuilder:
        """Sets whether the input field is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputNumberBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputNumberBuilder:
        """Sets the spacing before the input field.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.Number dictionary.

        Returns:
            The configured Input.Number dictionary.
        """
        return self._input
