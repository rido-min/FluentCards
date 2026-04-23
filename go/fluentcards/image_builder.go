package fluentcards

// ImageBuilder builds an Image Adaptive Card element.
type ImageBuilder struct {
	data map[string]any
}

func newImageBuilder() *ImageBuilder {
	return &ImageBuilder{data: map[string]any{"type": "Image"}}
}

func (b *ImageBuilder) WithID(id string) *ImageBuilder {
	b.data["id"] = id
	return b
}

func (b *ImageBuilder) WithURL(url string) *ImageBuilder {
	b.data["url"] = url
	return b
}

func (b *ImageBuilder) WithAltText(altText string) *ImageBuilder {
	b.data["altText"] = altText
	return b
}

func (b *ImageBuilder) WithSize(size ImageSize) *ImageBuilder {
	b.data["size"] = string(size)
	return b
}

func (b *ImageBuilder) WithStyle(style ImageStyle) *ImageBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *ImageBuilder) WithWidth(width string) *ImageBuilder {
	b.data["width"] = width
	return b
}

func (b *ImageBuilder) WithHeight(height string) *ImageBuilder {
	b.data["height"] = height
	return b
}

func (b *ImageBuilder) WithHorizontalAlignment(alignment HorizontalAlignment) *ImageBuilder {
	b.data["horizontalAlignment"] = string(alignment)
	return b
}

func (b *ImageBuilder) WithBackgroundColor(color string) *ImageBuilder {
	b.data["backgroundColor"] = color
	return b
}

func (b *ImageBuilder) WithSpacing(spacing Spacing) *ImageBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *ImageBuilder) WithSeparator(separator bool) *ImageBuilder {
	b.data["separator"] = separator
	return b
}

func (b *ImageBuilder) WithIsVisible(isVisible bool) *ImageBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *ImageBuilder) WithFallback(fallback any) *ImageBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *ImageBuilder) WithRequires(key, version string) *ImageBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *ImageBuilder) WithRtl(rtl bool) *ImageBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *ImageBuilder) WithSelectAction(configure func(*ActionBuilder)) *ImageBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["selectAction"] = ab.Build()
	return b
}

func (b *ImageBuilder) Build() Card {
	return b.data
}
