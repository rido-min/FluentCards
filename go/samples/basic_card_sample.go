package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createWelcomeCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Welcome to FluentCards!").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("This library helps you create Adaptive Cards using a fluent API.").
				WithWrap(true)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://adaptivecards.io").WithTitle("Learn More")
		}).
		Build()
}

func createNotificationCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Notification").
				WithSize(fluentcards.TextSizeMedium).
				WithWeight(fluentcards.TextWeightBolder).
				WithColor(fluentcards.TextColorAttention)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("You have a new message waiting for you.").
				WithWrap(true)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://example.com/messages").WithTitle("View Messages")
		}).
		Build()
}

func createImageCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddImage(func(img *fluentcards.ImageBuilder) {
			img.WithURL("https://adaptivecards.io/content/adaptive-card-50.png").
				WithSize(fluentcards.ImageSizeMedium).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Adaptive Cards").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Platform-agnostic snippets of UI").
				WithWrap(true).
				WithHorizontalAlignment(fluentcards.HorizontalAlignmentCenter)
		}).
		Build()
}
