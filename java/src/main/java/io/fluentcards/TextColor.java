package io.fluentcards;

public enum TextColor {
    DEFAULT("default"),
    DARK("dark"),
    LIGHT("light"),
    ACCENT("accent"),
    GOOD("good"),
    ATTENTION("attention"),
    WARNING("warning"),
    WHITE("white");

    private final String value;

    TextColor(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
