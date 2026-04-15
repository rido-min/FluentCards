package fluentcards_test

import (
	"testing"

	"github.com/rido-min/FluentCards/go/fluentcards"
	"github.com/stretchr/testify/assert"
)

func TestColumnBuilder_WithIsVisible(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithIsVisible(false).
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("hidden") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, false, col["isVisible"])
}

func TestColumnBuilder_WithSpacing(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithSpacing(fluentcards.SpacingLarge).
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("spaced") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, "large", col["spacing"])
}

func TestColumnBuilder_WithSeparator(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithSeparator(true).
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("sep") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, true, col["separator"])
}

func TestColumnBuilder_WithHeight(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithHeight("stretch").
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("tall") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, "stretch", col["height"])
}

func TestColumnBuilder_WithFallback(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithFallback("drop").
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("fb") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, "drop", col["fallback"])
}

func TestColumnBuilder_WithRequires(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithRequires("adaptiveCards", "1.2").
					WithRequires("myFeature", "1.0").
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("req") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	reqs := col["requires"].(map[string]any)
	assert.Equal(t, "1.2", reqs["adaptiveCards"])
	assert.Equal(t, "1.0", reqs["myFeature"])
}

func TestColumnBuilder_WithRtl(t *testing.T) {
	t.Parallel()
	card := fluentcards.NewAdaptiveCardBuilder().
		AddColumnSet(func(cs *fluentcards.ColumnSetBuilder) {
			cs.AddColumn(func(c *fluentcards.ColumnBuilder) {
				c.WithRtl(true).
					AddTextBlock(func(tb *fluentcards.TextBlockBuilder) { tb.WithText("rtl") })
			})
		}).Build()
	col := card["body"].([]any)[0].(map[string]any)["columns"].([]any)[0].(map[string]any)
	assert.Equal(t, true, col["rtl"])
}
