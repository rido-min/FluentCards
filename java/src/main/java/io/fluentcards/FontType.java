package io.fluentcards;

public enum FontType {
    DEFAULT("default"),
    MONOSPACE("monospace");

    private final String value;

    FontType(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
