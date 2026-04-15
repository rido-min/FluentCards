package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ContainerBuilder {
    private final LinkedHashMap<String, Object> data;

    public ContainerBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Container");
        data.put("items", new ArrayList<>());
    }

    public ContainerBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ContainerBuilder withStyle(ContainerStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public ContainerBuilder withVerticalContentAlignment(VerticalAlignment alignment) {
        data.put("verticalContentAlignment", alignment.getValue());
        return this;
    }

    public ContainerBuilder withBleed(boolean bleed) {
        data.put("bleed", bleed);
        return this;
    }

    public ContainerBuilder withMinHeight(String minHeight) {
        data.put("minHeight", minHeight);
        return this;
    }

    public ContainerBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public ContainerBuilder withSeparator(boolean separator) {
        data.put("separator", separator);
        return this;
    }

    public ContainerBuilder withIsVisible(boolean isVisible) {
        data.put("isVisible", isVisible);
        return this;
    }

    public ContainerBuilder withBackgroundImage(Consumer<BackgroundImageBuilder> configure) {
        BackgroundImageBuilder builder = new BackgroundImageBuilder();
        configure.accept(builder);
        data.put("backgroundImage", builder.build());
        return this;
    }

    public ContainerBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addTextBlock(Consumer<TextBlockBuilder> configure) {
        TextBlockBuilder builder = new TextBlockBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addImage(Consumer<ImageBuilder> configure) {
        ImageBuilder builder = new ImageBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addContainer(Consumer<ContainerBuilder> configure) {
        ContainerBuilder builder = new ContainerBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addColumnSet(Consumer<ColumnSetBuilder> configure) {
        ColumnSetBuilder builder = new ColumnSetBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addFactSet(Consumer<FactSetBuilder> configure) {
        FactSetBuilder builder = new FactSetBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addRichTextBlock(Consumer<RichTextBlockBuilder> configure) {
        RichTextBlockBuilder builder = new RichTextBlockBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addActionSet(Consumer<ActionSetBuilder> configure) {
        ActionSetBuilder builder = new ActionSetBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("items")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ContainerBuilder addElement(Map<String, Object> element) {
        ((List<Object>) data.get("items")).add(element);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
