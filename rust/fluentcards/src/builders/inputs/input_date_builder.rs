use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;

/// Builds an Input.Date Adaptive Card element.
pub struct InputDateBuilder {
    data: Card,
}

impl InputDateBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Input.Date".into()));
        data.insert("id".into(), Value::String(String::new()));
        InputDateBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_label(&mut self, label: &str) -> &mut Self {
        self.data
            .insert("label".into(), Value::String(label.into()));
        self
    }

    pub fn with_placeholder(&mut self, placeholder: &str) -> &mut Self {
        self.data
            .insert("placeholder".into(), Value::String(placeholder.into()));
        self
    }

    pub fn with_value(&mut self, value: &str) -> &mut Self {
        self.data
            .insert("value".into(), Value::String(value.into()));
        self
    }

    /// Sets the minimum date (format: YYYY-MM-DD).
    pub fn with_min(&mut self, min: &str) -> &mut Self {
        self.data
            .insert("min".into(), Value::String(min.into()));
        self
    }

    /// Sets the maximum date (format: YYYY-MM-DD).
    pub fn with_max(&mut self, max: &str) -> &mut Self {
        self.data
            .insert("max".into(), Value::String(max.into()));
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
