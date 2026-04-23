from __future__ import annotations
from ...enums import Spacing, InputLabelPosition, InputStyle


class InputDateBuilder:
    """Fluent builder for creating Input.Date elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.Date', 'id': ''}

    def with_id(self, id: str) -> InputDateBuilder:
        """Sets the unique identifier for the Input.Date element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputDateBuilder:
        """Sets the label displayed above the input field.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputDateBuilder:
        """Sets the placeholder text shown when the input is empty.

        Args:
            placeholder: The placeholder text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputDateBuilder:
        """Sets the initial value of the date input.

        Args:
            value: The default date value in YYYY-MM-DD format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_min(self, min: str) -> InputDateBuilder:
        """Sets the minimum allowed date.

        Args:
            min: The minimum date in YYYY-MM-DD format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['min'] = min
        return self

    def with_max(self, max: str) -> InputDateBuilder:
        """Sets the maximum allowed date.

        Args:
            max: The maximum date in YYYY-MM-DD format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['max'] = max
        return self

    def with_is_required(self, is_required: bool = True) -> InputDateBuilder:
        """Sets whether the input field is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputDateBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputDateBuilder:
        """Sets the spacing before the input field.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def with_is_visible(self, is_visible: bool) -> InputDateBuilder:
        """Sets whether the element is visible."""
        self._input['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> InputDateBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._input['separator'] = separator
        return self

    def with_height(self, height: str) -> InputDateBuilder:
        """Sets the height of the element."""
        self._input['height'] = height
        return self

    def with_fallback(self, fallback) -> InputDateBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._input['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> InputDateBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._input:
            self._input['requires'] = {}
        self._input['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> InputDateBuilder:
        """Sets right-to-left text direction."""
        self._input['rtl'] = rtl
        return self

    def with_label_position(self, position: InputLabelPosition) -> InputDateBuilder:
        """Sets the position of the label relative to the input."""
        self._input['labelPosition'] = position.value
        return self

    def with_label_width(self, width: str) -> InputDateBuilder:
        """Sets the width of the label."""
        self._input['labelWidth'] = width
        return self

    def with_input_style(self, style: InputStyle) -> InputDateBuilder:
        """Sets the visual style of the input."""
        self._input['inputStyle'] = style.value
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.Date dictionary.

        Returns:
            The configured Input.Date dictionary.
        """
        return self._input
