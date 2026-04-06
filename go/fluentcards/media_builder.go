package fluentcards

// MediaBuilder builds a Media Adaptive Card element.
type MediaBuilder struct {
	data map[string]any
}

func newMediaBuilder() *MediaBuilder {
	return &MediaBuilder{data: map[string]any{"type": "Media", "sources": []any{}}}
}

func (b *MediaBuilder) WithID(id string) *MediaBuilder {
	b.data["id"] = id
	return b
}

func (b *MediaBuilder) WithPoster(poster string) *MediaBuilder {
	b.data["poster"] = poster
	return b
}

func (b *MediaBuilder) WithAltText(altText string) *MediaBuilder {
	b.data["altText"] = altText
	return b
}

func (b *MediaBuilder) WithSpacing(spacing Spacing) *MediaBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

// AddSource adds a media source with the given URL and MIME type.
func (b *MediaBuilder) AddSource(url, mimeType string) *MediaBuilder {
	sources := b.data["sources"].([]any)
	b.data["sources"] = append(sources, map[string]any{"url": url, "mimeType": mimeType})
	return b
}

// AddSourceMap adds a pre-built source map directly.
func (b *MediaBuilder) AddSourceMap(source map[string]any) *MediaBuilder {
	sources := b.data["sources"].([]any)
	b.data["sources"] = append(sources, source)
	return b
}

func (b *MediaBuilder) Build() Card {
	return b.data
}
