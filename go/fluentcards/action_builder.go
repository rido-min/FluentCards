package fluentcards

// ActionBuilder builds an Adaptive Card action (OpenUrl, Submit, ShowCard, ToggleVisibility, Execute).
// Call one of OpenURL, Submit, ShowCard, ToggleVisibility, or Execute to set the action type,
// then use With* methods to configure it.
type ActionBuilder struct {
	data                 map[string]any
	dataSet              bool
	teamsDataSet         bool
	teamsSubmitTypedSet  bool
	teamsSubmitRawSet    bool
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

func (b *ActionBuilder) ensureActionTypeSet() {
	if b.data == nil {
		panic("ActionBuilder: no action type specified — call OpenURL, Submit, ShowCard, ToggleVisibility, or Execute before setting properties")
	}
}

func (b *ActionBuilder) WithID(id string) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["id"] = id
	return b
}

func (b *ActionBuilder) WithTitle(title string) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["title"] = title
	return b
}

func (b *ActionBuilder) WithIconURL(iconURL string) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["iconUrl"] = iconURL
	return b
}

func (b *ActionBuilder) WithStyle(style ActionStyle) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["style"] = string(style)
	return b
}

func (b *ActionBuilder) WithIsEnabled(isEnabled bool) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["isEnabled"] = isEnabled
	return b
}

func (b *ActionBuilder) WithTooltip(tooltip string) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["tooltip"] = tooltip
	return b
}

// WithData sets the data payload for Action.Submit or Action.Execute.
func (b *ActionBuilder) WithData(data any) *ActionBuilder {
	b.ensureActionTypeSet()
	if b.teamsDataSet {
		panic("ActionBuilder: cannot use both WithData and WithTeamsData/WithTeamsTaskFetch on the same action")
	}
	t, _ := b.data["type"].(string)
	if t == "Action.Submit" || t == "Action.Execute" {
		b.data["data"] = data
		b.dataSet = true
	}
	return b
}

// WithAssociatedInputs sets which inputs are submitted for Action.Submit or Action.Execute.
func (b *ActionBuilder) WithAssociatedInputs(ai AssociatedInputs) *ActionBuilder {
	b.ensureActionTypeSet()
	t, _ := b.data["type"].(string)
	if t == "Action.Submit" || t == "Action.Execute" {
		b.data["associatedInputs"] = string(ai)
	}
	return b
}

// WithVerb sets the verb for Action.Execute.
func (b *ActionBuilder) WithVerb(verb string) *ActionBuilder {
	b.ensureActionTypeSet()
	if t, _ := b.data["type"].(string); t == "Action.Execute" {
		b.data["verb"] = verb
	}
	return b
}

// WithCard sets the nested card for Action.ShowCard.
func (b *ActionBuilder) WithCard(card Card) *ActionBuilder {
	b.ensureActionTypeSet()
	if t, _ := b.data["type"].(string); t == "Action.ShowCard" {
		b.data["card"] = card
	}
	return b
}

// AddTargetElement adds a target element for Action.ToggleVisibility.
// Pass isVisible as a *bool to pin visibility; pass nil to toggle.
func (b *ActionBuilder) AddTargetElement(elementID string, isVisible *bool) *ActionBuilder {
	b.ensureActionTypeSet()
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

func (b *ActionBuilder) WithMode(mode ActionMode) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["mode"] = string(mode)
	return b
}

func (b *ActionBuilder) WithRequires(key, version string) *ActionBuilder {
	b.ensureActionTypeSet()
	reqs, ok := b.data["requires"].(map[string]any)
	if !ok {
		reqs = map[string]any{}
	}
	reqs[key] = version
	b.data["requires"] = reqs
	return b
}

func (b *ActionBuilder) WithFallback(fallback any) *ActionBuilder {
	b.ensureActionTypeSet()
	b.data["fallback"] = fallback
	return b
}

// ── Teams-specific methods (Submit-only) ──────────────────────────────────────

func (b *ActionBuilder) ensureSubmitOnly(method string) {
	b.ensureActionTypeSet()
	if t, _ := b.data["type"].(string); t != "Action.Submit" {
		panic("ActionBuilder: " + method + " is only available on Submit actions — call Submit() before using this method")
	}
}

func (b *ActionBuilder) ensureNoDataConflict() {
	if b.dataSet {
		panic("ActionBuilder: cannot use both WithData and WithTeamsData/WithTeamsTaskFetch on the same action")
	}
}

// WithTeamsTaskFetch sets the action data to {"msteams": {"type": "task/fetch"}} (Submit-only).
func (b *ActionBuilder) WithTeamsTaskFetch() *ActionBuilder {
	b.ensureSubmitOnly("WithTeamsTaskFetch")
	b.ensureNoDataConflict()
	db := newTeamsDataBuilder()
	db.WithTaskFetch()
	b.data["data"] = db.Build()
	b.teamsDataSet = true
	return b
}

// WithTeamsData configures a Teams-specific data payload (Submit-only).
func (b *ActionBuilder) WithTeamsData(configure func(*TeamsDataBuilder)) *ActionBuilder {
	b.ensureSubmitOnly("WithTeamsData")
	b.ensureNoDataConflict()
	db := newTeamsDataBuilder()
	configure(db)
	b.data["data"] = db.Build()
	b.teamsDataSet = true
	return b
}

// WithTeamsSubmitFeedback configures Teams submit feedback properties (Submit-only).
func (b *ActionBuilder) WithTeamsSubmitFeedback(configure func(*TeamsSubmitPropertiesBuilder)) *ActionBuilder {
	b.ensureSubmitOnly("WithTeamsSubmitFeedback")
	if b.teamsSubmitRawSet {
		panic("ActionBuilder: cannot use both WithTeamsSubmitFeedback and WithTeamsSubmitRaw on the same action")
	}
	sb := newTeamsSubmitPropertiesBuilder()
	configure(sb)
	b.data["msteams"] = sb.Build()
	b.teamsSubmitTypedSet = true
	return b
}

// WithTeamsSubmitRaw sets the Teams action-level msteams property from a raw map (Submit-only).
func (b *ActionBuilder) WithTeamsSubmitRaw(value map[string]any) *ActionBuilder {
	b.ensureSubmitOnly("WithTeamsSubmitRaw")
	if b.teamsSubmitTypedSet {
		panic("ActionBuilder: cannot use both WithTeamsSubmitFeedback and WithTeamsSubmitRaw on the same action")
	}
	m := make(map[string]any, len(value))
	for k, v := range value {
		m[k] = v
	}
	b.data["msteams"] = m
	b.teamsSubmitRawSet = true
	return b
}

// Build returns the built action Card. Panics if no action type was set.
func (b *ActionBuilder) Build() Card {
	if b.data == nil {
		panic("ActionBuilder: no action type specified — call OpenURL, Submit, ShowCard, ToggleVisibility, or Execute first")
	}
	return b.data
}
