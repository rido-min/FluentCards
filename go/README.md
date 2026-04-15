# FluentCards — Go

A Go library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder API with strong typing and built-in validation. Supports the full Adaptive Cards 1.6.0 specification.

## Installation

```bash
go get github.com/rido-min/FluentCards/go
```

Requires Go 1.22+.

## Quick Start

```go
package main

import (
    "fmt"
    "github.com/rido-min/FluentCards/go/fluentcards"
)

func main() {
    card := fluentcards.NewAdaptiveCardBuilder().
        WithVersion("1.5").
        AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
            tb.WithText("Hello, FluentCards!").
                WithSize(fluentcards.TextSizeLarge).
                WithWeight(fluentcards.TextWeightBolder).
                WithWrap(true)
        }).
        AddAction(func(a *fluentcards.ActionBuilder) {
            a.OpenURL("https://adaptivecards.io").
                WithTitle("Learn More")
        }).
        Build()

    json, err := fluentcards.ToJSON(card)
    if err != nil {
        panic(err)
    }
    fmt.Println(json)
}
```

## API overview

### Builders

All builders use method chaining. Pass a `func(*ChildBuilder)` to nested add methods:

```go
card := fluentcards.NewAdaptiveCardBuilder().
    AddContainer(func(c *fluentcards.ContainerBuilder) {
        c.WithStyle(fluentcards.ContainerStyleEmphasis).
            AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
                tb.WithText("Inside a container")
            })
    }).
    Build()
```

Available builders: `TextBlockBuilder`, `ImageBuilder`, `ContainerBuilder`, `ColumnSetBuilder`, `ColumnBuilder`, `FactSetBuilder`, `RichTextBlockBuilder`, `TextRunBuilder`, `ActionSetBuilder`, `MediaBuilder`, `ImageSetBuilder`, `TableBuilder`, `ActionBuilder`, `BackgroundImageBuilder`, `RefreshBuilder`, `AuthenticationBuilder`, `InputTextBuilder`, `InputNumberBuilder`, `InputDateBuilder`, `InputTimeBuilder`, `InputToggleBuilder`, `InputChoiceSetBuilder`.

### Enums

Go has no namespaced enum type. Enums use the `TypeName + ValueName` pattern:

```go
fluentcards.TextSizeLarge       // TextSize enum
fluentcards.TextWeightBolder    // TextWeight enum
fluentcards.TextColorAttention  // TextColor enum
fluentcards.SpacingMedium       // Spacing enum
fluentcards.ActionStylePositive // ActionStyle enum
```

### Serialization

```go
// Serialize to JSON with 2-space indentation
json, err := fluentcards.ToJSON(card)

// Compact (no indentation)
json, err := fluentcards.ToJSONIndent(card, 0)

// Parse JSON back to a Card
card := fluentcards.FromJSON(jsonStr) // returns nil if invalid
```

### Validation

```go
// Returns a slice of ValidationIssue (may be empty)
issues := fluentcards.Validate(card)
for _, issue := range issues {
    fmt.Printf("[%s] %s: %s\n", issue.Severity, issue.Code, issue.Message)
}

// Panics with *AdaptiveCardValidationError if any Error-severity issues exist
fluentcards.ValidateAndPanic(card)
```

### Teams helpers

Pre-built card layouts for Microsoft Teams:

```go
card := fluentcards.TeamsCards.ApprovalCard(fluentcards.ApprovalCardParams{
    RequesterName: "Alice",
    Title:         "Budget Request",
    // ...
})

card := fluentcards.TeamsCards.StatusUpdateCard(fluentcards.StatusUpdateCardParams{...})
card := fluentcards.TeamsCards.TaskUpdateCard(fluentcards.TaskUpdateCardParams{...})
card := fluentcards.TeamsCards.MeetingReminderCard(fluentcards.MeetingReminderCardParams{...})
card := fluentcards.TeamsCards.ExpenseReportCard(fluentcards.ExpenseReportCardParams{...})
```

## Project layout

```
go/
├── go.mod
├── go.sum
├── README.md
└── fluentcards/          # Single importable package
    ├── doc.go
    ├── enums.go          # All typed string enum constants
    ├── models.go         # type Card = map[string]any
    ├── adaptive_card_builder.go
    ├── *_builder.go      # 22 builder files
    ├── serialization.go  # ToJSON, FromJSON
    ├── validation.go     # Validate, ValidateAndPanic
    ├── teams.go          # TeamsCards helpers
    └── *_test.go         # Test files (package fluentcards_test)
```

## Build & test

```bash
cd go

# Build
go build ./...

# Run all tests with race detector
go test ./... -race

# Run with coverage
go test ./... -coverprofile=coverage.out -covermode=atomic
go tool cover -html=coverage.out
```

## Documentation

- [Go-specific patterns](../docs/go-patterns.md) — builder pattern, enum naming, error handling
- [Schema Validation](../docs/schema-validation.md) — validation rules and version-aware checks
- [Teams Adaptive Cards](../docs/teams-cards.md) — pre-built Teams card layouts
- [pkg.go.dev reference](https://pkg.go.dev/github.com/rido-min/FluentCards/go/fluentcards)
