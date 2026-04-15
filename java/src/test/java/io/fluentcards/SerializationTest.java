package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class SerializationTest {

    @Test
    void testToJsonBasicCard() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Hello"))
                .build();
        String json = CardSerializer.toJson(card);
        assertTrue(json.contains("AdaptiveCard"));
        assertTrue(json.contains("Hello"));
    }

    @Test
    void testToJsonOmitsUnsetOptionalProperties() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Test"))
                .build();
        String json = CardSerializer.toJson(card);
        assertFalse(json.contains("\"size\""));
        assertFalse(json.contains("\"weight\""));
        assertFalse(json.contains("\"color\""));
        assertFalse(json.contains("\"wrap\""));
    }

    @Test
    void testToJsonEnumValuesAreCamelCase() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("x")
                        .withSize(TextSize.EXTRA_LARGE)
                        .withColor(TextColor.ATTENTION))
                .build();
        String json = CardSerializer.toJson(card);
        assertTrue(json.contains("extraLarge"));
        assertTrue(json.contains("attention"));
    }

    @Test
    void testToJsonCompact() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Test"))
                .build();
        String json = CardSerializer.toJson(card, 0);
        assertFalse(json.contains("\n"));
    }

    @Test
    void testToJsonTwoSpaces() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Test"))
                .build();
        String json = CardSerializer.toJson(card, 2);
        assertTrue(json.contains("\n"));
        assertTrue(json.contains("  "), "expected 2-space indent");
    }

    @Test
    void testFromJsonValidCard() {
        String raw = "{\"type\":\"AdaptiveCard\",\"version\":\"1.5\",\"$schema\":\"https://example.com\"}";
        Map<String, Object> card = CardSerializer.fromJson(raw);
        assertNotNull(card);
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
    }

    @Test
    void testFromJsonInvalidJson() {
        Map<String, Object> card = CardSerializer.fromJson("not json");
        assertNull(card);
    }

    @Test
    void testFromJsonWrongRootType() {
        Map<String, Object> card = CardSerializer.fromJson("{\"type\":\"TextBlock\",\"text\":\"oops\"}");
        assertNull(card);
    }

    @SuppressWarnings("unchecked")
    @Test
    void testRoundTrip() {
        Map<String, Object> original = AdaptiveCardBuilder.create()
                .withVersion("1.5")
                .addTextBlock(tb -> tb.withText("Round trip").withSize(TextSize.LARGE))
                .addAction(a -> a.submit("OK").withStyle(ActionStyle.POSITIVE))
                .build();
        String json = CardSerializer.toJson(original);
        Map<String, Object> parsed = CardSerializer.fromJson(json);
        assertNotNull(parsed);
        assertEquals("1.5", parsed.get("version"));
        List<Object> body = (List<Object>) parsed.get("body");
        assertEquals(1, body.size());
    }
}
