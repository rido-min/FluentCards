package fluentcards

// TextRunBuilder builds a TextRun inline element for use within RichTextBlock.
type TextRunBuilder struct {
	data map[string]any
}

func newTextRunBuilder() *TextRunBuilder {
	return &TextRunBuilder{data: map[string]any{"type": "TextRun"}}
}

func (b *TextRunBuilder) WithText(text string) *TextRunBuilder {
	b.data["text"] = text
	return b
}

func (b *TextRunBuilder) WithSize(size TextSize) *TextRunBuilder {
	b.data["size"] = string(size)
	return b
}

func (b *TextRunBuilder) WithWeight(weight TextWeight) *TextRunBuilder {
	b.data["weight"] = string(weight)
	return b
}

func (b *TextRunBuilder) WithColor(color TextColor) *TextRunBuilder {
	b.data["color"] = string(color)
	return b
}

func (b *TextRunBuilder) WithIsSubtle(subtle bool) *TextRunBuilder {
	b.data["isSubtle"] = subtle
	return b
}

func (b *TextRunBuilder) WithItalic(italic bool) *TextRunBuilder {
	b.data["italic"] = italic
	return b
}

func (b *TextRunBuilder) WithStrikethrough(strikethrough bool) *TextRunBuilder {
	b.data["strikethrough"] = strikethrough
	return b
}

func (b *TextRunBuilder) WithUnderline(underline bool) *TextRunBuilder {
	b.data["underline"] = underline
	return b
}

func (b *TextRunBuilder) WithHighlight(highlight bool) *TextRunBuilder {
	b.data["highlight"] = highlight
	return b
}

func (b *TextRunBuilder) WithSelectAction(configure func(*ActionBuilder)) *TextRunBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

func (b *TextRunBuilder) Build() Card {
	return b.data
}
