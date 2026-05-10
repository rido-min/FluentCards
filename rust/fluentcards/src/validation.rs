use std::collections::HashMap;
use std::fmt;

use serde_json::Value;

use crate::enums::ValidationSeverity;
use crate::models::{is_known_version, Card};

/// Represents a single validation finding for an Adaptive Card.
#[derive(Debug, Clone)]
pub struct ValidationIssue {
    pub severity: ValidationSeverity,
    pub path: String,
    pub code: String,
    pub message: String,
}

/// Panic payload used by `validate_and_panic` when error-severity issues are found.
#[derive(Debug, Clone)]
pub struct AdaptiveCardValidationError {
    pub issues: Vec<ValidationIssue>,
}

impl fmt::Display for AdaptiveCardValidationError {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        let errors: Vec<&ValidationIssue> = self
            .issues
            .iter()
            .filter(|i| i.severity == ValidationSeverity::Error)
            .collect();
        if errors.len() == 1 {
            write!(
                f,
                "Adaptive Card validation failed: {}",
                errors[0].message
            )
        } else {
            writeln!(
                f,
                "Adaptive Card validation failed with {} errors:",
                errors.len()
            )?;
            for e in &errors {
                writeln!(f, "  - [{}] {}", e.path, e.message)?;
            }
            Ok(())
        }
    }
}

impl std::error::Error for AdaptiveCardValidationError {}

/// Checks an Adaptive Card for structural and semantic issues.
/// Returns a `Vec` of `ValidationIssue` values (may be empty if the card is valid).
pub fn validate(card: &Card) -> Vec<ValidationIssue> {
    let mut issues = Vec::new();
    let mut ids: HashMap<String, bool> = HashMap::new();
    validate_card(card, &mut issues, &mut ids);
    if let Some(version) = card.get("version").and_then(Value::as_str) {
        if is_known_version(version) {
            validate_version_mismatch(card, version, &mut issues);
        }
    }
    issues
}

/// Validates the card and panics with an `AdaptiveCardValidationError`
/// if any Error-severity issues are found.
pub fn validate_and_panic(card: &Card) {
    let issues = validate(card);
    let errors: Vec<ValidationIssue> = issues
        .into_iter()
        .filter(|i| i.severity == ValidationSeverity::Error)
        .collect();
    if !errors.is_empty() {
        panic!(
            "{}",
            AdaptiveCardValidationError { issues: errors }
        );
    }
}

fn add_issue(
    issues: &mut Vec<ValidationIssue>,
    severity: ValidationSeverity,
    path: &str,
    code: &str,
    message: &str,
) {
    issues.push(ValidationIssue {
        severity,
        path: path.to_string(),
        code: code.to_string(),
        message: message.to_string(),
    });
}

fn track_id(id: &str, path: &str, issues: &mut Vec<ValidationIssue>, ids: &mut HashMap<String, bool>) {
    if id.is_empty() {
        return;
    }
    if ids.contains_key(id) {
        add_issue(
            issues,
            ValidationSeverity::Warning,
            path,
            "DUPLICATE_ID",
            &format!(
                "Duplicate id '{}' found. Element IDs should be unique within a card.",
                id
            ),
        );
    } else {
        ids.insert(id.to_string(), true);
    }
}

fn is_absolute_url(raw_url: &str) -> bool {
    if let Ok(url) = url_parse(raw_url) {
        !url.scheme.is_empty() && !url.host.is_empty()
    } else {
        false
    }
}

struct SimpleUrl {
    scheme: String,
    host: String,
}

fn url_parse(raw: &str) -> Result<SimpleUrl, ()> {
    // Simple URL parsing: look for scheme://host
    if let Some(pos) = raw.find("://") {
        let scheme = &raw[..pos];
        let rest = &raw[pos + 3..];
        let host = rest.split('/').next().unwrap_or("");
        let host = host.split('?').next().unwrap_or("");
        let host = host.split('#').next().unwrap_or("");
        Ok(SimpleUrl {
            scheme: scheme.to_string(),
            host: host.to_string(),
        })
    } else {
        Err(())
    }
}

fn get_str<'a>(map: &'a serde_json::Map<String, Value>, key: &str) -> &'a str {
    map.get(key).and_then(Value::as_str).unwrap_or("")
}

fn get_array<'a>(map: &'a serde_json::Map<String, Value>, key: &str) -> Option<&'a Vec<Value>> {
    map.get(key).and_then(Value::as_array)
}

fn get_object<'a>(
    map: &'a serde_json::Map<String, Value>,
    key: &str,
) -> Option<&'a serde_json::Map<String, Value>> {
    map.get(key).and_then(Value::as_object)
}

fn validate_card(
    card: &serde_json::Map<String, Value>,
    issues: &mut Vec<ValidationIssue>,
    ids: &mut HashMap<String, bool>,
) {
    let schema = get_str(card, "$schema");
    if schema.is_empty() {
        add_issue(
            issues,
            ValidationSeverity::Warning,
            "$schema",
            "MISSING_SCHEMA",
            "The '$schema' property is missing. While optional, including it enables better tooling support.",
        );
    }

    let version = get_str(card, "version");
    if version.is_empty() {
        add_issue(
            issues,
            ValidationSeverity::Error,
            "version",
            "MISSING_VERSION",
            "The 'version' property is required. Use a value like '1.5' to specify the schema version.",
        );
    } else if !is_known_version(version) {
        add_issue(
            issues,
            ValidationSeverity::Warning,
            "version",
            "UNKNOWN_VERSION",
            &format!(
                "The version '{}' is not a known Adaptive Cards version. Known versions: 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6.",
                version
            ),
        );
    }

    let body = get_array(card, "body");
    let actions = get_array(card, "actions");
    let body_len = body.map_or(0, |b| b.len());
    let actions_len = actions.map_or(0, |a| a.len());

    if body_len == 0 && actions_len == 0 {
        add_issue(
            issues,
            ValidationSeverity::Warning,
            "",
            "EMPTY_CARD",
            "The card has no body elements and no actions. It will render as empty.",
        );
    }

    if let Some(body) = body {
        if !body.is_empty() {
            validate_elements(body, issues, "body", ids);
        }
    }

    if let Some(actions) = actions {
        if !actions.is_empty() {
            validate_actions(actions, issues, "actions", ids);
            if actions.len() > 5 {
                add_issue(
                    issues,
                    ValidationSeverity::Warning,
                    "actions",
                    "TOO_MANY_ACTIONS",
                    &format!(
                        "The card has {} actions. Some hosts limit the number of visible actions to 5.",
                        actions.len()
                    ),
                );
            }
        }
    }

    if let Some(sa) = card.get("selectAction") {
        validate_select_action(Some(sa), issues, "selectAction");
    }
}

fn validate_elements(
    elements: &[Value],
    issues: &mut Vec<ValidationIssue>,
    path: &str,
    ids: &mut HashMap<String, bool>,
) {
    for (i, el) in elements.iter().enumerate() {
        if let Some(el_map) = el.as_object() {
            let el_path = format!("{}[{}]", path, i);
            let id = get_str(el_map, "id");
            if !id.is_empty() {
                track_id(id, &el_path, issues, ids);
            }
            validate_element(el_map, issues, &el_path, ids);
        }
    }
}

fn validate_element(
    element: &serde_json::Map<String, Value>,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
    ids: &mut HashMap<String, bool>,
) {
    let t = get_str(element, "type");
    match t {
        "TextBlock" => {
            let text = get_str(element, "text");
            if text.is_empty() {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.text", path),
                    "MISSING_TEXT",
                    "TextBlock is missing the required 'text' property.",
                );
            }
        }
        "Image" => {
            let raw_url = get_str(element, "url");
            if raw_url.is_empty() {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.url", path),
                    "MISSING_IMAGE_URL",
                    "Image element is missing the required 'url' property.",
                );
            } else if !is_absolute_url(raw_url) {
                add_issue(
                    issues,
                    ValidationSeverity::Warning,
                    &format!("{}.url", path),
                    "INVALID_IMAGE_URL",
                    &format!("Image URL '{}' is not a valid absolute URL.", raw_url),
                );
            }
            validate_select_action(element.get("selectAction"), issues, &format!("{}.selectAction", path));
        }
        "ImageSet" => {
            let images = get_array(element, "images");
            if images.map_or(true, |i| i.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.images", path),
                    "MISSING_IMAGES",
                    "ImageSet is missing the required 'images' property.",
                );
            } else if let Some(images) = images {
                for (i, img) in images.iter().enumerate() {
                    if let Some(img_map) = img.as_object() {
                        let u = get_str(img_map, "url");
                        if u.is_empty() {
                            add_issue(
                                issues,
                                ValidationSeverity::Error,
                                &format!("{}.images[{}].url", path, i),
                                "MISSING_IMAGE_URL",
                                "Image element is missing the required 'url' property.",
                            );
                        }
                    }
                }
            }
        }
        "FactSet" => {
            let facts = get_array(element, "facts");
            if facts.map_or(true, |f| f.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.facts", path),
                    "MISSING_FACTS",
                    "FactSet is missing the required 'facts' property.",
                );
            }
        }
        "ActionSet" => {
            let actions = get_array(element, "actions");
            if actions.map_or(true, |a| a.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.actions", path),
                    "MISSING_ACTIONSET_ACTIONS",
                    "ActionSet is missing the required 'actions' property.",
                );
            } else if let Some(actions) = actions {
                validate_actions(actions, issues, &format!("{}.actions", path), ids);
            }
        }
        "RichTextBlock" => {
            let inlines = get_array(element, "inlines");
            if inlines.map_or(true, |i| i.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.inlines", path),
                    "MISSING_INLINES",
                    "RichTextBlock is missing the required 'inlines' property.",
                );
            }
        }
        "Media" => {
            let sources = get_array(element, "sources");
            if sources.map_or(true, |s| s.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.sources", path),
                    "MISSING_MEDIA_SOURCES",
                    "Media is missing the required 'sources' property.",
                );
            }
        }
        "Input.Text" | "Input.Number" | "Input.Date" | "Input.Time" | "Input.Toggle"
        | "Input.ChoiceSet" => {
            let id = get_str(element, "id");
            if id.is_empty() {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.id", path),
                    "MISSING_INPUT_ID",
                    "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.",
                );
            } else {
                track_id(id, path, issues, ids);
            }
            validate_input_element(element, issues, path);
        }
        "Container" => {
            let items = get_array(element, "items");
            if items.map_or(true, |i| i.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Warning,
                    &format!("{}.items", path),
                    "EMPTY_CONTAINER",
                    "Container has no items. It will render as empty.",
                );
            } else if let Some(items) = items {
                validate_elements(items, issues, &format!("{}.items", path), ids);
            }
            validate_select_action(
                element.get("selectAction"),
                issues,
                &format!("{}.selectAction", path),
            );
        }
        "ColumnSet" => {
            if let Some(columns) = get_array(element, "columns") {
                for (i, col) in columns.iter().enumerate() {
                    if let Some(col_map) = col.as_object() {
                        let col_path = format!("{}.columns[{}]", path, i);
                        let id = get_str(col_map, "id");
                        if !id.is_empty() {
                            track_id(id, &col_path, issues, ids);
                        }
                        if let Some(items) = get_array(col_map, "items") {
                            if !items.is_empty() {
                                validate_elements(
                                    items,
                                    issues,
                                    &format!("{}.items", col_path),
                                    ids,
                                );
                            }
                        }
                        validate_select_action(
                            col_map.get("selectAction"),
                            issues,
                            &format!("{}.selectAction", col_path),
                        );
                    }
                }
            }
            validate_select_action(
                element.get("selectAction"),
                issues,
                &format!("{}.selectAction", path),
            );
        }
        "Table" => {
            if let Some(rows) = get_array(element, "rows") {
                for (r, row) in rows.iter().enumerate() {
                    if let Some(row_map) = row.as_object() {
                        if let Some(cells) = get_array(row_map, "cells") {
                            for (c, cell) in cells.iter().enumerate() {
                                if let Some(cell_map) = cell.as_object() {
                                    if let Some(items) = get_array(cell_map, "items") {
                                        if !items.is_empty() {
                                            validate_elements(
                                                items,
                                                issues,
                                                &format!(
                                                    "{}.rows[{}].cells[{}].items",
                                                    path, r, c
                                                ),
                                                ids,
                                            );
                                        }
                                    }
                                    validate_select_action(
                                        cell_map.get("selectAction"),
                                        issues,
                                        &format!(
                                            "{}.rows[{}].cells[{}].selectAction",
                                            path, r, c
                                        ),
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }
        _ => {}
    }
}

fn validate_input_element(
    element: &serde_json::Map<String, Value>,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
) {
    let t = get_str(element, "type");
    match t {
        "Input.Number" => {
            let min = element.get("min");
            let max = element.get("max");
            if let (Some(min_val), Some(max_val)) = (min, max) {
                if let (Some(min_f), Some(max_f)) = (to_f64(min_val), to_f64(max_val)) {
                    if min_f > max_f {
                        add_issue(
                            issues,
                            ValidationSeverity::Error,
                            path,
                            "MIN_GREATER_THAN_MAX",
                            &format!(
                                "Input.Number 'min' ({}) is greater than 'max' ({}).",
                                min_val, max_val
                            ),
                        );
                    }
                }
            }
        }
        "Input.Date" | "Input.Time" => {
            let min_s = get_str(element, "min");
            let max_s = get_str(element, "max");
            if !min_s.is_empty() && !max_s.is_empty() && min_s > max_s {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    path,
                    "MIN_GREATER_THAN_MAX",
                    &format!("{} 'min' ({}) is greater than 'max' ({}).", t, min_s, max_s),
                );
            }
        }
        "Input.Toggle" => {
            let title = get_str(element, "title");
            if title.is_empty() {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.title", path),
                    "MISSING_TOGGLE_TITLE",
                    "Input.Toggle is missing the required 'title' property.",
                );
            }
        }
        _ => {}
    }
}

fn to_f64(v: &Value) -> Option<f64> {
    v.as_f64()
}

fn validate_select_action(
    action: Option<&Value>,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
) {
    let action = match action {
        Some(a) => a,
        None => return,
    };
    if let Some(action_map) = action.as_object() {
        let t = get_str(action_map, "type");
        if t == "Action.ShowCard" {
            add_issue(
                issues,
                ValidationSeverity::Error,
                path,
                "INVALID_SELECT_ACTION",
                "Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, Action.Execute, or Action.ToggleVisibility.",
            );
        }
    }
}

fn validate_actions(
    actions: &[Value],
    issues: &mut Vec<ValidationIssue>,
    path: &str,
    ids: &mut HashMap<String, bool>,
) {
    for (i, action) in actions.iter().enumerate() {
        if let Some(action_map) = action.as_object() {
            let action_path = format!("{}[{}]", path, i);
            let id = get_str(action_map, "id");
            if !id.is_empty() {
                track_id(id, &action_path, issues, ids);
            }
            validate_action(action_map, issues, &action_path, ids);
        }
    }
}

fn validate_action(
    action: &serde_json::Map<String, Value>,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
    ids: &mut HashMap<String, bool>,
) {
    let t = get_str(action, "type");
    match t {
        "Action.OpenUrl" => {
            let raw_url = get_str(action, "url");
            if raw_url.is_empty() {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.url", path),
                    "MISSING_ACTION_URL",
                    "Action.OpenUrl is missing the required 'url' property.",
                );
            } else if !is_absolute_url(raw_url) {
                add_issue(
                    issues,
                    ValidationSeverity::Warning,
                    &format!("{}.url", path),
                    "INVALID_ACTION_URL",
                    &format!(
                        "Action.OpenUrl URL '{}' is not a valid absolute URL.",
                        raw_url
                    ),
                );
            }
        }
        "Action.ShowCard" => {
            let card = action.get("card");
            if card.is_none() || card == Some(&Value::Null) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.card", path),
                    "MISSING_SHOWCARD",
                    "Action.ShowCard is missing the required 'card' property.",
                );
            } else if let Some(card_map) = card.and_then(Value::as_object) {
                validate_card(card_map, issues, ids);
            }
        }
        "Action.ToggleVisibility" => {
            let targets = get_array(action, "targetElements");
            if targets.map_or(true, |t| t.is_empty()) {
                add_issue(
                    issues,
                    ValidationSeverity::Error,
                    &format!("{}.targetElements", path),
                    "MISSING_TARGET_ELEMENTS",
                    "Action.ToggleVisibility is missing the required 'targetElements' property.",
                );
            }
        }
        _ => {}
    }
}

// Version-aware validation

fn element_version(type_str: &str) -> Option<u32> {
    match type_str {
        "TextBlock" | "Image" | "Container" | "ColumnSet" | "FactSet" | "ImageSet" | "Column"
        | "Fact" | "Choice" | "Action.OpenUrl" | "Action.Submit" | "Action.ShowCard"
        | "Input.Text" | "Input.Number" | "Input.Date" | "Input.Time" | "Input.Toggle"
        | "Input.ChoiceSet" => Some(0),
        "Media" => Some(1),
        "RichTextBlock" | "ActionSet" | "Action.ToggleVisibility" => Some(2),
        "Action.Execute" => Some(4),
        "Table" => Some(5),
        _ => None,
    }
}

fn card_property_version(prop: &str) -> Option<u32> {
    match prop {
        "selectAction" => Some(1),
        "minHeight" | "verticalContentAlignment" | "backgroundImage" => Some(2),
        "refresh" | "authentication" => Some(4),
        "rtl" => Some(5),
        "metadata" => Some(6),
        _ => None,
    }
}

fn version_minor(v: &str) -> u32 {
    if let Some(pos) = v.find('.') {
        v[pos + 1..].parse::<u32>().unwrap_or(0)
    } else {
        0
    }
}

fn version_mismatch_issue(
    issues: &mut Vec<ValidationIssue>,
    path: &str,
    feature_name: &str,
    required_version: &str,
    card_version: &str,
) {
    add_issue(
        issues,
        ValidationSeverity::Warning,
        path,
        "VERSION_MISMATCH",
        &format!(
            "'{}' requires Adaptive Cards {} but card version is {}.",
            feature_name, required_version, card_version
        ),
    );
}

fn check_element_version(
    type_str: &str,
    card_version: &str,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
) {
    if let Some(required) = element_version(type_str) {
        if required > version_minor(card_version) {
            version_mismatch_issue(
                issues,
                path,
                type_str,
                &format!("1.{}", required),
                card_version,
            );
        }
    }
}

fn check_card_property_version(
    prop: &str,
    card_version: &str,
    issues: &mut Vec<ValidationIssue>,
) {
    if let Some(required) = card_property_version(prop) {
        if required > version_minor(card_version) {
            version_mismatch_issue(issues, prop, prop, &format!("1.{}", required), card_version);
        }
    }
}

fn validate_version_mismatch(
    card: &serde_json::Map<String, Value>,
    card_version: &str,
    issues: &mut Vec<ValidationIssue>,
) {
    for prop in &[
        "selectAction",
        "minHeight",
        "verticalContentAlignment",
        "backgroundImage",
        "refresh",
        "authentication",
        "metadata",
    ] {
        if card.get(*prop).is_some() {
            check_card_property_version(prop, card_version, issues);
        }
    }
    if card.get("rtl").is_some() {
        check_card_property_version("rtl", card_version, issues);
    }

    if let Some(body) = get_array(card, "body") {
        if !body.is_empty() {
            check_element_versions_in_list(body, card_version, issues, "body");
        }
    }
    if let Some(actions) = get_array(card, "actions") {
        if !actions.is_empty() {
            check_action_versions_in_list(actions, card_version, issues, "actions");
        }
    }
}

fn check_element_versions_in_list(
    elements: &[Value],
    card_version: &str,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
) {
    for (i, el) in elements.iter().enumerate() {
        if let Some(el_map) = el.as_object() {
            let p = format!("{}[{}]", path, i);
            let t = get_str(el_map, "type");
            check_element_version(t, card_version, issues, &p);
            match t {
                "Container" => {
                    if let Some(items) = get_array(el_map, "items") {
                        if !items.is_empty() {
                            check_element_versions_in_list(
                                items,
                                card_version,
                                issues,
                                &format!("{}.items", p),
                            );
                        }
                    }
                }
                "ColumnSet" => {
                    if let Some(cols) = get_array(el_map, "columns") {
                        for (ci, col) in cols.iter().enumerate() {
                            if let Some(col_map) = col.as_object() {
                                if let Some(items) = get_array(col_map, "items") {
                                    if !items.is_empty() {
                                        check_element_versions_in_list(
                                            items,
                                            card_version,
                                            issues,
                                            &format!("{}.columns[{}].items", p, ci),
                                        );
                                    }
                                }
                            }
                        }
                    }
                }
                "ActionSet" => {
                    if let Some(actions) = get_array(el_map, "actions") {
                        if !actions.is_empty() {
                            check_action_versions_in_list(
                                actions,
                                card_version,
                                issues,
                                &format!("{}.actions", p),
                            );
                        }
                    }
                }
                "Table" => {
                    if let Some(rows) = get_array(el_map, "rows") {
                        for (r, row) in rows.iter().enumerate() {
                            if let Some(row_map) = row.as_object() {
                                if let Some(cells) = get_array(row_map, "cells") {
                                    for (c, cell) in cells.iter().enumerate() {
                                        if let Some(cell_map) = cell.as_object() {
                                            if let Some(items) = get_array(cell_map, "items") {
                                                if !items.is_empty() {
                                                    check_element_versions_in_list(
                                                        items,
                                                        card_version,
                                                        issues,
                                                        &format!(
                                                            "{}.rows[{}].cells[{}].items",
                                                            p, r, c
                                                        ),
                                                    );
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                _ => {}
            }
        }
    }
}

fn check_action_versions_in_list(
    actions: &[Value],
    card_version: &str,
    issues: &mut Vec<ValidationIssue>,
    path: &str,
) {
    for (i, action) in actions.iter().enumerate() {
        if let Some(action_map) = action.as_object() {
            let p = format!("{}[{}]", path, i);
            let t = get_str(action_map, "type");
            check_element_version(t, card_version, issues, &p);
            if t == "Action.ShowCard" {
                if let Some(inner) = get_object(action_map, "card") {
                    if let Some(body) = get_array(inner, "body") {
                        if !body.is_empty() {
                            check_element_versions_in_list(
                                body,
                                card_version,
                                issues,
                                &format!("{}.card.body", p),
                            );
                        }
                    }
                    if let Some(inner_actions) = get_array(inner, "actions") {
                        if !inner_actions.is_empty() {
                            check_action_versions_in_list(
                                inner_actions,
                                card_version,
                                issues,
                                &format!("{}.card.actions", p),
                            );
                        }
                    }
                }
            }
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    use crate::AdaptiveCardBuilder;

    #[test]
    fn validate_valid_card() {
        let card = AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_text_block(|tb| { tb.with_text("Hello"); })
            .build();
        let issues = validate(&card);
        assert!(issues.is_empty());
    }

    #[test]
    fn validate_missing_version() {
        let mut card = Card::new();
        card.insert("type".into(), Value::String("AdaptiveCard".into()));
        card.insert("$schema".into(), Value::String("https://x.com".into()));
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "MISSING_VERSION" && i.severity == ValidationSeverity::Error));
    }

    #[test]
    fn validate_empty_card() {
        let card = AdaptiveCardBuilder::new().build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "EMPTY_CARD" && i.severity == ValidationSeverity::Warning));
    }

    #[test]
    fn validate_missing_text_block_text() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|_tb| {})
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "MISSING_TEXT" && i.severity == ValidationSeverity::Error));
    }

    #[test]
    fn validate_missing_image_url() {
        let card = AdaptiveCardBuilder::new()
            .add_image(|_img| {})
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "MISSING_IMAGE_URL"));
    }

    #[test]
    fn validate_missing_input_id() {
        let card = AdaptiveCardBuilder::new()
            .add_input_text(|_it| {})
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "MISSING_INPUT_ID" && i.severity == ValidationSeverity::Error));
    }

    #[test]
    fn validate_input_number_min_greater_than_max() {
        let card = AdaptiveCardBuilder::new()
            .add_input_number(|inp| {
                inp.with_id("qty").with_min(100.0).with_max(10.0);
            })
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "MIN_GREATER_THAN_MAX" && i.severity == ValidationSeverity::Error));
    }

    #[test]
    fn validate_duplicate_id() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("First").with_id("dup"); })
            .add_text_block(|tb| { tb.with_text("Second").with_id("dup"); })
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "DUPLICATE_ID"));
    }

    #[test]
    fn validate_invalid_select_action_show_card() {
        let mut card = Card::new();
        card.insert("type".into(), Value::String("AdaptiveCard".into()));
        card.insert("version".into(), Value::String("1.5".into()));
        card.insert("$schema".into(), Value::String("https://x.com".into()));
        let mut show_card = serde_json::Map::new();
        show_card.insert("type".into(), Value::String("Action.ShowCard".into()));
        card.insert("selectAction".into(), Value::Object(show_card));
        let mut body = Vec::new();
        let mut tb = serde_json::Map::new();
        tb.insert("type".into(), Value::String("TextBlock".into()));
        tb.insert("text".into(), Value::String("x".into()));
        body.push(Value::Object(tb));
        card.insert("body".into(), Value::Array(body));
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "INVALID_SELECT_ACTION"));
    }

    #[test]
    fn validate_version_mismatch_table() {
        let card = AdaptiveCardBuilder::new()
            .with_version("1.2")
            .add_table(|tb| {
                tb.add_column(serde_json::json!({"width": 1}).as_object().unwrap().clone())
                  .add_row(serde_json::json!({"cells": []}).as_object().unwrap().clone());
            })
            .build();
        let issues = validate(&card);
        assert!(issues.iter().any(|i| i.code == "VERSION_MISMATCH" && i.severity == ValidationSeverity::Warning));
    }

    #[test]
    fn validate_and_panic_valid_card() {
        let card = AdaptiveCardBuilder::new()
            .with_version("1.5")
            .add_text_block(|tb| { tb.with_text("OK"); })
            .build();
        validate_and_panic(&card); // should not panic
    }

    #[test]
    #[should_panic]
    fn validate_and_panic_invalid_card() {
        let mut card = Card::new();
        card.insert("type".into(), Value::String("AdaptiveCard".into()));
        validate_and_panic(&card);
    }

    #[test]
    fn validation_error_message() {
        let result = std::panic::catch_unwind(|| {
            let mut card = Card::new();
            card.insert("type".into(), Value::String("AdaptiveCard".into()));
            validate_and_panic(&card);
        });
        assert!(result.is_err());
    }
}
