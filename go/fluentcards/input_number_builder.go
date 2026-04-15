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

func (b *InputNumberBuilder) WithIsVisible(isVisible bool) *InputNumberBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *InputNumberBuilder) WithSeparator(separator bool) *InputNumberBuilder {
	b.data["separator"] = separator
	return b
}

func (b *InputNumberBuilder) WithHeight(height string) *InputNumberBuilder {
	b.data["height"] = height
	return b
}

func (b *InputNumberBuilder) WithFallback(fallback any) *InputNumberBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *InputNumberBuilder) WithRequires(key, version string) *InputNumberBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *InputNumberBuilder) WithRtl(rtl bool) *InputNumberBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *InputNumberBuilder) WithLabelPosition(position InputLabelPosition) *InputNumberBuilder {
	b.data["labelPosition"] = string(position)
	return b
}

func (b *InputNumberBuilder) WithLabelWidth(width string) *InputNumberBuilder {
	b.data["labelWidth"] = width
	return b
}

func (b *InputNumberBuilder) WithInputStyle(style InputStyle) *InputNumberBuilder {
	b.data["inputStyle"] = string(style)
	return b
}

func (b *InputNumberBuilder) Build()Card {
	return b.data
}
