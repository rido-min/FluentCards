package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ColumnSetBuilder {
    private final LinkedHashMap<String, Object> data;

    public ColumnSetBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "ColumnSet");
        data.put("columns", new ArrayList<>());
    }

    public ColumnSetBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ColumnSetBuilder withStyle(ContainerStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public ColumnSetBuilder withBleed(boolean bleed) {
        data.put("bleed", bleed);
        return this;
    }

    public ColumnSetBuilder withMinHeight(String minHeight) {
        data.put("minHeight", minHeight);
        return this;
    }

    public ColumnSetBuilder withHorizontalAlignment(HorizontalAlignment alignment) {
        data.put("horizontalAlignment", alignment.getValue());
        return this;
    }

    public ColumnSetBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public ColumnSetBuilder withSeparator(boolean separator) {
        data.put("separator", separator);
        return this;
    }

    public ColumnSetBuilder withSelectAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("selectAction", builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnSetBuilder addColumn(Consumer<ColumnBuilder> configure) {
        ColumnBuilder builder = new ColumnBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("columns")).add(builder.build());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ColumnSetBuilder addColumnWithWidth(String width, Consumer<ColumnBuilder> configure) {
        ColumnBuilder builder = new ColumnBuilder();
        builder.withWidth(width);
        configure.accept(builder);
        ((List<Object>) data.get("columns")).add(builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
