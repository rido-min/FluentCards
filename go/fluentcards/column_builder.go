package fluentcards

// ColumnBuilder builds a Column element within a ColumnSet.
type ColumnBuilder struct {
	data map[string]any
}

func newColumnBuilder() *ColumnBuilder {
	return &ColumnBuilder{data: map[string]any{"type": "Column", "items": []any{}}}
}

func (b *ColumnBuilder) WithID(id string) *ColumnBuilder {
	b.data["id"] = id
	return b
}

func (b *ColumnBuilder) WithWidth(width string) *ColumnBuilder {
	b.data["width"] = width
	return b
}

func (b *ColumnBuilder) WithStyle(style ContainerStyle) *ColumnBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *ColumnBuilder) WithVerticalContentAlignment(alignment VerticalAlignment) *ColumnBuilder {
	b.data["verticalContentAlignment"] = string(alignment)
	return b
}

func (b *ColumnBuilder) WithBleed(bleed bool) *ColumnBuilder {
	b.data["bleed"] = bleed
	return b
}

func (b *ColumnBuilder) WithMinHeight(minHeight string) *ColumnBuilder {
	b.data["minHeight"] = minHeight
	return b
}

func (b *ColumnBuilder) WithBackgroundImage(configure func(*BackgroundImageBuilder)) *ColumnBuilder {
	bib := newBackgroundImageBuilder()
	configure(bib)
	b.data["backgroundImage"] = bib.Build()
	return b
}

func (b *ColumnBuilder) WithSelectAction(configure func(*ActionBuilder)) *ColumnBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

func (b *ColumnBuilder) AddTextBlock(configure func(*TextBlockBuilder)) *ColumnBuilder {
	tb := newTextBlockBuilder()
	configure(tb)
	b.data["items"] = append(b.data["items"].([]any), tb.Build())
	return b
}

func (b *ColumnBuilder) AddImage(configure func(*ImageBuilder)) *ColumnBuilder {
	ib := newImageBuilder()
	configure(ib)
	b.data["items"] = append(b.data["items"].([]any), ib.Build())
	return b
}

func (b *ColumnBuilder) AddContainer(configure func(*ContainerBuilder)) *ColumnBuilder {
	cb := newContainerBuilder()
	configure(cb)
	b.data["items"] = append(b.data["items"].([]any), cb.Build())
	return b
}

func (b *ColumnBuilder) AddElement(element Card) *ColumnBuilder {
	b.data["items"] = append(b.data["items"].([]any), element)
	return b
}

func (b *ColumnBuilder) Build() Card {
	return b.data
}
