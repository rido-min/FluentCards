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

func (b *InputTextBuilder) WithIsVisible(isVisible bool) *InputTextBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *InputTextBuilder) WithSeparator(separator bool) *InputTextBuilder {
	b.data["separator"] = separator
	return b
}

func (b *InputTextBuilder) WithHeight(height string) *InputTextBuilder {
	b.data["height"] = height
	return b
}

func (b *InputTextBuilder) WithFallback(fallback any) *InputTextBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *InputTextBuilder) WithRequires(key, version string) *InputTextBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *InputTextBuilder) WithRtl(rtl bool) *InputTextBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *InputTextBuilder) WithLabelPosition(position InputLabelPosition) *InputTextBuilder {
	b.data["labelPosition"] = string(position)
	return b
}

func (b *InputTextBuilder) WithLabelWidth(width string) *InputTextBuilder {
	b.data["labelWidth"] = width
	return b
}

func (b *InputTextBuilder) WithInputStyle(style InputStyle) *InputTextBuilder {
	b.data["inputStyle"] = string(style)
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
