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

// ── Media ────────────────────────────────────────────────────────────────────

func TestSchemaConformance_Media_AllProperties(t *testing.T) {
	t.Parallel()
	m := newMediaBuilder().
		WithID("media1").
		WithPoster("https://example.com/poster.png").
		WithAltText("A video").
		WithSpacing(SpacingLarge).
		WithSeparator(true).
		WithIsVisible(false).
		WithHeight("stretch").
		AddSource("https://example.com/video.mp4", "video/mp4").
		AddSource("https://example.com/video.webm", "video/webm").
		Build()

	assert.Equal(t, "Media", m["type"])
	assert.Equal(t, "media1", m["id"])
	assert.Equal(t, "https://example.com/poster.png", m["poster"])
	assert.Equal(t, "A video", m["altText"])
	assert.Equal(t, string(SpacingLarge), m["spacing"])
	assert.Equal(t, true, m["separator"])
	assert.Equal(t, false, m["isVisible"])
	assert.Equal(t, "stretch", m["height"])

	sources := m["sources"].([]any)
	require.Len(t, sources, 2)
	assert.Equal(t, "https://example.com/video.mp4", sources[0].(Card)["url"])
	assert.Equal(t, "video/mp4", sources[0].(Card)["mimeType"])
	assert.Equal(t, "https://example.com/video.webm", sources[1].(Card)["url"])
}

// ── ImageSet ─────────────────────────────────────────────────────────────────

func TestSchemaConformance_ImageSet_AllProperties(t *testing.T) {
	t.Parallel()
	is := newImageSetBuilder().
		WithID("is1").
		WithImageSize(ImageSizeMedium).
		WithSpacing(SpacingSmall).
		WithSeparator(true).
		WithIsVisible(false).
		WithHeight("auto").
		AddImage(func(img *ImageBuilder) {
			img.WithURL("https://example.com/1.png")
		}).
		AddImage(func(img *ImageBuilder) {
			img.WithURL("https://example.com/2.png")
		}).
		Build()

	assert.Equal(t, "ImageSet", is["type"])
	assert.Equal(t, "is1", is["id"])
	assert.Equal(t, string(ImageSizeMedium), is["imageSize"])
	assert.Equal(t, string(SpacingSmall), is["spacing"])
	assert.Equal(t, true, is["separator"])
	assert.Equal(t, false, is["isVisible"])

	images := is["images"].([]any)
	require.Len(t, images, 2)
	assert.Equal(t, "https://example.com/1.png", images[0].(Card)["url"])
}

// ── Column (standalone) ──────────────────────────────────────────────────────

func TestSchemaConformance_Column_AllProperties(t *testing.T) {
	t.Parallel()
	col := newColumnBuilder().
		WithID("col1").
		WithWidth("50px").
		WithStyle(ContainerStyleGood).
		WithVerticalContentAlignment(VerticalAlignmentBottom).
		WithBleed(true).
		WithMinHeight("100px").
		WithSpacing(SpacingSmall).
		WithSeparator(true).
		WithIsVisible(false).
		WithHeight("stretch").
		WithRtl(true).
		WithBackgroundImage(func(bi *BackgroundImageBuilder) {
			bi.WithURL("https://example.com/bg.png").WithFillMode(BackgroundImageFillModeRepeat)
		}).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		AddTextBlock(func(tb *TextBlockBuilder) {
			tb.WithText("inside")
		}).
		Build()

	assert.Equal(t, "Column", col["type"])
	assert.Equal(t, "col1", col["id"])
	assert.Equal(t, "50px", col["width"])
	assert.Equal(t, string(ContainerStyleGood), col["style"])
	assert.Equal(t, string(VerticalAlignmentBottom), col["verticalContentAlignment"])
	assert.Equal(t, true, col["bleed"])
	assert.Equal(t, "100px", col["minHeight"])
	assert.Equal(t, string(SpacingSmall), col["spacing"])
	assert.Equal(t, true, col["separator"])
	assert.Equal(t, false, col["isVisible"])
	assert.Equal(t, "stretch", col["height"])
	assert.Equal(t, true, col["rtl"])

	bg := col["backgroundImage"].(Card)
	assert.Equal(t, "https://example.com/bg.png", bg["url"])
	assert.Equal(t, string(BackgroundImageFillModeRepeat), bg["fillMode"])

	sa := col["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])

	items := col["items"].([]any)
	require.Len(t, items, 1)
}

// ── Container with multiple item types ───────────────────────────────────────

func TestSchemaConformance_Container_MultipleItemTypes(t *testing.T) {
	t.Parallel()
	c := newContainerBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("text") }).
		AddImage(func(img *ImageBuilder) { img.WithURL("https://example.com/img.png") }).
		AddContainer(func(inner *ContainerBuilder) {
			inner.AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("nested") })
		}).
		Build()

	items := c["items"].([]any)
	require.Len(t, items, 3)
	assert.Equal(t, "TextBlock", items[0].(Card)["type"])
	assert.Equal(t, "Image", items[1].(Card)["type"])
	assert.Equal(t, "Container", items[2].(Card)["type"])
}

// ── TextRun decorations ──────────────────────────────────────────────────────

func TestSchemaConformance_TextRun_AllDecorations(t *testing.T) {
	t.Parallel()
	rtb := newRichTextBlockBuilder().
		AddTextRun(func(tr *TextRunBuilder) {
			tr.WithText("decorated").
				WithSize(TextSizeLarge).
				WithWeight(TextWeightBolder).
				WithColor(TextColorGood).
				WithIsSubtle(true).
				WithItalic(true).
				WithStrikethrough(true).
				WithUnderline(true).
				WithHighlight(true).
				WithFontType(FontTypeMonospace).
				WithSelectAction(func(a *ActionBuilder) {
					a.OpenURL("https://example.com")
				})
		}).
		Build()

	inlines := rtb["inlines"].([]any)
	require.Len(t, inlines, 1)
	run := inlines[0].(Card)
	assert.Equal(t, "TextRun", run["type"])
	assert.Equal(t, "decorated", run["text"])
	assert.Equal(t, string(TextSizeLarge), run["size"])
	assert.Equal(t, string(TextWeightBolder), run["weight"])
	assert.Equal(t, string(TextColorGood), run["color"])
	assert.Equal(t, true, run["isSubtle"])
	assert.Equal(t, true, run["italic"])
	assert.Equal(t, true, run["strikethrough"])
	assert.Equal(t, true, run["underline"])
	assert.Equal(t, true, run["highlight"])
	assert.Equal(t, string(FontTypeMonospace), run["fontType"])
	sa := run["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
}

// ── BackgroundImage ──────────────────────────────────────────────────────────

func TestSchemaConformance_BackgroundImage_AllProperties(t *testing.T) {
	t.Parallel()
	bi := newBackgroundImageBuilder().
		WithURL("https://example.com/bg.png").
		WithFillMode(BackgroundImageFillModeCover).
		WithHorizontalAlignment(HorizontalAlignmentCenter).
		WithVerticalAlignment(VerticalAlignmentBottom).
		Build()

	assert.Equal(t, "https://example.com/bg.png", bi["url"])
	assert.Equal(t, string(BackgroundImageFillModeCover), bi["fillMode"])
	assert.Equal(t, string(HorizontalAlignmentCenter), bi["horizontalAlignment"])
	assert.Equal(t, string(VerticalAlignmentBottom), bi["verticalAlignment"])
}

// ── Action tests ─────────────────────────────────────────────────────────────

func TestSchemaConformance_Action_OpenUrl_AllProperties(t *testing.T) {
	t.Parallel()
	a := newActionBuilder().
		OpenURL("https://example.com").
		WithID("a1").
		WithTitle("Open").
		WithIconURL("https://example.com/icon.png").
		WithStyle(ActionStylePositive).
		WithTooltip("Click to open").
		WithIsEnabled(false).
		WithMode(ActionModeSecondary).
		WithRequires("adaptiveCards", "1.5").
		Build()

	assert.Equal(t, "Action.OpenUrl", a["type"])
	assert.Equal(t, "https://example.com", a["url"])
	assert.Equal(t, "a1", a["id"])
	assert.Equal(t, "Open", a["title"])
	assert.Equal(t, "https://example.com/icon.png", a["iconUrl"])
	assert.Equal(t, string(ActionStylePositive), a["style"])
	assert.Equal(t, "Click to open", a["tooltip"])
	assert.Equal(t, false, a["isEnabled"])
	assert.Equal(t, string(ActionModeSecondary), a["mode"])
	reqs := a["requires"].(map[string]any)
	assert.Equal(t, "1.5", reqs["adaptiveCards"])
}

func TestSchemaConformance_Action_Submit_AllProperties(t *testing.T) {
	t.Parallel()
	a := newActionBuilder().
		Submit("Send").
		WithID("a2").
		WithData(map[string]any{"key": "val"}).
		WithAssociatedInputs(AssociatedInputsAuto).
		WithIconURL("https://example.com/icon.png").
		WithStyle(ActionStyleDestructive).
		WithTooltip("Submit form").
		WithIsEnabled(true).
		WithMode(ActionModePrimary).
		Build()

	assert.Equal(t, "Action.Submit", a["type"])
	assert.Equal(t, "a2", a["id"])
	assert.Equal(t, "Send", a["title"])
	assert.Equal(t, map[string]any{"key": "val"}, a["data"])
	assert.Equal(t, string(AssociatedInputsAuto), a["associatedInputs"])
	assert.Equal(t, "https://example.com/icon.png", a["iconUrl"])
	assert.Equal(t, string(ActionStyleDestructive), a["style"])
	assert.Equal(t, "Submit form", a["tooltip"])
	assert.Equal(t, true, a["isEnabled"])
	assert.Equal(t, string(ActionModePrimary), a["mode"])
}

func TestSchemaConformance_Action_Execute_AllProperties(t *testing.T) {
	t.Parallel()
	a := newActionBuilder().
		Execute("Run").
		WithID("a3").
		WithVerb("doSomething").
		WithData(map[string]any{"x": 1}).
		WithAssociatedInputs(AssociatedInputsNone).
		WithIconURL("https://example.com/icon.png").
		WithStyle(ActionStylePositive).
		WithTooltip("Execute action").
		WithIsEnabled(false).
		WithMode(ActionModeSecondary).
		Build()

	assert.Equal(t, "Action.Execute", a["type"])
	assert.Equal(t, "a3", a["id"])
	assert.Equal(t, "Run", a["title"])
	assert.Equal(t, "doSomething", a["verb"])
	assert.Equal(t, map[string]any{"x": 1}, a["data"])
	assert.Equal(t, string(AssociatedInputsNone), a["associatedInputs"])
	assert.Equal(t, "https://example.com/icon.png", a["iconUrl"])
	assert.Equal(t, string(ActionStylePositive), a["style"])
	assert.Equal(t, "Execute action", a["tooltip"])
	assert.Equal(t, false, a["isEnabled"])
	assert.Equal(t, string(ActionModeSecondary), a["mode"])
}

func TestSchemaConformance_Action_ShowCard(t *testing.T) {
	t.Parallel()
	innerCard := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("inner") }).
		Build()

	a := newActionBuilder().
		ShowCard("Details").
		WithID("a4").
		WithCard(innerCard).
		WithIconURL("https://example.com/icon.png").
		WithStyle(ActionStyleDefault).
		WithTooltip("Show details").
		Build()

	assert.Equal(t, "Action.ShowCard", a["type"])
	assert.Equal(t, "a4", a["id"])
	assert.Equal(t, "Details", a["title"])
	assert.Equal(t, "https://example.com/icon.png", a["iconUrl"])
	assert.Equal(t, string(ActionStyleDefault), a["style"])
	assert.Equal(t, "Show details", a["tooltip"])

	nested := a["card"].(Card)
	assert.Equal(t, "AdaptiveCard", nested["type"])
	body := nested["body"].([]any)
	require.Len(t, body, 1)
	assert.Equal(t, "inner", body[0].(Card)["text"])
}

func TestSchemaConformance_Action_ToggleVisibility(t *testing.T) {
	t.Parallel()
	boolTrue := true
	boolFalse := false
	a := newActionBuilder().
		ToggleVisibility("Toggle").
		WithID("a5").
		AddTargetElement("el1", nil).
		AddTargetElement("el2", &boolTrue).
		AddTargetElement("el3", &boolFalse).
		WithIconURL("https://example.com/icon.png").
		WithStyle(ActionStylePositive).
		Build()

	assert.Equal(t, "Action.ToggleVisibility", a["type"])
	assert.Equal(t, "a5", a["id"])
	assert.Equal(t, "Toggle", a["title"])

	targets := a["targetElements"].([]any)
	require.Len(t, targets, 3)
	assert.Equal(t, "el1", targets[0])
	target2 := targets[1].(map[string]any)
	assert.Equal(t, "el2", target2["elementId"])
	assert.Equal(t, true, target2["isVisible"])
	target3 := targets[2].(map[string]any)
	assert.Equal(t, "el3", target3["elementId"])
	assert.Equal(t, false, target3["isVisible"])
}

// ── Advanced features ────────────────────────────────────────────────────────

func TestSchemaConformance_Refresh_AllProperties(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithRefresh(func(r *RefreshBuilder) {
			r.WithAction(func(a *ActionBuilder) {
				a.Execute("Refresh").WithVerb("refresh")
			}).
			AddUserID("user1").
			AddUserID("user2").
			WithExpires("2024-12-31T23:59:59Z")
		}).
		Build()

	refresh := card["refresh"].(Card)
	action := refresh["action"].(Card)
	assert.Equal(t, "Action.Execute", action["type"])
	assert.Equal(t, "refresh", action["verb"])

	userIDs := refresh["userIds"].([]any)
	require.Len(t, userIDs, 2)
	assert.Equal(t, "user1", userIDs[0])
	assert.Equal(t, "user2", userIDs[1])
	assert.Equal(t, "2024-12-31T23:59:59Z", refresh["expires"])
}

func TestSchemaConformance_Authentication_AllProperties(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithAuthentication(func(auth *AuthenticationBuilder) {
			auth.WithText("Please sign in").
				WithConnectionName("myConnection").
				WithTokenExchangeResource(map[string]any{
					"id":         "tok1",
					"uri":        "https://example.com/token",
					"providerId": "myProvider",
				}).
				AddButton(map[string]any{
					"type":  "signin",
					"title": "Sign In",
					"image": "https://example.com/signin.png",
					"value": "https://example.com/auth",
				})
		}).
		Build()

	auth := card["authentication"].(Card)
	assert.Equal(t, "Please sign in", auth["text"])
	assert.Equal(t, "myConnection", auth["connectionName"])

	ter := auth["tokenExchangeResource"].(map[string]any)
	assert.Equal(t, "tok1", ter["id"])
	assert.Equal(t, "https://example.com/token", ter["uri"])
	assert.Equal(t, "myProvider", ter["providerId"])

	buttons := auth["buttons"].([]any)
	require.Len(t, buttons, 1)
	btn := buttons[0].(map[string]any)
	assert.Equal(t, "signin", btn["type"])
	assert.Equal(t, "Sign In", btn["title"])
}

func TestSchemaConformance_Metadata(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithMetadata("https://example.com/card").
		Build()

	meta := card["metadata"].(Card)
	assert.Equal(t, "https://example.com/card", meta["webUrl"])
}

func TestSchemaConformance_CaptionSources(t *testing.T) {
	t.Parallel()
	m := newMediaBuilder().
		AddSource("https://example.com/video.mp4", "video/mp4").
		AddCaptionSource("text/vtt", "https://example.com/captions.vtt", "English").
		Build()

	assert.Equal(t, "Media", m["type"])
	captions := m["captionSources"].([]any)
	require.Len(t, captions, 1)
	cap := captions[0].(map[string]any)
	assert.Equal(t, "CaptionSource", cap["type"])
	assert.Equal(t, "text/vtt", cap["mimeType"])
	assert.Equal(t, "https://example.com/captions.vtt", cap["url"])
	assert.Equal(t, "English", cap["label"])
}

// ── Common properties (fallback, requires, height) ───────────────────────────

func TestSchemaConformance_CommonProperties_Fallback(t *testing.T) {
	t.Parallel()
	c := newContainerBuilder().
		WithFallback("drop").
		WithRequires("adaptiveCards", "1.5").
		WithHeight("stretch").
		WithRtl(true).
		Build()

	assert.Equal(t, "drop", c["fallback"])
	reqs := c["requires"].(map[string]any)
	assert.Equal(t, "1.5", reqs["adaptiveCards"])
	assert.Equal(t, "stretch", c["height"])
	assert.Equal(t, true, c["rtl"])
}

func TestSchemaConformance_CommonProperties_TextBlock(t *testing.T) {
	t.Parallel()
	tb := newTextBlockBuilder().
		WithText("test").
		WithFallback("drop").
		WithRequires("adaptiveCards", "1.2").
		WithHeight("stretch").
		Build()

	assert.Equal(t, "drop", tb["fallback"])
	reqs := tb["requires"].(map[string]any)
	assert.Equal(t, "1.2", reqs["adaptiveCards"])
	assert.Equal(t, "stretch", tb["height"])
}

func TestSchemaConformance_CommonProperties_MediaRequires(t *testing.T) {
	t.Parallel()
	m := newMediaBuilder().
		WithRequires("adaptiveCards", "1.1").
		WithRtl(true).
		WithFallback("drop").
		Build()

	assert.Equal(t, "drop", m["fallback"])
	reqs := m["requires"].(map[string]any)
	assert.Equal(t, "1.1", reqs["adaptiveCards"])
	assert.Equal(t, true, m["rtl"])
}

func TestSchemaConformance_CommonProperties_ImageSetRequires(t *testing.T) {
	t.Parallel()
	is := newImageSetBuilder().
		WithRequires("adaptiveCards", "1.2").
		WithFallback("drop").
		WithRtl(true).
		Build()

	assert.Equal(t, "drop", is["fallback"])
	reqs := is["requires"].(map[string]any)
	assert.Equal(t, "1.2", reqs["adaptiveCards"])
	assert.Equal(t, true, is["rtl"])
}

func TestSchemaConformance_CommonProperties_RichTextBlockRequires(t *testing.T) {
	t.Parallel()
	rtb := newRichTextBlockBuilder().
		WithRequires("adaptiveCards", "1.2").
		WithFallback("drop").
		WithHeight("stretch").
		WithRtl(true).
		Build()

	assert.Equal(t, "drop", rtb["fallback"])
	reqs := rtb["requires"].(map[string]any)
	assert.Equal(t, "1.2", reqs["adaptiveCards"])
	assert.Equal(t, "stretch", rtb["height"])
	assert.Equal(t, true, rtb["rtl"])
}

func TestSchemaConformance_CommonProperties_TableRequires(t *testing.T) {
	t.Parallel()
	tbl := newTableBuilder().
		WithRequires("adaptiveCards", "1.5").
		WithFallback("drop").
		WithHeight("auto").
		WithRtl(true).
		Build()

	assert.Equal(t, "drop", tbl["fallback"])
	reqs := tbl["requires"].(map[string]any)
	assert.Equal(t, "1.5", reqs["adaptiveCards"])
	assert.Equal(t, "auto", tbl["height"])
	assert.Equal(t, true, tbl["rtl"])
}

func TestSchemaConformance_CommonProperties_ColumnSetRequires(t *testing.T) {
	t.Parallel()
	cs := newColumnSetBuilder().
		WithRequires("adaptiveCards", "1.0").
		WithFallback("drop").
		WithHeight("auto").
		WithRtl(true).
		WithIsVisible(false).
		Build()

	assert.Equal(t, "drop", cs["fallback"])
	reqs := cs["requires"].(map[string]any)
	assert.Equal(t, "1.0", reqs["adaptiveCards"])
	assert.Equal(t, "auto", cs["height"])
	assert.Equal(t, true, cs["rtl"])
	assert.Equal(t, false, cs["isVisible"])
}

func TestSchemaConformance_CommonProperties_ColumnRequires(t *testing.T) {
	t.Parallel()
	col := newColumnBuilder().
		WithRequires("adaptiveCards", "1.0").
		WithFallback("drop").
		Build()

	assert.Equal(t, "drop", col["fallback"])
	reqs := col["requires"].(map[string]any)
	assert.Equal(t, "1.0", reqs["adaptiveCards"])
}

// ── Card-level individual property tests ─────────────────────────────────────

func TestSchemaConformance_CardLevel_FallbackText(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithFallbackText("fallback").Build()
	assert.Equal(t, "fallback", card["fallbackText"])
}

func TestSchemaConformance_CardLevel_Speak(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithSpeak("speak text").Build()
	assert.Equal(t, "speak text", card["speak"])
}

func TestSchemaConformance_CardLevel_Lang(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithLang("en").Build()
	assert.Equal(t, "en", card["lang"])
}

func TestSchemaConformance_CardLevel_RTL(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithRTL(true).Build()
	assert.Equal(t, true, card["rtl"])
}

func TestSchemaConformance_CardLevel_MinHeight(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithMinHeight("300px").Build()
	assert.Equal(t, "300px", card["minHeight"])
}

func TestSchemaConformance_CardLevel_VerticalContentAlignment(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().WithVerticalContentAlignment(VerticalAlignmentCenter).Build()
	assert.Equal(t, string(VerticalAlignmentCenter), card["verticalContentAlignment"])
}

func TestSchemaConformance_CardLevel_SelectAction(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		Build()

	sa := card["selectAction"].(Card)
	assert.Equal(t, "Action.OpenUrl", sa["type"])
}

func TestSchemaConformance_CardLevel_WithBodyAndActions(t *testing.T) {
	t.Parallel()
	card := NewAdaptiveCardBuilder().
		WithVersion("1.6").
		WithFallbackText("fb").
		WithSpeak("speak").
		WithLang("en").
		WithRTL(true).
		WithMinHeight("300px").
		WithVerticalContentAlignment(VerticalAlignmentBottom).
		WithMetadata("https://example.com/card").
		WithBackgroundImage(func(bi *BackgroundImageBuilder) {
			bi.WithURL("https://example.com/bg.png").
				WithFillMode(BackgroundImageFillModeCover).
				WithHorizontalAlignment(HorizontalAlignmentCenter).
				WithVerticalAlignment(VerticalAlignmentTop)
		}).
		WithSelectAction(func(a *ActionBuilder) {
			a.OpenURL("https://example.com")
		}).
		WithRefresh(func(r *RefreshBuilder) {
			r.WithAction(func(a *ActionBuilder) { a.Execute("Refresh") })
		}).
		WithAuthentication(func(auth *AuthenticationBuilder) {
			auth.WithText("Sign in")
		}).
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("Hello") }).
		AddAction(func(a *ActionBuilder) { a.Submit("Go") }).
		Build()

	assert.Equal(t, "AdaptiveCard", card["type"])
	assert.Equal(t, "1.6", card["version"])
	assert.Equal(t, "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json", card["$schema"])
	assert.Equal(t, "fb", card["fallbackText"])
	assert.Equal(t, "speak", card["speak"])
	assert.Equal(t, "en", card["lang"])
	assert.Equal(t, true, card["rtl"])
	assert.Equal(t, "300px", card["minHeight"])
	assert.Equal(t, string(VerticalAlignmentBottom), card["verticalContentAlignment"])
	assert.NotNil(t, card["metadata"])
	assert.NotNil(t, card["backgroundImage"])
	assert.NotNil(t, card["selectAction"])
	assert.NotNil(t, card["refresh"])
	assert.NotNil(t, card["authentication"])

	body := card["body"].([]any)
	require.Len(t, body, 1)
	actions := card["actions"].([]any)
	require.Len(t, actions, 1)
}

// ── Enum conformance tests ───────────────────────────────────────────────────

func TestSchemaConformance_Enum_TextSize(t *testing.T) {
	t.Parallel()
	values := []string{
		string(TextSizeSmall), string(TextSizeDefault), string(TextSizeMedium),
		string(TextSizeLarge), string(TextSizeExtraLarge),
	}
	assert.Contains(t, values, "small")
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "medium")
	assert.Contains(t, values, "large")
	assert.Contains(t, values, "extraLarge")
	assert.Len(t, values, 5)
}

func TestSchemaConformance_Enum_TextWeight(t *testing.T) {
	t.Parallel()
	values := []string{
		string(TextWeightLighter), string(TextWeightDefault), string(TextWeightBolder),
	}
	assert.Contains(t, values, "lighter")
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "bolder")
	assert.Len(t, values, 3)
}

func TestSchemaConformance_Enum_TextColor(t *testing.T) {
	t.Parallel()
	values := []string{
		string(TextColorDefault), string(TextColorDark), string(TextColorLight),
		string(TextColorAccent), string(TextColorGood), string(TextColorAttention),
		string(TextColorWarning),
	}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "dark")
	assert.Contains(t, values, "light")
	assert.Contains(t, values, "accent")
	assert.Contains(t, values, "good")
	assert.Contains(t, values, "attention")
	assert.Contains(t, values, "warning")
	assert.Len(t, values, 7)
}

func TestSchemaConformance_Enum_FontType(t *testing.T) {
	t.Parallel()
	values := []string{string(FontTypeDefault), string(FontTypeMonospace)}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "monospace")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_TextBlockStyle(t *testing.T) {
	t.Parallel()
	values := []string{string(TextBlockStyleDefault), string(TextBlockStyleHeading)}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "heading")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_HorizontalAlignment(t *testing.T) {
	t.Parallel()
	values := []string{
		string(HorizontalAlignmentLeft), string(HorizontalAlignmentCenter),
		string(HorizontalAlignmentRight),
	}
	assert.Contains(t, values, "left")
	assert.Contains(t, values, "center")
	assert.Contains(t, values, "right")
	assert.Len(t, values, 3)
}

func TestSchemaConformance_Enum_VerticalAlignment(t *testing.T) {
	t.Parallel()
	values := []string{
		string(VerticalAlignmentTop), string(VerticalAlignmentCenter),
		string(VerticalAlignmentBottom),
	}
	assert.Contains(t, values, "top")
	assert.Contains(t, values, "center")
	assert.Contains(t, values, "bottom")
	assert.Len(t, values, 3)
}

func TestSchemaConformance_Enum_Spacing(t *testing.T) {
	t.Parallel()
	values := []string{
		string(SpacingDefault), string(SpacingNone), string(SpacingSmall),
		string(SpacingMedium), string(SpacingLarge), string(SpacingExtraLarge),
		string(SpacingPadding),
	}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "none")
	assert.Contains(t, values, "small")
	assert.Contains(t, values, "medium")
	assert.Contains(t, values, "large")
	assert.Contains(t, values, "extraLarge")
	assert.Contains(t, values, "padding")
	assert.Len(t, values, 7)
}

func TestSchemaConformance_Enum_ContainerStyle(t *testing.T) {
	t.Parallel()
	values := []string{
		string(ContainerStyleDefault), string(ContainerStyleEmphasis),
		string(ContainerStyleGood), string(ContainerStyleAttention),
		string(ContainerStyleWarning), string(ContainerStyleAccent),
	}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "emphasis")
	assert.Contains(t, values, "good")
	assert.Contains(t, values, "attention")
	assert.Contains(t, values, "warning")
	assert.Contains(t, values, "accent")
	assert.Len(t, values, 6)
}

func TestSchemaConformance_Enum_ImageSize(t *testing.T) {
	t.Parallel()
	values := []string{
		string(ImageSizeAuto), string(ImageSizeStretch), string(ImageSizeSmall),
		string(ImageSizeMedium), string(ImageSizeLarge),
	}
	assert.Contains(t, values, "auto")
	assert.Contains(t, values, "stretch")
	assert.Contains(t, values, "small")
	assert.Contains(t, values, "medium")
	assert.Contains(t, values, "large")
	assert.Len(t, values, 5)
}

func TestSchemaConformance_Enum_ImageStyle(t *testing.T) {
	t.Parallel()
	values := []string{string(ImageStyleDefault), string(ImageStylePerson)}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "person")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_ActionStyle(t *testing.T) {
	t.Parallel()
	values := []string{
		string(ActionStyleDefault), string(ActionStylePositive), string(ActionStyleDestructive),
	}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "positive")
	assert.Contains(t, values, "destructive")
	assert.Len(t, values, 3)
}

func TestSchemaConformance_Enum_ActionMode(t *testing.T) {
	t.Parallel()
	values := []string{string(ActionModePrimary), string(ActionModeSecondary)}
	assert.Contains(t, values, "primary")
	assert.Contains(t, values, "secondary")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_TextInputStyle(t *testing.T) {
	t.Parallel()
	values := []string{
		string(TextInputStyleText), string(TextInputStyleTel), string(TextInputStyleUrl),
		string(TextInputStyleEmail), string(TextInputStylePassword),
	}
	assert.Contains(t, values, "text")
	assert.Contains(t, values, "tel")
	assert.Contains(t, values, "url")
	assert.Contains(t, values, "email")
	assert.Contains(t, values, "password")
	assert.Len(t, values, 5)
}

func TestSchemaConformance_Enum_ChoiceInputStyle(t *testing.T) {
	t.Parallel()
	values := []string{
		string(ChoiceInputStyleCompact), string(ChoiceInputStyleExpanded),
		string(ChoiceInputStyleFiltered),
	}
	assert.Contains(t, values, "compact")
	assert.Contains(t, values, "expanded")
	assert.Contains(t, values, "filtered")
	assert.Len(t, values, 3)
}

func TestSchemaConformance_Enum_InputLabelPosition(t *testing.T) {
	t.Parallel()
	values := []string{string(InputLabelPositionInline), string(InputLabelPositionAbove)}
	assert.Contains(t, values, "inline")
	assert.Contains(t, values, "above")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_InputStyle(t *testing.T) {
	t.Parallel()
	values := []string{string(InputStyleDefault), string(InputStyleRevealOnHover)}
	assert.Contains(t, values, "default")
	assert.Contains(t, values, "revealOnHover")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_AssociatedInputs(t *testing.T) {
	t.Parallel()
	values := []string{string(AssociatedInputsAuto), string(AssociatedInputsNone)}
	assert.Contains(t, values, "auto")
	assert.Contains(t, values, "none")
	assert.Len(t, values, 2)
}

func TestSchemaConformance_Enum_BackgroundImageFillMode(t *testing.T) {
	t.Parallel()
	values := []string{
		string(BackgroundImageFillModeCover), string(BackgroundImageFillModeRepeatHorizontally),
		string(BackgroundImageFillModeRepeatVertically), string(BackgroundImageFillModeRepeat),
	}
	assert.Contains(t, values, "cover")
	assert.Contains(t, values, "repeatHorizontally")
	assert.Contains(t, values, "repeatVertically")
	assert.Contains(t, values, "repeat")
	assert.Len(t, values, 4)
}

// ── Image minimal defaults ───────────────────────────────────────────────────

func TestSchemaConformance_Image_MinimalDefaults(t *testing.T) {
	t.Parallel()
	img := newImageBuilder().WithURL("https://example.com/img.png").Build()
	assert.Equal(t, "Image", img["type"])
	assert.Equal(t, "https://example.com/img.png", img["url"])
	assert.Nil(t, img["id"])
	assert.Nil(t, img["altText"])
	assert.Nil(t, img["size"])
	assert.Nil(t, img["style"])
}

// ── ActionSet with ShowCard ──────────────────────────────────────────────────

func TestSchemaConformance_ActionSet_WithShowCard(t *testing.T) {
	t.Parallel()
	innerCard := NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *TextBlockBuilder) { tb.WithText("details") }).
		Build()

	as := newActionSetBuilder().
		AddAction(func(a *ActionBuilder) {
			a.ShowCard("Show").WithCard(innerCard)
		}).
		Build()

	actions := as["actions"].([]any)
	require.Len(t, actions, 1)
	assert.Equal(t, "Action.ShowCard", actions[0].(Card)["type"])
	nested := actions[0].(Card)["card"].(Card)
	assert.Equal(t, "AdaptiveCard", nested["type"])
}

// ── ActionSet with ToggleVisibility ──────────────────────────────────────────

func TestSchemaConformance_ActionSet_WithToggleVisibility(t *testing.T) {
	t.Parallel()
	as := newActionSetBuilder().
		AddAction(func(a *ActionBuilder) {
			a.ToggleVisibility("Toggle").AddTargetElement("panel1", nil)
		}).
		Build()

	actions := as["actions"].([]any)
	require.Len(t, actions, 1)
	assert.Equal(t, "Action.ToggleVisibility", actions[0].(Card)["type"])
	targets := actions[0].(Card)["targetElements"].([]any)
	require.Len(t, targets, 1)
	assert.Equal(t, "panel1", targets[0])
}

// ── Card version/schema auto-mapping ─────────────────────────────────────────

func TestSchemaConformance_CardLevel_VersionSchemaMapping(t *testing.T) {
	t.Parallel()
	versions := map[string]string{
		"1.0": "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json",
		"1.1": "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json",
		"1.2": "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json",
		"1.3": "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json",
		"1.4": "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json",
		"1.5": "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
		"1.6": "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json",
	}
	for ver, expectedURL := range versions {
		card := NewAdaptiveCardBuilder().WithVersion(ver).Build()
		assert.Equal(t, ver, card["version"], "version mismatch for %s", ver)
		assert.Equal(t, expectedURL, card["$schema"], "schema URL mismatch for %s", ver)
	}
}
