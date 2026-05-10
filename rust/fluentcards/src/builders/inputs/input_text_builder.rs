use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use crate::builders::action_builder::ActionBuilder;

/// Builds an Input.Text Adaptive Card element.
pub struct InputTextBuilder {
    data: Card,
}

impl InputTextBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Input.Text".into()));
        data.insert("id".into(), Value::String(String::new()));
        InputTextBuilder { data }
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

    pub fn with_max_length(&mut self, max_length: i64) -> &mut Self {
        self.data
            .insert("maxLength".into(), Value::Number(max_length.into()));
        self
    }

    pub fn with_is_multiline(&mut self, is_multiline: bool) -> &mut Self {
        self.data
            .insert("isMultiline".into(), Value::Bool(is_multiline));
        self
    }

    pub fn with_style(&mut self, style: TextInputStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
        self
    }

    pub fn with_regex(&mut self, regex: &str) -> &mut Self {
        self.data
            .insert("regex".into(), Value::String(regex.into()));
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

    pub fn with_inline_action(
        &mut self,
        configure: impl FnOnce(&mut ActionBuilder),
    ) -> &mut Self {
        let mut ab = ActionBuilder::new();
        configure(&mut ab);
        self.data
            .insert("inlineAction".into(), Value::Object(ab.build()));
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
