package io.fluentcards;

import java.util.*;
import java.util.function.Consumer;

public class InputTextBuilder {
    private final LinkedHashMap<String, Object> data;

    public InputTextBuilder() {
        data = new LinkedHashMap<>();
        data.put("type", "Input.Text");
        data.put("id", "");
    }

    public InputTextBuilder withId(String id) {
        data.put("id", id);
        return this;
    }

    public InputTextBuilder withLabel(String label) {
        data.put("label", label);
        return this;
    }

    public InputTextBuilder withPlaceholder(String placeholder) {
        data.put("placeholder", placeholder);
        return this;
    }

    public InputTextBuilder withValue(String value) {
        data.put("value", value);
        return this;
    }

    public InputTextBuilder withMaxLength(int maxLength) {
        data.put("maxLength", maxLength);
        return this;
    }

    public InputTextBuilder withIsMultiline(boolean isMultiline) {
        data.put("isMultiline", isMultiline);
        return this;
    }

    public InputTextBuilder withStyle(TextInputStyle style) {
        data.put("style", style.getValue());
        return this;
    }

    public InputTextBuilder withRegex(String regex) {
        data.put("regex", regex);
        return this;
    }

    public InputTextBuilder withIsRequired(boolean isRequired) {
        data.put("isRequired", isRequired);
        return this;
    }

    public InputTextBuilder withErrorMessage(String errorMessage) {
        data.put("errorMessage", errorMessage);
        return this;
    }

    public InputTextBuilder withSpacing(Spacing spacing) {
        data.put("spacing", spacing.getValue());
        return this;
    }

    public InputTextBuilder withInlineAction(Consumer<ActionBuilder> configure) {
        ActionBuilder builder = new ActionBuilder();
        configure.accept(builder);
        data.put("inlineAction", builder.build());
        return this;
    }

    public Map<String, Object> build() {
        return data;
    }
}
