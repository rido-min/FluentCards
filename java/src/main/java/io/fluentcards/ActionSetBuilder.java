package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class ActionSetBuilder {
    private final LinkedHashMap<String, Object> data;

    public ActionSetBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "ActionSet");
        data.put("actions", new ArrayList<>());
    }

    public ActionSetBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public ActionSetBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public ActionSetBuilder addAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        ((List<Object>) data.get("actions")).add(builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
