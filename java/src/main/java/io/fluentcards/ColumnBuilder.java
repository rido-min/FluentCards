package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ColumnBuilder {
    private final LinkedHashMap<String, Object> data;

    public ColumnBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Column");
        data.put("items", new ArrayList<>());
    }

    public ColumnBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ColumnBuilder withWidth(String width) {
        data.put("width", width);
        return this;
    }

    public ColumnBuilder withStyle(ContainerStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public ColumnBuilder withVerticalContentAlignment(VerticalAlignment alignment) {
        data.put("verticalContentAlignment", alignment.getValue());
        return this;
    }

    public ColumnBuilder withBleed(boolean bleed) {
        data.put("bleed", bleed);
        return this;
    }

    public ColumnBuilder withMinHeight(String minHeight) {
        data.put("minHeight", minHeight);
        return this;
    }

    public ColumnBuilder withBackgroundImage(Consumer<BackgroundImageBuilder> configure) {
        BackgroundImageBuilder builder = new BackgroundImageBuilder();
        configure.accept(builder);
        data.put("backgroundImage", builder.build());
        return this;
    }

    public ColumnBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnBuilder addTextBlock(Consumer<TextBlockBuilder> configure) {
        TextBlockBuilder builder = new TextBlockBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnBuilder addImage(Consumer<ImageBuilder> configure) {
        ImageBuilder builder = new ImageBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnBuilder addContainer(Consumer<ContainerBuilder> configure) {
        ContainerBuilder builder = new ContainerBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnBuilder addElement(Map<String, Object> element) {
        ((List<Object>) data.get("items")).add(element);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
