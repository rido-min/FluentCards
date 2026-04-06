package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestAdaptiveCardBuilder_DefaultVersionAndSchema(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().Build()
	assert.Equal(t, "AdaptiveCard", card["type"])
	assert.Equal(t, "1.5", card["version"])
	assert.NotEmpty(t, card["$schema"])
}

func TestAdaptiveCardBuilder_WithVersion(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().WithVersion("1.6").Build()
	assert.Equal(t, "1.6", card["version"])
	assert.Contains(t, card["$schema"], "1.6")
}

func TestAdaptiveCardBuilder_WithSchema_Override(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithSchema("https://example.com/custom-schema.json").
		Build()
	assert.Equal(t, "https://example.com/custom-schema.json", card["$schema"])
}

func TestAdaptiveCardBuilder_AddTextBlock(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Hello, World!")
		}).
		Build()
	body := card["body"].([]any)
	require.Len(t, body, 1)
	el := body[0].(map[string]any)
	assert.Equal(t, "TextBlock", el["type"])
	assert.Equal(t, "Hello, World!", el["text"])
}

func TestAdaptiveCardBuilder_AddAction(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("x") }).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Click me")
		}).
		Build()
	actions := card["actions"].([]any)
	require.Len(t, actions, 1)
	action := actions[0].(map[string]any)
	assert.Equal(t, "Action.Submit", action["type"])
	assert.Equal(t, "Click me", action["title"])
}

func TestAdaptiveCardBuilder_MultipleBodyElements(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("First") }).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Second") }).
		AddImage(func(img *fluentcards.ImageBuilder) { img.WithURL("https://example.com/img.png") }).
		Build()
	body := card["body"].([]any)
	assert.Len(t, body, 3)
}

func TestAdaptiveCardBuilder_WithMetadata(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithMetadata("https://example.com/card").
		Build()
	meta := card["metadata"].(map[string]any)
	assert.Equal(t, "https://example.com/card", meta["webUrl"])
}

func TestAdaptiveCardBuilder_WithRefresh(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithRefresh(func(r *fluentcards.RefreshBuilder) {
			r.AddUserID("user1").WithExpires("2026-01-01T00:00:00Z")
		}).
		Build()
	refresh := card["refresh"].(map[string]any)
	assert.Equal(t, "2026-01-01T00:00:00Z", refresh["expires"])
	userIds := refresh["userIds"].([]any)
	assert.Equal(t, "user1", userIds[0])
}

func TestAdaptiveCardBuilder_AddElement_PreBuilt(t *testing.T) {
	t.Parallel()
	prebuilt := map[string]any{"type": "TextBlock", "text": "Pre-built"}
	card := fluentcards.NewAdaptiveCardBuilder().
		AddElement(prebuilt).
		Build()
	body := card["body"].([]any)
	require.Len(t, body, 1)
	assert.Equal(t, "Pre-built", body[0].(map[string]any)["text"])
}
