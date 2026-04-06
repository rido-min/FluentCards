package io.fluentcards;

public enum ContainerStyle {
    DEFAULT("default"),
    EMPHASIS("emphasis"),
    GOOD("good"),
    ATTENTION("attention"),
    WARNING("warning"),
    ACCENT("accent");

    private final String value;

    ContainerStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
