use serde_json::Value;

use crate::enums::*;
use crate::models::Card;

/// Builds a Table Adaptive Card element (requires Adaptive Cards 1.5+).
pub struct TableBuilder {
    data: Card,
}

impl TableBuilder {
    pub(crate) fn new() -> Self {
        let mut data = Card::new();
        data.insert("type".into(), Value::String("Table".into()));
        data.insert("columns".into(), Value::Array(Vec::new()));
        data.insert("rows".into(), Value::Array(Vec::new()));
        TableBuilder { data }
    }

    pub fn with_id(&mut self, id: &str) -> &mut Self {
        self.data.insert("id".into(), Value::String(id.into()));
        self
    }

    pub fn with_first_row_as_header(&mut self, first_row_as_header: bool) -> &mut Self {
        self.data
            .insert("firstRowAsHeader".into(), Value::Bool(first_row_as_header));
        self
    }

    pub fn with_show_grid_lines(&mut self, show_grid_lines: bool) -> &mut Self {
        self.data
            .insert("showGridLines".into(), Value::Bool(show_grid_lines));
        self
    }

    pub fn with_grid_style(&mut self, grid_style: ContainerStyle) -> &mut Self {
        self.data.insert("gridStyle".into(), grid_style.into());
        self
    }

    pub fn with_horizontal_cell_content_alignment(
        &mut self,
        alignment: HorizontalAlignment,
    ) -> &mut Self {
        self.data
            .insert("horizontalCellContentAlignment".into(), alignment.into());
        self
    }

    pub fn with_vertical_cell_content_alignment(
        &mut self,
        alignment: VerticalAlignment,
    ) -> &mut Self {
        self.data
            .insert("verticalCellContentAlignment".into(), alignment.into());
        self
    }

    pub fn with_spacing(&mut self, spacing: Spacing) -> &mut Self {
        self.data.insert("spacing".into(), spacing.into());
        self
    }

    /// Adds a table column definition map (e.g. `{"width": 1}`).
    pub fn add_column(&mut self, column: Card) -> &mut Self {
        if let Some(Value::Array(cols)) = self.data.get_mut("columns") {
            cols.push(Value::Object(column));
        }
        self
    }

    /// Adds a table row map (e.g. `{"cells": [...]}`).
    pub fn add_row(&mut self, row: Card) -> &mut Self {
        if let Some(Value::Array(rows)) = self.data.get_mut("rows") {
            rows.push(Value::Object(row));
        }
        self
    }

    pub fn build(&self) -> Card {
        self.data.clone()
    }
}
