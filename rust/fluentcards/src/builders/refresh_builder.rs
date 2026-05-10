use serde_json::Value;

use crate::models::Card;
use super::action_builder::ActionBuilder;

/// Builds the refresh configuration for an Adaptive Card.
pub struct RefreshBuilder {
    data: Card,
}

impl RefreshBuilder {
    pub(crate) fn new() -> Self {
        RefreshBuilder { data: Card::new() }
    }

    pub fn with_action(
        &mut self,
        configure: impl FnOnce(&mut ActionBuilder),
    ) -> &mut Self {
        let mut ab = ActionBuilder::new();
        configure(&mut ab);
        self.data
            .insert("action".into(), Value::Object(ab.build()));
        self
    }

    pub fn add_user_id(&mut self, user_id: &str) -> &mut Self {
        let user_ids = self
            .data
            .entry("userIds")
            .or_insert_with(|| Value::Array(Vec::new()));
        if let Value::Array(arr) = user_ids {
            arr.push(Value::String(user_id.into()));
        }
        self
    }

    pub fn with_expires(&mut self, expires: &str) -> &mut Self {
        self.data
            .insert("expires".into(), Value::String(expires.into()));
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
