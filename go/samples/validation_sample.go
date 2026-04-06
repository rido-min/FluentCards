package main

import (
	"fmt"

	"github.com/rido-min/FluentCards/go/fluentcards"
)

// RunValidationSamples runs all validation demonstrations.
func runValidationSamples() {
	demonstrateValidCard()
	demonstrateStructuralErrors()
	demonstrateInvalidInputRange()
	demonstrateVersionMismatch()
	demonstrateValidateAndPanic()
}

// demonstrateValidCard validates a well-formed card — expects zero issues.
func demonstrateValidCard() {
	fmt.Println("\n=== Validation: Valid Card ===")

	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("All good!").WithSize(fluentcards.TextSizeLarge).WithWrap(true)
		}).
		AddAction(func(a *fluentcards.ActionBuilder) {
			a.OpenURL("https://adaptivecards.io").WithTitle("Learn More")
		}).
		Build()

	printValidationIssues(fluentcards.Validate(card))
}

// demonstrateStructuralErrors validates a card with missing required fields — expects multiple errors.
func demonstrateStructuralErrors() {
	fmt.Println("\n=== Validation: Structural Errors ===")

	// Intentional problems: no version, TextBlock with no text, Image with no URL
	card := fluentcards.Card{
		"type":    "AdaptiveCard",
		"version": "",
		"body": []any{
			map[string]any{"type": "TextBlock", "text": ""},
			map[string]any{"type": "Image", "url": ""},
		},
	}

	printValidationIssues(fluentcards.Validate(card))
}

// demonstrateInvalidInputRange validates an Input.Number with min > max — expects a range error.
func demonstrateInvalidInputRange() {
	fmt.Println("\n=== Validation: Invalid Input Range ===")

	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.5").
		AddInputNumber(func(i *fluentcards.InputNumberBuilder) {
			i.WithID("qty").WithLabel("Quantity").WithMin(100).WithMax(10)
		}).
		Build()

	printValidationIssues(fluentcards.Validate(card))
}

// demonstrateVersionMismatch validates a card that uses elements requiring a newer schema version.
func demonstrateVersionMismatch() {
	fmt.Println("\n=== Validation: Version Mismatch ===")

	// Table requires v1.5; declaring v1.0 should trigger a VERSION_MISMATCH warning
	card := fluentcards.NewAdaptiveCardBuilder().
		WithVersion("1.0").
		AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
			tb.WithText("Sales Report").WithWeight(fluentcards.TextWeightBolder)
		}).
		AddTable(func(table *fluentcards.TableBuilder) {
			table.
				AddColumn(map[string]any{"width": "1"}).
				AddColumn(map[string]any{"width": "1"}).
				AddRow(map[string]any{
					"type": "TableRow",
					"cells": []any{
						map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "Product"}}},
						map[string]any{"type": "TableCell", "items": []any{map[string]any{"type": "TextBlock", "text": "Sales"}}},
					},
				})
		}).
		Build()

	printValidationIssues(fluentcards.Validate(card))
}

// demonstrateValidateAndPanic demonstrates ValidateAndPanic — recovers from the panic on validation errors.
func demonstrateValidateAndPanic() {
	fmt.Println("\n=== Validation: ValidateAndPanic ===")

	card := fluentcards.Card{"type": "AdaptiveCard", "version": ""}

	func() {
		defer func() {
			if r := recover(); r != nil {
				if err, ok := r.(*fluentcards.AdaptiveCardValidationError); ok {
					fmt.Println("Caught AdaptiveCardValidationError:")
					for _, e := range err.Issues {
						if e.Severity == fluentcards.ValidationSeverityError {
							fmt.Printf("  [%s] %s\n", e.Code, e.Message)
						}
					}
				} else {
					panic(r)
				}
			}
		}()
		fluentcards.ValidateAndPanic(card)
		fmt.Println("No errors found.")
	}()
}

func printValidationIssues(issues []fluentcards.ValidationIssue) {
	if len(issues) == 0 {
		fmt.Println("✓ Card is valid — no issues found.")
		return
	}
	fmt.Printf("Found %d issue(s):\n", len(issues))
	for _, issue := range issues {
		icon := "⚠"
		if issue.Severity == fluentcards.ValidationSeverityError {
			icon = "✗"
		}
		fmt.Printf("  %s [%s] %s at '%s': %s\n", icon, issue.Severity, issue.Code, issue.Path, issue.Message)
	}
}
