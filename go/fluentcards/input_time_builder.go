package fluentcards

// InputTimeBuilder builds an Input.Time Adaptive Card element.
type InputTimeBuilder struct {
	data map[string]any
}

func newInputTimeBuilder() *InputTimeBuilder {
	return &InputTimeBuilder{data: map[string]any{"type": "Input.Time", "id": ""}}
}

func (b *InputTimeBuilder) WithID(id string) *InputTimeBuilder {
	b.data["id"] = id
	return b
}

func (b *InputTimeBuilder) WithLabel(label string) *InputTimeBuilder {
	b.data["label"] = label
	return b
}

func (b *InputTimeBuilder) WithPlaceholder(placeholder string) *InputTimeBuilder {
	b.data["placeholder"] = placeholder
	return b
}

func (b *InputTimeBuilder) WithValue(value string) *InputTimeBuilder {
	b.data["value"] = value
	return b
}

// WithMin sets the minimum time (format: HH:MM).
func (b *InputTimeBuilder) WithMin(min string) *InputTimeBuilder {
	b.data["min"] = min
	return b
}

// WithMax sets the maximum time (format: HH:MM).
func (b *InputTimeBuilder) WithMax(max string) *InputTimeBuilder {
	b.data["max"] = max
	return b
}

func (b *InputTimeBuilder) WithIsRequired(isRequired bool) *InputTimeBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputTimeBuilder) WithErrorMessage(errorMessage string) *InputTimeBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputTimeBuilder) WithSpacing(spacing Spacing) *InputTimeBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputTimeBuilder) Build() Card {
	return b.data
}
