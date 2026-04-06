use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;

/// Builds a FactSet Adaptive Card element.
pub struct FactSetBuilder {
    data: Card,
}

impl FactSetBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("FactSet".into()));
        data.insert("facts".into(), Value::Array(Vec::new()));
        FactSetBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    /// Adds a fact with the given title and value strings.
    pub fn add_fact(&mut self, title: &str, value: &str) -> &mut Self {
        let mut fact = Card::new();
        fact.insert("title".into(), Value::String(title.into()));
        fact.insert("value".into(), Value::String(value.into()));
        if let Some(Value::Array(facts)) = self.data.get_mut("facts") {
            facts.push(Value::Object(fact));
        }
        self
    }

    /// Adds a pre-built fact map directly.
    pub fn add_fact_map(&mut self, fact: Card) -> &mut Self {
        if let Some(Value::Array(facts)) = self.data.get_mut("facts") {
            facts.push(Value::Object(fact));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
