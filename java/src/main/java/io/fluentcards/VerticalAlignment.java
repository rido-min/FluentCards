package io.fluentcards;

public enum VerticalAlignment {
    TOP("top"),
    CENTER("center"),
    BOTTOM("bottom");

    private final String value;

    VerticalAlignment(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
