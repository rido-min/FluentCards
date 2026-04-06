from __future__ import annotations
from typing import Optional, Union
from ..enums import ActionStyle, AssociatedInputs


class ActionBuilder:
    def __init__(self):
        self._action: Optional[dict] = None

    def open_url(self, url: str, title: Optional[str] = None) -> ActionBuilder:
        self._action = {'type': 'Action.OpenUrl', 'url': url, 'title': title}
        return self

    def submit(self, title: Optional[str] = None) -> ActionBuilder:
        self._action = {'type': 'Action.Submit', 'title': title}
        return self

    def show_card(self, title: Optional[str] = None) -> ActionBuilder:
        self._action = {'type': 'Action.ShowCard', 'title': title}
        return self

    def toggle_visibility(self, title: Optional[str] = None) -> ActionBuilder:
        self._action = {'type': 'Action.ToggleVisibility', 'title': title}
        return self

    def execute(self, title: Optional[str] = None) -> ActionBuilder:
        self._action = {'type': 'Action.Execute', 'title': title}
        return self

    def with_id(self, id: str) -> ActionBuilder:
        if self._action is not None:
            self._action['id'] = id
        return self

    def with_title(self, title: str) -> ActionBuilder:
        if self._action is not None:
            self._action['title'] = title
        return self

    def with_icon_url(self, icon_url: str) -> ActionBuilder:
        if self._action is not None:
            self._action['iconUrl'] = icon_url
        return self

    def with_style(self, style: ActionStyle) -> ActionBuilder:
        if self._action is not None:
            self._action['style'] = style.value
        return self

    def with_is_enabled(self, is_enabled: bool) -> ActionBuilder:
        if self._action is not None:
            self._action['isEnabled'] = is_enabled
        return self

    def with_tooltip(self, tooltip: str) -> ActionBuilder:
        if self._action is not None:
            self._action['tooltip'] = tooltip
        return self

    def with_data(self, data) -> ActionBuilder:
        if self._action is not None and self._action.get('type') in ('Action.Submit', 'Action.Execute'):
            self._action['data'] = data
        return self

    def with_associated_inputs(self, associated_inputs: AssociatedInputs) -> ActionBuilder:
        if self._action is not None and self._action.get('type') in ('Action.Submit', 'Action.Execute'):
            self._action['associatedInputs'] = associated_inputs.value
        return self

    def with_verb(self, verb: str) -> ActionBuilder:
        if self._action is not None and self._action.get('type') == 'Action.Execute':
            self._action['verb'] = verb
        return self

    def with_card(self, card: dict) -> ActionBuilder:
        if self._action is not None and self._action.get('type') == 'Action.ShowCard':
            self._action['card'] = card
        return self

    def add_target_element(self, element_id: str, is_visible: Optional[bool] = None) -> ActionBuilder:
        if self._action is not None and self._action.get('type') == 'Action.ToggleVisibility':
            if 'targetElements' not in self._action:
                self._action['targetElements'] = []
            if is_visible is None:
                self._action['targetElements'].append(element_id)
            else:
                self._action['targetElements'].append({'elementId': element_id, 'isVisible': is_visible})
        return self

    def build(self) -> dict:
        if self._action is None:
            raise ValueError(
                'No action type specified. Call open_url(), submit(), show_card(), '
                'toggle_visibility(), or execute() first.'
            )
        return self._action
