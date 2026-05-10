use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;
use super::action_builder::ActionBuilder;

/// Builds an ActionSet body element (a group of actions within the card body).
pub struct ActionSetBuilder {
    data: Card,
}

impl ActionSetBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("ActionSet".into()));
        data.insert("actions".into(), Value::Array(Vec::new()));
        ActionSetBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    pub fn add_action(
        &mut self,
        configure: impl FnOnce(&mut ActionBuilder),
    ) -> &mut Self {
        let mut ab = ActionBuilder::new();
        configure(&mut ab);
        if let Some(Value::Array(actions)) = self.data.get_mut("actions") {
            actions.push(Value::Object(ab.build()));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
