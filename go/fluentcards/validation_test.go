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

func TestValidateAndPanic_WarningsOnly_DoesNotPanic(t *testing.T) {
	t.Parallel()
	// A card with only warnings (e.g., missing $schema) should not panic.
	card := fluentcards.Card{
		"type":    "AdaptiveCard",
		"version": "1.5",
		"body":    []any{map[string]any{"type": "TextBlock", "text": "Test"}},
	}
	assert.NotPanics(t, func() {
		fluentcards.ValidateAndPanic(card)
	})
}

func TestAdaptiveCardValidationError_SingleError(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{"type": "AdaptiveCard"} // missing version → single error
	defer func() {
		r := recover()
		require.NotNil(t, r)
		err, ok := r.(*fluentcards.AdaptiveCardValidationError)
		require.True(t, ok)
		assert.Contains(t, err.Error(), "Adaptive Card validation failed:")
		assert.NotEmpty(t, err.Issues)
	}()
	fluentcards.ValidateAndPanic(card)
}

func TestAdaptiveCardValidationError_MultipleErrors(t *testing.T) {
	t.Parallel()
	// Card with multiple errors: missing version + missing text + missing input id
	card := fluentcards.Card{
		"type": "AdaptiveCard",
		"body": []any{
			map[string]any{"type": "TextBlock"},
			map[string]any{"type": "Input.Text"},
		},
	}
	defer func() {
		r := recover()
		require.NotNil(t, r)
		err, ok := r.(*fluentcards.AdaptiveCardValidationError)
		require.True(t, ok)
		assert.Contains(t, err.Error(), "errors")
	}()
	fluentcards.ValidateAndPanic(card)
}

func TestAdaptiveCardValidationError_ExposesIssues(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{"type": "AdaptiveCard"} // missing version
	defer func() {
		r := recover()
		require.NotNil(t, r)
		err, ok := r.(*fluentcards.AdaptiveCardValidationError)
		require.True(t, ok)
		assert.NotEmpty(t, err.Issues)
		found := false
		for _, issue := range err.Issues {
			if issue.Code == "MISSING_VERSION" {
				found = true
			}
		}
		assert.True(t, found, "expected MISSING_VERSION in Issues")
	}()
	fluentcards.ValidateAndPanic(card)
}

func TestValidate_MissingSchema(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{
		"type":    "AdaptiveCard",
		"version": "1.5",
		"body":    []any{map[string]any{"type": "TextBlock", "text": "Hi"}},
	}
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_SCHEMA" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
			assert.Equal(t, "$schema", i.Path)
		}
	}
	assert.True(t, found, "expected MISSING_SCHEMA warning")
}

func TestValidate_UnknownVersion(t *testing.T) {
	t.Parallel()
	card := fluentcards.Card{
		"type":    "AdaptiveCard",
		"version": "9.9",
		"$schema": "https://adaptivecards.io/schemas/adaptive-card.json",
		"body":    []any{map[string]any{"type": "TextBlock", "text": "Test"}},
	}
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "UNKNOWN_VERSION" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
		}
	}
	assert.True(t, found, "expected UNKNOWN_VERSION warning")
}

func TestValidate_InvalidImageURL_RelativeURL(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddImage(func(img *fluentcards.ImageBuilder) {
			img.WithURL("not-a-url")
		}).Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "INVALID_IMAGE_URL" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
			assert.Equal(t, "body[0].url", i.Path)
		}
	}
	assert.True(t, found, "expected INVALID_IMAGE_URL warning")
}

func TestValidate_TooManyActions(t *testing.T) {
	t.Parallel()
	builder := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") })
	for i := 0; i < 6; i++ {
		builder.AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://example.com").WithTitle("Action")
		})
	}
	issues := fluentcards.Validate(builder.Build())
	found := false
	for _, i := range issues {
		if i.Code == "TOO_MANY_ACTIONS" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
			assert.Equal(t, "actions", i.Path)
		}
	}
	assert.True(t, found, "expected TOO_MANY_ACTIONS warning")
}

func TestValidate_MissingActionURL(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		Build()
	card["actions"] = []any{map[string]any{"type": "Action.OpenUrl", "url": ""}}
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_ACTION_URL" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
			assert.Equal(t, "actions[0].url", i.Path)
		}
	}
	assert.True(t, found, "expected MISSING_ACTION_URL error")
}

func TestValidate_InvalidActionURL_RelativeURL(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("not-a-url").WithTitle("Test")
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "INVALID_ACTION_URL" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
			assert.Equal(t, "actions[0].url", i.Path)
		}
	}
	assert.True(t, found, "expected INVALID_ACTION_URL warning")
}

func TestValidate_NestedContainerValidation(t *testing.T) {
	t.Parallel()
	inputNoID := fluentcards.Card{"type": "Input.Text", "id": "", "placeholder": "Test"}
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
				// empty text
			})
			c.AddElement(inputNoID)
		}).
		Build()
	issues := fluentcards.Validate(card)
	foundText := false
	foundInput := false
	for _, i := range issues {
		if i.Code == "MISSING_TEXT" && i.Path == "body[0].items[0].text" {
			foundText = true
		}
		if i.Code == "MISSING_INPUT_ID" && i.Path == "body[0].items[1].id" {
			foundInput = true
		}
	}
	assert.True(t, foundText, "expected MISSING_TEXT for nested container item")
	assert.True(t, foundInput, "expected MISSING_INPUT_ID for nested container item")
}

func TestValidate_NestedColumnSetValidation(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(col *fluentcards.ColumnBuilder) {
				col.AddImage(func(img *fluentcards.ImageBuilder) {
					// no URL set
				})
			})
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_IMAGE_URL" && i.Path == "body[0].columns[0].items[0].url" {
			found = true
		}
	}
	assert.True(t, found, "expected MISSING_IMAGE_URL for nested column item")
}

func TestValidate_MissingToggleTitle(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddInputToggle(func(it *fluentcards.InputToggleBuilder) {
			it.WithID("t1")
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_TOGGLE_TITLE" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MISSING_TOGGLE_TITLE error")
}

func TestValidate_InputDateMinGreaterThanMax(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddInputDate(func(id *fluentcards.InputDateBuilder) {
			id.WithID("d1").WithMin("2024-12-31").WithMax("2024-01-01")
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MIN_GREATER_THAN_MAX" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MIN_GREATER_THAN_MAX for Input.Date")
}

func TestValidate_MissingFacts(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddFactSet(func(fs *fluentcards.FactSetBuilder) {
			// empty fact set
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_FACTS" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MISSING_FACTS error")
}

func TestValidate_MissingInlines(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddRichTextBlock(func(rtb *fluentcards.RichTextBlockBuilder) {
			// empty rich text block
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "MISSING_INLINES" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected MISSING_INLINES error")
}

func TestValidate_VersionMismatch_RefreshWithV10(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.0").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Hello") }).
		WithRefresh(func(r *fluentcards.RefreshBuilder) {
			r.WithAction(func(a *fluentcards.ActionBuilder) { a.Execute("Refresh") })
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "VERSION_MISMATCH" && i.Path == "refresh" {
			found = true
		}
	}
	assert.True(t, found, "expected VERSION_MISMATCH for refresh with v1.0")
}

func TestValidate_VersionMismatch_ActionExecuteWithV10(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.0").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Test") }).
		AddAction(func(a *fluentcards.ActionBuilder) { a.Execute("Run") }).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "VERSION_MISMATCH" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityWarning, i.Severity)
		}
	}
	assert.True(t, found, "expected VERSION_MISMATCH for Action.Execute with v1.0")
}

func TestValidate_InvalidSelectAction_ContainerShowCard(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Hi") })
			c.WithSelectAction(func(a *fluentcards.ActionBuilder) { a.ShowCard("Bad") })
		}).
		Build()
	issues := fluentcards.Validate(card)
	found := false
	for _, i := range issues {
		if i.Code == "INVALID_SELECT_ACTION" {
			found = true
			assert.Equal(t, fluentcards.ValidationSeverityError, i.Severity)
		}
	}
	assert.True(t, found, "expected INVALID_SELECT_ACTION for container selectAction")
}
