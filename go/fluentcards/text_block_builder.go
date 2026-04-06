package fluentcards

// TextBlockBuilder builds a TextBlock Adaptive Card element.
type TextBlockBuilder struct {
	data map[string]any
}

func newTextBlockBuilder() *TextBlockBuilder {
	return &TextBlockBuilder{data: map[string]any{"type": "TextBlock", "text": ""}}
}

func (b *TextBlockBuilder) WithID(id string) *TextBlockBuilder {
	b.data["id"] = id
	return b
}

func (b *TextBlockBuilder) WithText(text string) *TextBlockBuilder {
	b.data["text"] = text
	return b
}

func (b *TextBlockBuilder) WithSize(size TextSize) *TextBlockBuilder {
	b.data["size"] = string(size)
	return b
}

func (b *TextBlockBuilder) WithWeight(weight TextWeight) *TextBlockBuilder {
	b.data["weight"] = string(weight)
	return b
}

func (b *TextBlockBuilder) WithColor(color TextColor) *TextBlockBuilder {
	b.data["color"] = string(color)
	return b
}

func (b *TextBlockBuilder) WithIsSubtle(isSubtle bool) *TextBlockBuilder {
	b.data["isSubtle"] = isSubtle
	return b
}

// WithSubtle is a convenience method that sets isSubtle to true.
func (b *TextBlockBuilder) WithSubtle() *TextBlockBuilder {
	return b.WithIsSubtle(true)
}

func (b *TextBlockBuilder) WithWrap(wrap bool) *TextBlockBuilder {
	b.data["wrap"] = wrap
	return b
}

func (b *TextBlockBuilder) WithMaxLines(maxLines int) *TextBlockBuilder {
	b.data["maxLines"] = maxLines
	return b
}

func (b *TextBlockBuilder) WithHorizontalAlignment(alignment HorizontalAlignment) *TextBlockBuilder {
	b.data["horizontalAlignment"] = string(alignment)
	return b
}

func (b *TextBlockBuilder) WithFontType(fontType FontType) *TextBlockBuilder {
	b.data["fontType"] = string(fontType)
	return b
}

func (b *TextBlockBuilder) WithStyle(style TextBlockStyle) *TextBlockBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *TextBlockBuilder) WithSpacing(spacing Spacing) *TextBlockBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *TextBlockBuilder) WithSeparator(separator bool) *TextBlockBuilder {
	b.data["separator"] = separator
	return b
}

func (b *TextBlockBuilder) WithIsVisible(isVisible bool) *TextBlockBuilder {
	b.data["isVisible"] = isVisible
	return b
}

// WithSelectAction sets the selectAction using a pre-built action Card.
func (b *TextBlockBuilder) WithSelectAction(action Card) *TextBlockBuilder {
	b.data["selectAction"] = action
	return b
}

func (b *TextBlockBuilder) Build() Card {
	return b.data
}
