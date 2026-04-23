package fluentcards

// FactSetBuilder builds a FactSet Adaptive Card element.
type FactSetBuilder struct {
	data map[string]any
}

func newFactSetBuilder() *FactSetBuilder {
	return &FactSetBuilder{data: map[string]any{"type": "FactSet", "facts": []any{}}}
}

func (b *FactSetBuilder) WithID(id string) *FactSetBuilder {
	b.data["id"] = id
	return b
}

func (b *FactSetBuilder) WithSpacing(spacing Spacing) *FactSetBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *FactSetBuilder) WithIsVisible(isVisible bool) *FactSetBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *FactSetBuilder) WithSeparator(separator bool) *FactSetBuilder {
	b.data["separator"] = separator
	return b
}

func (b *FactSetBuilder) WithHeight(height string) *FactSetBuilder {
	b.data["height"] = height
	return b
}

func (b *FactSetBuilder) WithFallback(fallback any) *FactSetBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *FactSetBuilder) WithRequires(key, version string) *FactSetBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *FactSetBuilder) WithRtl(rtl bool) *FactSetBuilder {
	b.data["rtl"] = rtl
	return b
}

// AddFactadds a fact with the given title and value strings.
func (b *FactSetBuilder) AddFact(title, value string) *FactSetBuilder {
	facts := b.data["facts"].([]any)
	b.data["facts"] = append(facts, map[string]any{"title": title, "value": value})
	return b
}

// AddFactMap adds a pre-built fact map directly.
func (b *FactSetBuilder) AddFactMap(fact map[string]any) *FactSetBuilder {
	facts := b.data["facts"].([]any)
	b.data["facts"] = append(facts, fact)
	return b
}

func (b *FactSetBuilder) Build() Card {
	return b.data
}
