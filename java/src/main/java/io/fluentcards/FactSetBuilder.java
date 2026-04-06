package io.fluentcards;

import java.util.*;

public class FactSetBuilder {
    private final LinkedHashMap<String, Object> data;

    public FactSetBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "FactSet");
        data.put("facts", new ArrayList<>());
    }

    public FactSetBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public FactSetBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public FactSetBuilder addFact(String title, String value) {
        LinkedHashMap<String, Object> fact = new LinkedHashMap<>();
        fact.put("title", title);
        fact.put("value", value);
        ((List<Object>) data.get("facts")).add(fact);
        return this;
    }

    @SuppressWarnings("unchecked")
    public FactSetBuilder addFactMap(Map<String, Object> fact) {
        ((List<Object>) data.get("facts")).add(fact);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
