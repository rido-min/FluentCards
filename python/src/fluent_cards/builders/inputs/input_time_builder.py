from __future__ import annotations
from ...enums import Spacing, InputLabelPosition, InputStyle


class InputTimeBuilder:
    """Fluent builder for creating Input.Time elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.Time', 'id': ''}

    def with_id(self, id: str) -> InputTimeBuilder:
        """Sets the unique identifier for the Input.Time element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputTimeBuilder:
        """Sets the label displayed above the input field.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputTimeBuilder:
        """Sets the placeholder text shown when the input is empty.

        Args:
            placeholder: The placeholder text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputTimeBuilder:
        """Sets the initial value of the time input.

        Args:
            value: The default time value in HH:MM format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_min(self, min: str) -> InputTimeBuilder:
        """Sets the minimum allowed time.

        Args:
            min: The minimum time in HH:MM format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['min'] = min
        return self

    def with_max(self, max: str) -> InputTimeBuilder:
        """Sets the maximum allowed time.

        Args:
            max: The maximum time in HH:MM format.

        Returns:
            The builder instance for method chaining.
        """
        self._input['max'] = max
        return self

    def with_is_required(self, is_required: bool = True) -> InputTimeBuilder:
        """Sets whether the input field is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputTimeBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputTimeBuilder:
        """Sets the spacing before the input field.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def with_is_visible(self, is_visible: bool) -> InputTimeBuilder:
        """Sets whether the element is visible."""
        self._input['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> InputTimeBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._input['separator'] = separator
        return self

    def with_height(self, height: str) -> InputTimeBuilder:
        """Sets the height of the element."""
        self._input['height'] = height
        return self

    def with_fallback(self, fallback) -> InputTimeBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._input['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> InputTimeBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._input:
            self._input['requires'] = {}
        self._input['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> InputTimeBuilder:
        """Sets right-to-left text direction."""
        self._input['rtl'] = rtl
        return self

    def with_label_position(self, position: InputLabelPosition) -> InputTimeBuilder:
        """Sets the position of the label relative to the input."""
        self._input['labelPosition'] = position.value
        return self

    def with_label_width(self, width: str) -> InputTimeBuilder:
        """Sets the width of the label."""
        self._input['labelWidth'] = width
        return self

    def with_input_style(self, style: InputStyle) -> InputTimeBuilder:
        """Sets the visual style of the input."""
        self._input['inputStyle'] = style.value
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.Time dictionary.

        Returns:
            The configured Input.Time dictionary.
        """
        return self._input
