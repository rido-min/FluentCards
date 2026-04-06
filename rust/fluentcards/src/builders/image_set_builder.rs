use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::image_builder::ImageBuilder;

/// Builds an ImageSet Adaptive Card element.
pub struct ImageSetBuilder {
    data: Card,
}

impl ImageSetBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("ImageSet".into()));
        data.insert("images".into(), Value::Array(Vec::new()));
        ImageSetBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_image_size(&mut self, size: ImageSize) -> &mut Self {
        self.data.insert("imageSize".into(), size.into());
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    /// Adds an image configured by the provided closure.
    pub fn add_image(
        &mut self,
        configure: impl FnOnce(&mut ImageBuilder),
    ) -> &mut Self {
        let mut ib = ImageBuilder::new();
        configure(&mut ib);
        if let Some(Value::Array(images)) = self.data.get_mut("images") {
            images.push(Value::Object(ib.build()));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
