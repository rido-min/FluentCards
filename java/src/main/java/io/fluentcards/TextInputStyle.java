package io.fluentcards;

public enum TextInputStyle {
    TEXT("text"),
    TEL("tel"),
    URL("url"),
    EMAIL("email"),
    PASSWORD("password");

    private final String value;

    TextInputStyle(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
