package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class TextRunBuilder {
    private final LinkedHashMap<String, Object> data;

    public TextRunBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "TextRun");
        data.put("text", "");
    }

    public TextRunBuilder withText(String text) {
        data.put("text", text);
        return this;
    }

    public TextRunBuilder withSize(TextSize size) {
        data.put("size", size.getValue());
        return this;
    }

    public TextRunBuilder withWeight(TextWeight weight) {
        data.put("weight", weight.getValue());
        return this;
    }

    public TextRunBuilder withColor(TextColor color) {
        data.put("color", color.getValue());
        return this;
    }

    public TextRunBuilder withIsSubtle(boolean isSubtle) {
        data.put("isSubtle", isSubtle);
        return this;
    }

    public TextRunBuilder withItalic(boolean italic) {
        data.put("italic", italic);
        return this;
    }

    public TextRunBuilder withStrikethrough(boolean strikethrough) {
        data.put("strikethrough", strikethrough);
        return this;
    }

    public TextRunBuilder withUnderline(boolean underline) {
        data.put("underline", underline);
        return this;
    }

    public TextRunBuilder withHighlight(boolean highlight) {
        data.put("highlight", highlight);
        return this;
    }

    public TextRunBuilder withFontType(FontType fontType) {
        data.put("fontType", fontType.getValue());
        return this;
    }

    public TextRunBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
