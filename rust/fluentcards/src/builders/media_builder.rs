use serde_json::Value;

use crate::enums::Spacing;
use crate::models::Card;

/// Builds a Media Adaptive Card element.
pub struct MediaBuilder {
    data: Card,
}

impl MediaBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Media".into()));
        data.insert("sources".into(), Value::Array(Vec::new()));
        MediaBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_poster(&mut self, poster: &str) -> &mut Self {
        self.data
            .insert("poster".into(), Value::String(poster.into()));
        self
    }

    pub fn with_alt_text(&mut self, alt_text: &str) -> &mut Self {
        self.data
            .insert("altText".into(), Value::String(alt_text.into()));
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    /// Adds a media source with the given URL and MIME type.
    pub fn add_source(&mut self, url: &str, mime_type: &str) -> &mut Self {
        let mut source = Card::new();
        source.insert("url".into(), Value::String(url.into()));
        source.insert("mimeType".into(), Value::String(mime_type.into()));
        if let Some(Value::Array(sources)) = self.data.get_mut("sources") {
            sources.push(Value::Object(source));
        }
        self
    }

    /// Adds a pre-built source map directly.
    pub fn add_source_map(&mut self, source: Card) -> &mut Self {
        if let Some(Value::Array(sources)) = self.data.get_mut("sources") {
            sources.push(Value::Object(source));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
