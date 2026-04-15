package io.fluentcards;

import java.util.*;

public class InputToggleBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputToggleBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.Toggle");
        data.put("id", "");
        data.put("title", "");
    }

    public InputToggleBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputToggleBuilder withTitle(String title) {
        data.put("title", title);
        return this;
    }

    public InputToggleBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputToggleBuilder withValue(String value) {
        data.put("value", value);
        return this;
    }

    public InputToggleBuilder withValueOn(String valueOn) {
        data.put("valueOn", valueOn);
        return this;
    }

    public InputToggleBuilder withValueOff(String valueOff) {
        data.put("valueOff", valueOff);
        return this;
    }

    public InputToggleBuilder withWrap(boolean wrap) {
        data.put("wrap", wrap);
        return this;
    }

    public InputToggleBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputToggleBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputToggleBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
