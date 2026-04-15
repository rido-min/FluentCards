package io.fluentcards;

public enum Spacing {
    DEFAULT("default"),
    NONE("none"),
    SMALL("small"),
    MEDIUM("medium"),
    LARGE("large"),
    EXTRA_LARGE("extraLarge"),
    PADDING("padding");

    private final String value;

    Spacing(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
