package fluentcards

// ActionBuilder builds an Adaptive Card action (OpenUrl, Submit, ShowCard, ToggleVisibility, Execute).
// Call one of OpenURL, Submit, ShowCard, ToggleVisibility, or Execute to set the action type,
// then use With* methods to configure it.
type ActionBuilder struct {
	data map[string]any
}

func newActionBuilder() *ActionBuilder {
	return &ActionBuilder{}
}

// OpenURL creates an Action.OpenUrl action.
func (b *ActionBuilder) OpenURL(url string) *ActionBuilder {
	b.data = map[string]any{"type": "Action.OpenUrl", "url": url}
	return b
}

// Submit creates an Action.Submit action.
func (b *ActionBuilder) Submit(title ...string) *ActionBuilder {
	b.data = map[string]any{"type": "Action.Submit"}
	if len(title) > 0 && title[0] != "" {
		b.data["title"] = title[0]
	}
	return b
}

// ShowCard creates an Action.ShowCard action.
func (b *ActionBuilder) ShowCard(title ...string) *ActionBuilder {
	b.data = map[string]any{"type": "Action.ShowCard"}
	if len(title) > 0 && title[0] != "" {
		b.data["title"] = title[0]
	}
	return b
}

// ToggleVisibility creates an Action.ToggleVisibility action.
func (b *ActionBuilder) ToggleVisibility(title ...string) *ActionBuilder {
	b.data = map[string]any{"type": "Action.ToggleVisibility"}
	if len(title) > 0 && title[0] != "" {
		b.data["title"] = title[0]
	}
	return b
}

// Execute creates an Action.Execute action.
func (b *ActionBuilder) Execute(title ...string) *ActionBuilder {
	b.data = map[string]any{"type": "Action.Execute"}
	if len(title) > 0 && title[0] != "" {
		b.data["title"] = title[0]
	}
	return b
}

func (b *ActionBuilder) WithID(id string) *ActionBuilder {
	if b.data != nil {
		b.data["id"] = id
	}
	return b
}

func (b *ActionBuilder) WithTitle(title string) *ActionBuilder {
	if b.data != nil {
		b.data["title"] = title
	}
	return b
}

func (b *ActionBuilder) WithIconURL(iconURL string) *ActionBuilder {
	if b.data != nil {
		b.data["iconUrl"] = iconURL
	}
	return b
}

func (b *ActionBuilder) WithStyle(style ActionStyle) *ActionBuilder {
	if b.data != nil {
		b.data["style"] = string(style)
	}
	return b
}

func (b *ActionBuilder) WithIsEnabled(isEnabled bool) *ActionBuilder {
	if b.data != nil {
		b.data["isEnabled"] = isEnabled
	}
	return b
}

func (b *ActionBuilder) WithTooltip(tooltip string) *ActionBuilder {
	if b.data != nil {
		b.data["tooltip"] = tooltip
	}
	return b
}

// WithData sets the data payload for Action.Submit or Action.Execute.
func (b *ActionBuilder) WithData(data any) *ActionBuilder {
	if b.data != nil {
		t, _ := b.data["type"].(string)
		if t == "Action.Submit" || t == "Action.Execute" {
			b.data["data"] = data
		}
	}
	return b
}

// WithAssociatedInputs sets which inputs are submitted for Action.Submit or Action.Execute.
func (b *ActionBuilder) WithAssociatedInputs(ai AssociatedInputs) *ActionBuilder {
	if b.data != nil {
		t, _ := b.data["type"].(string)
		if t == "Action.Submit" || t == "Action.Execute" {
			b.data["associatedInputs"] = string(ai)
		}
	}
	return b
}

// WithVerb sets the verb for Action.Execute.
func (b *ActionBuilder) WithVerb(verb string) *ActionBuilder {
	if b.data != nil {
		if t, _ := b.data["type"].(string); t == "Action.Execute" {
			b.data["verb"] = verb
		}
	}
	return b
}

// WithCard sets the nested card for Action.ShowCard.
func (b *ActionBuilder) WithCard(card Card) *ActionBuilder {
	if b.data != nil {
		if t, _ := b.data["type"].(string); t == "Action.ShowCard" {
			b.data["card"] = card
		}
	}
	return b
}

// AddTargetElement adds a target element for Action.ToggleVisibility.
// Pass isVisible as a *bool to pin visibility; pass nil to toggle.
func (b *ActionBuilder) AddTargetElement(elementID string, isVisible *bool) *ActionBuilder {
	if b.data == nil {
		return b
	}
	if t, _ := b.data["type"].(string); t != "Action.ToggleVisibility" {
		return b
	}
	if b.data["targetElements"] == nil {
		b.data["targetElements"] = []any{}
	}
	targets := b.data["targetElements"].([]any)
	if isVisible == nil {
		b.data["targetElements"] = append(targets, elementID)
	} else {
		b.data["targetElements"] = append(targets, map[string]any{
			"elementId": elementID,
			"isVisible": *isVisible,
		})
	}
	return b
}

// Build returns the built action Card. Panics if no action type was set.
func (b *ActionBuilder) Build() Card {
	if b.data == nil {
		panic("ActionBuilder: no action type specified — call OpenURL, Submit, ShowCard, ToggleVisibility, or Execute first")
	}
	return b.data
}
