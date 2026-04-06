package io.fluentcards;

public enum ChoiceInputStyle {
    COMPACT("compact"),
    EXPANDED("expanded"),
    FILTERED("filtered");

    private final String value;

    ChoiceInputStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
