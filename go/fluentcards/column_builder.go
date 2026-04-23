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

// WithIsVisible sets whether the column is visible.
func (b *ColumnBuilder) WithIsVisible(isVisible bool) *ColumnBuilder {
	b.data["isVisible"] = isVisible
	return b
}

// WithSpacing sets the spacing between this column and the preceding column.
func (b *ColumnBuilder) WithSpacing(spacing Spacing) *ColumnBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

// WithSeparator sets whether a separator line is drawn at the left of the column.
func (b *ColumnBuilder) WithSeparator(separator bool) *ColumnBuilder {
	b.data["separator"] = separator
	return b
}

// WithHeight sets the height of the column ("auto" or "stretch").
func (b *ColumnBuilder) WithHeight(height string) *ColumnBuilder {
	b.data["height"] = height
	return b
}

// WithFallback sets the fallback behavior when the column is unsupported.
func (b *ColumnBuilder) WithFallback(fallback any) *ColumnBuilder {
	b.data["fallback"] = fallback
	return b
}

// WithRequires sets a feature requirement for the column.
func (b *ColumnBuilder) WithRequires(key, version string) *ColumnBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

// WithRtl sets whether content should be presented right to left.
func (b *ColumnBuilder) WithRtl(rtl bool) *ColumnBuilder {
	b.data["rtl"] = rtl
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
