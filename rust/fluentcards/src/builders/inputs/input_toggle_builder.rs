use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;

/// Builds an Input.Toggle Adaptive Card element.
pub struct InputToggleBuilder {
    data: Card,
}

impl InputToggleBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Input.Toggle".into()));
        data.insert("id".into(), Value::String(String::new()));
        data.insert("title".into(), Value::String(String::new()));
        InputToggleBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_title(&mut self, title: &str) -> &mut Self {
        self.data
            .insert("title".into(), Value::String(title.into()));
        self
    }

    pub fn with_label(&mut self, label: &str) -> &mut Self {
        self.data
            .insert("label".into(), Value::String(label.into()));
        self
    }

    pub fn with_value(&mut self, value: &str) -> &mut Self {
        self.data
            .insert("value".into(), Value::String(value.into()));
        self
    }

    pub fn with_value_on(&mut self, value_on: &str) -> &mut Self {
        self.data
            .insert("valueOn".into(), Value::String(value_on.into()));
        self
    }

    pub fn with_value_off(&mut self, value_off: &str) -> &mut Self {
        self.data
            .insert("valueOff".into(), Value::String(value_off.into()));
        self
    }

    pub fn with_wrap(&mut self, wrap: bool) -> &mut Self {
        self.data.insert("wrap".into(), Value::Bool(wrap));
        self
    }

    pub fn with_is_required(&mut self, is_required: bool) -> &mut Self {
        self.data
            .insert("isRequired".into(), Value::Bool(is_required));
        self
    }

    pub fn with_error_message(&mut self, error_message: &str) -> &mut Self {
        self.data
            .insert("errorMessage".into(), Value::String(error_message.into()));
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
