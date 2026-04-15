package io.fluentcards;

import java.util.*;

public class InputNumberBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputNumberBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.Number");
        data.put("id", "");
    }

    public InputNumberBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputNumberBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputNumberBuilder withPlaceholder(String placeholder) {
        data.put("placeholder", placeholder);
        return this;
    }

    public InputNumberBuilder withValue(double value) {
        data.put("value", value);
        return this;
    }

    public InputNumberBuilder withMin(double min) {
        data.put("min", min);
        return this;
    }

    public InputNumberBuilder withMax(double max) {
        data.put("max", max);
        return this;
    }

    public InputNumberBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputNumberBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputNumberBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
