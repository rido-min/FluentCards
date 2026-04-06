package fluentcards

// ColumnSetBuilder builds a ColumnSet Adaptive Card element.
type ColumnSetBuilder struct {
	data map[string]any
}

func newColumnSetBuilder() *ColumnSetBuilder {
	return &ColumnSetBuilder{data: map[string]any{"type": "ColumnSet", "columns": []any{}}}
}

func (b *ColumnSetBuilder) WithID(id string) *ColumnSetBuilder {
	b.data["id"] = id
	return b
}

func (b *ColumnSetBuilder) WithStyle(style ContainerStyle) *ColumnSetBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *ColumnSetBuilder) WithBleed(bleed bool) *ColumnSetBuilder {
	b.data["bleed"] = bleed
	return b
}

func (b *ColumnSetBuilder) WithMinHeight(minHeight string) *ColumnSetBuilder {
	b.data["minHeight"] = minHeight
	return b
}

func (b *ColumnSetBuilder) WithHorizontalAlignment(alignment HorizontalAlignment) *ColumnSetBuilder {
	b.data["horizontalAlignment"] = string(alignment)
	return b
}

func (b *ColumnSetBuilder) WithSpacing(spacing Spacing) *ColumnSetBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *ColumnSetBuilder) WithSeparator(separator bool) *ColumnSetBuilder {
	b.data["separator"] = separator
	return b
}

func (b *ColumnSetBuilder) WithSelectAction(configure func(*ActionBuilder)) *ColumnSetBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

// AddColumn adds a column configured by the provided function.
func (b *ColumnSetBuilder) AddColumn(configure func(*ColumnBuilder)) *ColumnSetBuilder {
	cb := newColumnBuilder()
	configure(cb)
	cols := b.data["columns"].([]any)
	b.data["columns"] = append(cols, cb.Build())
	return b
}

// AddColumnWithWidth adds a column with an explicit width string plus additional configuration.
func (b *ColumnSetBuilder) AddColumnWithWidth(width string, configure func(*ColumnBuilder)) *ColumnSetBuilder {
	cb := newColumnBuilder()
	cb.WithWidth(width)
	configure(cb)
	cols := b.data["columns"].([]any)
	b.data["columns"] = append(cols, cb.Build())
	return b
}

func (b *ColumnSetBuilder) Build() Card {
	return b.data
}
