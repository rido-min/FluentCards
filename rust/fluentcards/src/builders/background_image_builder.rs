use serde_json::Value;

use crate::enums::*;
use crate::models::Card;

/// Builds a backgroundImage object for containers and cards.
pub struct BackgroundImageBuilder {
    data: Card,
}

impl BackgroundImageBuilder {
    pub(crate) fn new() -> Self {
        BackgroundImageBuilder { data: Card::new() }
    }

    pub fn with_url(&mut self, url: &str) -> &mut Self {
        self.data
            .insert("url".into(), Value::String(url.into()));
        self
    }

    pub fn with_fill_mode(&mut self, fill_mode: BackgroundImageFillMode) -> &mut Self {
        self.data.insert("fillMode".into(), fill_mode.into());
        self
    }

    pub fn with_horizontal_alignment(&mut self, alignment: HorizontalAlignment) -> &mut Self {
        self.data
            .insert("horizontalAlignment".into(), alignment.into());
        self
    }

    pub fn with_vertical_alignment(&mut self, alignment: VerticalAlignment) -> &mut Self {
        self.data
            .insert("verticalAlignment".into(), alignment.into());
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
