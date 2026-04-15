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

func (b *InputToggleBuilder) WithIsVisible(isVisible bool) *InputToggleBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *InputToggleBuilder) WithSeparator(separator bool) *InputToggleBuilder {
	b.data["separator"] = separator
	return b
}

func (b *InputToggleBuilder) WithHeight(height string) *InputToggleBuilder {
	b.data["height"] = height
	return b
}

func (b *InputToggleBuilder) WithFallback(fallback any) *InputToggleBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *InputToggleBuilder) WithRequires(key, version string) *InputToggleBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *InputToggleBuilder) WithRtl(rtl bool) *InputToggleBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *InputToggleBuilder) WithLabelPosition(position InputLabelPosition) *InputToggleBuilder {
	b.data["labelPosition"] = string(position)
	return b
}

func (b *InputToggleBuilder) WithLabelWidth(width string) *InputToggleBuilder {
	b.data["labelWidth"] = width
	return b
}

func (b *InputToggleBuilder) WithInputStyle(style InputStyle) *InputToggleBuilder {
	b.data["inputStyle"] = string(style)
	return b
}

func (b *InputToggleBuilder) Build()Card {
	return b.data
}
