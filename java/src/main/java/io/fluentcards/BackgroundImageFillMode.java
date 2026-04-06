package io.fluentcards;

public enum BackgroundImageFillMode {
    COVER("cover"),
    REPEAT_HORIZONTALLY("repeatHorizontally"),
    REPEAT_VERTICALLY("repeatVertically"),
    REPEAT("repeat");

    private final String value;

    BackgroundImageFillMode(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
