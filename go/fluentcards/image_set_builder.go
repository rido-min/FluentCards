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

// AddImage adds an image configured by the provided function.
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
