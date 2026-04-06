package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/require"
)

func TestTextBlockBuilder_AllProperties(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Hello").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder).
				WithColor(fluentcards.TextColorAccent).
				WithWrap(true).
				WithMaxLines(3).
				WithIsSubtle(false).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter).
				WithFontType(fluentcards.FontTypeMonospace).
				WithStyle(fluentcards.TextBlockStyleHeading).
				WithSpacing(fluentcards.SpacingMedium).
				WithSeparator(true).
				WithID("tb1")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "TextBlock", el["type"])
	assert.Equal(t, "Hello", el["text"])
	assert.Equal(t, "large", el["size"])
	assert.Equal(t, "bolder", el["weight"])
	assert.Equal(t, "accent", el["color"])
	assert.Equal(t, true, el["wrap"])
	assert.Equal(t, 3, el["maxLines"])
	assert.Equal(t, "center", el["horizontalAlignment"])
	assert.Equal(t, "monospace", el["fontType"])
	assert.Equal(t, "heading", el["style"])
	assert.Equal(t, "medium", el["spacing"])
	assert.Equal(t, true, el["separator"])
	assert.Equal(t, "tb1", el["id"])
}

func TestImageBuilder_Properties(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddImage(func(img *fluentcards.ImageBuilder) {
			img.WithURL("https://example.com/img.png").
				WithAltText("An image").
				WithSize(fluentcards.ImageSizeMedium).
				WithStyle(fluentcards.ImageStylePerson).
				WithWidth("100px").
				WithHeight("100px").
				WithBackgroundColor("#FFFFFF")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Image", el["type"])
	assert.Equal(t, "https://example.com/img.png", el["url"])
	assert.Equal(t, "An image", el["altText"])
	assert.Equal(t, "medium", el["size"])
	assert.Equal(t, "person", el["style"])
	assert.Equal(t, "100px", el["width"])
	assert.Equal(t, "#FFFFFF", el["backgroundColor"])
}

func TestContainerBuilder_WithItems(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddContainer(func(c *fluentcards.ContainerBuilder) {
			c.WithStyle(fluentcards.ContainerStyleEmphasis).
				WithBleed(true).
				AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Inside container") }).
				AddImage(func(img *fluentcards.ImageBuilder) { img.WithURL("https://example.com/x.png") })
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Container", el["type"])
	assert.Equal(t, "emphasis", el["style"])
	assert.Equal(t, true, el["bleed"])
	items := el["items"].([]any)
	assert.Len(t, items, 2)
}

func TestColumnSetBuilder_WithColumns(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumnWithWidth("auto", func(col *fluentcards.ColumnBuilder) {
				col.AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Left") })
			}).AddColumnWithWidth("stretch", func(col *fluentcards.ColumnBuilder) {
				col.WithVerticalContentAlignment(fluentcards.VerticalAlignmentCenter).
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("Right") })
			})
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "ColumnSet", el["type"])
	cols := el["columns"].([]any)
	require.Len(t, cols, 2)
	assert.Equal(t, "auto", cols[0].(map[string]any)["width"])
	assert.Equal(t, "stretch", cols[1].(map[string]any)["width"])
}

func TestFactSetBuilder_AddFact(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddFactSet(func(fs *fluentcards.FactSetBuilder) {
			fs.AddFact("Name", "Alice").
				AddFact("Role", "Engineer")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	facts := el["facts"].([]any)
	require.Len(t, facts, 2)
	assert.Equal(t, "Name", facts[0].(map[string]any)["title"])
	assert.Equal(t, "Alice", facts[0].(map[string]any)["value"])
}

func TestRichTextBlockBuilder_Inlines(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddRichTextBlock(func(rtb *fluentcards.RichTextBlockBuilder) {
			rtb.AddText("plain text").
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText("bold").WithWeight(fluentcards.TextWeightBolder).WithItalic(true)
				})
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "RichTextBlock", el["type"])
	inlines := el["inlines"].([]any)
	require.Len(t, inlines, 2)
	assert.Equal(t, "plain text", inlines[0])
	run := inlines[1].(map[string]any)
	assert.Equal(t, "TextRun", run["type"])
	assert.Equal(t, "bold", run["text"])
	assert.Equal(t, "bolder", run["weight"])
	assert.Equal(t, true, run["italic"])
}

func TestActionSetBuilder_WithActions(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddActionSet(func(as *fluentcards.ActionSetBuilder) {
			as.AddAction(func(a *fluentcards.ActionBuilder) { a.Submit("OK") }).
				AddAction(func(a *fluentcards.ActionBuilder) { a.OpenURL("https://example.com") })
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "ActionSet", el["type"])
	actions := el["actions"].([]any)
	assert.Len(t, actions, 2)
}

func TestMediaBuilder_WithSources(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddMedia(func(m *fluentcards.MediaBuilder) {
			m.WithPoster("https://example.com/poster.png").
				AddSource("https://example.com/video.mp4", "video/mp4")
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Media", el["type"])
	sources := el["sources"].([]any)
	require.Len(t, sources, 1)
	s := sources[0].(map[string]any)
	assert.Equal(t, "https://example.com/video.mp4", s["url"])
	assert.Equal(t, "video/mp4", s["mimeType"])
}

func TestImageSetBuilder_AddImages(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddImageSet(func(is *fluentcards.ImageSetBuilder) {
			is.WithImageSize(fluentcards.ImageSizeMedium).
				AddImage(func(img *fluentcards.ImageBuilder) { img.WithURL("https://example.com/1.png") }).
				AddImage(func(img *fluentcards.ImageBuilder) { img.WithURL("https://example.com/2.png") })
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "ImageSet", el["type"])
	assert.Equal(t, "medium", el["imageSize"])
	images := el["images"].([]any)
	assert.Len(t, images, 2)
}

func TestTableBuilder_AddColumnsAndRows(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddTable(func(tb *fluentcards.TableBuilder) {
			tb.WithFirstRowAsHeader(true).
				WithShowGridLines(true).
				AddColumn(map[string]any{"width": 1}).
				AddColumn(map[string]any{"width": 2}).
				AddRow(map[string]any{
					"cells": []any{
						map[string]any{"items": []any{map[string]any{"type": "TextBlock", "text": "H1"}}},
						map[string]any{"items": []any{map[string]any{"type": "TextBlock", "text": "H2"}}},
					},
				})
		}).Build()
	el := card["body"].([]any)[0].(map[string]any)
	assert.Equal(t, "Table", el["type"])
	assert.Equal(t, true, el["firstRowAsHeader"])
	cols := el["columns"].([]any)
	assert.Len(t, cols, 2)
	rows := el["rows"].([]any)
	assert.Len(t, rows, 1)
}
