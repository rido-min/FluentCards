package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestActionBuilder_OpenURL(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://example.com").WithTitle("Go There")
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	assert.Equal(t, "Action.OpenUrl", action["type"])
	assert.Equal(t, "https://example.com", action["url"])
	assert.Equal(t, "Go There", action["title"])
}

func TestActionBuilder_Submit(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Send").WithStyle(fluentcards.ActionStylePositive)
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	assert.Equal(t, "Action.Submit", action["type"])
	assert.Equal(t, "Send", action["title"])
	assert.Equal(t, "positive", action["style"])
}

func TestActionBuilder_Submit_WithData(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit().WithData(map[string]any{"action": "approve"})
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	data := action["data"].(map[string]any)
	assert.Equal(t, "approve", data["action"])
}

func TestActionBuilder_ShowCard(t *testing.T) {
	t.Parallel()
	innerCard := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Inner") }).
		Build()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.ShowCard("Show More").WithCard(innerCard)
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	assert.Equal(t, "Action.ShowCard", action["type"])
	assert.NotNil(t, action["card"])
}

func TestActionBuilder_ToggleVisibility(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			visible := true
			a.ToggleVisibility("Toggle").
				AddTargetElement("details-section", nil).
				AddTargetElement("header", &visible)
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	assert.Equal(t, "Action.ToggleVisibility", action["type"])
	targets := action["targetElements"].([]any)
	require.Len(t, targets, 2)
	assert.Equal(t, "details-section", targets[0])
	target2 := targets[1].(map[string]any)
	assert.Equal(t, "header", target2["elementId"])
	assert.Equal(t, true, target2["isVisible"])
}

func TestActionBuilder_Execute(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Execute("Run").
				WithVerb("doSomething").
				WithAssociatedInputs(fluentcards.AssociatedInputsAuto)
		}).Build()
	action := card["actions"].([]any)[0].(map[string]any)
	assert.Equal(t, "Action.Execute", action["type"])
	assert.Equal(t, "doSomething", action["verb"])
	assert.Equal(t, "auto", action["associatedInputs"])
}

func TestActionBuilder_Build_PanicsWithoutType(t *testing.T) {
	t.Parallel()
	assert.Panics(t, func() {
		fluentcards.NewAdaptiveCardBuilder().
			AddAction(func(a *fluentcards.ActionBuilder) {
				// no type set — intentionally omitted
			}).Build()
	})
}

func TestActionBuilder_ModifierIgnoredWithoutType(t *testing.T) {
	t.Parallel()
	// Ensure modifier calls on an untyped builder don't panic before Build()
	ab := &fluentcards.ActionBuilder{}
	ab.WithTitle("ignored") // should not panic
}
