package fluentcards

import (
	"encoding/json"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

// ── Card-level msteams (TeamsCardPropertiesBuilder) ──────────────────────────

func TestTeamsCardFullWidth(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hello") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.WithFullWidth() }).
		Build()
	msteams := card["msteams"].(map[string]any)
	assert.Equal(t, "Full", msteams["width"])
}

func TestTeamsCardWithMention(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi <at>John</at>") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.AddMention("John", "user-123") }).
		Build()
	msteams := card["msteams"].(map[string]any)
	entities := msteams["entities"].([]any)
	require.Len(t, entities, 1)
	entity := entities[0].(map[string]any)
	assert.Equal(t, "mention", entity["type"])
	assert.Equal(t, "<at>John</at>", entity["text"])
	mentioned := entity["mentioned"].(map[string]any)
	assert.Equal(t, "user-123", mentioned["id"])
	assert.Equal(t, "John", mentioned["name"])
}

func TestTeamsCardFullWidthWithMentions(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi <at>Alice</at> and <at>Bob</at>") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) {
			tc.WithFullWidth().AddMention("Alice", "user-1").AddMention("Bob", "user-2")
		}).
		Build()
	msteams := card["msteams"].(map[string]any)
	assert.Equal(t, "Full", msteams["width"])
	entities := msteams["entities"].([]any)
	assert.Len(t, entities, 2)
}

func TestTeamsCardRaw(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi") }).
		WithTeamsCardRaw(map[string]any{"width": "Full", "custom": true}).
		Build()
	msteams := card["msteams"].(map[string]any)
	assert.Equal(t, "Full", msteams["width"])
	assert.Equal(t, true, msteams["custom"])
}

func TestTeamsCardTypedThenRawConflict(t *testing.T) {
	assert.PanicsWithValue(t,
		"AdaptiveCardBuilder: cannot use both WithTeamsCard and WithTeamsCardRaw on the same card",
		func() {
			NewAdaptiveCardBuilder().
				WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.WithFullWidth() }).
				WithTeamsCardRaw(map[string]any{"width": "Full"})
		})
}

func TestTeamsCardRawThenTypedConflict(t *testing.T) {
	assert.PanicsWithValue(t,
		"AdaptiveCardBuilder: cannot use both WithTeamsCard and WithTeamsCardRaw on the same card",
		func() {
			NewAdaptiveCardBuilder().
				WithTeamsCardRaw(map[string]any{"width": "Full"}).
				WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.WithFullWidth() })
		})
}

// ── Action-level msteams (Submit feedback) ───────────────────────────────────

func TestSubmitWithFeedbackHidden(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddActionSet(func(as *ActionSetBuilder) {
			as.AddAction(func(a *ActionBuilder) {
				a.Submit("Click me").WithTeamsSubmitFeedback(func(f *TeamsSubmitPropertiesBuilder) {
					f.WithFeedbackHidden()
				})
			})
		}).
		Build()
	body := card["body"].([]any)
	actionSet := body[0].(map[string]any)
	actions := actionSet["actions"].([]any)
	action := actions[0].(map[string]any)
	msteams := action["msteams"].(map[string]any)
	feedback := msteams["feedback"].(map[string]any)
	assert.Equal(t, true, feedback["hide"])
}

func TestSubmitWithTeamsSubmitRaw(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddActionSet(func(as *ActionSetBuilder) {
			as.AddAction(func(a *ActionBuilder) {
				a.Submit("Click me").WithTeamsSubmitRaw(map[string]any{
					"feedback": map[string]any{"hide": true},
				})
			})
		}).
		Build()
	body := card["body"].([]any)
	action := body[0].(map[string]any)["actions"].([]any)[0].(map[string]any)
	msteams := action["msteams"].(map[string]any)
	feedback := msteams["feedback"].(map[string]any)
	assert.Equal(t, true, feedback["hide"])
}

func TestTeamsSubmitTypedThenRawConflict(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Submit("Click me").
						WithTeamsSubmitFeedback(func(f *TeamsSubmitPropertiesBuilder) { f.WithFeedbackHidden() }).
						WithTeamsSubmitRaw(map[string]any{"feedback": map[string]any{"hide": true}})
				})
			}).Build()
	})
}

func TestTeamsSubmitRawThenTypedConflict(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Submit("Click me").
						WithTeamsSubmitRaw(map[string]any{"feedback": map[string]any{"hide": true}}).
						WithTeamsSubmitFeedback(func(f *TeamsSubmitPropertiesBuilder) { f.WithFeedbackHidden() })
				})
			}).Build()
	})
}

// ── Data-level Teams (task/fetch, msteams in data) ───────────────────────────

func TestTeamsTaskFetchShorthand(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddActionSet(func(as *ActionSetBuilder) {
			as.AddAction(func(a *ActionBuilder) {
				a.Submit("Open").WithTeamsTaskFetch()
			})
		}).
		Build()
	body := card["body"].([]any)
	action := body[0].(map[string]any)["actions"].([]any)[0].(map[string]any)
	data := action["data"].(map[string]any)
	msteams := data["msteams"].(map[string]any)
	assert.Equal(t, "task/fetch", msteams["type"])
}

func TestTeamsDataWithCustomProperties(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddActionSet(func(as *ActionSetBuilder) {
			as.AddAction(func(a *ActionBuilder) {
				a.Submit("Open").WithTeamsData(func(d *TeamsDataBuilder) {
					d.WithTaskFetch().WithProperty("customKey", "customValue").WithProperty("count", 42)
				})
			})
		}).
		Build()
	body := card["body"].([]any)
	action := body[0].(map[string]any)["actions"].([]any)[0].(map[string]any)
	data := action["data"].(map[string]any)
	assert.Equal(t, map[string]any{"type": "task/fetch"}, data["msteams"])
	assert.Equal(t, "customValue", data["customKey"])
	assert.Equal(t, 42, data["count"])
}

func TestTeamsDataWithCustomMsteams(t *testing.T) {
	b := newTeamsDataBuilder()
	b.WithMsteams(map[string]any{"type": "task/submit", "custom": true})
	b.WithProperty("key", "value")
	result := b.Build()
	assert.Equal(t, map[string]any{"type": "task/submit", "custom": true}, result["msteams"])
	assert.Equal(t, "value", result["key"])
}

func TestTeamsDataMsteamsPropertyRejected(t *testing.T) {
	assert.Panics(t, func() {
		b := newTeamsDataBuilder()
		b.WithProperty("msteams", map[string]any{"type": "task/fetch"})
	})
}

func TestTeamsDataMsteamsPropertyCaseInsensitive(t *testing.T) {
	assert.Panics(t, func() {
		b := newTeamsDataBuilder()
		b.WithProperty("MSTEAMS", "value")
	})
}

// ── Submit-only gating ───────────────────────────────────────────────────────

func TestTeamsTaskFetchOnExecutePanics(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Execute("Run").WithTeamsTaskFetch()
				})
			}).Build()
	})
}

func TestTeamsDataOnOpenURLPanics(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.OpenURL("https://example.com").WithTeamsData(func(d *TeamsDataBuilder) { d.WithTaskFetch() })
				})
			}).Build()
	})
}

func TestTeamsSubmitFeedbackOnTogglePanics(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.ToggleVisibility("Toggle").WithTeamsSubmitFeedback(func(f *TeamsSubmitPropertiesBuilder) { f.WithFeedbackHidden() })
				})
			}).Build()
	})
}

func TestTeamsSubmitRawOnExecutePanics(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Execute("Run").WithTeamsSubmitRaw(map[string]any{"feedback": map[string]any{"hide": true}})
				})
			}).Build()
	})
}

// ── Data conflict detection ──────────────────────────────────────────────────

func TestWithDataThenTeamsDataConflict(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Submit("Click").WithData(map[string]any{"key": "value"}).WithTeamsData(func(d *TeamsDataBuilder) { d.WithTaskFetch() })
				})
			}).Build()
	})
}

func TestTeamsDataThenWithDataConflict(t *testing.T) {
	assert.Panics(t, func() {
		NewAdaptiveCardBuilder().
			AddActionSet(func(as *ActionSetBuilder) {
				as.AddAction(func(a *ActionBuilder) {
					a.Submit("Click").WithTeamsData(func(d *TeamsDataBuilder) { d.WithTaskFetch() }).WithData(map[string]any{"key": "value"})
				})
			}).Build()
	})
}

// ── Mention validation ───────────────────────────────────────────────────────

func TestMatchedMentionsPass(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi <at>John</at>") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.AddMention("John", "user-123") }).
		Build()
	issues := Validate(card)
	for _, i := range issues {
		assert.NotContains(t, i.Code, "MENTION")
		assert.NotContains(t, i.Code, "AT_TOKEN")
	}
}

func TestOrphanedMentionEntity(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hello world") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.AddMention("John", "user-123") }).
		Build()
	issues := Validate(card)
	var orphaned []ValidationIssue
	for _, i := range issues {
		if i.Code == "ORPHANED_MENTION_ENTITY" {
			orphaned = append(orphaned, i)
		}
	}
	require.Len(t, orphaned, 1)
	assert.Equal(t, ValidationSeverityWarning, orphaned[0].Severity)
}

func TestOrphanedAtToken(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi <at>Unknown</at>") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) { tc.AddMention("John", "user-123") }).
		Build()
	issues := Validate(card)
	var atIssues []ValidationIssue
	for _, i := range issues {
		if i.Code == "ORPHANED_AT_TOKEN" {
			atIssues = append(atIssues, i)
		}
	}
	require.Len(t, atIssues, 1)
}

// ── Serialization round-trip ─────────────────────────────────────────────────

func TestFullTeamsCardRoundTrip(t *testing.T) {
	card := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hi <at>Alice</at>") }).
		WithTeamsCard(func(tc *TeamsCardPropertiesBuilder) {
			tc.WithFullWidth().AddMention("Alice", "user-1")
		}).
		AddActionSet(func(as *ActionSetBuilder) {
			as.AddAction(func(a *ActionBuilder) {
				a.Submit("Submit").
					WithTeamsTaskFetch().
					WithTeamsSubmitFeedback(func(f *TeamsSubmitPropertiesBuilder) { f.WithFeedbackHidden() })
			})
		}).
		Build()

	jsonStr, err := ToJSON(card)
	require.NoError(t, err)

	var parsed map[string]any
	err = json.Unmarshal([]byte(jsonStr), &parsed)
	require.NoError(t, err)

	msteams := parsed["msteams"].(map[string]any)
	assert.Equal(t, "Full", msteams["width"])
	entities := msteams["entities"].([]any)
	assert.Len(t, entities, 1)
}

func TestFromJSONPreservesMsteams(t *testing.T) {
	original := `{"type":"AdaptiveCard","version":"1.5","$schema":"http://adaptivecards.io/schemas/adaptive-card.json","msteams":{"width":"Full"},"body":[{"type":"TextBlock","text":"Hello"}]}`
	card := FromJSON(original)
	require.NotNil(t, card)
	msteams := card["msteams"].(map[string]any)
	assert.Equal(t, "Full", msteams["width"])
}
