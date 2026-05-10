use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;

/// Builds an Input.Number Adaptive Card element.
pub struct InputNumberBuilder {
    data: Card,
}

impl InputNumberBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Input.Number".into()));
        data.insert("id".into(), Value::String(String::new()));
        InputNumberBuilder { data }
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

    pub fn with_value(&mut self, value: f64) -> &mut Self {
        self.data.insert(
            "value".into(),
            serde_json::Number::from_f64(value)
                .map(Value::Number)
                .unwrap_or(Value::Null),
        );
        self
    }

    pub fn with_min(&mut self, min: f64) -> &mut Self {
        self.data.insert(
            "min".into(),
            serde_json::Number::from_f64(min)
                .map(Value::Number)
                .unwrap_or(Value::Null),
        );
        self
    }

    pub fn with_max(&mut self, max: f64) -> &mut Self {
        self.data.insert(
            "max".into(),
            serde_json::Number::from_f64(max)
                .map(Value::Number)
                .unwrap_or(Value::Null),
        );
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
