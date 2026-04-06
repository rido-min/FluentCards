use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::text_run_builder::TextRunBuilder;

/// Builds a RichTextBlock Adaptive Card element.
pub struct RichTextBlockBuilder {
    data: Card,
}

impl RichTextBlockBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("RichTextBlock".into()));
        data.insert("inlines".into(), Value::Array(Vec::new()));
        RichTextBlockBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_horizontal_alignment(&mut self, alignment: HorizontalAlignment) -> &mut Self {
        self.data
            .insert("horizontalAlignment".into(), alignment.into());
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    /// Adds a plain string inline.
    pub fn add_text(&mut self, text: &str) -> &mut Self {
        if let Some(Value::Array(inlines)) = self.data.get_mut("inlines") {
            inlines.push(Value::String(text.into()));
        }
        self
    }

    /// Adds a TextRun inline configured by the provided closure.
    pub fn add_text_run(
        &mut self,
        configure: impl FnOnce(&mut TextRunBuilder),
    ) -> &mut Self {
        let mut tb = TextRunBuilder::new();
        configure(&mut tb);
        if let Some(Value::Array(inlines)) = self.data.get_mut("inlines") {
            inlines.push(Value::Object(tb.build()));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
