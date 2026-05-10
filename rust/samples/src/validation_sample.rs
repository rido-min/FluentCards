use fluent_cards::*;

/// Runs all validation demonstrations.
pub fn run_validation_samples() {
    demonstrate_valid_card();
    demonstrate_structural_errors();
    demonstrate_invalid_input_range();
    demonstrate_version_mismatch();
    demonstrate_validate_and_panic();
}

fn demonstrate_valid_card() {
    println!("\n=== Validation: Valid Card ===");

    let card = AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("All good!")
                .with_size(TextSize::Large)
                .with_wrap(true);
        })
        .add_action(|a| {
            a.open_url("https://adaptivecards.io")
                .with_title("Learn More");
        })
        .build();

    print_validation_issues(&validate(&card));
}

fn demonstrate_structural_errors() {
    println!("\n=== Validation: Structural Errors ===");

    let mut card = Card::new();
    card.insert(
        "type".into(),
        serde_json::Value::String("AdaptiveCard".into()),
    );
    card.insert("version".into(), serde_json::Value::String(String::new()));
    let body = serde_json::json!([
        {"type": "TextBlock", "text": ""},
        {"type": "Image", "url": ""}
    ]);
    card.insert("body".into(), body);

    print_validation_issues(&validate(&card));
}

fn demonstrate_invalid_input_range() {
    println!("\n=== Validation: Invalid Input Range ===");

    let card = AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_input_number(|i| {
            i.with_id("qty")
                .with_label("Quantity")
                .with_min(100.0)
                .with_max(10.0);
        })
        .build();

    print_validation_issues(&validate(&card));
}

fn demonstrate_version_mismatch() {
    println!("\n=== Validation: Version Mismatch ===");

    let card = AdaptiveCardBuilder::new()
        .with_version("1.0")
        .add_text_block(|tb| {
            tb.with_text("Sales Report")
                .with_weight(TextWeight::Bolder);
        })
        .add_table(|table| {
            table
                .add_column(
                    serde_json::json!({"width": "1"})
                        .as_object()
                        .unwrap()
                        .clone(),
                )
                .add_column(
                    serde_json::json!({"width": "1"})
                        .as_object()
                        .unwrap()
                        .clone(),
                )
                .add_row(
                    serde_json::json!({
                        "type": "TableRow",
                        "cells": [
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Product"}]},
                            {"type": "TableCell", "items": [{"type": "TextBlock", "text": "Sales"}]}
                        ]
                    })
                    .as_object()
                    .unwrap()
                    .clone(),
                );
        })
        .build();

    print_validation_issues(&validate(&card));
}

fn demonstrate_validate_and_panic() {
    println!("\n=== Validation: ValidateAndPanic ===");

    let mut card = Card::new();
    card.insert(
        "type".into(),
        serde_json::Value::String("AdaptiveCard".into()),
    );
    card.insert("version".into(), serde_json::Value::String(String::new()));

    let result = std::panic::catch_unwind(std::panic::AssertUnwindSafe(|| {
        validate_and_panic(&card);
    }));

    match result {
        Ok(()) => println!("No errors found."),
        Err(_) => {
            // Re-validate to get the issues for display
            let issues = validate(&card);
            println!("Caught AdaptiveCardValidationError:");
            for e in &issues {
                if e.severity == ValidationSeverity::Error {
                    println!("  [{}] {}", e.code, e.message);
                }
            }
        }
    }
}

fn print_validation_issues(issues: &[ValidationIssue]) {
    if issues.is_empty() {
        println!("✓ Card is valid — no issues found.");
        return;
    }
    println!("Found {} issue(s):", issues.len());
    for issue in issues {
        let icon = if issue.severity == ValidationSeverity::Error {
            "✗"
        } else {
            "⚠"
        };
        println!(
            "  {icon} [{severity}] {code} at '{path}': {message}",
            severity = issue.severity,
            code = issue.code,
            path = issue.path,
            message = issue.message
        );
    }
}
