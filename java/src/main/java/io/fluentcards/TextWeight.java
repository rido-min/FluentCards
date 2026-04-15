package io.fluentcards;

public enum TextWeight {
    LIGHTER("lighter"),
    DEFAULT("default"),
    BOLDER("bolder");

    private final String value;

    TextWeight(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
