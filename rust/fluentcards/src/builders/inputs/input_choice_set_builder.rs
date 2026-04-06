use serde_json::Value;

use crate::enums::*;
use crate::models::Card;

/// Builds an Input.ChoiceSet Adaptive Card element.
pub struct InputChoiceSetBuilder {
    data: Card,
}

impl InputChoiceSetBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Input.ChoiceSet".into()));
        data.insert("id".into(), Value::String(String::new()));
        data.insert("choices".into(), Value::Array(Vec::new()));
        InputChoiceSetBuilder { data }
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

    pub fn with_style(&mut self, style: ChoiceInputStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
        self
    }

    pub fn with_is_multi_select(&mut self, is_multi_select: bool) -> &mut Self {
        self.data
            .insert("isMultiSelect".into(), Value::Bool(is_multi_select));
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

    /// Adds a choice with the given title and value strings.
    pub fn add_choice(&mut self, title: &str, value: &str) -> &mut Self {
        let mut choice = Card::new();
        choice.insert("title".into(), Value::String(title.into()));
        choice.insert("value".into(), Value::String(value.into()));
        if let Some(Value::Array(choices)) = self.data.get_mut("choices") {
            choices.push(Value::Object(choice));
        }
        self
    }

    /// Adds a pre-built choice map directly.
    pub fn add_choice_map(&mut self, choice: Card) -> &mut Self {
        if let Some(Value::Array(choices)) = self.data.get_mut("choices") {
            choices.push(Value::Object(choice));
        }
        self
    }

    /// Sets a dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+).
    /// `dataset` is the dataset identifier, e.g. `"graph.microsoft.com/users"`.
    pub fn with_choices_data(&mut self, dataset: &str) -> &mut Self {
        let mut data_query = Card::new();
        data_query.insert("type".into(), Value::String("Data.Query".into()));
        data_query.insert("dataset".into(), Value::String(dataset.into()));
        self.data
            .insert("choices.data".into(), Value::Object(data_query));
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
