package fluentcards

// RichTextBlockBuilder builds a RichTextBlock Adaptive Card element.
type RichTextBlockBuilder struct {
	data map[string]any
}

func newRichTextBlockBuilder() *RichTextBlockBuilder {
	return &RichTextBlockBuilder{data: map[string]any{"type": "RichTextBlock", "inlines": []any{}}}
}

func (b *RichTextBlockBuilder) WithID(id string) *RichTextBlockBuilder {
	b.data["id"] = id
	return b
}

func (b *RichTextBlockBuilder) WithHorizontalAlignment(alignment HorizontalAlignment) *RichTextBlockBuilder {
	b.data["horizontalAlignment"] = string(alignment)
	return b
}

func (b *RichTextBlockBuilder) WithSpacing(spacing Spacing) *RichTextBlockBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

// AddText adds a plain string inline.
func (b *RichTextBlockBuilder) AddText(text string) *RichTextBlockBuilder {
	inlines := b.data["inlines"].([]any)
	b.data["inlines"] = append(inlines, text)
	return b
}

// AddTextRun adds a TextRun inline configured by the provided function.
func (b *RichTextBlockBuilder) AddTextRun(configure func(*TextRunBuilder)) *RichTextBlockBuilder {
	tb := newTextRunBuilder()
	configure(tb)
	inlines := b.data["inlines"].([]any)
	b.data["inlines"] = append(inlines, tb.Build())
	return b
}

func (b *RichTextBlockBuilder) Build() Card {
	return b.data
}
