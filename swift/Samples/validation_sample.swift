import FluentCards

func runValidationSamples() {
    demonstrateValidCard()
    demonstrateStructuralErrors()
    demonstrateInvalidInputRange()
    demonstrateVersionMismatch()
    demonstrateValidateAndThrow()
}

func demonstrateValidCard() {
    print("\n=== Validation: Valid Card ===")
    let card = AdaptiveCardBuilder()
        .withVersion("1.5")
        .addTextBlock { tb in
            tb.withText("All good!").withSize(.large).withWrap(true)
        }
        .addAction { a in
            a.openURL("https://adaptivecards.io").withTitle("Learn More")
        }
        .build()
    printValidationIssues(validate(card))
}

func demonstrateStructuralErrors() {
    print("\n=== Validation: Structural Errors ===")
    let card: Card = [
        "type": "AdaptiveCard",
        "version": "",
        "body": [
            ["type": "TextBlock", "text": ""],
            ["type": "Image", "url": ""],
        ] as [Any],
    ]
    printValidationIssues(validate(card))
}

func demonstrateInvalidInputRange() {
    print("\n=== Validation: Invalid Input Range ===")
    let card = AdaptiveCardBuilder()
        .withVersion("1.5")
        .addInputNumber { i in
            i.withID("qty").withLabel("Quantity").withMin(100).withMax(10)
        }
        .build()
    printValidationIssues(validate(card))
}

func demonstrateVersionMismatch() {
    print("\n=== Validation: Version Mismatch ===")
    let card = AdaptiveCardBuilder()
        .withVersion("1.0")
        .addTextBlock { tb in
            tb.withText("Sales Report").withWeight(.bolder)
        }
        .addTable { table in
            table
                .addColumn(["width": "1"])
                .addColumn(["width": "1"])
                .addRow([
                    "type": "TableRow",
                    "cells": [
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "Product"]]] as [String: Any],
                        ["type": "TableCell", "items": [["type": "TextBlock", "text": "Sales"]]] as [String: Any],
                    ] as [Any],
                ])
        }
        .build()
    printValidationIssues(validate(card))
}

func demonstrateValidateAndThrow() {
    print("\n=== Validation: validateAndThrow ===")
    let card: Card = ["type": "AdaptiveCard", "version": ""]
    do {
        try validateAndThrow(card)
        print("No errors found.")
    } catch let error as AdaptiveCardValidationError {
        print("Caught AdaptiveCardValidationError:")
        for issue in error.issues {
            if issue.severity == .error {
                print("  [\(issue.code)] \(issue.message)")
            }
        }
    } catch {
        print("Unexpected error: \(error)")
    }
}

func printValidationIssues(_ issues: [ValidationIssue]) {
    if issues.isEmpty {
        print("✓ Card is valid — no issues found.")
        return
    }
    print("Found \(issues.count) issue(s):")
    for issue in issues {
        let icon = issue.severity == .error ? "✗" : "⚠"
        print("  \(icon) [\(issue.severity.rawValue)] \(issue.code) at '\(issue.path)': \(issue.message)")
    }
}
