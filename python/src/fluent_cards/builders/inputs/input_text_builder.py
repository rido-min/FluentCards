from __future__ import annotations
from typing import Callable, TYPE_CHECKING
from ...enums import TextInputStyle, Spacing, InputLabelPosition, InputStyle

if TYPE_CHECKING:
    from ..action_builder import ActionBuilder


class InputTextBuilder:
    """Fluent builder for creating Input.Text elements."""

    def __init__(self):
        self._input: dict = {'type': 'Input.Text', 'id': ''}

    def with_id(self, id: str) -> InputTextBuilder:
        """Sets the unique identifier for the Input.Text element.

        Args:
            id: The unique identifier used when submitting form data.

        Returns:
            The builder instance for method chaining.
        """
        self._input['id'] = id
        return self

    def with_label(self, label: str) -> InputTextBuilder:
        """Sets the label displayed above the input field.

        Args:
            label: The label text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['label'] = label
        return self

    def with_placeholder(self, placeholder: str) -> InputTextBuilder:
        """Sets the placeholder text shown when the input is empty.

        Args:
            placeholder: The placeholder text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['placeholder'] = placeholder
        return self

    def with_value(self, value: str) -> InputTextBuilder:
        """Sets the initial value of the input field.

        Args:
            value: The default value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['value'] = value
        return self

    def with_max_length(self, max_length: int) -> InputTextBuilder:
        """Sets the maximum number of characters allowed.

        Args:
            max_length: The maximum character count.

        Returns:
            The builder instance for method chaining.
        """
        self._input['maxLength'] = max_length
        return self

    def with_is_multiline(self, is_multiline: bool = True) -> InputTextBuilder:
        """Sets whether the input allows multi-line text entry.

        Args:
            is_multiline: True to enable multi-line mode.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isMultiline'] = is_multiline
        return self

    def with_style(self, style: TextInputStyle) -> InputTextBuilder:
        """Sets the input style hint for the text field.

        Args:
            style: The input style (e.g. email, tel, url).

        Returns:
            The builder instance for method chaining.
        """
        self._input['style'] = style.value
        return self

    def with_regex(self, regex: str) -> InputTextBuilder:
        """Sets a regular expression pattern that the input value must match.

        Args:
            regex: The regular expression pattern.

        Returns:
            The builder instance for method chaining.
        """
        self._input['regex'] = regex
        return self

    def with_is_required(self, is_required: bool = True) -> InputTextBuilder:
        """Sets whether the input field is required.

        Args:
            is_required: True to mark the field as required.

        Returns:
            The builder instance for method chaining.
        """
        self._input['isRequired'] = is_required
        return self

    def with_error_message(self, error_message: str) -> InputTextBuilder:
        """Sets the error message shown when validation fails.

        Args:
            error_message: The validation error message text.

        Returns:
            The builder instance for method chaining.
        """
        self._input['errorMessage'] = error_message
        return self

    def with_spacing(self, spacing: Spacing) -> InputTextBuilder:
        """Sets the spacing before the input field.

        Args:
            spacing: The spacing value.

        Returns:
            The builder instance for method chaining.
        """
        self._input['spacing'] = spacing.value
        return self

    def with_is_visible(self, is_visible: bool) -> InputTextBuilder:
        """Sets whether the element is visible."""
        self._input['isVisible'] = is_visible
        return self

    def with_separator(self, separator: bool = True) -> InputTextBuilder:
        """Sets whether a separator line is drawn above the element."""
        self._input['separator'] = separator
        return self

    def with_height(self, height: str) -> InputTextBuilder:
        """Sets the height of the element."""
        self._input['height'] = height
        return self

    def with_fallback(self, fallback) -> InputTextBuilder:
        """Sets the fallback content if the element is unsupported."""
        self._input['fallback'] = fallback
        return self

    def with_requires(self, key: str, version: str) -> InputTextBuilder:
        """Sets a feature requirement for the element."""
        if 'requires' not in self._input:
            self._input['requires'] = {}
        self._input['requires'][key] = version
        return self

    def with_rtl(self, rtl: bool = True) -> InputTextBuilder:
        """Sets right-to-left text direction."""
        self._input['rtl'] = rtl
        return self

    def with_label_position(self, position: InputLabelPosition) -> InputTextBuilder:
        """Sets the position of the label relative to the input."""
        self._input['labelPosition'] = position.value
        return self

    def with_label_width(self, width: str) -> InputTextBuilder:
        """Sets the width of the label."""
        self._input['labelWidth'] = width
        return self

    def with_input_style(self, style: InputStyle) -> InputTextBuilder:
        """Sets the visual style of the input."""
        self._input['inputStyle'] = style.value
        return self

    def with_inline_action(self, configure: Callable[[ActionBuilder], None]) -> InputTextBuilder:
        """Sets an inline action button displayed inside the text input.

        Args:
            configure: A callable that receives an ActionBuilder to configure the action.

        Returns:
            The builder instance for method chaining.
        """
        from ..action_builder import ActionBuilder
        b = ActionBuilder()
        configure(b)
        self._input['inlineAction'] = b.build()
        return self

    def build(self) -> dict:
        """Builds and returns the configured Input.Text dictionary.

        Returns:
            The configured Input.Text dictionary.
        """
        return self._input
