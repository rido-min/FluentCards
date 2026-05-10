use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::action_builder::ActionBuilder;

/// Builds a TextRun inline element for use within RichTextBlock.
pub struct TextRunBuilder {
    data: Card,
}

impl TextRunBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("TextRun".into()));
        TextRunBuilder { data }
    }

    pub fn with_text(&mut self, text: &str) -> &mut Self {
        self.data
            .insert("text".into(), Value::String(text.into()));
        self
    }

    pub fn with_size(&mut self, size: TextSize) -> &mut Self {
        self.data.insert("size".into(), size.into());
        self
    }

    pub fn with_weight(&mut self, weight: TextWeight) -> &mut Self {
        self.data.insert("weight".into(), weight.into());
        self
    }

    pub fn with_color(&mut self, color: TextColor) -> &mut Self {
        self.data.insert("color".into(), color.into());
        self
    }

    pub fn with_is_subtle(&mut self, subtle: bool) -> &mut Self {
        self.data
            .insert("isSubtle".into(), Value::Bool(subtle));
        self
    }

    pub fn with_italic(&mut self, italic: bool) -> &mut Self {
        self.data.insert("italic".into(), Value::Bool(italic));
        self
    }

    pub fn with_strikethrough(&mut self, strikethrough: bool) -> &mut Self {
        self.data
            .insert("strikethrough".into(), Value::Bool(strikethrough));
        self
    }

    pub fn with_underline(&mut self, underline: bool) -> &mut Self {
        self.data
            .insert("underline".into(), Value::Bool(underline));
        self
    }

    pub fn with_highlight(&mut self, highlight: bool) -> &mut Self {
        self.data
            .insert("highlight".into(), Value::Bool(highlight));
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

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
