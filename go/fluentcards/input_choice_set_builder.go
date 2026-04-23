package fluentcards

// InputChoiceSetBuilder builds an Input.ChoiceSet Adaptive Card element.
type InputChoiceSetBuilder struct {
	data map[string]any
}

func newInputChoiceSetBuilder() *InputChoiceSetBuilder {
	return &InputChoiceSetBuilder{data: map[string]any{"type": "Input.ChoiceSet", "id": "", "choices": []any{}}}
}

func (b *InputChoiceSetBuilder) WithID(id string) *InputChoiceSetBuilder {
	b.data["id"] = id
	return b
}

func (b *InputChoiceSetBuilder) WithLabel(label string) *InputChoiceSetBuilder {
	b.data["label"] = label
	return b
}

func (b *InputChoiceSetBuilder) WithPlaceholder(placeholder string) *InputChoiceSetBuilder {
	b.data["placeholder"] = placeholder
	return b
}

func (b *InputChoiceSetBuilder) WithValue(value string) *InputChoiceSetBuilder {
	b.data["value"] = value
	return b
}

func (b *InputChoiceSetBuilder) WithStyle(style ChoiceInputStyle) *InputChoiceSetBuilder {
	b.data["style"] = string(style)
	return b
}

func (b *InputChoiceSetBuilder) WithIsMultiSelect(isMultiSelect bool) *InputChoiceSetBuilder {
	b.data["isMultiSelect"] = isMultiSelect
	return b
}

func (b *InputChoiceSetBuilder) WithWrap(wrap bool) *InputChoiceSetBuilder {
	b.data["wrap"] = wrap
	return b
}

func (b *InputChoiceSetBuilder) WithIsRequired(isRequired bool) *InputChoiceSetBuilder {
	b.data["isRequired"] = isRequired
	return b
}

func (b *InputChoiceSetBuilder) WithErrorMessage(errorMessage string) *InputChoiceSetBuilder {
	b.data["errorMessage"] = errorMessage
	return b
}

func (b *InputChoiceSetBuilder) WithSpacing(spacing Spacing) *InputChoiceSetBuilder {
	b.data["spacing"] = string(spacing)
	return b
}

func (b *InputChoiceSetBuilder) WithIsVisible(isVisible bool) *InputChoiceSetBuilder {
	b.data["isVisible"] = isVisible
	return b
}

func (b *InputChoiceSetBuilder) WithSeparator(separator bool) *InputChoiceSetBuilder {
	b.data["separator"] = separator
	return b
}

func (b *InputChoiceSetBuilder) WithHeight(height string) *InputChoiceSetBuilder {
	b.data["height"] = height
	return b
}

func (b *InputChoiceSetBuilder) WithFallback(fallback any) *InputChoiceSetBuilder {
	b.data["fallback"] = fallback
	return b
}

func (b *InputChoiceSetBuilder) WithRequires(key, version string) *InputChoiceSetBuilder {
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *InputChoiceSetBuilder) WithRtl(rtl bool) *InputChoiceSetBuilder {
	b.data["rtl"] = rtl
	return b
}

func (b *InputChoiceSetBuilder) WithLabelPosition(position InputLabelPosition) *InputChoiceSetBuilder {
	b.data["labelPosition"] = string(position)
	return b
}

func (b *InputChoiceSetBuilder) WithLabelWidth(width string) *InputChoiceSetBuilder {
	b.data["labelWidth"] = width
	return b
}

func (b *InputChoiceSetBuilder) WithInputStyle(style InputStyle) *InputChoiceSetBuilder {
	b.data["inputStyle"] = string(style)
	return b
}

// AddChoiceadds a choice with the given title and value strings.
func (b *InputChoiceSetBuilder) AddChoice(title, value string) *InputChoiceSetBuilder {
	choices := b.data["choices"].([]any)
	b.data["choices"] = append(choices, map[string]any{"title": title, "value": value})
	return b
}

// AddChoiceMap adds a pre-built choice map directly.
func (b *InputChoiceSetBuilder) AddChoiceMap(choice map[string]any) *InputChoiceSetBuilder {
	choices := b.data["choices"].([]any)
	b.data["choices"] = append(choices, choice)
	return b
}

// WithChoicesData sets a dynamic data query for fetching choices from a data source (Adaptive Cards 1.6+).
// dataset is the dataset identifier, e.g. "graph.microsoft.com/users".
func (b *InputChoiceSetBuilder) WithChoicesData(dataset string) *InputChoiceSetBuilder {
	b.data["choices.data"] = map[string]any{"type": "Data.Query", "dataset": dataset}
	return b
}

// WithChoicesDataFull sets a dynamic data query with dataset, count, and skip parameters.
func (b *InputChoiceSetBuilder) WithChoicesDataFull(dataset string, count int, skip int) *InputChoiceSetBuilder {
	b.data["choices.data"] = map[string]any{"type": "Data.Query", "dataset": dataset, "count": count, "skip": skip}
	return b
}

func (b *InputChoiceSetBuilder) Build() Card {
	return b.data
}
