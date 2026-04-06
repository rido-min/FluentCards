use serde_json::Value;

use crate::enums::*;
use crate::models::Card;

/// Builds a TextBlock Adaptive Card element.
pub struct TextBlockBuilder {
    data: Card,
}

impl TextBlockBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("TextBlock".into()));
        data.insert("text".into(), Value::String(String::new()));
        TextBlockBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
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

    pub fn with_is_subtle(&mut self, is_subtle: bool) -> &mut Self {
        self.data
            .insert("isSubtle".into(), Value::Bool(is_subtle));
        self
    }

    /// Convenience method that sets `isSubtle` to `true`.
    pub fn with_subtle(&mut self) -> &mut Self {
        self.with_is_subtle(true)
    }

    pub fn with_wrap(&mut self, wrap: bool) -> &mut Self {
        self.data.insert("wrap".into(), Value::Bool(wrap));
        self
    }

    pub fn with_max_lines(&mut self, max_lines: i64) -> &mut Self {
        self.data
            .insert("maxLines".into(), Value::Number(max_lines.into()));
        self
    }

    pub fn with_horizontal_alignment(&mut self, alignment: HorizontalAlignment) -> &mut Self {
        self.data
            .insert("horizontalAlignment".into(), alignment.into());
        self
    }

    pub fn with_font_type(&mut self, font_type: FontType) -> &mut Self {
        self.data.insert("fontType".into(), font_type.into());
        self
    }

    pub fn with_style(&mut self, style: TextBlockStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    pub fn with_separator(&mut self, separator: bool) -> &mut Self {
        self.data
            .insert("separator".into(), Value::Bool(separator));
        self
    }

    pub fn with_is_visible(&mut self, is_visible: bool) -> &mut Self {
        self.data
            .insert("isVisible".into(), Value::Bool(is_visible));
        self
    }

    /// Sets the `selectAction` using a pre-built action `Card`.
    pub fn with_select_action(&mut self, action: Card) -> &mut Self {
        self.data
            .insert("selectAction".into(), Value::Object(action));
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
