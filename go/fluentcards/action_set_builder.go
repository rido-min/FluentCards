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
