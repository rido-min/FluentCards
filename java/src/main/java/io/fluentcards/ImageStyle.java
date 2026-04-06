package io.fluentcards;

public enum ImageStyle {
    DEFAULT("default"),
    PERSON("person");

    private final String value;

    ImageStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
