use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::action_builder::ActionBuilder;
use super::background_image_builder::BackgroundImageBuilder;
use super::text_block_builder::TextBlockBuilder;
use super::image_builder::ImageBuilder;
use super::column_set_builder::ColumnSetBuilder;
use super::fact_set_builder::FactSetBuilder;
use super::rich_text_block_builder::RichTextBlockBuilder;
use super::action_set_builder::ActionSetBuilder;

/// Builds a Container Adaptive Card element.
pub struct ContainerBuilder {
    data: Card,
}

impl ContainerBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Container".into()));
        data.insert("items".into(), Value::Array(Vec::new()));
        ContainerBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_style(&mut self, style: ContainerStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
        self
    }

    pub fn with_vertical_content_alignment(&mut self, alignment: VerticalAlignment) -> &mut Self {
        self.data
            .insert("verticalContentAlignment".into(), alignment.into());
        self
    }

    pub fn with_bleed(&mut self, bleed: bool) -> &mut Self {
        self.data.insert("bleed".into(), Value::Bool(bleed));
        self
    }

    pub fn with_min_height(&mut self, min_height: &str) -> &mut Self {
        self.data
            .insert("minHeight".into(), Value::String(min_height.into()));
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

    pub fn add_text_block(
        &mut self,
        configure: impl FnOnce(&mut TextBlockBuilder),
    ) -> &mut Self {
        let mut tb = TextBlockBuilder::new();
        configure(&mut tb);
        self.push_item(Value::Object(tb.build()));
        self
    }

    pub fn add_image(
        &mut self,
        configure: impl FnOnce(&mut ImageBuilder),
    ) -> &mut Self {
        let mut ib = ImageBuilder::new();
        configure(&mut ib);
        self.push_item(Value::Object(ib.build()));
        self
    }

    pub fn add_container(
        &mut self,
        configure: impl FnOnce(&mut ContainerBuilder),
    ) -> &mut Self {
        let mut cb = ContainerBuilder::new();
        configure(&mut cb);
        self.push_item(Value::Object(cb.build()));
        self
    }

    pub fn add_column_set(
        &mut self,
        configure: impl FnOnce(&mut ColumnSetBuilder),
    ) -> &mut Self {
        let mut cs = ColumnSetBuilder::new();
        configure(&mut cs);
        self.push_item(Value::Object(cs.build()));
        self
    }

    pub fn add_fact_set(
        &mut self,
        configure: impl FnOnce(&mut FactSetBuilder),
    ) -> &mut Self {
        let mut fs = FactSetBuilder::new();
        configure(&mut fs);
        self.push_item(Value::Object(fs.build()));
        self
    }

    pub fn add_rich_text_block(
        &mut self,
        configure: impl FnOnce(&mut RichTextBlockBuilder),
    ) -> &mut Self {
        let mut rtb = RichTextBlockBuilder::new();
        configure(&mut rtb);
        self.push_item(Value::Object(rtb.build()));
        self
    }

    pub fn add_action_set(
        &mut self,
        configure: impl FnOnce(&mut ActionSetBuilder),
    ) -> &mut Self {
        let mut asb = ActionSetBuilder::new();
        configure(&mut asb);
        self.push_item(Value::Object(asb.build()));
        self
    }

    /// Adds a pre-built element `Card` directly.
    pub fn add_element(&mut self, element: Card) -> &mut Self {
        self.push_item(Value::Object(element));
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }

    fn push_item(&mut self, element: Value) {
        if let Some(Value::Array(arr)) = self.data.get_mut("items") {
            arr.push(element);
        }
    }
}
