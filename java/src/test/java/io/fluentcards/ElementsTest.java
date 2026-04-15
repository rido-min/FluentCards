package io.fluentcards;

import org.junit.jupiter.api.Test;

import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;

class ElementsTest {

    @SuppressWarnings("unchecked")
    @Test
    void testTextBlockBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTextBlock(tb -> tb
                        .withText("Hello")
                        .withSize(TextSize.LARGE)
                        .withWeight(TextWeight.BOLDER)
                        .withColor(TextColor.ACCENT)
                        .withWrap(true)
                        .withMaxLines(3)
                        .withHorizontalAlignment(HorizontalAlignment.CENTER)
                        .withFontType(FontType.MONOSPACE)
                        .withStyle(TextBlockStyle.HEADING)
                        .withSpacing(Spacing.MEDIUM)
                        .withSeparator(true)
                        .withId("tb1"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("TextBlock", el.get("type"));
        assertEquals("Hello", el.get("text"));
        assertEquals("large", el.get("size"));
        assertEquals("bolder", el.get("weight"));
        assertEquals("accent", el.get("color"));
        assertEquals(true, el.get("wrap"));
        assertEquals(3, el.get("maxLines"));
        assertEquals("center", el.get("horizontalAlignment"));
        assertEquals("monospace", el.get("fontType"));
        assertEquals("heading", el.get("style"));
        assertEquals("medium", el.get("spacing"));
        assertEquals(true, el.get("separator"));
        assertEquals("tb1", el.get("id"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testImageBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addImage(img -> img
                        .withUrl("https://example.com/img.png")
                        .withAltText("An image")
                        .withSize(ImageSize.MEDIUM)
                        .withStyle(ImageStyle.PERSON)
                        .withWidth("100px")
                        .withHeight("100px")
                        .withBackgroundColor("#FFFFFF"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Image", el.get("type"));
        assertEquals("https://example.com/img.png", el.get("url"));
        assertEquals("An image", el.get("altText"));
        assertEquals("medium", el.get("size"));
        assertEquals("person", el.get("style"));
        assertEquals("100px", el.get("width"));
        assertEquals("#FFFFFF", el.get("backgroundColor"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testContainerBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addContainer(c -> c
                        .withStyle(ContainerStyle.EMPHASIS)
                        .withBleed(true)
                        .addTextBlock(tb -> tb.withText("Inside container"))
                        .addImage(img -> img.withUrl("https://example.com/x.png")))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Container", el.get("type"));
        assertEquals("emphasis", el.get("style"));
        assertEquals(true, el.get("bleed"));
        List<Object> items = (List<Object>) el.get("items");
        assertEquals(2, items.size());
    }

    @SuppressWarnings("unchecked")
    @Test
    void testColumnSetBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addColumnSet(cs -> cs
                        .addColumnWithWidth("auto", col ->
                                col.addTextBlock(tb -> tb.withText("Left")))
                        .addColumnWithWidth("stretch", col ->
                                col.withVerticalContentAlignment(VerticalAlignment.CENTER)
                                        .addTextBlock(tb -> tb.withText("Right"))))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("ColumnSet", el.get("type"));
        List<Object> cols = (List<Object>) el.get("columns");
        assertEquals(2, cols.size());
        assertEquals("auto", ((Map<String, Object>) cols.get(0)).get("width"));
        assertEquals("stretch", ((Map<String, Object>) cols.get(1)).get("width"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testFactSetBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addFactSet(fs -> fs
                        .addFact("Name", "Alice")
                        .addFact("Role", "Engineer"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        List<Object> facts = (List<Object>) el.get("facts");
        assertEquals(2, facts.size());
        assertEquals("Name", ((Map<String, Object>) facts.get(0)).get("title"));
        assertEquals("Alice", ((Map<String, Object>) facts.get(0)).get("value"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testRichTextBlockBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addRichTextBlock(rtb -> rtb
                        .addInline("plain text")
                        .addTextRun(tr -> tr
                                .withText("bold")
                                .withWeight(TextWeight.BOLDER)
                                .withItalic(true)))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("RichTextBlock", el.get("type"));
        List<Object> inlines = (List<Object>) el.get("inlines");
        assertEquals(2, inlines.size());
        assertEquals("plain text", inlines.get(0));
        Map<String, Object> run = (Map<String, Object>) inlines.get(1);
        assertEquals("TextRun", run.get("type"));
        assertEquals("bold", run.get("text"));
        assertEquals("bolder", run.get("weight"));
        assertEquals(true, run.get("italic"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testImageSetBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addImageSet(is -> is
                        .withImageSize(ImageSize.MEDIUM)
                        .addImage(img -> img.withUrl("https://example.com/1.png"))
                        .addImage(img -> img.withUrl("https://example.com/2.png")))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("ImageSet", el.get("type"));
        assertEquals("medium", el.get("imageSize"));
        List<Object> images = (List<Object>) el.get("images");
        assertEquals(2, images.size());
    }

    @SuppressWarnings("unchecked")
    @Test
    void testMediaBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addMedia(m -> m
                        .withPoster("https://example.com/poster.png")
                        .addSource("https://example.com/video.mp4", "video/mp4"))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Media", el.get("type"));
        List<Object> sources = (List<Object>) el.get("sources");
        assertEquals(1, sources.size());
        Map<String, Object> s = (Map<String, Object>) sources.get(0);
        assertEquals("https://example.com/video.mp4", s.get("url"));
        assertEquals("video/mp4", s.get("mimeType"));
    }

    @SuppressWarnings("unchecked")
    @Test
    void testTableBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addTable(tb -> tb
                        .withFirstRowAsHeader(true)
                        .withShowGridLines(true)
                        .addColumn(Map.of("width", 1))
                        .addColumn(Map.of("width", 2))
                        .addRow(Map.of("cells", List.of(
                                Map.of("items", List.of(Map.of("type", "TextBlock", "text", "H1"))),
                                Map.of("items", List.of(Map.of("type", "TextBlock", "text", "H2")))))))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("Table", el.get("type"));
        assertEquals(true, el.get("firstRowAsHeader"));
        List<Object> cols = (List<Object>) el.get("columns");
        assertEquals(2, cols.size());
        List<Object> rows = (List<Object>) el.get("rows");
        assertEquals(1, rows.size());
    }

    @SuppressWarnings("unchecked")
    @Test
    void testActionSetBuilder() {
        Map<String, Object> card = AdaptiveCardBuilder.create()
                .addActionSet(as -> as
                        .addAction(a -> a.submit("OK"))
                        .addAction(a -> a.openUrl("https://example.com")))
                .build();
        Map<String, Object> el = (Map<String, Object>) ((List<Object>) card.get("body")).get(0);
        assertEquals("ActionSet", el.get("type"));
        List<Object> actions = (List<Object>) el.get("actions");
        assertEquals(2, actions.size());
    }
}
