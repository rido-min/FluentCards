use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::action_builder::ActionBuilder;

/// Builds an Image Adaptive Card element.
pub struct ImageBuilder {
    data: Card,
}

impl ImageBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Image".into()));
        ImageBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_url(&mut self, url: &str) -> &mut Self {
        self.data
            .insert("url".into(), Value::String(url.into()));
        self
    }

    pub fn with_alt_text(&mut self, alt_text: &str) -> &mut Self {
        self.data
            .insert("altText".into(), Value::String(alt_text.into()));
        self
    }

    pub fn with_size(&mut self, size: ImageSize) -> &mut Self {
        self.data.insert("size".into(), size.into());
        self
    }

    pub fn with_style(&mut self, style: ImageStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
        self
    }

    pub fn with_width(&mut self, width: &str) -> &mut Self {
        self.data
            .insert("width".into(), Value::String(width.into()));
        self
    }

    pub fn with_height(&mut self, height: &str) -> &mut Self {
        self.data
            .insert("height".into(), Value::String(height.into()));
        self
    }

    pub fn with_horizontal_alignment(&mut self, alignment: HorizontalAlignment) -> &mut Self {
        self.data
            .insert("horizontalAlignment".into(), alignment.into());
        self
    }

    pub fn with_background_color(&mut self, color: &str) -> &mut Self {
        self.data
            .insert("backgroundColor".into(), Value::String(color.into()));
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
