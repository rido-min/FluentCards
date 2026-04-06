package main

import (
	"fmt"

	"github.com/rido-min/FluentCards/go/fluentcards"
)

func main() {
	fmt.Println("=== FluentCards Demo ===")
	fmt.Println()

	// Create a card using the fluent builder pattern
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Hello, FluentCards!").
				WithSize(fluentcards.TextSizeLarge).
				WithWeight(fluentcards.TextWeightBolder).
				WithWrap(true)
		}).
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("This card was built with a fluent interface.").
				WithColor(fluentcards.TextColorAccent)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://adaptivecards.io").
				WithTitle("Learn More")
		}).
		Build()

	// Serialize to JSON
	json, err := fluentcards.ToJSON(card)
	if err != nil {
		fmt.Printf("Error serializing card: %v\n", err)
		return
	}
	fmt.Println(json)

	// Demonstrate roundtrip serialization
	fmt.Println("\n=== Roundtrip Test ===")
	deserializedCard := fluentcards.FromJSON(json)
	if deserializedCard != nil {
		fmt.Println("✓ Successfully deserialized card")
		if version, ok := deserializedCard["version"].(string); ok {
			fmt.Printf("  Version: %s\n", version)
		}
		body, _ := deserializedCard["body"].([]any)
		fmt.Printf("  Body elements: %d\n", len(body))
		actions, _ := deserializedCard["actions"].([]any)
		fmt.Printf("  Actions: %d\n", len(actions))
	}

	// Demonstrate validation
	fmt.Println("\n=== Validation Test ===")
	issues := fluentcards.Validate(card)
	if len(issues) == 0 {
		fmt.Println("✓ Card is valid!")
	} else {
		fmt.Printf("⚠ Found %d validation issue(s):\n", len(issues))
		for _, issue := range issues {
			fmt.Printf("  [%s] %s: %s\n", issue.Severity, issue.Path, issue.Message)
		}
	}

	// Demonstrate validation with invalid card
	fmt.Println("\n=== Validation with Invalid Card ===")
	invalidCard := fluentcards.Card{"type": "AdaptiveCard", "version": ""}
	invalidIssues := fluentcards.Validate(invalidCard)
	fmt.Printf("Found %d validation issue(s):\n", len(invalidIssues))
	for _, issue := range invalidIssues {
		fmt.Printf("  [%s] %s at '%s': %s\n", issue.Severity, issue.Code, issue.Path, issue.Message)
	}

	// Run all samples and print their JSON
	printSample("Welcome Card", createWelcomeCard())
	printSample("Notification Card", createNotificationCard())
	printSample("Image Card", createImageCard())
	printSample("Contact Form", createContactForm())
	printSample("Survey Form", createSurveyForm())
	printSample("Registration Form", createRegistrationForm())
	printSample("Two Column Card", createTwoColumnCard())
	printSample("Styled Container Card", createStyledContainerCard())
	printSample("Fact Set Card", createFactSetCard())
	printSample("Nested Container Card", createNestedContainerCard())
	printSample("Rich Text Card", createRichTextCard())
	printSample("Image Set Card", createImageSetCard())
	printSample("Table Card", createTableCard())
	printSample("Media Card", createMediaCard())
	printSample("Comprehensive Card", createComprehensiveCard())
	printSample("People Picker Card", createPeoplePickerCard())

	// Validation samples
	runValidationSamples()
}

func printSample(name string, card fluentcards.Card) {
	fmt.Printf("\n=== %s ===\n", name)
	json, err := fluentcards.ToJSON(card)
	if err != nil {
		fmt.Printf("Error: %v\n", err)
		return
	}
	fmt.Println(json)
}
