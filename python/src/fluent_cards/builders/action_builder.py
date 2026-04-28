from __future__ import annotations
from typing import Optional, Union
from ..enums import ActionStyle, AssociatedInputs, ActionMode


class ActionBuilder:
    """Fluent builder for creating Action elements."""

    def __init__(self):
        self._action: Optional[dict] = None
        self._data_set: bool = False
        self._teams_data_set: bool = False
        self._teams_submit_typed_set: bool = False
        self._teams_submit_raw_set: bool = False

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

    def _ensure_action_type_set(self) -> None:
        if self._action is None:
            raise ValueError(
                'No action type specified. Call open_url(), submit(), show_card(), '
                'toggle_visibility(), or execute() before setting properties.'
            )

    def with_id(self, id: str) -> ActionBuilder:
        """Sets the unique identifier for the action.

        Args:
            id: The unique identifier.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['id'] = id
        return self

    def with_title(self, title: str) -> ActionBuilder:
        """Sets the title displayed on the action button.

        Args:
            title: The button title text.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['title'] = title
        return self

    def with_icon_url(self, icon_url: str) -> ActionBuilder:
        """Sets the icon URL displayed on the action button.

        Args:
            icon_url: The URL of the icon image.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['iconUrl'] = icon_url
        return self

    def with_style(self, style: ActionStyle) -> ActionBuilder:
        """Sets the visual style of the action button.

        Args:
            style: The action style.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['style'] = style.value
        return self

    def with_is_enabled(self, is_enabled: bool) -> ActionBuilder:
        """Sets whether the action button is enabled.

        Args:
            is_enabled: False to disable the button.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['isEnabled'] = is_enabled
        return self

    def with_tooltip(self, tooltip: str) -> ActionBuilder:
        """Sets the tooltip text for the action button.

        Args:
            tooltip: The tooltip text to display on hover.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        self._action['tooltip'] = tooltip
        return self

    def with_data(self, data) -> ActionBuilder:
        """Sets the data payload for Action.Submit or Action.Execute.

        Args:
            data: The data to submit when the action is invoked.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit or Execute action, or if with_teams_data
                was already called.
        """
        self._ensure_submit_or_execute('with_data')
        if self._teams_data_set:
            raise ValueError(
                'Cannot use both with_data and with_teams_data on the same action. '
                'Use with_teams_data to combine msteams properties with custom data, '
                'or with_data for raw data.'
            )
        self._action['data'] = data
        self._data_set = True
        return self

    def with_associated_inputs(self, associated_inputs: AssociatedInputs) -> ActionBuilder:
        """Sets which inputs are submitted with this Action.Submit or Action.Execute.

        Args:
            associated_inputs: The associated inputs policy.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit or Execute action.
        """
        self._ensure_submit_or_execute('with_associated_inputs')
        self._action['associatedInputs'] = associated_inputs.value
        return self

    def with_verb(self, verb: str) -> ActionBuilder:
        """Sets the verb for an Action.Execute action.

        Args:
            verb: The verb string sent to the bot.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not an Execute action.
        """
        self._ensure_execute_only('with_verb')
        self._action['verb'] = verb
        return self

    def with_card(self, card: dict) -> ActionBuilder:
        """Sets the inline card for an Action.ShowCard action.

        Args:
            card: The Adaptive Card dictionary to show.

        Returns:
            The builder instance for method chaining.
        """
        self._ensure_action_type_set()
        if self._action.get('type') == 'Action.ShowCard':
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
        self._ensure_action_type_set()
        if self._action.get('type') == 'Action.ToggleVisibility':
            if 'targetElements' not in self._action:
                self._action['targetElements'] = []
            if is_visible is None:
                self._action['targetElements'].append(element_id)
            else:
                self._action['targetElements'].append({'elementId': element_id, 'isVisible': is_visible})
        return self

    def with_mode(self, mode: ActionMode) -> ActionBuilder:
        """Sets whether the action is primary or secondary."""
        self._ensure_action_type_set()
        self._action['mode'] = mode.value
        return self

    def with_requires(self, key: str, version: str) -> ActionBuilder:
        """Sets a feature requirement for the action."""
        self._ensure_action_type_set()
        if 'requires' not in self._action:
            self._action['requires'] = {}
        self._action['requires'][key] = version
        return self

    def with_fallback(self, fallback) -> ActionBuilder:
        """Sets the fallback content if the action is unsupported."""
        self._ensure_action_type_set()
        self._action['fallback'] = fallback
        return self

    # ── Teams-specific methods (Submit-only) ────────────────────────────────

    def _ensure_submit_or_execute(self, method_name: str) -> None:
        self._ensure_action_type_set()
        if self._action.get('type') not in ('Action.Submit', 'Action.Execute'):
            raise ValueError(
                f'{method_name}() is only available on Submit or Execute actions. '
                'Call submit() or execute() before using this method.'
            )

    def _ensure_execute_only(self, method_name: str) -> None:
        self._ensure_action_type_set()
        if self._action.get('type') != 'Action.Execute':
            raise ValueError(
                f'{method_name}() is only available on Execute actions. '
                'Call execute() before using this method.'
            )

    def _ensure_submit_only(self, method_name: str) -> None:
        self._ensure_action_type_set()
        if self._action.get('type') != 'Action.Submit':
            raise ValueError(
                f'{method_name} is only available on Submit actions. '
                'Call submit() before using this method.'
            )

    def _ensure_no_data_conflict(self) -> None:
        if self._data_set:
            raise ValueError(
                'Cannot use both with_data and with_teams_data on the same action. '
                'Use with_teams_data to combine msteams properties with custom data, '
                'or with_data for raw data.'
            )

    def with_teams_task_fetch(self) -> ActionBuilder:
        """Sets the action data to ``{'msteams': {'type': 'task/fetch'}}`` (Submit-only).

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit action or if with_data was already called.
        """
        from .teams_data_builder import TeamsDataBuilder
        self._ensure_submit_only('with_teams_task_fetch')
        self._ensure_no_data_conflict()
        b = TeamsDataBuilder()
        b.with_task_fetch()
        self._action['data'] = b.build()
        self._teams_data_set = True
        return self

    def with_teams_data(self, configure) -> ActionBuilder:
        """Configures a Teams-specific data payload (Submit-only).

        Args:
            configure: A callable that receives a TeamsDataBuilder.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit action or if with_data was already called.
        """
        from .teams_data_builder import TeamsDataBuilder
        self._ensure_submit_only('with_teams_data')
        self._ensure_no_data_conflict()
        b = TeamsDataBuilder()
        configure(b)
        self._action['data'] = b.build()
        self._teams_data_set = True
        return self

    def with_teams_submit_feedback(self, configure) -> ActionBuilder:
        """Configures Teams submit feedback properties (Submit-only).

        Args:
            configure: A callable that receives a TeamsSubmitPropertiesBuilder.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit action or if with_teams_submit_raw was called.
        """
        from .teams_submit_properties_builder import TeamsSubmitPropertiesBuilder
        self._ensure_submit_only('with_teams_submit_feedback')
        if self._teams_submit_raw_set:
            raise ValueError(
                'Cannot use both with_teams_submit_feedback and with_teams_submit_raw '
                'on the same action. Use one or the other.'
            )
        b = TeamsSubmitPropertiesBuilder()
        configure(b)
        self._action['msteams'] = b.build()
        self._teams_submit_typed_set = True
        return self

    def with_teams_submit_raw(self, value: dict) -> ActionBuilder:
        """Sets the Teams action-level msteams property from a raw dict (Submit-only).

        Args:
            value: The raw msteams properties dictionary.

        Returns:
            The builder instance for method chaining.

        Raises:
            ValueError: If not a Submit action or if with_teams_submit_feedback was called.
        """
        self._ensure_submit_only('with_teams_submit_raw')
        if self._teams_submit_typed_set:
            raise ValueError(
                'Cannot use both with_teams_submit_feedback and with_teams_submit_raw '
                'on the same action. Use one or the other.'
            )
        self._action['msteams'] = dict(value)
        self._teams_submit_raw_set = True
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
