package io.fluentcards;

import java.util.List;

public class AdaptiveCardValidationException extends RuntimeException {

    private final List<ValidationIssue> issues;

    public AdaptiveCardValidationException(List<ValidationIssue> issues) {
        super(buildMessage(issues));
        this.issues = issues;
    }

    public List<ValidationIssue> getIssues() {
        return issues;
    }

    private static String buildMessage(List<ValidationIssue> issues) {
        List<ValidationIssue> errors = issues.stream()
                .filter(i -> i.getSeverity() == ValidationSeverity.ERROR)
                .toList();

        if (errors.size() == 1) {
            return "Adaptive Card validation failed: " + errors.get(0).getMessage();
        }

        StringBuilder sb = new StringBuilder();
        sb.append("Adaptive Card validation failed with ").append(errors.size()).append(" errors:");
        for (ValidationIssue error : errors) {
            sb.append("\n  - [").append(error.getPath()).append("] ").append(error.getMessage());
        }
        return sb.toString();
    }
}
