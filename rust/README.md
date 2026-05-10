# FluentCards — Rust

A Rust library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder API with strong typing and built-in validation. Supports the full Adaptive Cards 1.6.0 specification.

## Installation

Add to your `Cargo.toml`:

```toml
[dependencies]
fluent-cards = "0.1"
```

Requires Rust 1.70+ (edition 2021).

## Quick Start

```rust
use fluent_cards::*;

let card = AdaptiveCardBuilder::new()
    .with_version("1.5")
    .add_text_block(|tb| {
        tb.with_text("Hello, FluentCards!")
          .with_size(TextSize::Large)
          .with_weight(TextWeight::Bolder)
          .with_wrap(true);
    })
    .add_action(|a| {
        a.submit("Click me").with_style(ActionStyle::Positive);
    })
    .build();

let json = to_json(&card).unwrap();
println!("{json}");
```

## API overview

### Builders

All builders use method chaining. Pass a closure to nested add methods:

```rust
use fluent_cards::*;

let card = AdaptiveCardBuilder::new()
    .add_container(|c| {
        c.with_style(ContainerStyle::Emphasis)
         .add_text_block(|tb| {
             tb.with_text("Inside a container");
         });
    })
    .build();
```

Available builders: `AdaptiveCardBuilder`, `TextBlockBuilder`, `ImageBuilder`, `ContainerBuilder`, `ColumnSetBuilder`, `ColumnBuilder`, `FactSetBuilder`, `RichTextBlockBuilder`, `TextRunBuilder`, `ActionSetBuilder`, `MediaBuilder`, `ImageSetBuilder`, `TableBuilder`, `ActionBuilder`, `BackgroundImageBuilder`, `RefreshBuilder`, `AuthenticationBuilder`, `InputTextBuilder`, `InputNumberBuilder`, `InputDateBuilder`, `InputTimeBuilder`, `InputToggleBuilder`, `InputChoiceSetBuilder`.

### Enums

Rust enums with PascalCase variants that serialize to camelCase strings:

```rust
use fluent_cards::*;

TextSize::Large          // → "large"
TextWeight::Bolder       // → "bolder"
TextColor::Attention     // → "attention"
Spacing::Medium          // → "medium"
ActionStyle::Positive    // → "positive"
```

### Serialization

```rust
use fluent_cards::*;

// Serialize to JSON with 2-space indentation
let json = to_json(&card).unwrap();

// Compact (no indentation)
let json = to_json_indent(&card, 0).unwrap();

// Parse JSON back to a Card
let card = from_json(&json_str); // returns None if invalid
```

### Validation

```rust
use fluent_cards::*;

// Returns a Vec<ValidationIssue> (may be empty)
let issues = validate(&card);
for issue in &issues {
    println!("[{}] {}: {}", issue.severity, issue.code, issue.message);
}

// Panics with descriptive message if any Error-severity issues exist
validate_and_panic(&card);
```

### Teams helpers

Pre-built card layouts for Microsoft Teams:

```rust
use fluent_cards::*;

let card = TeamsCards::approval_card(&ApprovalCardParams {
    requester_name: "Alice".into(),
    title: "Budget Request".into(),
    // populate the remaining required fields here
    submitted_date: "2025-01-15".into(),
    category: "Travel".into(),
    amount: "$1,200.00".into(),
    business_unit: "Engineering".into(),
    due_date: "2025-01-30".into(),
    description: "Conference travel expenses".into(),
    requester_image_url: None,
});

// Also available:
// TeamsCards::status_update_card(&StatusUpdateCardParams { ... })
// TeamsCards::task_update_card(&TaskUpdateCardParams { ... })
// TeamsCards::meeting_reminder_card(&MeetingReminderCardParams { ... })
// TeamsCards::expense_report_card(&ExpenseReportCardParams { ... })
```

## Project layout

```
rust/
├── Cargo.toml              # Workspace root
├── fluentcards/
│   ├── Cargo.toml          # Library crate (fluent-cards)
│   └── src/
│       ├── lib.rs
│       ├── models.rs       # type Card = serde_json::Map<String, Value>
│       ├── enums.rs        # All typed string enum types
│       ├── builders/       # 22 builder modules
│       │   ├── mod.rs
│       │   ├── adaptive_card_builder.rs
│       │   ├── *_builder.rs
│       │   └── inputs/     # 6 input builder modules
│       ├── serialization.rs
│       ├── validation.rs
│       └── teams.rs
└── samples/
    ├── Cargo.toml          # Binary crate (samples)
    └── src/
        ├── main.rs
        └── *_sample.rs     # 6 sample modules
```

## Build & test

```bash
cd rust

# Build
cargo build

# Run all tests
cargo test

# Run with verbose output
cargo test -- --nocapture

# Run samples
cd samples && cargo run
```

## Versioning

The library version is derived from the root `version.json` using [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning). During CI, the computed version is stamped into `Cargo.toml` via `nbgv-python` before packaging, then reset afterward — the same strategy used by the Python port. The placeholder version in `Cargo.toml` (`0.0.0-placeholder`) should never be published.

## Documentation

- [Schema Validation](../docs/schema-validation.md) — validation rules and version-aware checks
- [Teams Adaptive Cards](../docs/teams-cards.md) — pre-built Teams card layouts
