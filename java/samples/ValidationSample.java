package io.fluentcards.samples;

import io.fluentcards.*;
import java.util.List;
import java.util.Map;

public class ValidationSample {
    public static void run() {
        System.out.println("=== Validation Sample ===");

        // 1. Validate a well-formed card
        System.out.println("\n--- Valid Card ---");
        Map<String, Object> validCard = AdaptiveCardBuilder.create()
            .withVersion("1.5")
            .addTextBlock(tb -> tb
                .withText("All good!")
                .withSize(TextSize.LARGE)
                .withWrap(true))
            .addAction(a -> a
                .openUrl("https://adaptivecards.io")
                .withTitle("Learn More"))
            .build();

        List<ValidationIssue> issues = CardValidator.validate(validCard);
        if (issues.isEmpty()) {
            System.out.println("Valid card: no issues");
        }

        // 2. Validate a card with problems (missing version, empty body)
        System.out.println("\n--- Invalid Card ---");
        Map<String, Object> invalidCard = Map.of(
            "type", "AdaptiveCard",
            "version", "",
            "body", List.of()
        );

        List<ValidationIssue> invalidIssues = CardValidator.validate(invalidCard);
        System.out.println("Found " + invalidIssues.size() + " issue(s):");
        for (ValidationIssue issue : invalidIssues) {
            System.out.println("  [" + issue.getSeverity() + "] " + issue.getCode()
                + ": " + issue.getMessage());
        }

        // 3. Demonstrate validateAndThrow
        System.out.println("\n--- ValidateAndThrow ---");
        try {
            CardValidator.validateAndThrow(invalidCard);
        } catch (AdaptiveCardValidationException e) {
            System.out.println("Caught exception: " + e.getMessage());
        }
    }
}
