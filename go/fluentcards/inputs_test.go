package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestInputTextBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputText(func(it *fluentcards.InputTextBuilder) {
			it.WithID("name").
				WithLabel("Your Name").
				WithPlaceholder("Enter name").
				WithMaxLength(100).
				WithIsMultiline(false).
				WithStyle(fluentcards.TextInputStyleEmail).
				WithIsRequired(true).
				WithErrorMessage("Name is required")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.Text", el["type"])
	assert.Equal(t, "name", el["id"])
	assert.Equal(t, "Your Name", el["label"])
	assert.Equal(t, "Enter name", el["placeholder"])
	assert.Equal(t, 100, el["maxLength"])
	assert.Equal(t, false, el["isMultiline"])
	assert.Equal(t, "email", el["style"])
	assert.Equal(t, true, el["isRequired"])
	assert.Equal(t, "Name is required", el["errorMessage"])
}

func TestInputNumberBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputNumber(func(in *fluentcards.InputNumberBuilder) {
			in.WithID("qty").
				WithLabel("Quantity").
				WithMin(1).
				WithMax(100).
				WithValue(10)
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.Number", el["type"])
	assert.Equal(t, "qty", el["id"])
	assert.Equal(t, float64(1), el["min"])
	assert.Equal(t, float64(100), el["max"])
	assert.Equal(t, float64(10), el["value"])
}

func TestInputDateBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputDate(func(id *fluentcards.InputDateBuilder) {
			id.WithID("start").
				WithLabel("Start Date").
				WithMin("2025-01-01").
				WithMax("2026-12-31").
				WithValue("2025-06-15")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.Date", el["type"])
	assert.Equal(t, "start", el["id"])
	assert.Equal(t, "2025-01-01", el["min"])
	assert.Equal(t, "2026-12-31", el["max"])
	assert.Equal(t, "2025-06-15", el["value"])
}

func TestInputTimeBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputTime(func(it *fluentcards.InputTimeBuilder) {
			it.WithID("meeting-time").
				WithMin("09:00").
				WithMax("17:00")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.Time", el["type"])
	assert.Equal(t, "meeting-time", el["id"])
	assert.Equal(t, "09:00", el["min"])
	assert.Equal(t, "17:00", el["max"])
}

func TestInputToggleBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputToggle(func(it *fluentcards.InputToggleBuilder) {
			it.WithID("agree").
				WithTitle("I agree to the terms").
				WithValueOn("true").
				WithValueOff("false").
				WithWrap(true)
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.Toggle", el["type"])
	assert.Equal(t, "agree", el["id"])
	assert.Equal(t, "I agree to the terms", el["title"])
	assert.Equal(t, "true", el["valueOn"])
	assert.Equal(t, "false", el["valueOff"])
	assert.Equal(t, true, el["wrap"])
}

func TestInputChoiceSetBuilder(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputChoiceSet(func(ics *fluentcards.InputChoiceSetBuilder) {
			ics.WithID("color").
				WithLabel("Favorite Color").
				WithStyle(fluentcards.ChoiceInputStyleExpanded).
				AddChoice("Red", "red").
				AddChoice("Green", "green").
				AddChoice("Blue", "blue")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Input.ChoiceSet", el["type"])
	assert.Equal(t, "color", el["id"])
	assert.Equal(t, "expanded", el["style"])
	choices := el["choices"].([]any)
	require.Len(t, choices, 3)
	assert.Equal(t, "Red", choices[0].(map[string]any)["title"])
	assert.Equal(t, "red", choices[0].(map[string]any)["value"])
}
