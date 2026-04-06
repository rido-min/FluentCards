package io.fluentcards;

import java.util.*;

public class TextBlockBuilder {
    private final LinkedHashMap<String, Object> data;

    public TextBlockBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "TextBlock");
        data.put("text", "");
    }

    public TextBlockBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public TextBlockBuilder withText(String text) {
        data.put("text", text);
        return this;
    }

    public TextBlockBuilder withSize(TextSize size) {
        data.put("size", size.getValue());
        return this;
    }

    public TextBlockBuilder withWeight(TextWeight weight) {
        data.put("weight", weight.getValue());
        return this;
    }

    public TextBlockBuilder withColor(TextColor color) {
        data.put("color", color.getValue());
        return this;
    }

    public TextBlockBuilder withIsSubtle(boolean isSubtle) {
        data.put("isSubtle", isSubtle);
        return this;
    }

    public TextBlockBuilder withSubtle() {
        data.put("isSubtle", true);
        return this;
    }

    public TextBlockBuilder withWrap(boolean wrap) {
        data.put("wrap", wrap);
        return this;
    }

    public TextBlockBuilder withMaxLines(int maxLines) {
        data.put("maxLines", maxLines);
        return this;
    }

    public TextBlockBuilder withHorizontalAlignment(HorizontalAlignment alignment) {
        data.put("horizontalAlignment", alignment.getValue());
        return this;
    }

    public TextBlockBuilder withFontType(FontType fontType) {
        data.put("fontType", fontType.getValue());
        return this;
    }

    public TextBlockBuilder withStyle(TextBlockStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public TextBlockBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public TextBlockBuilder withSeparator(boolean separator) {
        data.put("separator", separator);
        return this;
    }

    public TextBlockBuilder withIsVisible(boolean isVisible) {
        data.put("isVisible", isVisible);
        return this;
    }

    public TextBlockBuilder withSelectAction(Map<String, Object> action) {
        data.put("selectAction", action);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
