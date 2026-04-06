package io.fluentcards;

import java.util.*;

public class InputDateBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputDateBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.Date");
        data.put("id", "");
    }

    public InputDateBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputDateBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputDateBuilder withPlaceholder(String placeholder) {
        data.put("placeholder", placeholder);
        return this;
    }

    public InputDateBuilder withValue(String value) {
        data.put("value", value);
        return this;
    }

    public InputDateBuilder withMin(String min) {
        data.put("min", min);
        return this;
    }

    public InputDateBuilder withMax(String max) {
        data.put("max", max);
        return this;
    }

    public InputDateBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputDateBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputDateBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
