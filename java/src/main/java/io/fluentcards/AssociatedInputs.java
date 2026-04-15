package io.fluentcards;

public enum AssociatedInputs {
    AUTO("auto"),
    NONE("none");

    private final String value;

    AssociatedInputs(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
