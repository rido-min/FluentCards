import FluentCards

// Entry point for the Samples executable.
// Demonstrates card creation, serialization, validation, and all samples.

func printSample(_ name: String, _ card: Card) {
    print("\n=== \(name) ===")
    do {
        let json = try toJSON(card)
        print(json)
    } catch {
        print("Error: \(error)")
    }
}

print("=== FluentCards Swift Demo ===\n")

// Build a demo card
let card = AdaptiveCardBuilder()
    .withVersion("1.5")
    .addTextBlock { tb in
        tb.withText("Hello, FluentCards!")
            .withSize(.large)
            .withWeight(.bolder)
            .withWrap(true)
    }
    .addTextBlock { tb in
        tb.withText("This card was built with a fluent interface.")
            .withColor(.accent)
    }
    .addAction { a in
        a.openURL("https://adaptivecards.io").withTitle("Learn More")
    }
    .build()

// Serialize to JSON
do {
    let json = try toJSON(card)
    print(json)
} catch {
    print("Error serializing card: \(error)")
}

// Roundtrip
print("\n=== Roundtrip Test ===")
if let json = try? toJSON(card), let deserialized = fromJSON(json) {
    print("✓ Successfully deserialized card")
    if let version = deserialized["version"] as? String {
        print("  Version: \(version)")
    }
    let body = deserialized["body"] as? [Any] ?? []
    print("  Body elements: \(body.count)")
    let actions = deserialized["actions"] as? [Any] ?? []
    print("  Actions: \(actions.count)")
}

// Validation
print("\n=== Validation Test ===")
let issues = validate(card)
if issues.isEmpty {
    print("✓ Card is valid!")
} else {
    print("⚠ Found \(issues.count) validation issue(s):")
    for issue in issues {
        print("  [\(issue.severity.rawValue)] \(issue.path): \(issue.message)")
    }
}

// Invalid card
print("\n=== Validation with Invalid Card ===")
let invalidCard: Card = ["type": "AdaptiveCard", "version": ""]
let invalidIssues = validate(invalidCard)
print("Found \(invalidIssues.count) validation issue(s):")
for issue in invalidIssues {
    print("  [\(issue.severity.rawValue)] \(issue.code) at '\(issue.path)': \(issue.message)")
}

// All samples
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
