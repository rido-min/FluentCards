use serde_json::Value;

use crate::models::{schema_url_for, Card};
use crate::enums::VerticalAlignment;
use super::action_builder::ActionBuilder;
use super::background_image_builder::BackgroundImageBuilder;
use super::text_block_builder::TextBlockBuilder;
use super::image_builder::ImageBuilder;
use super::container_builder::ContainerBuilder;
use super::column_set_builder::ColumnSetBuilder;
use super::fact_set_builder::FactSetBuilder;
use super::rich_text_block_builder::RichTextBlockBuilder;
use super::action_set_builder::ActionSetBuilder;
use super::media_builder::MediaBuilder;
use super::image_set_builder::ImageSetBuilder;
use super::table_builder::TableBuilder;
use super::refresh_builder::RefreshBuilder;
use super::authentication_builder::AuthenticationBuilder;
use super::inputs::*;

/// Builds a root Adaptive Card.
/// Use `AdaptiveCardBuilder::new()` to create one, chain `with_*`/`add_*` methods, then call `build()`.
pub struct AdaptiveCardBuilder {
    data: Card,
}

impl AdaptiveCardBuilder {
    /// Creates a new `AdaptiveCardBuilder` with default version 1.5.
    #[must_use]
    pub fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("AdaptiveCard".into()));
        data.insert("version".into(), Value::String("1.5".into()));
        data.insert(
            "$schema".into(),
            Value::String(schema_url_for("1.5").into()),
        );
        AdaptiveCardBuilder { data }
    }

    /// Sets the Adaptive Cards schema version (e.g. "1.5").
    /// The `$schema` URL is updated automatically for known versions.
    pub fn with_version(&mut self, version: &str) -> &mut Self {
        self.data
            .insert("version".into(), Value::String(version.into()));
        self.data.insert(
            "$schema".into(),
            Value::String(schema_url_for(version).into()),
        );
        self
    }

    /// Overrides the `$schema` URL.
    pub fn with_schema(&mut self, schema: &str) -> &mut Self {
        self.data
            .insert("$schema".into(), Value::String(schema.into()));
        self
    }

    pub fn with_fallback_text(&mut self, fallback_text: &str) -> &mut Self {
        self.data
            .insert("fallbackText".into(), Value::String(fallback_text.into()));
        self
    }

    pub fn with_speak(&mut self, speak: &str) -> &mut Self {
        self.data
            .insert("speak".into(), Value::String(speak.into()));
        self
    }

    pub fn with_lang(&mut self, lang: &str) -> &mut Self {
        self.data
            .insert("lang".into(), Value::String(lang.into()));
        self
    }

    pub fn with_rtl(&mut self, rtl: bool) -> &mut Self {
        self.data.insert("rtl".into(), Value::Bool(rtl));
        self
    }

    pub fn with_min_height(&mut self, min_height: &str) -> &mut Self {
        self.data
            .insert("minHeight".into(), Value::String(min_height.into()));
        self
    }

    pub fn with_vertical_content_alignment(&mut self, alignment: VerticalAlignment) -> &mut Self {
        self.data.insert(
            "verticalContentAlignment".into(),
            alignment.into(),
        );
        self
    }

    pub fn with_background_image(
        &mut self,
        configure: impl FnOnce(&mut BackgroundImageBuilder),
    ) -> &mut Self {
        let mut bib = BackgroundImageBuilder::new();
        configure(&mut bib);
        self.data
            .insert("backgroundImage".into(), Value::Object(bib.build()));
        self
    }

    pub fn with_select_action(
        &mut self,
        configure: impl FnOnce(&mut ActionBuilder),
    ) -> &mut Self {
        let mut ab = ActionBuilder::new();
        configure(&mut ab);
        self.data
            .insert("selectAction".into(), Value::Object(ab.build()));
        self
    }

    pub fn with_metadata(&mut self, web_url: &str) -> &mut Self {
        let mut meta = Card::new();
        meta.insert("webUrl".into(), Value::String(web_url.into()));
        self.data
            .insert("metadata".into(), Value::Object(meta));
        self
    }

    // ── Body elements ────────────────────────────────────────────

    pub fn add_text_block(
        &mut self,
        configure: impl FnOnce(&mut TextBlockBuilder),
    ) -> &mut Self {
        let mut tb = TextBlockBuilder::new();
        configure(&mut tb);
        self.push_body(Value::Object(tb.build()));
        self
    }

    pub fn add_image(
        &mut self,
        configure: impl FnOnce(&mut ImageBuilder),
    ) -> &mut Self {
        let mut ib = ImageBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_container(
        &mut self,
        configure: impl FnOnce(&mut ContainerBuilder),
    ) -> &mut Self {
        let mut cb = ContainerBuilder::new();
        configure(&mut cb);
        self.push_body(Value::Object(cb.build()));
        self
    }

    pub fn add_column_set(
        &mut self,
        configure: impl FnOnce(&mut ColumnSetBuilder),
    ) -> &mut Self {
        let mut cs = ColumnSetBuilder::new();
        configure(&mut cs);
        self.push_body(Value::Object(cs.build()));
        self
    }

    pub fn add_fact_set(
        &mut self,
        configure: impl FnOnce(&mut FactSetBuilder),
    ) -> &mut Self {
        let mut fs = FactSetBuilder::new();
        configure(&mut fs);
        self.push_body(Value::Object(fs.build()));
        self
    }

    pub fn add_rich_text_block(
        &mut self,
        configure: impl FnOnce(&mut RichTextBlockBuilder),
    ) -> &mut Self {
        let mut rtb = RichTextBlockBuilder::new();
        configure(&mut rtb);
        self.push_body(Value::Object(rtb.build()));
        self
    }

    pub fn add_action_set(
        &mut self,
        configure: impl FnOnce(&mut ActionSetBuilder),
    ) -> &mut Self {
        let mut asb = ActionSetBuilder::new();
        configure(&mut asb);
        self.push_body(Value::Object(asb.build()));
        self
    }

    pub fn add_media(
        &mut self,
        configure: impl FnOnce(&mut MediaBuilder),
    ) -> &mut Self {
        let mut mb = MediaBuilder::new();
        configure(&mut mb);
        self.push_body(Value::Object(mb.build()));
        self
    }

    pub fn add_image_set(
        &mut self,
        configure: impl FnOnce(&mut ImageSetBuilder),
    ) -> &mut Self {
        let mut isb = ImageSetBuilder::new();
        configure(&mut isb);
        self.push_body(Value::Object(isb.build()));
        self
    }

    pub fn add_table(
        &mut self,
        configure: impl FnOnce(&mut TableBuilder),
    ) -> &mut Self {
        let mut tb = TableBuilder::new();
        configure(&mut tb);
        self.push_body(Value::Object(tb.build()));
        self
    }

    // ── Input elements ───────────────────────────────────────────

    pub fn add_input_text(
        &mut self,
        configure: impl FnOnce(&mut InputTextBuilder),
    ) -> &mut Self {
        let mut ib = InputTextBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_input_number(
        &mut self,
        configure: impl FnOnce(&mut InputNumberBuilder),
    ) -> &mut Self {
        let mut ib = InputNumberBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_input_date(
        &mut self,
        configure: impl FnOnce(&mut InputDateBuilder),
    ) -> &mut Self {
        let mut ib = InputDateBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_input_time(
        &mut self,
        configure: impl FnOnce(&mut InputTimeBuilder),
    ) -> &mut Self {
        let mut ib = InputTimeBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_input_toggle(
        &mut self,
        configure: impl FnOnce(&mut InputToggleBuilder),
    ) -> &mut Self {
        let mut ib = InputToggleBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    pub fn add_input_choice_set(
        &mut self,
        configure: impl FnOnce(&mut InputChoiceSetBuilder),
    ) -> &mut Self {
        let mut ib = InputChoiceSetBuilder::new();
        configure(&mut ib);
        self.push_body(Value::Object(ib.build()));
        self
    }

    /// Adds a pre-built element `Card` directly to the card body.
    pub fn add_element(&mut self, element: Card) -> &mut Self {
        self.push_body(Value::Object(element));
        self
    }

    // ── Actions ──────────────────────────────────────────────────

    pub fn add_action(
        &mut self,
        configure: impl FnOnce(&mut ActionBuilder),
    ) -> &mut Self {
        let mut ab = ActionBuilder::new();
        configure(&mut ab);
        let actions = self
            .data
            .entry("actions")
            .or_insert_with(|| Value::Array(Vec::new()));
        if let Value::Array(arr) = actions {
            arr.push(Value::Object(ab.build()));
        }
        self
    }

    // ── Advanced configuration ───────────────────────────────────

    pub fn with_refresh(
        &mut self,
        configure: impl FnOnce(&mut RefreshBuilder),
    ) -> &mut Self {
        let mut rb = RefreshBuilder::new();
        configure(&mut rb);
        self.data
            .insert("refresh".into(), Value::Object(rb.build()));
        self
    }

    pub fn with_authentication(
        &mut self,
        configure: impl FnOnce(&mut AuthenticationBuilder),
    ) -> &mut Self {
        let mut authb = AuthenticationBuilder::new();
        configure(&mut authb);
        self.data
            .insert("authentication".into(), Value::Object(authb.build()));
        self
    }

    /// Returns the completed Adaptive Card as a `Card`.
    pub fn build(&self) -> Card {
        self.data.clone()
    }

    fn push_body(&mut self, element: Value) {
        let body = self
            .data
            .entry("body")
            .or_insert_with(|| Value::Array(Vec::new()));
        if let Value::Array(arr) = body {
            arr.push(element);
        }
    }
}

impl Default for AdaptiveCardBuilder {
    fn default() -> Self {
        Self::new()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn default_version_and_schema() {
        let card = AdaptiveCardBuilder::new().build();
        assert_eq!(card["type"].as_str().unwrap(), "AdaptiveCard");
        assert_eq!(card["version"].as_str().unwrap(), "1.5");
        assert!(card["$schema"].as_str().unwrap().contains("1.5"));
    }

    #[test]
    fn with_version() {
        let card = AdaptiveCardBuilder::new().with_version("1.6").build();
        assert_eq!(card["version"].as_str().unwrap(), "1.6");
        assert!(card["$schema"].as_str().unwrap().contains("1.6"));
    }

    #[test]
    fn with_schema_override() {
        let card = AdaptiveCardBuilder::new()
            .with_schema("https://example.com/custom-schema.json")
            .build();
        assert_eq!(
            card["$schema"].as_str().unwrap(),
            "https://example.com/custom-schema.json"
        );
    }

    #[test]
    fn add_text_block() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| {
                tb.with_text("Hello, World!");
            })
            .build();
        let body = card["body"].as_array().unwrap();
        assert_eq!(body.len(), 1);
        let el = body[0].as_object().unwrap();
        assert_eq!(el["type"].as_str().unwrap(), "TextBlock");
        assert_eq!(el["text"].as_str().unwrap(), "Hello, World!");
    }

    #[test]
    fn add_action() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("x"); })
            .add_action(|a| {
                a.submit(Some("Click me"));
            })
            .build();
        let actions = card["actions"].as_array().unwrap();
        assert_eq!(actions.len(), 1);
        let action = actions[0].as_object().unwrap();
        assert_eq!(action["type"].as_str().unwrap(), "Action.Submit");
        assert_eq!(action["title"].as_str().unwrap(), "Click me");
    }

    #[test]
    fn multiple_body_elements() {
        let card = AdaptiveCardBuilder::new()
            .add_text_block(|tb| { tb.with_text("First"); })
            .add_text_block(|tb| { tb.with_text("Second"); })
            .add_image(|img| { img.with_url("https://example.com/img.png"); })
            .build();
        let body = card["body"].as_array().unwrap();
        assert_eq!(body.len(), 3);
    }

    #[test]
    fn with_metadata() {
        let card = AdaptiveCardBuilder::new()
            .with_metadata("https://example.com/card")
            .build();
        let meta = card["metadata"].as_object().unwrap();
        assert_eq!(meta["webUrl"].as_str().unwrap(), "https://example.com/card");
    }

    #[test]
    fn with_refresh() {
        let card = AdaptiveCardBuilder::new()
            .with_refresh(|r| {
                r.add_user_id("user1").with_expires("2026-01-01T00:00:00Z");
            })
            .build();
        let refresh = card["refresh"].as_object().unwrap();
        assert_eq!(refresh["expires"].as_str().unwrap(), "2026-01-01T00:00:00Z");
        let user_ids = refresh["userIds"].as_array().unwrap();
        assert_eq!(user_ids[0].as_str().unwrap(), "user1");
    }

    #[test]
    fn add_element_pre_built() {
        let mut prebuilt = Card::new();
        prebuilt.insert("type".into(), Value::String("TextBlock".into()));
        prebuilt.insert("text".into(), Value::String("Pre-built".into()));
        let card = AdaptiveCardBuilder::new().add_element(prebuilt).build();
        let body = card["body"].as_array().unwrap();
        assert_eq!(body.len(), 1);
        assert_eq!(
            body[0].as_object().unwrap()["text"].as_str().unwrap(),
            "Pre-built"
        );
    }
}
