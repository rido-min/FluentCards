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

func (b *RichTextBlockBuilder) WithIsVisible(isVisible bool) *RichTextBlockBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *RichTextBlockBuilder) WithSeparator(separator bool) *RichTextBlockBuilder {
	b.data["separator"] = separator
	return b
}

func (b *RichTextBlockBuilder) WithHeight(height string) *RichTextBlockBuilder {
	b.data["height"] = height
	return b
}

func (b *RichTextBlockBuilder) WithFallback(fallback any) *RichTextBlockBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *RichTextBlockBuilder) WithRequires(key, version string) *RichTextBlockBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *RichTextBlockBuilder) WithRtl(rtl bool) *RichTextBlockBuilder {
	b.data["rtl"] = rtl
	return b
}

// AddTextadds a plain string inline.
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
