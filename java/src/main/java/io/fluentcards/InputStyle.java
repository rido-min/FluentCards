package io.fluentcards;

public enum InputStyle {
    DEFAULT("default"),
    REVEAL_ON_HOVER("revealOnHover");

    private final String value;

    InputStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
