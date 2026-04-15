package io.fluentcards;

public class ValidationIssue {

    private final ValidationSeverity severity;
    private final String path;
    private final String code;
    private final String message;

    public ValidationIssue(ValidationSeverity severity, String path, String code, String message) {
        this.severity = severity;
        this.path = path;
        this.code = code;
        this.message = message;
    }

    public ValidationSeverity getSeverity() {
        return severity;
    }

    public String getPath() {
        return path;
    }

    public String getCode() {
        return code;
    }

    public String getMessage() {
        return message;
    }
}
