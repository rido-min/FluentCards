from __future__ import annotations
from ...enums import Spacing, InputLabelPosition, InputStyle


class InputToggleBuilder:
    """Fluent builder for creating Input.Toggle elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.Toggle', 'id': '', 'title': ''}

    def with_id(self, id: str) -> InputToggleBuilder:
        """Sets the unique identifier for the Input.Toggle element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_title(self, title: str) -> InputToggleBuilder:
        """Sets the title text displayed next to the toggle.

        Args:
            title: The toggle label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['title'] = title
        return self

    def with_label(self, label: str) -> InputToggleBuilder:
        """Sets the label displayed above the toggle.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_value(self, value: str) -> InputToggleBuilder:
        """Sets the initial value of the toggle.

        Args:
            value: The default value ('true' or 'false', or custom valueOn/valueOff strings).

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_value_on(self, value_on: str) -> InputToggleBuilder:
        """Sets the value submitted when the toggle is on.

        Args:
            value_on: The value representing the on state.

        Returns:
            The builder instance for method chaining.
        """
        self._input['valueOn'] = value_on
        return self

    def with_value_off(self, value_off: str) -> InputToggleBuilder:
        """Sets the value submitted when the toggle is off.

        Args:
            value_off: The value representing the off state.

        Returns:
            The builder instance for method chaining.
        """
        self._input['valueOff'] = value_off
        return self

    def with_wrap(self, wrap: bool = True) -> InputToggleBuilder:
        """Sets whether the toggle title text wraps.

        Args:
            wrap: True to allow text wrapping.

        Returns:
            The builder instance for method chaining.
        """
        self._input['wrap'] = wrap
        return self

    def with_is_required(self, is_required: bool = True) -> InputToggleBuilder:
        """Sets whether the input field is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputToggleBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputToggleBuilder:
        """Sets the spacing before the toggle element.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def with_is_visible(self, is_visible: bool) -> InputToggleBuilder:
        """Sets whether the element is visible."""
        self._input['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> InputToggleBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._input['separator'] = separator
        return self

    def with_height(self, height: str) -> InputToggleBuilder:
        """Sets the height of the element."""
        self._input['height'] = height
        return self

    def with_fallback(self, fallback) -> InputToggleBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._input['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> InputToggleBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._input:
            self._input['requires'] = {}
        self._input['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> InputToggleBuilder:
        """Sets right-to-left text direction."""
        self._input['rtl'] = rtl
        return self

    def with_label_position(self, position: InputLabelPosition) -> InputToggleBuilder:
        """Sets the position of the label relative to the input."""
        self._input['labelPosition'] = position.value
        return self

    def with_label_width(self, width: str) -> InputToggleBuilder:
        """Sets the width of the label."""
        self._input['labelWidth'] = width
        return self

    def with_input_style(self, style: InputStyle) -> InputToggleBuilder:
        """Sets the visual style of the input."""
        self._input['inputStyle'] = style.value
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.Toggle dictionary.

        Returns:
            The configured Input.Toggle dictionary.
        """
        return self._input
