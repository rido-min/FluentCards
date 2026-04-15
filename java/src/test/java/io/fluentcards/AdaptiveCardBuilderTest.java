package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class AdaptiveCardBuilderTest {

    @Test
    void testDefaultVersionAndSchema() {
        Map<String, Object> card = AdaptiveCardBuilder.create().build();
        assertEquals("AdaptiveCard", card.get("type"));
        assertEquals("1.5", card.get("version"));
        assertNotNull(card.get("$schema"));
        assertFalse(((String) card.get("$schema")).isEmpty());
    }

    @Test
    void testWithVersion() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withVersion("1.6")
                .build();
        assertEquals("1.6", card.get("version"));
        assertTrue(((String) card.get("$schema")).contains("1.6"));
    }

    @Test
    void testWithSchemaOverride() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withSchema("https://example.com/custom-schema.json")
                .build();
        assertEquals("https://example.com/custom-schema.json", card.get("$schema"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testAddTextBlock() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Hello, World!"))
                .build();
        List<Object> body = (List<Object>) card.get("body");
        assertEquals(1, body.size());
        Map<String, Object> el = (Map<String, Object>) body.get(0);
        assertEquals("TextBlock", el.get("type"));
        assertEquals("Hello, World!", el.get("text"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testAddAction() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("x"))
                .addAction(a -> a.submit("Click me"))
                .build();
        List<Object> actions = (List<Object>) card.get("actions");
        assertEquals(1, actions.size());
        Map<String, Object> action = (Map<String, Object>) actions.get(0);
        assertEquals("Action.Submit", action.get("type"));
        assertEquals("Click me", action.get("title"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testMultipleBodyElements() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("First"))
                .addTextBlock(tb -> tb.withText("Second"))
                .addImage(img -> img.withUrl("https://example.com/img.png"))
                .build();
        List<Object> body = (List<Object>) card.get("body");
        assertEquals(3, body.size());
    }

    @SuppressWarnings("unchecked")
    @Test
    void testWithMetadata() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withMetadata("https://example.com/card")
                .build();
        Map<String, Object> meta = (Map<String, Object>) card.get("metadata");
        assertEquals("https://example.com/card", meta.get("webUrl"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testWithRefresh() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .withRefresh(r -> r.addUserId("user1").withExpires("2026-01-01T00:00:00Z"))
                .build();
        Map<String, Object> refresh = (Map<String, Object>) card.get("refresh");
        assertEquals("2026-01-01T00:00:00Z", refresh.get("expires"));
        List<Object> userIds = (List<Object>) refresh.get("userIds");
        assertEquals("user1", userIds.get(0));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testAddElementPreBuilt() {
        Map<String, Object> prebuilt = new LinkedHashMap<>();
        prebuilt.put("type", "TextBlock");
        prebuilt.put("text", "Pre-built");
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addElement(prebuilt)
                .build();
        List<Object> body = (List<Object>) card.get("body");
        assertEquals(1, body.size());
        Map<String, Object> el = (Map<String, Object>) body.get(0);
        assertEquals("Pre-built", el.get("text"));
    }
}
