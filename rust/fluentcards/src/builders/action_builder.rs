use serde_json::Value;

use crate::enums::*;
use crate::models::Card;

/// Builds an Adaptive Card action (OpenUrl, Submit, ShowCard, ToggleVisibility, Execute).
/// Call one of `open_url`, `submit`, `show_card`, `toggle_visibility`, or `execute` to set the action type,
/// then use `with_*` methods to configure it.
pub struct ActionBuilder {
    data: Option<Card>,
}

impl ActionBuilder {
    pub(crate) fn new() -> Self {
        ActionBuilder { data: None }
    }

    /// Creates an Action.OpenUrl action.
    pub fn open_url(&mut self, url: &str) -> &mut Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Action.OpenUrl".into()));
        data.insert("url".into(), Value::String(url.into()));
        self.data = Some(data);
        self
    }

    /// Creates an Action.Submit action with an optional title.
    pub fn submit(&mut self, title: &str) -> &mut Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Action.Submit".into()));
        if !title.is_empty() {
            data.insert("title".into(), Value::String(title.into()));
        }
        self.data = Some(data);
        self
    }

    /// Creates an Action.ShowCard action with an optional title.
    pub fn show_card(&mut self, title: &str) -> &mut Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Action.ShowCard".into()));
        if !title.is_empty() {
            data.insert("title".into(), Value::String(title.into()));
        }
        self.data = Some(data);
        self
    }

    /// Creates an Action.ToggleVisibility action with an optional title.
    pub fn toggle_visibility(&mut self, title: &str) -> &mut Self {
        let mut data = Card::new();
        data.insert(
            "type".into(),
            Value::String("Action.ToggleVisibility".into()),
        );
        if !title.is_empty() {
            data.insert("title".into(), Value::String(title.into()));
        }
        self.data = Some(data);
        self
    }

    /// Creates an Action.Execute action with an optional title.
    pub fn execute(&mut self, title: &str) -> &mut Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Action.Execute".into()));
        if !title.is_empty() {
            data.insert("title".into(), Value::String(title.into()));
        }
        self.data = Some(data);
        self
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("id".into(), Value::String(id.into()));
        }
        self
    }

    pub fn with_title(&mut self, title: &str) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("title".into(), Value::String(title.into()));
        }
        self
    }

    pub fn with_icon_url(&mut self, icon_url: &str) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("iconUrl".into(), Value::String(icon_url.into()));
        }
        self
    }

    pub fn with_style(&mut self, style: ActionStyle) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("style".into(), style.into());
        }
        self
    }

    pub fn with_is_enabled(&mut self, is_enabled: bool) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("isEnabled".into(), Value::Bool(is_enabled));
        }
        self
    }

    pub fn with_tooltip(&mut self, tooltip: &str) -> &mut Self {
        if let Some(data) = &mut self.data {
            data.insert("tooltip".into(), Value::String(tooltip.into()));
        }
        self
    }

    /// Sets the data payload for Action.Submit or Action.Execute.
    pub fn with_data(&mut self, data_val: Value) -> &mut Self {
        if let Some(data) = &mut self.data {
            let t = data.get("type").and_then(Value::as_str).unwrap_or("");
            if t == "Action.Submit" || t == "Action.Execute" {
                data.insert("data".into(), data_val);
            }
        }
        self
    }

    /// Sets which inputs are submitted for Action.Submit or Action.Execute.
    pub fn with_associated_inputs(&mut self, ai: AssociatedInputs) -> &mut Self {
        if let Some(data) = &mut self.data {
            let t = data.get("type").and_then(Value::as_str).unwrap_or("");
            if t == "Action.Submit" || t == "Action.Execute" {
                data.insert("associatedInputs".into(), ai.into());
            }
        }
        self
    }

    /// Sets the verb for Action.Execute.
    pub fn with_verb(&mut self, verb: &str) -> &mut Self {
        if let Some(data) = &mut self.data {
            let t = data.get("type").and_then(Value::as_str).unwrap_or("");
            if t == "Action.Execute" {
                data.insert("verb".into(), Value::String(verb.into()));
            }
        }
        self
    }

    /// Sets the nested card for Action.ShowCard.
    pub fn with_card(&mut self, card: Card) -> &mut Self {
        if let Some(data) = &mut self.data {
            let t = data.get("type").and_then(Value::as_str).unwrap_or("");
            if t == "Action.ShowCard" {
                data.insert("card".into(), Value::Object(card));
            }
        }
        self
    }

    /// Adds a target element for Action.ToggleVisibility.
    /// Pass `Some(bool)` to pin visibility; pass `None` to toggle.
    pub fn add_target_element(&mut self, element_id: &str, is_visible: Option<bool>) -> &mut Self {
        if let Some(data) = &mut self.data {
            let t = data.get("type").and_then(Value::as_str).unwrap_or("");
            if t != "Action.ToggleVisibility" {
                return self;
            }
            let targets = data
                .entry("targetElements")
                .or_insert_with(|| Value::Array(Vec::new()));
            if let Value::Array(arr) = targets {
                match is_visible {
                    None => {
                        arr.push(Value::String(element_id.into()));
                    }
                    Some(vis) => {
                        let mut target = Card::new();
                        target
                            .insert("elementId".into(), Value::String(element_id.into()));
                        target.insert("isVisible".into(), Value::Bool(vis));
                        arr.push(Value::Object(target));
                    }
                }
            }
        }
        self
    }

    /// Returns the built action Card. Panics if no action type was set.
    pub fn build(&self) -> Card {
        self.data
            .clone()
            .expect("ActionBuilder: no action type specified — call open_url, submit, show_card, toggle_visibility, or execute first")
    }
}
