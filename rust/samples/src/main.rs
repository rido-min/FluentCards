mod basic_card_sample;
mod form_card_sample;
mod layout_card_sample;
mod rich_content_sample;
mod people_picker_sample;
mod validation_sample;

use fluent_cards::*;

fn main() {
    println!("=== FluentCards Demo ===");
    println!();

    // Create a card using the fluent builder pattern
    let card = AdaptiveCardBuilder::new()
        .with_version("1.5")
        .add_text_block(|tb| {
            tb.with_text("Hello, FluentCards!")
                .with_size(TextSize::Large)
                .with_weight(TextWeight::Bolder)
                .with_wrap(true);
        })
        .add_text_block(|tb| {
            tb.with_text("This card was built with a fluent interface.")
                .with_color(TextColor::Accent);
        })
        .add_action(|a| {
            a.open_url("https://adaptivecards.io")
                .with_title("Learn More");
        })
        .build();

    // Serialize to JSON
    let json = to_json(&card).unwrap();
    println!("{json}");

    // Demonstrate roundtrip serialization
    println!("\n=== Roundtrip Test ===");
    if let Some(deserialized_card) = from_json(&json) {
        println!("✓ Successfully deserialized card");
        if let Some(version) = deserialized_card.get("version").and_then(|v| v.as_str()) {
            println!("  Version: {version}");
        }
        let body_len = deserialized_card
            .get("body")
            .and_then(|b| b.as_array())
            .map_or(0, |b| b.len());
        println!("  Body elements: {body_len}");
        let actions_len = deserialized_card
            .get("actions")
            .and_then(|a| a.as_array())
            .map_or(0, |a| a.len());
        println!("  Actions: {actions_len}");
    }

    // Demonstrate validation
    println!("\n=== Validation Test ===");
    let issues = validate(&card);
    if issues.is_empty() {
        println!("✓ Card is valid!");
    } else {
        println!("⚠ Found {} validation issue(s):", issues.len());
        for issue in &issues {
            println!(
                "  [{severity}] {path}: {message}",
                severity = issue.severity,
                path = issue.path,
                message = issue.message
            );
        }
    }

    // Demonstrate validation with invalid card
    println!("\n=== Validation with Invalid Card ===");
    let mut invalid_card = Card::new();
    invalid_card.insert(
        "type".into(),
        serde_json::Value::String("AdaptiveCard".into()),
    );
    invalid_card.insert("version".into(), serde_json::Value::String(String::new()));
    let invalid_issues = validate(&invalid_card);
    println!("Found {} validation issue(s):", invalid_issues.len());
    for issue in &invalid_issues {
        println!(
            "  [{severity}] {code} at '{path}': {message}",
            severity = issue.severity,
            code = issue.code,
            path = issue.path,
            message = issue.message
        );
    }

    // Run all samples and print their JSON
    print_sample("Welcome Card", basic_card_sample::create_welcome_card());
    print_sample(
        "Notification Card",
        basic_card_sample::create_notification_card(),
    );
    print_sample("Image Card", basic_card_sample::create_image_card());
    print_sample("Contact Form", form_card_sample::create_contact_form());
    print_sample("Survey Form", form_card_sample::create_survey_form());
    print_sample(
        "Registration Form",
        form_card_sample::create_registration_form(),
    );
    print_sample(
        "Two Column Card",
        layout_card_sample::create_two_column_card(),
    );
    print_sample(
        "Styled Container Card",
        layout_card_sample::create_styled_container_card(),
    );
    print_sample("Fact Set Card", layout_card_sample::create_fact_set_card());
    print_sample(
        "Nested Container Card",
        layout_card_sample::create_nested_container_card(),
    );
    print_sample("Rich Text Card", rich_content_sample::create_rich_text_card());
    print_sample(
        "Image Set Card",
        rich_content_sample::create_image_set_card(),
    );
    print_sample("Table Card", rich_content_sample::create_table_card());
    print_sample("Media Card", rich_content_sample::create_media_card());
    print_sample(
        "Comprehensive Card",
        rich_content_sample::create_comprehensive_card(),
    );
    print_sample(
        "People Picker Card",
        people_picker_sample::create_people_picker_card(),
    );

    // Validation samples
    validation_sample::run_validation_samples();
}

fn print_sample(name: &str, card: Card) {
    println!("\n=== {name} ===");
    match to_json(&card) {
        Ok(json) => println!("{json}"),
        Err(err) => println!("Error: {err}"),
    }
}
