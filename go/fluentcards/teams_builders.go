package fluentcards

import "strings"

// TeamsCardPropertiesBuilder builds the card-level msteams object (width, mentions).
type TeamsCardPropertiesBuilder struct {
	data map[string]any
}

func newTeamsCardPropertiesBuilder() *TeamsCardPropertiesBuilder {
	return &TeamsCardPropertiesBuilder{data: map[string]any{}}
}

// WithFullWidth sets the card width to "Full".
func (b *TeamsCardPropertiesBuilder) WithFullWidth() *TeamsCardPropertiesBuilder {
	b.data["width"] = "Full"
	return b
}

// AddMention adds an @mention entity with auto-generated <at>displayName</at> text.
func (b *TeamsCardPropertiesBuilder) AddMention(displayName, userID string) *TeamsCardPropertiesBuilder {
	if b.data["entities"] == nil {
		b.data["entities"] = []any{}
	}
	entities := b.data["entities"].([]any)
	b.data["entities"] = append(entities, map[string]any{
		"type": "mention",
		"text": "<at>" + displayName + "</at>",
		"mentioned": map[string]any{
			"id":   userID,
			"name": displayName,
		},
	})
	return b
}

// Build returns the configured Teams card properties.
func (b *TeamsCardPropertiesBuilder) Build() Card {
	return b.data
}

// TeamsSubmitPropertiesBuilder builds the action-level msteams object (feedback control).
type TeamsSubmitPropertiesBuilder struct {
	data map[string]any
}

func newTeamsSubmitPropertiesBuilder() *TeamsSubmitPropertiesBuilder {
	return &TeamsSubmitPropertiesBuilder{data: map[string]any{}}
}

// WithFeedbackHidden hides the feedback UI after the submit action is invoked.
func (b *TeamsSubmitPropertiesBuilder) WithFeedbackHidden() *TeamsSubmitPropertiesBuilder {
	b.data["feedback"] = map[string]any{"hide": true}
	return b
}

// Build returns the configured Teams submit action properties.
func (b *TeamsSubmitPropertiesBuilder) Build() Card {
	return b.data
}

// TeamsDataBuilder builds a Teams-specific action data payload with msteams and custom properties.
type TeamsDataBuilder struct {
	msteams    map[string]any
	properties map[string]any
}

func newTeamsDataBuilder() *TeamsDataBuilder {
	return &TeamsDataBuilder{properties: map[string]any{}}
}

// WithTaskFetch sets the msteams object to {"type": "task/fetch"}.
func (b *TeamsDataBuilder) WithTaskFetch() *TeamsDataBuilder {
	b.msteams = map[string]any{"type": "task/fetch"}
	return b
}

// WithMsteams sets the msteams object from a map.
func (b *TeamsDataBuilder) WithMsteams(value map[string]any) *TeamsDataBuilder {
	// shallow copy
	m := make(map[string]any, len(value))
	for k, v := range value {
		m[k] = v
	}
	b.msteams = m
	return b
}

// WithProperty adds a custom property to the data payload.
// Panics if key is "msteams" (case-insensitive).
func (b *TeamsDataBuilder) WithProperty(key string, value any) *TeamsDataBuilder {
	if strings.EqualFold(key, "msteams") {
		panic("TeamsDataBuilder: cannot use 'msteams' as a property key — use WithMsteams() or WithTaskFetch() instead")
	}
	b.properties[key] = value
	return b
}

// Build returns the data payload as a Card (map[string]any).
func (b *TeamsDataBuilder) Build() Card {
	result := make(map[string]any, len(b.properties)+1)
	if b.msteams != nil {
		result["msteams"] = b.msteams
	}
	for k, v := range b.properties {
		result[k] = v
	}
	return result
}
