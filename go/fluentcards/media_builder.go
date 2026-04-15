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

func (b *MediaBuilder) WithIsVisible(isVisible bool) *MediaBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *MediaBuilder) WithSeparator(separator bool) *MediaBuilder {
	b.data["separator"] = separator
	return b
}

func (b *MediaBuilder) WithHeight(height string) *MediaBuilder {
	b.data["height"] = height
	return b
}

func (b *MediaBuilder) WithFallback(fallback any) *MediaBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *MediaBuilder) WithRequires(key, version string) *MediaBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *MediaBuilder) WithRtl(rtl bool) *MediaBuilder {
	b.data["rtl"] = rtl
	return b
}

// AddSource addsa media source with the given URL and MIME type.
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
