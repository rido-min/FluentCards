from __future__ import annotations
from typing import Optional, Union
from ..enums import ActionStyle, AssociatedInputs


class ActionBuilder:
    """Fluent builder for creating Action elements."""

    def __init__(self):
        self._action: Optional[dict] = None

    def open_url(self, url: str, title: Optional[str] = None) -> ActionBuilder:
        """Sets the action type to Action.OpenUrl and specifies the URL to open.

        Args:
            url: The URL to open when the action is invoked.
            title: Optional title displayed on the action button.

        Returns:
            The builder instance for method chaining.
        """
        self._action = {'type': 'Action.OpenUrl', 'url': url, 'title': title}
        return self

    def submit(self, title: Optional[str] = None) -> ActionBuilder:
        """Sets the action type to Action.Submit.

        Args:
            title: Optional title displayed on the action button.

        Returns:
            The builder instance for method chaining.
        """
        self._action = {'type': 'Action.Submit', 'title': title}
        return self

    def show_card(self, title: Optional[str] = None) -> ActionBuilder:
        """Sets the action type to Action.ShowCard.

        Args:
            title: Optional title displayed on the action button.

        Returns:
            The builder instance for method chaining.
        """
        self._action = {'type': 'Action.ShowCard', 'title': title}
        return self

    def toggle_visibility(self, title: Optional[str] = None) -> ActionBuilder:
        """Sets the action type to Action.ToggleVisibility.

        Args:
            title: Optional title displayed on the action button.

        Returns:
            The builder instance for method chaining.
        """
        self._action = {'type': 'Action.ToggleVisibility', 'title': title}
        return self

    def execute(self, title: Optional[str] = None) -> ActionBuilder:
        """Sets the action type to Action.Execute.

        Args:
            title: Optional title displayed on the action button.

        Returns:
            The builder instance for method chaining.
        """
        self._action = {'type': 'Action.Execute', 'title': title}
        return self

    def with_id(self, id: str) -> ActionBuilder:
        """Sets the unique identifier for the action.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['id'] = id
        return self

    def with_title(self, title: str) -> ActionBuilder:
        """Sets the title displayed on the action button.

        Args:
            title: The button title text.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['title'] = title
        return self

    def with_icon_url(self, icon_url: str) -> ActionBuilder:
        """Sets the icon URL displayed on the action button.

        Args:
            icon_url: The URL of the icon image.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['iconUrl'] = icon_url
        return self

    def with_style(self, style: ActionStyle) -> ActionBuilder:
        """Sets the visual style of the action button.

        Args:
            style: The action style.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['style'] = style.value
        return self

    def with_is_enabled(self, is_enabled: bool) -> ActionBuilder:
        """Sets whether the action button is enabled.

        Args:
            is_enabled: False to disable the button.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['isEnabled'] = is_enabled
        return self

    def with_tooltip(self, tooltip: str) -> ActionBuilder:
        """Sets the tooltip text for the action button.

        Args:
            tooltip: The tooltip text to display on hover.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None:
            self._action['tooltip'] = tooltip
        return self

    def with_data(self, data) -> ActionBuilder:
        """Sets the data payload for Action.Submit or Action.Execute.

        Args:
            data: The data to submit when the action is invoked.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None and self._action.get('type') in ('Action.Submit', 'Action.Execute'):
            self._action['data'] = data
        return self

    def with_associated_inputs(self, associated_inputs: AssociatedInputs) -> ActionBuilder:
        """Sets which inputs are submitted with this Action.Submit or Action.Execute.

        Args:
            associated_inputs: The associated inputs policy.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None and self._action.get('type') in ('Action.Submit', 'Action.Execute'):
            self._action['associatedInputs'] = associated_inputs.value
        return self

    def with_verb(self, verb: str) -> ActionBuilder:
        """Sets the verb for an Action.Execute action.

        Args:
            verb: The verb string sent to the bot.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None and self._action.get('type') == 'Action.Execute':
            self._action['verb'] = verb
        return self

    def with_card(self, card: dict) -> ActionBuilder:
        """Sets the inline card for an Action.ShowCard action.

        Args:
            card: The Adaptive Card dictionary to show.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None and self._action.get('type') == 'Action.ShowCard':
            self._action['card'] = card
        return self

    def add_target_element(self, element_id: str, is_visible: Optional[bool] = None) -> ActionBuilder:
        """Adds a target element for an Action.ToggleVisibility action.

        Args:
            element_id: The ID of the element to toggle.
            is_visible: Optional explicit visibility state to set. If None, toggles the current state.

        Returns:
            The builder instance for method chaining.
        """
        if self._action is not None and self._action.get('type') == 'Action.ToggleVisibility':
            if 'targetElements' not in self._action:
                self._action['targetElements'] = []
            if is_visible is None:
                self._action['targetElements'].append(element_id)
            else:
                self._action['targetElements'].append({'elementId': element_id, 'isVisible': is_visible})
        return self

    def build(self) -> dict:
        """Builds and returns the configured action dictionary.

        Returns:
            The configured action dictionary.

        Raises:
            ValueError: If no action type has been set.
        """
        if self._action is None:
            raise ValueError(
                'No action type specified. Call open_url(), submit(), show_card(), '
                'toggle_visibility(), or execute() first.'
            )
        return self._action
