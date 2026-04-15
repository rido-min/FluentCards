package io.fluentcards;

public enum ValidationSeverity {
    INFO("info"),
    WARNING("warning"),
    ERROR("error");

    private final String value;

    ValidationSeverity(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }
}
