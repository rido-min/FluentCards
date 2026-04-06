package fluentcards

// ContainerBuilder builds a Container Adaptive Card element.
type ContainerBuilder struct {
	data map[string]any
}

func newContainerBuilder() *ContainerBuilder {
	return &ContainerBuilder{data: map[string]any{"type": "Container", "items": []any{}}}
}

func (b *ContainerBuilder) WithID(id string) *ContainerBuilder {
	b.data["id"] = id
	return b
}

func (b *ContainerBuilder) WithStyle(style ContainerStyle) *ContainerBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *ContainerBuilder) WithVerticalContentAlignment(alignment VerticalAlignment) *ContainerBuilder {
	b.data["verticalContentAlignment"] = string(alignment)
	return b
}

func (b *ContainerBuilder) WithBleed(bleed bool) *ContainerBuilder {
	b.data["bleed"] = bleed
	return b
}

func (b *ContainerBuilder) WithMinHeight(minHeight string) *ContainerBuilder {
	b.data["minHeight"] = minHeight
	return b
}

func (b *ContainerBuilder) WithSpacing(spacing Spacing) *ContainerBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *ContainerBuilder) WithSeparator(separator bool) *ContainerBuilder {
	b.data["separator"] = separator
	return b
}

func (b *ContainerBuilder) WithIsVisible(isVisible bool) *ContainerBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *ContainerBuilder) WithBackgroundImage(configure func(*BackgroundImageBuilder)) *ContainerBuilder {
	bib := newBackgroundImageBuilder()
	configure(bib)
	b.data["backgroundImage"] = bib.Build()
	return b
}

func (b *ContainerBuilder) WithSelectAction(configure func(*ActionBuilder)) *ContainerBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

func (b *ContainerBuilder) AddTextBlock(configure func(*TextBlockBuilder)) *ContainerBuilder {
	tb := newTextBlockBuilder()
	configure(tb)
	b.data["items"] = append(b.data["items"].([]any), tb.Build())
	return b
}

func (b *ContainerBuilder) AddImage(configure func(*ImageBuilder)) *ContainerBuilder {
	ib := newImageBuilder()
	configure(ib)
	b.data["items"] = append(b.data["items"].([]any), ib.Build())
	return b
}

func (b *ContainerBuilder) AddContainer(configure func(*ContainerBuilder)) *ContainerBuilder {
	cb := newContainerBuilder()
	configure(cb)
	b.data["items"] = append(b.data["items"].([]any), cb.Build())
	return b
}

func (b *ContainerBuilder) AddColumnSet(configure func(*ColumnSetBuilder)) *ContainerBuilder {
	cs := newColumnSetBuilder()
	configure(cs)
	b.data["items"] = append(b.data["items"].([]any), cs.Build())
	return b
}

func (b *ContainerBuilder) AddFactSet(configure func(*FactSetBuilder)) *ContainerBuilder {
	fs := newFactSetBuilder()
	configure(fs)
	b.data["items"] = append(b.data["items"].([]any), fs.Build())
	return b
}

func (b *ContainerBuilder) AddRichTextBlock(configure func(*RichTextBlockBuilder)) *ContainerBuilder {
	rtb := newRichTextBlockBuilder()
	configure(rtb)
	b.data["items"] = append(b.data["items"].([]any), rtb.Build())
	return b
}

func (b *ContainerBuilder) AddActionSet(configure func(*ActionSetBuilder)) *ContainerBuilder {
	as := newActionSetBuilder()
	configure(as)
	b.data["items"] = append(b.data["items"].([]any), as.Build())
	return b
}

func (b *ContainerBuilder) AddElement(element Card) *ContainerBuilder {
	b.data["items"] = append(b.data["items"].([]any), element)
	return b
}

func (b *ContainerBuilder) Build() Card {
	return b.data
}
