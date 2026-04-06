# FluentCards — Java

A Java library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder API with strong typing and built-in validation. Supports the full Adaptive Cards 1.6.0 specification.

## Requirements

- Java 17+
- Maven

## Installation

Add the following dependency to your `pom.xml`:

```xml
<dependency>
    <groupId>io.fluentcards</groupId>
    <artifactId>fluent-cards</artifactId>
    <version>0.1.0</version>
</dependency>
```

## Quick Start

```java
import io.fluentcards.*;
import java.util.Map;

public class Example {
    public static void main(String[] args) {
        Map<String, Object> card = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb -> tb
                .withText("Hello, FluentCards!")
                .withSize(TextSize.LARGE)
                .withWeight(TextWeight.BOLDER)
                .withWrap(true))
            .addAction(a -> a
                .submit("Click me")
                .withStyle(ActionStyle.POSITIVE))
            .build();

        System.out.println(CardSerializer.toJson(card));
    }
}
```

## API Overview

### Builders

All builders use method chaining. Pass a `Consumer<ChildBuilder>` to nested add methods:

```java
Map<String, Object> card = AdaptiveCardBuilder.create()
    .addContainer(c -> c
        .withStyle(ContainerStyle.EMPHASIS)
        .addTextBlock(tb -> tb
            .withText("Inside a container")))
    .build();
```

Available builders: `TextBlockBuilder`, `ImageBuilder`, `ContainerBuilder`, `ColumnSetBuilder`, `ColumnBuilder`, `FactSetBuilder`, `RichTextBlockBuilder`, `TextRunBuilder`, `ActionSetBuilder`, `MediaBuilder`, `ImageSetBuilder`, `TableBuilder`, `ActionBuilder`, `BackgroundImageBuilder`, `RefreshBuilder`, `AuthenticationBuilder`, `InputTextBuilder`, `InputNumberBuilder`, `InputDateBuilder`, `InputTimeBuilder`, `InputToggleBuilder`, `InputChoiceSetBuilder`.

### Enums

Java enums use `UPPER_CASE` members with `getValue()` returning the Adaptive Cards string value:

```java
TextSize.LARGE           // "large"
TextWeight.BOLDER        // "bolder"
TextColor.ATTENTION      // "attention"
Spacing.MEDIUM           // "medium"
ActionStyle.POSITIVE     // "positive"
ContainerStyle.EMPHASIS  // "emphasis"
```

### Serialization

```java
// Serialize to JSON with 2-space indentation
String json = CardSerializer.toJson(card);

// Serialize with custom indentation
String json = CardSerializer.toJson(card, 4);

// Parse JSON back to a Card
Map<String, Object> card = CardSerializer.fromJson(jsonStr);
```

### Validation

```java
// Returns a list of ValidationIssue (may be empty)
List<ValidationIssue> issues = CardValidator.validate(card);
for (ValidationIssue issue : issues) {
    System.out.printf("[%s] %s: %s%n", issue.getSeverity(), issue.getCode(), issue.getMessage());
}

// Throws AdaptiveCardValidationException if any Error-severity issues exist
CardValidator.validateAndThrow(card);
```

### Teams Helpers

Pre-built card layouts for Microsoft Teams:

```java
Map<String, Object> card = TeamsCards.approvalCard(ApprovalCardParams.builder()
    .requesterName("Alice")
    .title("Budget Request")
    .build());

Map<String, Object> card = TeamsCards.statusUpdateCard(StatusUpdateCardParams.builder().build());
Map<String, Object> card = TeamsCards.taskUpdateCard(TaskUpdateCardParams.builder().build());
Map<String, Object> card = TeamsCards.meetingReminderCard(MeetingReminderCardParams.builder().build());
Map<String, Object> card = TeamsCards.expenseReportCard(ExpenseReportCardParams.builder().build());
```

## Project Layout

```
java/
├── pom.xml
├── README.md
├── samples/                          # Standalone sample programs
│   ├── Program.java
│   ├── BasicCardSample.java
│   ├── FormCardSample.java
│   ├── LayoutCardSample.java
│   ├── PeoplePickerSample.java
│   ├── RichContentSample.java
│   └── ValidationSample.java
└── src/
    ├── main/java/io/fluentcards/    # Library source
    └── test/java/io/fluentcards/    # Tests
```

## Build & Test

```bash
cd java

# Build
mvn compile

# Run all tests
mvn test

# Package
mvn package
```

## Documentation

- [Schema Validation](../docs/schema-validation.md) — validation rules and version-aware checks
- [Teams Adaptive Cards](../docs/teams-cards.md) — pre-built Teams card layouts
- [Root README](../README.md) — overview of all language ports
