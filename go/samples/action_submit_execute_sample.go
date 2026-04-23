package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createActionSubmitExecuteCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.4").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("welcome to ac 11").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("click the buttons below")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Execute("Test AC Action").
				WithData(map[string]any{"message": "button clicked !!"}).
				WithVerb("testAction")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Open Task Module").
				WithData(map[string]any{"msteams": map[string]any{"type": "task/fetch"}})
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Execute("Request File Upload").
				WithVerb("requestFileUpload")
		}).
		Build()
}
