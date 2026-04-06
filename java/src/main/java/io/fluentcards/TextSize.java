package io.fluentcards;

public enum TextSize {
    SMALL("small"),
    DEFAULT("default"),
    MEDIUM("medium"),
    LARGE("large"),
    EXTRA_LARGE("extraLarge");

    private final String value;

    TextSize(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
