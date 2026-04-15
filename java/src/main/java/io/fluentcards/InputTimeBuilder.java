package io.fluentcards;

import java.util.*;

public class InputTimeBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputTimeBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.Time");
        data.put("id", "");
    }

    public InputTimeBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputTimeBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputTimeBuilder withPlaceholder(String placeholder) {
        data.put("placeholder", placeholder);
        return this;
    }

    public InputTimeBuilder withValue(String value) {
        data.put("value", value);
        return this;
    }

    public InputTimeBuilder withMin(String min) {
        data.put("min", min);
        return this;
    }

    public InputTimeBuilder withMax(String max) {
        data.put("max", max);
        return this;
    }

    public InputTimeBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputTimeBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputTimeBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
