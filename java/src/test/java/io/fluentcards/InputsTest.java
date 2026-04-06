package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class InputsTest {

    @SuppressWarnings("unchecked")
    @Test
    void testInputTextBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputText(it -> it
                        .withId("name")
                        .withLabel("Your Name")
                        .withPlaceholder("Enter name")
                        .withMaxLength(100)
                        .withIsMultiline(false)
                        .withStyle(TextInputStyle.EMAIL)
                        .withRegex("^[a-zA-Z]+$")
                        .withIsRequired(true)
                        .withErrorMessage("Name is required"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.Text", el.get("type"));
        assertEquals("name", el.get("id"));
        assertEquals("Your Name", el.get("label"));
        assertEquals("Enter name", el.get("placeholder"));
        assertEquals(100, el.get("maxLength"));
        assertEquals(false, el.get("isMultiline"));
        assertEquals("email", el.get("style"));
        assertEquals("^[a-zA-Z]+$", el.get("regex"));
        assertEquals(true, el.get("isRequired"));
        assertEquals("Name is required", el.get("errorMessage"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputNumberBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputNumber(in -> in
                        .withId("qty")
                        .withLabel("Quantity")
                        .withMin(1)
                        .withMax(100)
                        .withValue(10))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.Number", el.get("type"));
        assertEquals("qty", el.get("id"));
        assertEquals("Quantity", el.get("label"));
        assertEquals(1.0, el.get("min"));
        assertEquals(100.0, el.get("max"));
        assertEquals(10.0, el.get("value"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputDateBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputDate(id -> id
                        .withId("start")
                        .withLabel("Start Date")
                        .withMin("2025-01-01")
                        .withMax("2026-12-31")
                        .withValue("2025-06-15"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.Date", el.get("type"));
        assertEquals("start", el.get("id"));
        assertEquals("Start Date", el.get("label"));
        assertEquals("2025-01-01", el.get("min"));
        assertEquals("2026-12-31", el.get("max"));
        assertEquals("2025-06-15", el.get("value"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputTimeBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputTime(it -> it
                        .withId("meeting-time")
                        .withLabel("Meeting Time")
                        .withMin("09:00")
                        .withMax("17:00"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.Time", el.get("type"));
        assertEquals("meeting-time", el.get("id"));
        assertEquals("Meeting Time", el.get("label"));
        assertEquals("09:00", el.get("min"));
        assertEquals("17:00", el.get("max"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputToggleBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputToggle(it -> it
                        .withId("agree")
                        .withTitle("I agree to the terms")
                        .withValueOn("true")
                        .withValueOff("false")
                        .withWrap(true))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.Toggle", el.get("type"));
        assertEquals("agree", el.get("id"));
        assertEquals("I agree to the terms", el.get("title"));
        assertEquals("true", el.get("valueOn"));
        assertEquals("false", el.get("valueOff"));
        assertEquals(true, el.get("wrap"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputChoiceSetBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputChoiceSet(ics -> ics
                        .withId("color")
                        .withLabel("Favorite Color")
                        .withStyle(ChoiceInputStyle.EXPANDED)
                        .addChoice("Red", "red")
                        .addChoice("Green", "green"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Input.ChoiceSet", el.get("type"));
        assertEquals("color", el.get("id"));
        assertEquals("expanded", el.get("style"));
        List<Object> choices = (List<Object>) el.get("choices");
        assertEquals(2, choices.size());
        assertEquals("Red", ((Map<String, Object>) choices.get(0)).get("title"));
        assertEquals("red", ((Map<String, Object>) choices.get(0)).get("value"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testInputChoiceSetWithChoicesData() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addInputChoiceSet(ics -> ics
                        .withId("people-picker")
                        .withChoicesData("graph.microsoft.com/users"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        Map<String, Object> choicesData = (Map<String, Object>) el.get("choices.data");
        assertNotNull(choicesData, "choices.data should be present");
        assertEquals("Data.Query", choicesData.get("type"));
        assertEquals("graph.microsoft.com/users", choicesData.get("dataset"));
    }
}
