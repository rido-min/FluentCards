package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class RichTextBlockBuilder {
    private final LinkedHashMap<String, Object> data;

    public RichTextBlockBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "RichTextBlock");
        data.put("inlines", new ArrayList<>());
    }

    public RichTextBlockBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public RichTextBlockBuilder withHorizontalAlignment(HorizontalAlignment alignment) {
        data.put("horizontalAlignment", alignment.getValue());
        return this;
    }

    public RichTextBlockBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public RichTextBlockBuilder addInline(String text) {
        ((List<Object>) data.get("inlines")).add(text);
        return this;
    }

    @SuppressWarnings("unchecked")
    public RichTextBlockBuilder addTextRun(Consumer<TextRunBuilder> configure) {
        TextRunBuilder builder = new TextRunBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("inlines")).add(builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
