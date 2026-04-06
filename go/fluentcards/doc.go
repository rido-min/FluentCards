// Package fluentcards provides a fluent builder API for creating Microsoft Adaptive Cards.
//
// Adaptive Cards are a platform-agnostic card format used in Microsoft Teams, Outlook,
// and other host applications. This package supports the full Adaptive Cards 1.6.0
// specification with strong typing, schema validation, and JSON serialization.
//
// # Basic usage
//
//	card := fluentcards.NewAdaptiveCardBuilder().
//	    WithVersion("1.5").
//	    AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
//	        tb.WithText("Hello, FluentCards!").
//	            WithSize(fluentcards.TextSizeLarge).
//	            WithWeight(fluentcards.TextWeightBolder).
//	            WithWrap(true)
//	    }).
//	    Build()
//
//	json, err := fluentcards.ToJSON(card)
//
// # Validation
//
//	issues := fluentcards.Validate(card)
//	for _, issue := range issues {
//	    fmt.Printf("[%s] %s: %s\n", issue.Severity, issue.Code, issue.Message)
//	}
//
// # Teams helpers
//
//	card := fluentcards.TeamsCards.ApprovalCard(fluentcards.ApprovalCardParams{...})
package fluentcards
