package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createRichTextCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddRichTextBlock(func(rtb *fluentcards.RichTextBlockBuilder) {
			rtb.AddTextRun(func(tr *fluentcards.TextRunBuilder) {
				tr.WithText("Welcome ").WithSize(fluentcards.TextSizeLarge)
			}).
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText("to FluentCards!").
						WithSize(fluentcards.TextSizeLarge).
						WithWeight(fluentcards.TextWeightBolder).
						WithColor(fluentcards.TextColorAccent)
				}).
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText("\n\nThis demonstrates ").WithSize(fluentcards.TextSizeDefault)
				}).
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText("rich text formatting").
						WithWeight(fluentcards.TextWeightBolder).
						WithColor(fluentcards.TextColorGood)
				}).
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText(" with multiple text runs.").WithSize(fluentcards.TextSizeDefault)
				})
		}).
		Build()
}

func createImageSetCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Photo Gallery").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddImageSet(func(imgSet *fluentcards.ImageSetBuilder) {
			imgSet.WithImageSize(fluentcards.ImageSizeMedium).
				AddImage(func(img *fluentcards.ImageBuilder) {
					img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png")
				}).
				AddImage(func(img *fluentcards.ImageBuilder) {
					img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png")
				}).
				AddImage(func(img *fluentcards.ImageBuilder) {
					img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png")
				})
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("View more photos in the gallery").WithWrap(true)
		}).
		Build()
}

func createTableCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Sales Report").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddTable(func(table *fluentcards.TableBuilder) {
			table.
				AddColumn(map[string]any{"width": "2"}).
				AddColumn(map[string]any{"width": "1"}).
				AddColumn(map[string]any{"width": "1"}).
				AddRow(map[string]any{"type": "TableRow", "cells": []any{
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "Product A"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "150"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "$15,000"}}},
				}}).
				AddRow(map[string]any{"type": "TableRow", "cells": []any{
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "Product B"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "200"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "$20,000"}}},
				}}).
				AddRow(map[string]any{"type": "TableRow", "cells": []any{
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "Product C"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "100"}}},
					map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "$10,000"}}},
				}})
		}).
		Build()
}

func createMediaCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Video Tutorial").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddMedia(func(media *fluentcards.MediaBuilder) {
			media.AddSource("https://example.com/video.mp4", "video/mp4").
				WithPoster("https://example.com/poster.jpg").
				WithAltText("Getting started with FluentCards")
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Watch this tutorial to learn the basics of FluentCards.").
				WithWrap(true)
		}).
		Build()
}

func createComprehensiveCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Product Launch Announcement").
				WithSize(fluentcards.TextSizeExtraLarge).
				WithWeight(fluentcards.TextWeightBolder).
				WithColor(fluentcards.TextColorAccent)
		}).
		AddImage(func(img *fluentcards.ImageBuilder) {
			img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png").
				WithSize(fluentcards.ImageSizeLarge).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter)
		}).
		AddRichTextBlock(func(rtb *fluentcards.RichTextBlockBuilder) {
			rtb.AddTextRun(func(tr *fluentcards.TextRunBuilder) {
				tr.WithText("Introducing ").WithSize(fluentcards.TextSizeMedium)
			}).
				AddTextRun(func(tr *fluentcards.TextRunBuilder) {
					tr.WithText("FluentCards 2.0").
						WithSize(fluentcards.TextSizeMedium).
						WithWeight(fluentcards.TextWeightBolder).
						WithColor(fluentcards.TextColorGood)
				})
		}).
		AddFactSet(func(fs *fluentcards.FactSetBuilder) {
			fs.AddFact("Release Date", "January 1, 2025").
				AddFact("Version", "2.0.0").
				AddFact("License", "MIT")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://github.com/rido-min/FluentCards").WithTitle("View on GitHub")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Get Notified").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
}
