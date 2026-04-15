package io.fluentcards;

public enum HorizontalAlignment {
    LEFT("left"),
    CENTER("center"),
    RIGHT("right");

    private final String value;

    HorizontalAlignment(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
