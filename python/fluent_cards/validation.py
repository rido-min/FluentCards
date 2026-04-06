from __future__ import annotations
from dataclasses import dataclass
from typing import Optional
from urllib.parse import urlparse

from .enums import ValidationSeverity


@dataclass(frozen=True)
class ValidationIssue:
    """Describes a single validation issue found in an Adaptive Card."""

    severity: ValidationSeverity
    path: str
    code: str
    message: str


class AdaptiveCardValidationError(Exception):
    """Raised by validate_and_throw when an Adaptive Card contains validation errors."""

    def __init__(self, errors: list[ValidationIssue]):
        self.errors = errors
        self.message = _format_message(errors)
        super().__init__(self.message)


def _format_message(errors: list[ValidationIssue]) -> str:
    if len(errors) == 1:
        return f'Adaptive Card validation failed: {errors[0].message}'
    lines = '\n'.join(f'  - [{e.path}] {e.message}' for e in errors)
    return f'Adaptive Card validation failed with {len(errors)} errors:\n{lines}'


KNOWN_VERSIONS = {'1.0', '1.1', '1.2', '1.3', '1.4', '1.5', '1.6'}


def validate(card: dict) -> list[ValidationIssue]:
    """Validates an Adaptive Card and returns a list of validation issues.

    Args:
        card: The Adaptive Card dictionary to validate.

    Returns:
        A list of ValidationIssue instances describing any problems found.
    """
    issues: list[ValidationIssue] = []
    ids: set[str] = set()
    _validate_card(card, issues, ids)
    version = card.get('version')
    if version and version in KNOWN_VERSIONS:
        _validate_version_mismatch(card, version, issues)
    return issues


def validate_and_throw(card: dict) -> None:
    """Validates an Adaptive Card and raises AdaptiveCardValidationError if errors are found.

    Args:
        card: The Adaptive Card dictionary to validate.

    Raises:
        AdaptiveCardValidationError: If any validation issues with severity Error are found.
    """
    errors = [i for i in validate(card) if i.severity == ValidationSeverity.Error]
    if errors:
        raise AdaptiveCardValidationError(errors)


def _issue(issues, severity, path, code, message):
    issues.append(ValidationIssue(severity=severity, path=path, code=code, message=message))


def _track_id(id_val, path, issues, ids):
    if not id_val:
        return
    if id_val in ids:
        _issue(issues, ValidationSeverity.Warning, path, 'DUPLICATE_ID',
               f"Duplicate id '{id_val}' found. Element IDs should be unique within a card.")
    else:
        ids.add(id_val)


def _is_absolute_url(url: str) -> bool:
    try:
        result = urlparse(url)
        return bool(result.scheme and result.netloc)
    except Exception:
        return False


def _validate_card(card, issues, ids):
    if not card.get('$schema'):
        _issue(issues, ValidationSeverity.Warning, '$schema', 'MISSING_SCHEMA',
               "The '$schema' property is missing. While optional, including it enables better tooling support.")
    version = card.get('version')
    if not version:
        _issue(issues, ValidationSeverity.Error, 'version', 'MISSING_VERSION',
               "The 'version' property is required. Use a value like '1.5' to specify the schema version.")
    elif version not in KNOWN_VERSIONS:
        known = ', '.join(sorted(KNOWN_VERSIONS))
        _issue(issues, ValidationSeverity.Warning, 'version', 'UNKNOWN_VERSION',
               f"The version '{version}' is not a known Adaptive Cards version. Known versions: {known}.")

    body = card.get('body')
    actions = card.get('actions')
    if not body and not actions:
        _issue(issues, ValidationSeverity.Warning, '', 'EMPTY_CARD',
               'The card has no body elements and no actions. It will render as empty.')

    if body:
        _validate_elements(body, issues, 'body', ids)

    if actions:
        _validate_actions(actions, issues, 'actions', ids)
        if len(actions) > 5:
            _issue(issues, ValidationSeverity.Warning, 'actions', 'TOO_MANY_ACTIONS',
                   f'The card has {len(actions)} actions. Some hosts limit the number of visible actions to 5.')

    _validate_select_action(card.get('selectAction'), issues, 'selectAction')


def _validate_elements(elements, issues, path, ids):
    for i, element in enumerate(elements):
        element_path = f'{path}[{i}]'
        _track_id(element.get('id'), element_path, issues, ids)
        _validate_element(element, issues, element_path, ids)


def _validate_element(element, issues, path, ids):
    t = element.get('type')
    if t == 'TextBlock':
        if not element.get('text'):
            _issue(issues, ValidationSeverity.Error, f'{path}.text', 'MISSING_TEXT',
                   "TextBlock is missing the required 'text' property.")
    elif t == 'Image':
        url = element.get('url')
        if not url:
            _issue(issues, ValidationSeverity.Error, f'{path}.url', 'MISSING_IMAGE_URL',
                   "Image element is missing the required 'url' property.")
        elif not _is_absolute_url(url):
            _issue(issues, ValidationSeverity.Warning, f'{path}.url', 'INVALID_IMAGE_URL',
                   f"Image URL '{url}' is not a valid absolute URL.")
        _validate_select_action(element.get('selectAction'), issues, f'{path}.selectAction')
    elif t == 'ImageSet':
        images = element.get('images')
        if not images:
            _issue(issues, ValidationSeverity.Error, f'{path}.images', 'MISSING_IMAGES',
                   "ImageSet is missing the required 'images' property.")
        else:
            for i, img in enumerate(images):
                if not img.get('url'):
                    _issue(issues, ValidationSeverity.Error, f'{path}.images[{i}].url', 'MISSING_IMAGE_URL',
                           "Image element is missing the required 'url' property.")
    elif t == 'FactSet':
        if not element.get('facts'):
            _issue(issues, ValidationSeverity.Error, f'{path}.facts', 'MISSING_FACTS',
                   "FactSet is missing the required 'facts' property.")
    elif t == 'ActionSet':
        actions = element.get('actions')
        if not actions:
            _issue(issues, ValidationSeverity.Error, f'{path}.actions', 'MISSING_ACTIONSET_ACTIONS',
                   "ActionSet is missing the required 'actions' property.")
        else:
            _validate_actions(actions, issues, f'{path}.actions', ids)
    elif t == 'RichTextBlock':
        if not element.get('inlines'):
            _issue(issues, ValidationSeverity.Error, f'{path}.inlines', 'MISSING_INLINES',
                   "RichTextBlock is missing the required 'inlines' property.")
    elif t == 'Media':
        if not element.get('sources'):
            _issue(issues, ValidationSeverity.Error, f'{path}.sources', 'MISSING_MEDIA_SOURCES',
                   "Media is missing the required 'sources' property.")
    elif t in ('Input.Text', 'Input.Number', 'Input.Date', 'Input.Time', 'Input.Toggle', 'Input.ChoiceSet'):
        id_val = element.get('id')
        if not id_val:
            _issue(issues, ValidationSeverity.Error, f'{path}.id', 'MISSING_INPUT_ID',
                   "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.")
        else:
            _track_id(id_val, path, issues, ids)
        _validate_input_element(element, issues, path)
    elif t == 'Container':
        items = element.get('items')
        if not items:
            _issue(issues, ValidationSeverity.Warning, f'{path}.items', 'EMPTY_CONTAINER',
                   'Container has no items. It will render as empty.')
        else:
            _validate_elements(items, issues, f'{path}.items', ids)
        _validate_select_action(element.get('selectAction'), issues, f'{path}.selectAction')
    elif t == 'ColumnSet':
        columns = element.get('columns') or []
        for i, col in enumerate(columns):
            col_path = f'{path}.columns[{i}]'
            _track_id(col.get('id'), col_path, issues, ids)
            if col.get('items'):
                _validate_elements(col['items'], issues, f'{col_path}.items', ids)
            _validate_select_action(col.get('selectAction'), issues, f'{col_path}.selectAction')
        _validate_select_action(element.get('selectAction'), issues, f'{path}.selectAction')
    elif t == 'Table':
        rows = element.get('rows') or []
        for r, row in enumerate(rows):
            cells = row.get('cells') or []
            for c, cell in enumerate(cells):
                if cell.get('items'):
                    _validate_elements(cell['items'], issues, f'{path}.rows[{r}].cells[{c}].items', ids)
                _validate_select_action(cell.get('selectAction'), issues,
                                        f'{path}.rows[{r}].cells[{c}].selectAction')


def _validate_input_element(element, issues, path):
    t = element.get('type')
    if t == 'Input.Number':
        min_val = element.get('min')
        max_val = element.get('max')
        if min_val is not None and max_val is not None and min_val > max_val:
            _issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
                   f"Input.Number 'min' ({min_val}) is greater than 'max' ({max_val}).")
    elif t == 'Input.Date':
        min_val = element.get('min')
        max_val = element.get('max')
        if min_val and max_val and min_val > max_val:
            _issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
                   f"Input.Date 'min' ({min_val}) is greater than 'max' ({max_val}).")
    elif t == 'Input.Time':
        min_val = element.get('min')
        max_val = element.get('max')
        if min_val and max_val and min_val > max_val:
            _issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
                   f"Input.Time 'min' ({min_val}) is greater than 'max' ({max_val}).")
    elif t == 'Input.Toggle':
        if not element.get('title'):
            _issue(issues, ValidationSeverity.Error, f'{path}.title', 'MISSING_TOGGLE_TITLE',
                   "Input.Toggle is missing the required 'title' property.")


def _validate_select_action(action, issues, path):
    if not action:
        return
    if action.get('type') == 'Action.ShowCard':
        _issue(issues, ValidationSeverity.Error, path, 'INVALID_SELECT_ACTION',
               'Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, '
               'Action.Execute, or Action.ToggleVisibility.')


def _validate_actions(actions, issues, path, ids):
    for i, action in enumerate(actions):
        action_path = f'{path}[{i}]'
        _track_id(action.get('id'), action_path, issues, ids)
        _validate_action(action, issues, action_path, ids)


def _validate_action(action, issues, path, ids):
    t = action.get('type')
    if t == 'Action.OpenUrl':
        url = action.get('url')
        if not url:
            _issue(issues, ValidationSeverity.Error, f'{path}.url', 'MISSING_ACTION_URL',
                   "Action.OpenUrl is missing the required 'url' property.")
        elif not _is_absolute_url(url):
            _issue(issues, ValidationSeverity.Warning, f'{path}.url', 'INVALID_ACTION_URL',
                   f"Action.OpenUrl URL '{url}' is not a valid absolute URL.")
    elif t == 'Action.ShowCard':
        card = action.get('card')
        if not card:
            _issue(issues, ValidationSeverity.Error, f'{path}.card', 'MISSING_SHOWCARD',
                   "Action.ShowCard is missing the required 'card' property.")
        else:
            _validate_card(card, issues, ids)
    elif t == 'Action.ToggleVisibility':
        if not action.get('targetElements'):
            _issue(issues, ValidationSeverity.Error, f'{path}.targetElements', 'MISSING_TARGET_ELEMENTS',
                   "Action.ToggleVisibility is missing the required 'targetElements' property.")


# Version-aware validation
ELEMENT_VERSIONS: dict[str, str] = {
    'TextBlock': '1.0', 'Image': '1.0', 'Container': '1.0', 'ColumnSet': '1.0',
    'FactSet': '1.0', 'ImageSet': '1.0', 'Column': '1.0', 'Fact': '1.0', 'Choice': '1.0',
    'Action.OpenUrl': '1.0', 'Action.Submit': '1.0', 'Action.ShowCard': '1.0',
    'Input.Text': '1.0', 'Input.Number': '1.0', 'Input.Date': '1.0',
    'Input.Time': '1.0', 'Input.Toggle': '1.0', 'Input.ChoiceSet': '1.0',
    'Media': '1.1',
    'RichTextBlock': '1.2', 'ActionSet': '1.2', 'Action.ToggleVisibility': '1.2',
    'Action.Execute': '1.4',
    'Table': '1.5',
}

CARD_PROPERTY_VERSIONS: dict[str, str] = {
    'selectAction': '1.1',
    'minHeight': '1.2', 'verticalContentAlignment': '1.2', 'backgroundImage': '1.2',
    'refresh': '1.4', 'authentication': '1.4',
    'rtl': '1.5',
    'metadata': '1.6',
}


def _version_to_num(v: str) -> int:
    parts = v.split('.')
    return int(parts[1]) if len(parts) > 1 else 0


def _version_mismatch(issues, path, feature_name, required_version, card_version):
    _issue(issues, ValidationSeverity.Warning, path, 'VERSION_MISMATCH',
           f"'{feature_name}' requires Adaptive Cards {required_version} but card version is {card_version}.")


def _check_element_version(type_str, card_version, issues, path):
    required = ELEMENT_VERSIONS.get(type_str)
    if required and _version_to_num(required) > _version_to_num(card_version):
        _version_mismatch(issues, path, type_str, required, card_version)


def _check_card_property_version(prop, card_version, issues):
    required = CARD_PROPERTY_VERSIONS.get(prop)
    if required and _version_to_num(required) > _version_to_num(card_version):
        _version_mismatch(issues, prop, prop, required, card_version)


def _validate_version_mismatch(card, card_version, issues):
    for prop in ('selectAction', 'minHeight', 'verticalContentAlignment', 'backgroundImage',
                 'refresh', 'authentication', 'metadata'):
        if card.get(prop):
            _check_card_property_version(prop, card_version, issues)
    if card.get('rtl') is not None:
        _check_card_property_version('rtl', card_version, issues)

    body = card.get('body')
    if body:
        _check_element_versions_in_list(body, card_version, issues, 'body')
    actions = card.get('actions')
    if actions:
        _check_action_versions_in_list(actions, card_version, issues, 'actions')


def _check_element_versions_in_list(elements, card_version, issues, path):
    for i, el in enumerate(elements):
        p = f'{path}[{i}]'
        _check_element_version(el.get('type', ''), card_version, issues, p)
        t = el.get('type')
        if t == 'Container' and el.get('items'):
            _check_element_versions_in_list(el['items'], card_version, issues, f'{p}.items')
        elif t == 'ColumnSet':
            for ci, col in enumerate(el.get('columns') or []):
                if col.get('items'):
                    _check_element_versions_in_list(col['items'], card_version, issues,
                                                    f'{p}.columns[{ci}].items')
        elif t == 'ActionSet' and el.get('actions'):
            _check_action_versions_in_list(el['actions'], card_version, issues, f'{p}.actions')
        elif t == 'Table':
            for r, row in enumerate(el.get('rows') or []):
                for c, cell in enumerate(row.get('cells') or []):
                    if cell.get('items'):
                        _check_element_versions_in_list(cell['items'], card_version, issues,
                                                        f'{p}.rows[{r}].cells[{c}].items')


def _check_action_versions_in_list(actions, card_version, issues, path):
    for i, action in enumerate(actions):
        p = f'{path}[{i}]'
        _check_element_version(action.get('type', ''), card_version, issues, p)
        if action.get('type') == 'Action.ShowCard' and action.get('card'):
            inner = action['card']
            if inner.get('body'):
                _check_element_versions_in_list(inner['body'], card_version, issues, f'{p}.card.body')
            if inner.get('actions'):
                _check_action_versions_in_list(inner['actions'], card_version, issues, f'{p}.card.actions')
