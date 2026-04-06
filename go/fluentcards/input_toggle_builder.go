package fluentcards

// InputToggleBuilder builds an Input.Toggle Adaptive Card element.
type InputToggleBuilder struct {
	data map[string]any
}

func newInputToggleBuilder() *InputToggleBuilder {
	return &InputToggleBuilder{data: map[string]any{"type": "Input.Toggle", "id": "", "title": ""}}
}

func (b *InputToggleBuilder) WithID(id string) *InputToggleBuilder {
	b.data["id"] = id
	return b
}

func (b *InputToggleBuilder) WithTitle(title string) *InputToggleBuilder {
	b.data["title"] = title
	return b
}

func (b *InputToggleBuilder) WithLabel(label string) *InputToggleBuilder {
	b.data["label"] = label
	return b
}

func (b *InputToggleBuilder) WithValue(value string) *InputToggleBuilder {
	b.data["value"] = value
	return b
}

func (b *InputToggleBuilder) WithValueOn(valueOn string) *InputToggleBuilder {
	b.data["valueOn"] = valueOn
	return b
}

func (b *InputToggleBuilder) WithValueOff(valueOff string) *InputToggleBuilder {
	b.data["valueOff"] = valueOff
	return b
}

func (b *InputToggleBuilder) WithWrap(wrap bool) *InputToggleBuilder {
	b.data["wrap"] = wrap
	return b
}

func (b *InputToggleBuilder) WithIsRequired(isRequired bool) *InputToggleBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputToggleBuilder) WithErrorMessage(errorMessage string) *InputToggleBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputToggleBuilder) WithSpacing(spacing Spacing) *InputToggleBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputToggleBuilder) Build() Card {
	return b.data
}
