use serde_json::Value;

use crate::models::Card;

/// Builds the authentication configuration for an Adaptive Card.
pub struct AuthenticationBuilder {
    data: Card,
}

impl AuthenticationBuilder {
    pub(crate) fn new() -> Self {
        AuthenticationBuilder { data: Card::new() }
    }

    pub fn with_text(&mut self, text: &str) -> &mut Self {
        self.data
            .insert("text".into(), Value::String(text.into()));
        self
    }

    pub fn with_connection_name(&mut self, connection_name: &str) -> &mut Self {
        self.data
            .insert("connectionName".into(), Value::String(connection_name.into()));
        self
    }

    pub fn with_token_exchange_resource(&mut self, resource: Card) -> &mut Self {
        self.data
            .insert("tokenExchangeResource".into(), Value::Object(resource));
        self
    }

    pub fn add_button(&mut self, button: Card) -> &mut Self {
        let buttons = self
            .data
            .entry("buttons")
            .or_insert_with(|| Value::Array(Vec::new()));
        if let Value::Array(arr) = buttons {
            arr.push(Value::Object(button));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
