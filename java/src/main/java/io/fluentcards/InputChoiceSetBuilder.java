package io.fluentcards;

import java.util.*;

public class InputChoiceSetBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputChoiceSetBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.ChoiceSet");
        data.put("id", "");
        data.put("choices", new ArrayList<>());
    }

    public InputChoiceSetBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputChoiceSetBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputChoiceSetBuilder withPlaceholder(String placeholder) {
        data.put("placeholder", placeholder);
        return this;
    }

    public InputChoiceSetBuilder withValue(String value) {
        data.put("value", value);
        return this;
    }

    public InputChoiceSetBuilder withStyle(ChoiceInputStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public InputChoiceSetBuilder withIsMultiSelect(boolean isMultiSelect) {
        data.put("isMultiSelect", isMultiSelect);
        return this;
    }

    public InputChoiceSetBuilder withWrap(boolean wrap) {
        data.put("wrap", wrap);
        return this;
    }

    public InputChoiceSetBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputChoiceSetBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputChoiceSetBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    @SuppressWarnings("unchecked")
    public InputChoiceSetBuilder addChoice(String title, String value) {
        LinkedHashMap<String, Object> choice = new LinkedHashMap<>();
        choice.put("title", title);
        choice.put("value", value);
        ((List<Object>) data.get("choices")).add(choice);
        return this;
    }

    @SuppressWarnings("unchecked")
    public InputChoiceSetBuilder addChoiceMap(Map<String, Object> choice) {
        ((List<Object>) data.get("choices")).add(choice);
        return this;
    }

    public InputChoiceSetBuilder withChoicesData(String dataset) {
        LinkedHashMap<String, Object> choicesData = new LinkedHashMap<>();
        choicesData.put("type", "Data.Query");
        choicesData.put("dataset", dataset);
        data.put("choices.data", choicesData);
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
