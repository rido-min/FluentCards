package fluentcards_test

import (
	"strings"
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestToJSON_BasicCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Hello") }).
		Build()
	json, err := fluentcards.ToJSON(card)
	require.NoError(t, err)
	assert.Contains(t, json, `"type": "AdaptiveCard"`)
	assert.Contains(t, json, `"Hello"`)
}

func TestToJSON_OmitsUnsetOptionalProperties(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		Build()
	json, err := fluentcards.ToJSON(card)
	require.NoError(t, err)
	// unset properties should not appear
	assert.NotContains(t, json, `"size"`)
	assert.NotContains(t, json, `"weight"`)
	assert.NotContains(t, json, `"color"`)
	assert.NotContains(t, json, `"wrap"`)
}

func TestToJSON_EnumValuesAreCamelCase(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("x").
				WithSize(fluentcards.TextSizeExtraLarge).
				WithColor(fluentcards.TextColorAttention)
		}).
		Build()
	json, err := fluentcards.ToJSON(card)
	require.NoError(t, err)
	assert.Contains(t, json, `"extraLarge"`)
	assert.Contains(t, json, `"attention"`)
}

func TestToJSONIndent_Compact(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		Build()
	json, err := fluentcards.ToJSONIndent(card, 0)
	require.NoError(t, err)
	// compact: no newlines
	assert.NotContains(t, json, "\n")
}

func TestToJSONIndent_TwoSpaces(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		Build()
	json, err := fluentcards.ToJSONIndent(card, 2)
	require.NoError(t, err)
	assert.Contains(t, json, "\n")
	assert.True(t, strings.Contains(json, "  "), "expected 2-space indent")
}

func TestFromJSON_ValidCard(t *testing.T) {
	t.Parallel()
	raw := `{"type":"AdaptiveCard","version":"1.5","$schema":"https://example.com"}`
	card := fluentcards.FromJSON(raw)
	require.NotNil(t, card)
	assert.Equal(t, "AdaptiveCard", card["type"])
	assert.Equal(t, "1.5", card["version"])
}

func TestFromJSON_InvalidJSON(t *testing.T) {
	t.Parallel()
	card := fluentcards.FromJSON(`not json`)
	assert.Nil(t, card)
}

func TestFromJSON_WrongRootType(t *testing.T) {
	t.Parallel()
	card := fluentcards.FromJSON(`{"type":"TextBlock","text":"oops"}`)
	assert.Nil(t, card)
}

func TestToJSON_RoundTrip(t *testing.T) {
	t.Parallel()
	original := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Round trip").WithSize(fluentcards.TextSizeLarge)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("OK").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
	json, err := fluentcards.ToJSON(original)
	require.NoError(t, err)
	parsed := fluentcards.FromJSON(json)
	require.NotNil(t, parsed)
	assert.Equal(t, "1.5", parsed["version"])
	body := parsed["body"].([]any)
	assert.Len(t, body, 1)
}
