package fluentcards

// InputNumberBuilder builds an Input.Number Adaptive Card element.
type InputNumberBuilder struct {
	data map[string]any
}

func newInputNumberBuilder() *InputNumberBuilder {
	return &InputNumberBuilder{data: map[string]any{"type": "Input.Number", "id": ""}}
}

func (b *InputNumberBuilder) WithID(id string) *InputNumberBuilder {
	b.data["id"] = id
	return b
}

func (b *InputNumberBuilder) WithLabel(label string) *InputNumberBuilder {
	b.data["label"] = label
	return b
}

func (b *InputNumberBuilder) WithPlaceholder(placeholder string) *InputNumberBuilder {
	b.data["placeholder"] = placeholder
	return b
}

func (b *InputNumberBuilder) WithValue(value float64) *InputNumberBuilder {
	b.data["value"] = value
	return b
}

func (b *InputNumberBuilder) WithMin(min float64) *InputNumberBuilder {
	b.data["min"] = min
	return b
}

func (b *InputNumberBuilder) WithMax(max float64) *InputNumberBuilder {
	b.data["max"] = max
	return b
}

func (b *InputNumberBuilder) WithIsRequired(isRequired bool) *InputNumberBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputNumberBuilder) WithErrorMessage(errorMessage string) *InputNumberBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputNumberBuilder) WithSpacing(spacing Spacing) *InputNumberBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputNumberBuilder) Build() Card {
	return b.data
}
