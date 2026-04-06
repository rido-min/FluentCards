package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createContactForm() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Contact Us").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("name").
				WithLabel("Name").
				WithPlaceholder("Enter your name").
				WithIsRequired(true).
				WithErrorMessage("Name is required")
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("email").
				WithLabel("Email").
				WithPlaceholder("Enter your email").
				WithStyle(fluentcards.TextInputStyleEmail).
				WithIsRequired(true)
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("message").
				WithLabel("Message").
				WithPlaceholder("How can we help?").
				WithIsMultiline(true).
				WithMaxLength(500)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Send Message").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
}

func createSurveyForm() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Customer Satisfaction Survey").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddInputChoiceSet(func(i *fluentcards.InputChoiceSetBuilder) {
			i.WithID("satisfaction").
				WithLabel("How satisfied are you?").
				AddChoice("Very Satisfied", "5").
				AddChoice("Satisfied", "4").
				AddChoice("Neutral", "3").
				AddChoice("Dissatisfied", "2").
				AddChoice("Very Dissatisfied", "1").
				WithIsRequired(true)
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("feedback").
				WithLabel("Additional Feedback").
				WithPlaceholder("Tell us more...").
				WithIsMultiline(true)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Submit Survey").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
}

func createRegistrationForm() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Event Registration").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder)
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("fullName").
				WithLabel("Full Name").
				WithIsRequired(true)
		}).
		AddInputText(func(i *fluentcards.InputTextBuilder) {
			i.WithID("email").
				WithLabel("Email Address").
				WithStyle(fluentcards.TextInputStyleEmail).
				WithIsRequired(true)
		}).
		AddInputDate(func(i *fluentcards.InputDateBuilder) {
			i.WithID("eventDate").
				WithLabel("Event Date")
		}).
		AddInputToggle(func(i *fluentcards.InputToggleBuilder) {
			i.WithID("newsletter").
				WithTitle("Subscribe to newsletter").
				WithValue("true")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Register").WithStyle(fluentcards.ActionStylePositive)
		}).
		Build()
}
