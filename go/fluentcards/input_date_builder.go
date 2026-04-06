package fluentcards

// InputDateBuilder builds an Input.Date Adaptive Card element.
type InputDateBuilder struct {
	data map[string]any
}

func newInputDateBuilder() *InputDateBuilder {
	return &InputDateBuilder{data: map[string]any{"type": "Input.Date", "id": ""}}
}

func (b *InputDateBuilder) WithID(id string) *InputDateBuilder {
	b.data["id"] = id
	return b
}

func (b *InputDateBuilder) WithLabel(label string) *InputDateBuilder {
	b.data["label"] = label
	return b
}

func (b *InputDateBuilder) WithPlaceholder(placeholder string) *InputDateBuilder {
	b.data["placeholder"] = placeholder
	return b
}

func (b *InputDateBuilder) WithValue(value string) *InputDateBuilder {
	b.data["value"] = value
	return b
}

// WithMin sets the minimum date (format: YYYY-MM-DD).
func (b *InputDateBuilder) WithMin(min string) *InputDateBuilder {
	b.data["min"] = min
	return b
}

// WithMax sets the maximum date (format: YYYY-MM-DD).
func (b *InputDateBuilder) WithMax(max string) *InputDateBuilder {
	b.data["max"] = max
	return b
}

func (b *InputDateBuilder) WithIsRequired(isRequired bool) *InputDateBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputDateBuilder) WithErrorMessage(errorMessage string) *InputDateBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputDateBuilder) WithSpacing(spacing Spacing) *InputDateBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputDateBuilder) Build() Card {
	return b.data
}
