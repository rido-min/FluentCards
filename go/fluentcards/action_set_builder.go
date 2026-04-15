package fluentcards

// ActionSetBuilder builds an ActionSet body element (a group of actions within the card body).
type ActionSetBuilder struct {
	data map[string]any
}

func newActionSetBuilder() *ActionSetBuilder {
	return &ActionSetBuilder{data: map[string]any{"type": "ActionSet", "actions": []any{}}}
}

func (b *ActionSetBuilder) WithID(id string) *ActionSetBuilder {
	b.data["id"] = id
	return b
}

func (b *ActionSetBuilder) WithSpacing(spacing Spacing) *ActionSetBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *ActionSetBuilder) WithIsVisible(isVisible bool) *ActionSetBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *ActionSetBuilder) WithSeparator(separator bool) *ActionSetBuilder {
	b.data["separator"] = separator
	return b
}

func (b *ActionSetBuilder) WithHeight(height string) *ActionSetBuilder {
	b.data["height"] = height
	return b
}

func (b *ActionSetBuilder) WithFallback(fallback any) *ActionSetBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *ActionSetBuilder) WithRequires(key, version string) *ActionSetBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *ActionSetBuilder) WithRtl(rtl bool) *ActionSetBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *ActionSetBuilder) AddAction(configure func(*ActionBuilder)) *ActionSetBuilder {
	ab := newActionBuilder()
	configure(ab)
	actions := b.data["actions"].([]any)
	b.data["actions"] = append(actions, ab.Build())
	return b
}

func (b *ActionSetBuilder) Build() Card {
	return b.data
}
