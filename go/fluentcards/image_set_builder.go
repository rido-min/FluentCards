package fluentcards

// ImageSetBuilder builds an ImageSet Adaptive Card element.
type ImageSetBuilder struct {
	data map[string]any
}

func newImageSetBuilder() *ImageSetBuilder {
	return &ImageSetBuilder{data: map[string]any{"type": "ImageSet", "images": []any{}}}
}

func (b *ImageSetBuilder) WithID(id string) *ImageSetBuilder {
	b.data["id"] = id
	return b
}

func (b *ImageSetBuilder) WithImageSize(size ImageSize) *ImageSetBuilder {
	b.data["imageSize"] = string(size)
	return b
}

func (b *ImageSetBuilder) WithSpacing(spacing Spacing) *ImageSetBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *ImageSetBuilder) WithIsVisible(isVisible bool) *ImageSetBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *ImageSetBuilder) WithSeparator(separator bool) *ImageSetBuilder {
	b.data["separator"] = separator
	return b
}

func (b *ImageSetBuilder) WithHeight(height string) *ImageSetBuilder {
	b.data["height"] = height
	return b
}

func (b *ImageSetBuilder) WithFallback(fallback any) *ImageSetBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *ImageSetBuilder) WithRequires(key, version string) *ImageSetBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *ImageSetBuilder) WithRtl(rtl bool) *ImageSetBuilder {
	b.data["rtl"] = rtl
	return b
}

// AddImageadds an image configured by the provided function.
func (b *ImageSetBuilder) AddImage(configure func(*ImageBuilder)) *ImageSetBuilder {
	ib := newImageBuilder()
	configure(ib)
	images := b.data["images"].([]any)
	b.data["images"] = append(images, ib.Build())
	return b
}

func (b *ImageSetBuilder) Build() Card {
	return b.data
}
