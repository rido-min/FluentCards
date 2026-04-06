package main

import "github.com/rido-min/FluentCards/go/fluentcards"

func createPeoplePickerCard() fluentcards.Card {
	return fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.6").
		AddInputChoiceSet(func(i *fluentcards.InputChoiceSetBuilder) {
			i.WithID("people-picker").
				WithLabel("Select users in the whole organization").
				WithIsMultiSelect(true).
				WithValue("user1,user2").
				WithChoicesData("graph.microsoft.com/users")
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.Submit("Submit")
		}).
		Build()
}
