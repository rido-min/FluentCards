package fluentcards

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

// ── TextBlock ────────────────────────────────────────────────────────────────

func TestSchemaConformance_TextBlock_AllProperties(t *testing.T) {
	t.Parallel()
	action := newActionBuilder().OpenURL("https://example.com").WithTitle("Go").Build()
	tb := newTextBlockBuilder().
		WithID("tb1").
		WithText("Hello").
		WithSize(TextSizeExtraLarge).
		WithWeight(TextWeightBolder).
		WithColor(TextColorAccent).
		WithFontType(FontTypeMonospace).
		WithIsSubtle(true).
		WithStyle(TextBlockStyleHeading).
		WithWrap(true).
		WithMaxLines(3).
		WithHorizontalAlignment(HorizontalAlignmentCenter).
		WithSpacing(SpacingLarge).
		WithSeparator(true).
		WithIsVisible(false).
		WithSelectAction(action).
		Build()

	assert.Equal(t, "TextBlock", tb["type"])
	assert.Equal(t, "tb1", tb["id"])
	assert.Equal(t, "Hello", tb["text"])
	assert.Equal(t, string(TextSizeExtraLarge), tb["size"])
	assert.Equal(t, string(TextWeightBolder), tb["weight"])
	assert.Equal(t, string(TextColorAccent), tb["color"])
	assert.Equal(t, string(FontTypeMonospace), tb["fontType"])
	assert.Equal(t, true, tb["isSubtle"])
	assert.Equal(t, string(TextBlockStyleHeading), tb["style"])
	assert.Equal(t, true, tb["wrap"])
	assert.Equal(t, 3, tb["maxLines"])
	assert.Equal(t, string(HorizontalAlignmentCenter), tb["horizontalAlignment"])
	assert.Equal(t, string(SpacingLarge), tb["spacing"])
	assert.Equal(t, true, tb["separator"])
	assert.Equal(t, false, tb["isVisible"])
	sa := tb["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
}

func TestSchemaConformance_TextBlock_MinimalDefaults(t *testing.T) {
	t.Parallel()
	tb := newTextBlockBuilder().WithText("Hi").Build()
	assert.Equal(t, "TextBlock", tb["type"])
	assert.Equal(t, "Hi", tb["text"])
	assert.Nil(t, tb["id"])
	assert.Nil(t, tb["size"])
	assert.Nil(t, tb["weight"])
}

// ── Image ────────────────────────────────────────────────────────────────────

func TestSchemaConformance_Image_AllProperties(t *testing.T) {
	t.Parallel()
	img := newImageBuilder().
		WithID("img1").
		WithURL("https://example.com/image.png").
		WithAltText("A photo").
		WithSize(ImageSizeMedium).
		WithStyle(ImageStylePerson).
		WithWidth("100px").
		WithHeight("100px").
		WithHorizontalAlignment(HorizontalAlignmentRight).
		WithBackgroundColor("#FF0000").
		WithSpacing(SpacingSmall).
		WithSeparator(true).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		Build()

	assert.Equal(t, "Image", img["type"])
	assert.Equal(t, "img1", img["id"])
	assert.Equal(t, "https://example.com/image.png", img["url"])
	assert.Equal(t, "A photo", img["altText"])
	assert.Equal(t, string(ImageSizeMedium), img["size"])
	assert.Equal(t, string(ImageStylePerson), img["style"])
	assert.Equal(t, "100px", img["width"])
	assert.Equal(t, "100px", img["height"])
	assert.Equal(t, string(HorizontalAlignmentRight), img["horizontalAlignment"])
	assert.Equal(t, "#FF0000", img["backgroundColor"])
	assert.Equal(t, string(SpacingSmall), img["spacing"])
	assert.Equal(t, true, img["separator"])
	sa := img["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
}

// ── Container ────────────────────────────────────────────────────────────────

func TestSchemaConformance_Container_AllProperties(t *testing.T) {
	t.Parallel()
	c := newContainerBuilder().
		WithID("c1").
		WithStyle(ContainerStyleEmphasis).
		WithBleed(true).
		WithMinHeight("200px").
		WithSpacing(SpacingMedium).
		WithSeparator(true).
		WithIsVisible(false).
		WithVerticalContentAlignment(VerticalAlignmentCenter).
		WithBackgroundImage(func(bi *BackgroundImageBuilder) {
			bi.WithURL("https://example.com/bg.png").WithFillMode(BackgroundImageFillModeCover)
		}).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("inside")
		}).
		Build()

	assert.Equal(t, "Container", c["type"])
	assert.Equal(t, "c1", c["id"])
	assert.Equal(t, string(ContainerStyleEmphasis), c["style"])
	assert.Equal(t, true, c["bleed"])
	assert.Equal(t, "200px", c["minHeight"])
	assert.Equal(t, true, c["separator"])
	assert.Equal(t, false, c["isVisible"])

	bg := c["backgroundImage"].(Card)
	assert.Equal(t, "https://example.com/bg.png", bg["url"])
	assert.Equal(t, string(BackgroundImageFillModeCover), bg["fillMode"])

	sa := c["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])

	items := c["items"].([]any)
	require.Len(t, items, 1)
	assert.Equal(t, "TextBlock", items[0].(Card)["type"])
}

// ── ColumnSet ────────────────────────────────────────────────────────────────

func TestSchemaConformance_ColumnSet_WithColumns(t *testing.T) {
	t.Parallel()
	cs := newColumnSetBuilder().
		WithID("cs1").
		WithStyle(ContainerStyleAccent).
		WithBleed(true).
		WithMinHeight("100px").
		WithHorizontalAlignment(HorizontalAlignmentCenter).
		WithSpacing(SpacingLarge).
		WithSeparator(true).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		AddColumn(func(col *ColumnBuilder) {
			col.WithWidth("auto").AddTextBlock(func(tb *TextBlockBuilder) {
				tb.WithText("Col1")
			})
		}).
		AddColumn(func(col *ColumnBuilder) {
			col.WithWidth("stretch").AddTextBlock(func(tb *TextBlockBuilder) {
				tb.WithText("Col2")
			})
		}).
		Build()

	assert.Equal(t, "ColumnSet", cs["type"])
	assert.Equal(t, "cs1", cs["id"])
	assert.Equal(t, string(ContainerStyleAccent), cs["style"])
	assert.Equal(t, true, cs["bleed"])

	cols := cs["columns"].([]any)
	require.Len(t, cols, 2)
	assert.Equal(t, "auto", cols[0].(Card)["width"])
	assert.Equal(t, "stretch", cols[1].(Card)["width"])
}

// ── FactSet ──────────────────────────────────────────────────────────────────

func TestSchemaConformance_FactSet(t *testing.T) {
	t.Parallel()
	fs := newFactSetBuilder().
		WithID("fs1").
		WithSpacing(SpacingSmall).
		AddFact("Name", "John").
		AddFact("Age", "30").
		Build()

	assert.Equal(t, "FactSet", fs["type"])
	assert.Equal(t, "fs1", fs["id"])

	facts := fs["facts"].([]any)
	require.Len(t, facts, 2)
	assert.Equal(t, "Name", facts[0].(Card)["title"])
	assert.Equal(t, "John", facts[0].(Card)["value"])
}

// ── RichTextBlock ────────────────────────────────────────────────────────────

func TestSchemaConformance_RichTextBlock(t *testing.T) {
	t.Parallel()
	rtb := newRichTextBlockBuilder().
		WithID("rtb1").
		WithHorizontalAlignment(HorizontalAlignmentCenter).
		WithSpacing(SpacingMedium).
		AddText("plain text").
		AddTextRun(func(tr *TextRunBuilder) {
			tr.WithText("bold").WithWeight(TextWeightBolder).WithColor(TextColorAccent)
		}).
		Build()

	assert.Equal(t, "RichTextBlock", rtb["type"])
	assert.Equal(t, "rtb1", rtb["id"])
	assert.Equal(t, string(HorizontalAlignmentCenter), rtb["horizontalAlignment"])

	inlines := rtb["inlines"].([]any)
	require.Len(t, inlines, 2)
	assert.Equal(t, "plain text", inlines[0])
	run := inlines[1].(Card)
	assert.Equal(t, "TextRun", run["type"])
	assert.Equal(t, "bold", run["text"])
	assert.Equal(t, string(TextWeightBolder), run["weight"])
}

// ── Table ────────────────────────────────────────────────────────────────────

func TestSchemaConformance_Table(t *testing.T) {
	t.Parallel()
	tbl := newTableBuilder().
		WithID("tbl1").
		WithFirstRowAsHeader(true).
		WithShowGridLines(true).
		WithGridStyle(ContainerStyleEmphasis).
		WithHorizontalCellContentAlignment(HorizontalAlignmentCenter).
		WithVerticalCellContentAlignment(VerticalAlignmentCenter).
		WithSpacing(SpacingLarge).
		AddColumn(map[string]any{"width": 1}).
		AddColumn(map[string]any{"width": 2}).
		AddRow(map[string]any{
			"type": "TableRow",
			"cells": []any{
				map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "A"}}},
				map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "B"}}},
			},
		}).
		Build()

	assert.Equal(t, "Table", tbl["type"])
	assert.Equal(t, "tbl1", tbl["id"])
	assert.Equal(t, true, tbl["firstRowAsHeader"])
	assert.Equal(t, true, tbl["showGridLines"])

	cols := tbl["columns"].([]any)
	require.Len(t, cols, 2)

	rows := tbl["rows"].([]any)
	require.Len(t, rows, 1)
}

// ── ActionSet ────────────────────────────────────────────────────────────────

func TestSchemaConformance_ActionSet_MixedActions(t *testing.T) {
	t.Parallel()
	as := newActionSetBuilder().
		WithID("as1").
		WithSpacing(SpacingMedium).
		AddAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com").WithTitle("Open")
		}).
		AddAction(func(a *ActionBuilder) {
			a.Submit("Send").WithData(map[string]any{"key": "val"})
		}).
		AddAction(func(a *ActionBuilder) {
			a.Execute("Run").WithVerb("doSomething")
		}).
		Build()

	assert.Equal(t, "ActionSet", as["type"])
	assert.Equal(t, "as1", as["id"])

	actions := as["actions"].([]any)
	require.Len(t, actions, 3)
	assert.Equal(t, "Action.OpenUrl", actions[0].(Card)["type"])
	assert.Equal(t, "Action.Submit", actions[1].(Card)["type"])
	assert.Equal(t, "Action.Execute", actions[2].(Card)["type"])
}

// ── Card-level properties ────────────────────────────────────────────────────

func TestSchemaConformance_CardLevel_AllProperties(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithVersion("1.6").
		WithFallbackText("fallback").
		WithSpeak("speak text").
		WithLang("en").
		WithRTL(true).
		WithMinHeight("300px").
		WithVerticalContentAlignment(VerticalAlignmentBottom).
		WithMetadata("https://example.com/card").
		WithBackgroundImage(func(bi *BackgroundImageBuilder) {
			bi.WithURL("https://example.com/bg.png")
		}).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		Build()

	assert.Equal(t, "AdaptiveCard", card["type"])
	assert.Equal(t, "1.6", card["version"])
	assert.Equal(t, "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json", card["$schema"])
	assert.Equal(t, "fallback", card["fallbackText"])
	assert.Equal(t, "speak text", card["speak"])
	assert.Equal(t, "en", card["lang"])
	assert.Equal(t, true, card["rtl"])
	assert.Equal(t, "300px", card["minHeight"])
	assert.Equal(t, string(VerticalAlignmentBottom), card["verticalContentAlignment"])

	meta := card["metadata"].(Card)
	assert.Equal(t, "https://example.com/card", meta["webUrl"])

	bg := card["backgroundImage"].(Card)
	assert.Equal(t, "https://example.com/bg.png", bg["url"])

	sa := card["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
}

func TestSchemaConformance_CardLevel_DefaultVersionSchema(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().Build()
	assert.Equal(t, "1.5", card["version"])
	assert.Equal(t, "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json", card["$schema"])
}

// ── Input elements ───────────────────────────────────────────────────────────

func TestSchemaConformance_InputText(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputText(func(i *InputTextBuilder) {
			i.WithID("txt1").
				WithLabel("Name").
				WithPlaceholder("Enter name").
				WithValue("default").
				WithMaxLength(100).
				WithIsMultiline(true).
				WithStyle(TextInputStyleEmail).
				WithRegex(`^[a-z]+$`).
				WithIsRequired(true).
				WithErrorMessage("Required!").
				WithSpacing(SpacingSmall).
				WithInlineAction(func(a *ActionBuilder) {
					a.Submit("Go")
				})
		}).
		Build()

	body := card["body"].([]any)
	require.Len(t, body, 1)
	inp := body[0].(Card)
	assert.Equal(t, "Input.Text", inp["type"])
	assert.Equal(t, "txt1", inp["id"])
	assert.Equal(t, "Name", inp["label"])
	assert.Equal(t, "Enter name", inp["placeholder"])
	assert.Equal(t, "default", inp["value"])
	assert.Equal(t, 100, inp["maxLength"])
	assert.Equal(t, true, inp["isMultiline"])
	assert.Equal(t, string(TextInputStyleEmail), inp["style"])
	assert.Equal(t, `^[a-z]+$`, inp["regex"])
	assert.Equal(t, true, inp["isRequired"])
	assert.Equal(t, "Required!", inp["errorMessage"])
	assert.NotNil(t, inp["inlineAction"])
}

func TestSchemaConformance_InputNumber(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputNumber(func(i *InputNumberBuilder) {
			i.WithID("num1").
				WithLabel("Qty").
				WithPlaceholder("0-100").
				WithValue(42).
				WithMin(0).
				WithMax(100).
				WithIsRequired(true).
				WithErrorMessage("Bad").
				WithSpacing(SpacingMedium)
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	assert.Equal(t, "Input.Number", inp["type"])
	assert.Equal(t, "num1", inp["id"])
	assert.Equal(t, float64(42), inp["value"])
	assert.Equal(t, float64(0), inp["min"])
	assert.Equal(t, float64(100), inp["max"])
}

func TestSchemaConformance_InputDate(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputDate(func(i *InputDateBuilder) {
			i.WithID("date1").
				WithLabel("Date").
				WithPlaceholder("yyyy-mm-dd").
				WithValue("2024-01-15").
				WithMin("2024-01-01").
				WithMax("2024-12-31").
				WithIsRequired(true).
				WithErrorMessage("Bad date")
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	assert.Equal(t, "Input.Date", inp["type"])
	assert.Equal(t, "date1", inp["id"])
	assert.Equal(t, "2024-01-15", inp["value"])
	assert.Equal(t, "2024-01-01", inp["min"])
	assert.Equal(t, "2024-12-31", inp["max"])
}

func TestSchemaConformance_InputTime(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputTime(func(i *InputTimeBuilder) {
			i.WithID("time1").
				WithLabel("Time").
				WithPlaceholder("HH:mm").
				WithValue("09:00").
				WithMin("08:00").
				WithMax("17:00").
				WithIsRequired(true).
				WithErrorMessage("Bad time")
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	assert.Equal(t, "Input.Time", inp["type"])
	assert.Equal(t, "time1", inp["id"])
	assert.Equal(t, "09:00", inp["value"])
	assert.Equal(t, "08:00", inp["min"])
	assert.Equal(t, "17:00", inp["max"])
}

func TestSchemaConformance_InputToggle(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputToggle(func(i *InputToggleBuilder) {
			i.WithID("tog1").
				WithTitle("Agree").
				WithLabel("Agreement").
				WithValue("true").
				WithValueOn("yes").
				WithValueOff("no").
				WithWrap(true).
				WithIsRequired(true).
				WithErrorMessage("Must agree")
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	assert.Equal(t, "Input.Toggle", inp["type"])
	assert.Equal(t, "tog1", inp["id"])
	assert.Equal(t, "Agree", inp["title"])
	assert.Equal(t, "yes", inp["valueOn"])
	assert.Equal(t, "no", inp["valueOff"])
	assert.Equal(t, true, inp["wrap"])
}

func TestSchemaConformance_InputChoiceSet(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputChoiceSet(func(i *InputChoiceSetBuilder) {
			i.WithID("cs1").
				WithLabel("Color").
				WithPlaceholder("Pick one").
				WithValue("r").
				WithStyle(ChoiceInputStyleExpanded).
				WithIsMultiSelect(true).
				WithWrap(true).
				WithIsRequired(true).
				WithErrorMessage("Pick!").
				AddChoice("Red", "r").
				AddChoice("Blue", "b")
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	assert.Equal(t, "Input.ChoiceSet", inp["type"])
	assert.Equal(t, "cs1", inp["id"])
	assert.Equal(t, string(ChoiceInputStyleExpanded), inp["style"])
	assert.Equal(t, true, inp["isMultiSelect"])
	choices := inp["choices"].([]any)
	require.Len(t, choices, 2)
	assert.Equal(t, "Red", choices[0].(Card)["title"])
	assert.Equal(t, "r", choices[0].(Card)["value"])
}

func TestSchemaConformance_InputChoiceSet_ChoicesData(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		AddInputChoiceSet(func(i *InputChoiceSetBuilder) {
			i.WithID("people").WithChoicesData("graph.microsoft.com/users")
		}).
		Build()

	body := card["body"].([]any)
	inp := body[0].(Card)
	cd := inp["choices.data"].(Card)
	assert.Equal(t, "graph.microsoft.com/users", cd["dataset"])
}
