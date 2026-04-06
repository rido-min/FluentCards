package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class ValidationTest {

    @Test
    void testValidateValidCard() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withVersion("1.5")
                .addTextBlock(tb -> tb.withText("Hello"))
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        assertTrue(issues.isEmpty());
    }

    @Test
    void testValidateMissingVersion() {
        Map<String, Object> card = new LinkedHashMap<>();
        card.put("type", "AdaptiveCard");
        card.put("$schema", "https://x.com");

        List<ValidationIssue> issues = CardValidator.validate(card);
        assertFalse(issues.isEmpty());
        boolean found = issues.stream()
                .anyMatch(i -> "MISSING_VERSION".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.ERROR);
        assertTrue(found, "expected MISSING_VERSION issue");
    }

    @Test
    void testValidateEmptyCard() {
        Map<String, Object> card = AdaptiveCardBuilder.create().build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "EMPTY_CARD".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.WARNING);
        assertTrue(found, "expected EMPTY_CARD warning");
    }

    @Test
    void testValidateMissingTextBlockText() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> {
                    // no text set
                })
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "MISSING_TEXT".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.ERROR);
        assertTrue(found, "expected MISSING_TEXT issue");
    }

    @Test
    void testValidateMissingImageUrl() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addImage(img -> {
                    // no URL set
                })
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "MISSING_IMAGE_URL".equals(i.getCode()));
        assertTrue(found);
    }

    @Test
    void testValidateMissingInputId() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputText(it -> {
                    // no ID set
                })
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "MISSING_INPUT_ID".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.ERROR);
        assertTrue(found);
    }

    @Test
    void testValidateInputNumberMinGreaterThanMax() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputNumber(in -> in.withId("qty").withMin(100).withMax(10))
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "MIN_GREATER_THAN_MAX".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.ERROR);
        assertTrue(found);
    }

    @Test
    void testValidateDuplicateId() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("First").withId("dup"))
                .addTextBlock(tb -> tb.withText("Second").withId("dup"))
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "DUPLICATE_ID".equals(i.getCode()));
        assertTrue(found);
    }

    @SuppressWarnings("unchecked")
    @Test
    void testValidateInvalidSelectActionShowCard() {
        Map<String, Object> showCard = new LinkedHashMap<>();
        showCard.put("type", "Action.ShowCard");

        Map<String, Object> card = new LinkedHashMap<>();
        card.put("type", "AdaptiveCard");
        card.put("version", "1.5");
        card.put("$schema", "https://x.com");
        card.put("selectAction", showCard);
        List<Map<String, Object>> body = List.of(Map.of("type", "TextBlock", "text", "x"));
        card.put("body", body);

        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "INVALID_SELECT_ACTION".equals(i.getCode()));
        assertTrue(found);
    }

    @Test
    void testValidateVersionMismatchTable() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withVersion("1.2")
                .addTable(tb -> tb
                        .addColumn(Map.of("width", 1))
                        .addRow(Map.of("cells", List.of())))
                .build();
        List<ValidationIssue> issues = CardValidator.validate(card);
        boolean found = issues.stream()
                .anyMatch(i -> "VERSION_MISMATCH".equals(i.getCode())
                        && i.getSeverity() == ValidationSeverity.WARNING);
        assertTrue(found);
    }

    @Test
    void testValidateAndThrowValidCard() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withVersion("1.5")
                .addTextBlock(tb -> tb.withText("OK"))
                .build();
        assertDoesNotThrow(() -> CardValidator.validateAndThrow(card));
    }

    @Test
    void testValidateAndThrowInvalidCard() {
        Map<String, Object> card = new LinkedHashMap<>();
        card.put("type", "AdaptiveCard");

        assertThrows(AdaptiveCardValidationException.class,
                () -> CardValidator.validateAndThrow(card));
    }

    @Test
    void testAdaptiveCardValidationExceptionMessage() {
        Map<String, Object> card = new LinkedHashMap<>();
        card.put("type", "AdaptiveCard");

        try {
            CardValidator.validateAndThrow(card);
            fail("Expected AdaptiveCardValidationException");
        } catch (AdaptiveCardValidationException e) {
            assertTrue(e.getMessage().contains("validation failed"));
            assertFalse(e.getIssues().isEmpty());
        }
    }
}
