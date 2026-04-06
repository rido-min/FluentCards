package fluentcards

// TableBuilder builds a Table Adaptive Card element (requires Adaptive Cards 1.5+).
type TableBuilder struct {
	data map[string]any
}

func newTableBuilder() *TableBuilder {
	return &TableBuilder{data: map[string]any{"type": "Table", "columns": []any{}, "rows": []any{}}}
}

func (b *TableBuilder) WithID(id string) *TableBuilder {
	b.data["id"] = id
	return b
}

func (b *TableBuilder) WithFirstRowAsHeader(firstRowAsHeader bool) *TableBuilder {
	b.data["firstRowAsHeader"] = firstRowAsHeader
	return b
}

func (b *TableBuilder) WithShowGridLines(showGridLines bool) *TableBuilder {
	b.data["showGridLines"] = showGridLines
	return b
}

func (b *TableBuilder) WithGridStyle(gridStyle ContainerStyle) *TableBuilder {
	b.data["gridStyle"] = string(gridStyle)
	return b
}

func (b *TableBuilder) WithHorizontalCellContentAlignment(alignment HorizontalAlignment) *TableBuilder {
	b.data["horizontalCellContentAlignment"] = string(alignment)
	return b
}

func (b *TableBuilder) WithVerticalCellContentAlignment(alignment VerticalAlignment) *TableBuilder {
	b.data["verticalCellContentAlignment"] = string(alignment)
	return b
}

func (b *TableBuilder) WithSpacing(spacing Spacing) *TableBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

// AddColumn adds a table column definition map (e.g. {"width": 1}).
func (b *TableBuilder) AddColumn(column map[string]any) *TableBuilder {
	cols := b.data["columns"].([]any)
	b.data["columns"] = append(cols, column)
	return b
}

// AddRow adds a table row map (e.g. {"cells": [...]}).
func (b *TableBuilder) AddRow(row map[string]any) *TableBuilder {
	rows := b.data["rows"].([]any)
	b.data["rows"] = append(rows, row)
	return b
}

func (b *TableBuilder) Build() Card {
	return b.data
}
