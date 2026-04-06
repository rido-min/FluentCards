package io.fluentcards;

public enum ActionMode {
    PRIMARY("primary"),
    SECONDARY("secondary");

    private final String value;

    ActionMode(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
