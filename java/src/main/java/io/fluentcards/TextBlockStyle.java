package io.fluentcards;

public enum TextBlockStyle {
    DEFAULT("default"),
    HEADING("heading");

    private final String value;

    TextBlockStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
