package fluentcards

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

// ── InputText deep ──────────────────────────────────────────────────────────

func TestInputDeep_Text_Multiline(t *testing.T) {
	t.Parallel()
	inp := newInputTextBuilder().WithID("t").WithIsMultiline(true).Build()
	assert.Equal(t, true, inp["isMultiline"])
}

func TestInputDeep_Text_MaxLength(t *testing.T) {
	t.Parallel()
	inp := newInputTextBuilder().WithID("t").WithMaxLength(50).Build()
	assert.Equal(t, 50, inp["maxLength"])
}

func TestInputDeep_Text_Regex(t *testing.T) {
	t.Parallel()
	inp := newInputTextBuilder().WithID("t").WithRegex(`^\d{3}$`).Build()
	assert.Equal(t, `^\d{3}$`, inp["regex"])
}

func TestInputDeep_Text_Placeholder(t *testing.T) {
	t.Parallel()
	inp := newInputTextBuilder().WithID("t").WithPlaceholder("type here").Build()
	assert.Equal(t, "type here", inp["placeholder"])
}

func TestInputDeep_Text_InlineAction(t *testing.T) {
	t.Parallel()
	inp := newInputTextBuilder().WithID("t").
		WithInlineAction(func(a *ActionBuilder) {
			a.Submit("Search")
		}).
		Build()
	ia := inp["inlineAction"].(Card)
	assert.Equal(t, "Action.Submit", ia["type"])
	assert.Equal(t, "Search", ia["title"])
}

func TestInputDeep_Text_Style(t *testing.T) {
	t.Parallel()
	for _, style := range []TextInputStyle{TextInputStyleText, TextInputStyleTel, TextInputStyleUrl, TextInputStyleEmail, TextInputStylePassword} {
		inp := newInputTextBuilder().WithID("t").WithStyle(style).Build()
		assert.Equal(t, string(style), inp["style"])
	}
}

// ── InputNumber deep ────────────────────────────────────────────────────────

func TestInputDeep_Number_MinMax(t *testing.T) {
	t.Parallel()
	inp := newInputNumberBuilder().WithID("n").WithMin(1).WithMax(99).Build()
	assert.Equal(t, float64(1), inp["min"])
	assert.Equal(t, float64(99), inp["max"])
}

func TestInputDeep_Number_Placeholder(t *testing.T) {
	t.Parallel()
	inp := newInputNumberBuilder().WithID("n").WithPlaceholder("0-100").Build()
	assert.Equal(t, "0-100", inp["placeholder"])
}

func TestInputDeep_Number_Value(t *testing.T) {
	t.Parallel()
	inp := newInputNumberBuilder().WithID("n").WithValue(42.5).Build()
	assert.Equal(t, 42.5, inp["value"])
}

// ── InputDate deep ──────────────────────────────────────────────────────────

func TestInputDeep_Date_MinMaxValue(t *testing.T) {
	t.Parallel()
	inp := newInputDateBuilder().WithID("d").
		WithMin("2024-01-01").
		WithMax("2024-12-31").
		WithValue("2024-06-15").
		Build()
	assert.Equal(t, "2024-01-01", inp["min"])
	assert.Equal(t, "2024-12-31", inp["max"])
	assert.Equal(t, "2024-06-15", inp["value"])
}

func TestInputDeep_Date_Placeholder(t *testing.T) {
	t.Parallel()
	inp := newInputDateBuilder().WithID("d").WithPlaceholder("Select date").Build()
	assert.Equal(t, "Select date", inp["placeholder"])
}

// ── InputTime deep ──────────────────────────────────────────────────────────

func TestInputDeep_Time_MinMaxValue(t *testing.T) {
	t.Parallel()
	inp := newInputTimeBuilder().WithID("tm").
		WithMin("08:00").
		WithMax("17:00").
		WithValue("12:30").
		Build()
	assert.Equal(t, "08:00", inp["min"])
	assert.Equal(t, "17:00", inp["max"])
	assert.Equal(t, "12:30", inp["value"])
}

func TestInputDeep_Time_Placeholder(t *testing.T) {
	t.Parallel()
	inp := newInputTimeBuilder().WithID("tm").WithPlaceholder("HH:mm").Build()
	assert.Equal(t, "HH:mm", inp["placeholder"])
}

// ── InputToggle deep ────────────────────────────────────────────────────────

func TestInputDeep_Toggle_TitleAndValues(t *testing.T) {
	t.Parallel()
	inp := newInputToggleBuilder().
		WithID("tg").
		WithTitle("Accept terms").
		WithValueOn("yes").
		WithValueOff("no").
		WithValue("yes").
		WithWrap(true).
		Build()
	assert.Equal(t, "Input.Toggle", inp["type"])
	assert.Equal(t, "Accept terms", inp["title"])
	assert.Equal(t, "yes", inp["valueOn"])
	assert.Equal(t, "no", inp["valueOff"])
	assert.Equal(t, "yes", inp["value"])
	assert.Equal(t, true, inp["wrap"])
}

// ── InputChoiceSet deep ─────────────────────────────────────────────────────

func TestInputDeep_ChoiceSet_Choices(t *testing.T) {
	t.Parallel()
	inp := newInputChoiceSetBuilder().
		WithID("ch").
		AddChoice("A", "a").
		AddChoice("B", "b").
		AddChoice("C", "c").
		Build()
	choices := inp["choices"].([]any)
	assert.Len(t, choices, 3)
	assert.Equal(t, "A", choices[0].(Card)["title"])
	assert.Equal(t, "c", choices[2].(Card)["value"])
}

func TestInputDeep_ChoiceSet_IsMultiSelect(t *testing.T) {
	t.Parallel()
	inp := newInputChoiceSetBuilder().WithID("ch").WithIsMultiSelect(true).Build()
	assert.Equal(t, true, inp["isMultiSelect"])
}

func TestInputDeep_ChoiceSet_Style(t *testing.T) {
	t.Parallel()
	for _, style := range []ChoiceInputStyle{ChoiceInputStyleCompact, ChoiceInputStyleExpanded, ChoiceInputStyleFiltered} {
		inp := newInputChoiceSetBuilder().WithID("ch").WithStyle(style).Build()
		assert.Equal(t, string(style), inp["style"])
	}
}

func TestInputDeep_ChoiceSet_Placeholder(t *testing.T) {
	t.Parallel()
	inp := newInputChoiceSetBuilder().WithID("ch").WithPlaceholder("Choose").Build()
	assert.Equal(t, "Choose", inp["placeholder"])
}

func TestInputDeep_ChoiceSet_ChoicesData(t *testing.T) {
	t.Parallel()
	inp := newInputChoiceSetBuilder().WithID("ch").
		WithChoicesData("graph.microsoft.com/users").
		Build()
	cd := inp["choices.data"].(Card)
	assert.Equal(t, "Data.Query", cd["type"])
	assert.Equal(t, "graph.microsoft.com/users", cd["dataset"])
}

// ── Common input properties ─────────────────────────────────────────────────

func TestInputDeep_CommonProperties_ID(t *testing.T) {
	t.Parallel()
	builders := []Card{
		newInputTextBuilder().WithID("a").Build(),
		newInputNumberBuilder().WithID("a").Build(),
		newInputDateBuilder().WithID("a").Build(),
		newInputTimeBuilder().WithID("a").Build(),
		newInputToggleBuilder().WithID("a").Build(),
		newInputChoiceSetBuilder().WithID("a").Build(),
	}
	for _, b := range builders {
		assert.Equal(t, "a", b["id"])
	}
}

func TestInputDeep_CommonProperties_Label(t *testing.T) {
	t.Parallel()
	builders := []Card{
		newInputTextBuilder().WithID("a").WithLabel("L").Build(),
		newInputNumberBuilder().WithID("a").WithLabel("L").Build(),
		newInputDateBuilder().WithID("a").WithLabel("L").Build(),
		newInputTimeBuilder().WithID("a").WithLabel("L").Build(),
		newInputToggleBuilder().WithID("a").WithLabel("L").Build(),
		newInputChoiceSetBuilder().WithID("a").WithLabel("L").Build(),
	}
	for _, b := range builders {
		assert.Equal(t, "L", b["label"])
	}
}

func TestInputDeep_CommonProperties_IsRequired(t *testing.T) {
	t.Parallel()
	builders := []Card{
		newInputTextBuilder().WithID("a").WithIsRequired(true).Build(),
		newInputNumberBuilder().WithID("a").WithIsRequired(true).Build(),
		newInputDateBuilder().WithID("a").WithIsRequired(true).Build(),
		newInputTimeBuilder().WithID("a").WithIsRequired(true).Build(),
		newInputToggleBuilder().WithID("a").WithIsRequired(true).Build(),
		newInputChoiceSetBuilder().WithID("a").WithIsRequired(true).Build(),
	}
	for _, b := range builders {
		assert.Equal(t, true, b["isRequired"])
	}
}

func TestInputDeep_CommonProperties_ErrorMessage(t *testing.T) {
	t.Parallel()
	builders := []Card{
		newInputTextBuilder().WithID("a").WithErrorMessage("err").Build(),
		newInputNumberBuilder().WithID("a").WithErrorMessage("err").Build(),
		newInputDateBuilder().WithID("a").WithErrorMessage("err").Build(),
		newInputTimeBuilder().WithID("a").WithErrorMessage("err").Build(),
		newInputToggleBuilder().WithID("a").WithErrorMessage("err").Build(),
		newInputChoiceSetBuilder().WithID("a").WithErrorMessage("err").Build(),
	}
	for _, b := range builders {
		assert.Equal(t, "err", b["errorMessage"])
	}
}

func TestInputDeep_CommonProperties_Spacing(t *testing.T) {
	t.Parallel()
	builders := []Card{
		newInputTextBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
		newInputNumberBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
		newInputDateBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
		newInputTimeBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
		newInputToggleBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
		newInputChoiceSetBuilder().WithID("a").WithSpacing(SpacingLarge).Build(),
	}
	for _, b := range builders {
		assert.Equal(t, string(SpacingLarge), b["spacing"])
	}
}
