package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class ActionsTest {

    @SuppressWarnings("unchecked")
    @Test
    void testOpenUrlAction() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.openUrl("https://example.com").withTitle("Go There"))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("Action.OpenUrl", action.get("type"));
        assertEquals("https://example.com", action.get("url"));
        assertEquals("Go There", action.get("title"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testSubmitAction() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.submit("Send").withStyle(ActionStyle.POSITIVE))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("Action.Submit", action.get("type"));
        assertEquals("Send", action.get("title"));
        assertEquals("positive", action.get("style"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testShowCardAction() {
        Map<String, Object> innerCard = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb.withText("Inner"))
                .build();
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.showCard("Show More").withCard(innerCard))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("Action.ShowCard", action.get("type"));
        assertEquals("Show More", action.get("title"));
        assertNotNull(action.get("card"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testToggleVisibilityAction() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.toggleVisibility("Toggle")
                        .addTargetElement("details-section", null)
                        .addTargetElement("header", true))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("Action.ToggleVisibility", action.get("type"));
        List<Object> targets = (List<Object>) action.get("targetElements");
        assertEquals(2, targets.size());
        assertEquals("details-section", targets.get(0));
        Map<String, Object> target2 = (Map<String, Object>) targets.get(1);
        assertEquals("header", target2.get("elementId"));
        assertEquals(true, target2.get("isVisible"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testExecuteAction() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.execute("Run")
                        .withVerb("doSomething")
                        .withData(Map.of("key", "value")))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("Action.Execute", action.get("type"));
        assertEquals("Run", action.get("title"));
        assertEquals("doSomething", action.get("verb"));
        Map<String, Object> data = (Map<String, Object>) action.get("data");
        assertEquals("value", data.get("key"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testActionWithStyle() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addAction(a -> a.submit("Send").withStyle(ActionStyle.POSITIVE))
                .build();
        Map<String, Object> action = (Map<String, Object>) ((List<Object>) card.get("actions")).get(0);
        assertEquals("positive", action.get("style"));
    }

    @Test
    void testActionBuilderNullPanics() {
        assertThrows(IllegalStateException.class, () ->
                new ActionBuilder().build());
    }
}
