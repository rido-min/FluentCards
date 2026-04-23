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

func (b *InputTimeBuilder) WithIsVisible(isVisible bool) *InputTimeBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *InputTimeBuilder) WithSeparator(separator bool) *InputTimeBuilder {
	b.data["separator"] = separator
	return b
}

func (b *InputTimeBuilder) WithHeight(height string) *InputTimeBuilder {
	b.data["height"] = height
	return b
}

func (b *InputTimeBuilder) WithFallback(fallback any) *InputTimeBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *InputTimeBuilder) WithRequires(key, version string) *InputTimeBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *InputTimeBuilder) WithRtl(rtl bool) *InputTimeBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *InputTimeBuilder) WithLabelPosition(position InputLabelPosition) *InputTimeBuilder {
	b.data["labelPosition"] = string(position)
	return b
}

func (b *InputTimeBuilder) WithLabelWidth(width string) *InputTimeBuilder {
	b.data["labelWidth"] = width
	return b
}

func (b *InputTimeBuilder) WithInputStyle(style InputStyle) *InputTimeBuilder {
	b.data["inputStyle"] = string(style)
	return b
}

func (b *InputTimeBuilder) Build()Card {
	return b.data
}
