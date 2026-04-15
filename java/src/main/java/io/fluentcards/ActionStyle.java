package io.fluentcards;

public enum ActionStyle {
    DEFAULT("default"),
    POSITIVE("positive"),
    DESTRUCTIVE("destructive");

    private final String value;

    ActionStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
