package io.fluentcards;

public enum ImageSize {
    AUTO("auto"),
    STRETCH("stretch"),
    SMALL("small"),
    MEDIUM("medium"),
    LARGE("large");

    private final String value;

    ImageSize(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
