use serde_json::Value;

use crate::enums::*;
use crate::models::Card;
use super::action_builder::ActionBuilder;
use super::column_builder::ColumnBuilder;

/// Builds a ColumnSet Adaptive Card element.
pub struct ColumnSetBuilder {
    data: Card,
}

impl ColumnSetBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("ColumnSet".into()));
        data.insert("columns".into(), Value::Array(Vec::new()));
        ColumnSetBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_style(&mut self, style: ContainerStyle) -> &mut Self {
        self.data.insert("style".into(), style.into());
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

    pub fn with_horizontal_alignment(&mut self, alignment: HorizontalAlignment) -> &mut Self {
        self.data
            .insert("horizontalAlignment".into(), alignment.into());
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

    /// Adds a column configured by the provided closure.
    pub fn add_column(
        &mut self,
        configure: impl FnOnce(&mut ColumnBuilder),
    ) -> &mut Self {
        let mut cb = ColumnBuilder::new();
        configure(&mut cb);
        if let Some(Value::Array(cols)) = self.data.get_mut("columns") {
            cols.push(Value::Object(cb.build()));
        }
        self
    }

    /// Adds a column with an explicit width string plus additional configuration.
    pub fn add_column_with_width(
        &mut self,
        width: &str,
        configure: impl FnOnce(&mut ColumnBuilder),
    ) -> &mut Self {
        let mut cb = ColumnBuilder::new();
        cb.with_width(width);
        configure(&mut cb);
        if let Some(Value::Array(cols)) = self.data.get_mut("columns") {
            cols.push(Value::Object(cb.build()));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
