# FluentCards — Swift

A Swift port of the FluentCards library for building [Adaptive Cards](https://adaptivecards.io/) with a fluent builder API.

## Quick Start

```swift
import FluentCards

let card = AdaptiveCardBuilder()
    .withVersion("1.5")
    .addTextBlock { tb in
        tb.withText("Hello, FluentCards!")
            .withSize(.large)
            .withWeight(.bolder)
            .withWrap(true)
    }
    .addAction { a in
        a.openURL("https://adaptivecards.io").withTitle("Learn More")
    }
    .build()

let json = try toJSON(card)
print(json)
```

## Features

- Full [Adaptive Cards 1.6.0 specification](https://adaptivecards.io/schemas/1.6.0/adaptive-card.json) support
- Fluent builder API with trailing closure syntax
- JSON serialization (`toJSON`) and deserialization (`fromJSON`)
- Comprehensive validation (`validate` / `validateAndThrow`)
- Pure Swift with no third-party runtime dependencies

## Supported Elements

**Body elements**: TextBlock, Image, Container, ColumnSet, Column, FactSet, RichTextBlock, ImageSet, Media, ActionSet, Table

**Actions**: Action.OpenUrl, Action.Submit, Action.ShowCard, Action.ToggleVisibility, Action.Execute

**Inputs**: Input.Text, Input.Number, Input.Date, Input.Time, Input.Toggle, Input.ChoiceSet

## Installation

Add to your `Package.swift`:

```swift
dependencies: [
    .package(url: "https://github.com/rido-min/FluentCards", from: "1.0.0")
],
targets: [
    .target(name: "YourTarget", dependencies: [
        .product(name: "FluentCards", package: "FluentCards")
    ])
]
```

## Validation

```swift
let issues = validate(card)
for issue in issues {
    print("[\(issue.severity.rawValue)] \(issue.code): \(issue.message)")
}

// Throws AdaptiveCardValidationError if any errors found
try validateAndThrow(card)
```

## Building & Testing

```bash
cd swift
swift build
swift test
```

See the [root README](../README.md) for more information on all language ports.
