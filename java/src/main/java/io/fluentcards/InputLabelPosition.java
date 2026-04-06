package io.fluentcards;

public enum InputLabelPosition {
    INLINE("inline"),
    ABOVE("above");

    private final String value;

    InputLabelPosition(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
