package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestValidate_ValidCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Hello") }).
		Build()
	issues := fluentcards.Validate(card)
	assert.Empty(t, issues)
}

func TestValidate_MissingVersion(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{"type": "AdaptiveCard", "$schema": "https://x.com"}
	issues := fluentcards.Validate(card)
	require.NotEmpty(t, issues)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_VERSION" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MISSING_VERSION issue")
}

func TestValidate_EmptyCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "EMPTY_CARD" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
		}
	}
	assert.True(t, found, "expected EMPTY_CARD warning")
}

func TestValidate_MissingTextBlockText(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			// no text set
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_TEXT" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MISSING_TEXT issue")
}

func TestValidate_MissingImageURL(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddImage(func(img *fluentcards.ImageBuilder) {
			// no URL set
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_IMAGE_URL" {
			found = true
		}
	}
	assert.True(t, found)
}

func TestValidate_MissingInputID(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputText(func(it *fluentcards.InputTextBuilder) {
			// no ID set
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_INPUT_ID" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found)
}

func TestValidate_InputNumberMinGreaterThanMax(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddInputNumber(func(in *fluentcards.InputNumberBuilder) {
			in.WithID("qty").WithMin(100).WithMax(10)
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MIN_GREATER_THAN_MAX" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found)
}

func TestValidate_DuplicateID(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("First").WithID("dup") }).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Second").WithID("dup") }).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "DUPLICATE_ID" {
			found = true
		}
	}
	assert.True(t, found)
}

func TestValidate_InvalidSelectAction_ShowCard(t *testing.T) {
	t.Parallel()
	showCard := map[string]any{"type": "Action.ShowCard"}
	card := fluentcards.Card{
		"type":         "AdaptiveCard",
		"version":      "1.5",
		"$schema":      "https://x.com",
		"selectAction": showCard,
		"body":         []any{map[string]any{"type": "TextBlock", "text": "x"}},
	}
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "INVALID_SELECT_ACTION" {
			found = true
		}
	}
	assert.True(t, found)
}

func TestValidate_VersionMismatch_Table(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.2"). // Table requires 1.5
		AddTable(func(tb *fluentcards.TableBuilder) {
			tb.AddColumn(map[string]any{"width": 1}).
				AddRow(map[string]any{"cells": []any{}})
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "VERSION_MISMATCH" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
		}
	}
	assert.True(t, found)
}

func TestValidateAndPanic_ValidCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("OK") }).
		Build()
	assert.NotPanics(t, func() {
		fluentcards.ValidateAndPanic(card)
	})
}

func TestValidateAndPanic_InvalidCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{"type": "AdaptiveCard"} // missing version
	assert.Panics(t, func() {
		fluentcards.ValidateAndPanic(card)
	})
}

func TestAdaptiveCardValidationError_Message(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{"type": "AdaptiveCard"} // missing version
	defer func() {
		r := recover()
		require.NotNil(t, r)
		err, ok := r.(*fluentcards.AdaptiveCardValidationError)
		require.True(t, ok)
		assert.Contains(t, err.Error(), "validation failed")
		assert.NotEmpty(t, err.Issues)
	}()
	fluentcards.ValidateAndPanic(card)
}
