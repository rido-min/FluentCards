package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
)

const panicMsg = "ActionBuilder: no action type specified — call OpenURL, Submit, ShowCard, ToggleVisibility, or Execute before setting properties"

func TestActionBuilder_PanicWithoutType_WithID(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithID("id") })
}

func TestActionBuilder_PanicWithoutType_WithIconURL(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithIconURL("https://img.png") })
}

func TestActionBuilder_PanicWithoutType_WithStyle(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithStyle(fluentcards.ActionStylePositive) })
}

func TestActionBuilder_PanicWithoutType_WithIsEnabled(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithIsEnabled(false) })
}

func TestActionBuilder_PanicWithoutType_WithTooltip(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithTooltip("tip") })
}

func TestActionBuilder_PanicWithoutType_WithData(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithData(map[string]any{"k": "v"}) })
}

func TestActionBuilder_PanicWithoutType_WithVerb(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithVerb("doIt") })
}

func TestActionBuilder_PanicWithoutType_WithCard(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithCard(map[string]any{}) })
}

func TestActionBuilder_PanicWithoutType_AddTargetElement(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.AddTargetElement("el1", nil) })
}

func TestActionBuilder_PanicWithoutType_WithAssociatedInputs(t *testing.T) {
	t.Parallel()
	ab := &fluentcards.ActionBuilder{}
	assert.PanicsWithValue(t, panicMsg, func() { ab.WithAssociatedInputs(fluentcards.AssociatedInputsAuto) })
}
