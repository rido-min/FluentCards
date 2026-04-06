package fluentcards

// BackgroundImageBuilder builds a backgroundImage object for containers and cards.
type BackgroundImageBuilder struct {
	data map[string]any
}

func newBackgroundImageBuilder() *BackgroundImageBuilder {
	return &BackgroundImageBuilder{data: map[string]any{}}
}

func (b *BackgroundImageBuilder) WithURL(url string) *BackgroundImageBuilder {
	b.data["url"] = url
	return b
}

func (b *BackgroundImageBuilder) WithFillMode(fillMode BackgroundImageFillMode) *BackgroundImageBuilder {
	b.data["fillMode"] = string(fillMode)
	return b
}

func (b *BackgroundImageBuilder) WithHorizontalAlignment(alignment HorizontalAlignment) *BackgroundImageBuilder {
	b.data["horizontalAlignment"] = string(alignment)
	return b
}

func (b *BackgroundImageBuilder) WithVerticalAlignment(alignment VerticalAlignment) *BackgroundImageBuilder {
	b.data["verticalAlignment"] = string(alignment)
	return b
}

func (b *BackgroundImageBuilder) Build() Card {
	return b.data
}
