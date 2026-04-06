package fluentcards

// InputTextBuilder builds an Input.Text Adaptive Card element.
type InputTextBuilder struct {
	data map[string]any
}

func newInputTextBuilder() *InputTextBuilder {
	return &InputTextBuilder{data: map[string]any{"type": "Input.Text", "id": ""}}
}

func (b *InputTextBuilder) WithID(id string) *InputTextBuilder {
	b.data["id"] = id
	return b
}

func (b *InputTextBuilder) WithLabel(label string) *InputTextBuilder {
	b.data["label"] = label
	return b
}

func (b *InputTextBuilder) WithPlaceholder(placeholder string) *InputTextBuilder {
	b.data["placeholder"] = placeholder
	return b
}

func (b *InputTextBuilder) WithValue(value string) *InputTextBuilder {
	b.data["value"] = value
	return b
}

func (b *InputTextBuilder) WithMaxLength(maxLength int) *InputTextBuilder {
	b.data["maxLength"] = maxLength
	return b
}

func (b *InputTextBuilder) WithIsMultiline(isMultiline bool) *InputTextBuilder {
	b.data["isMultiline"] = isMultiline
	return b
}

func (b *InputTextBuilder) WithStyle(style TextInputStyle) *InputTextBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *InputTextBuilder) WithRegex(regex string) *InputTextBuilder {
	b.data["regex"] = regex
	return b
}

func (b *InputTextBuilder) WithIsRequired(isRequired bool) *InputTextBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputTextBuilder) WithErrorMessage(errorMessage string) *InputTextBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputTextBuilder) WithSpacing(spacing Spacing) *InputTextBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputTextBuilder) WithInlineAction(configure func(*ActionBuilder)) *InputTextBuilder {
	ab := newActionBuilder()
	configure(ab)
	b.data["inlineAction"] = ab.Build()
	return b
}

func (b *InputTextBuilder) Build() Card {
	return b.data
}
