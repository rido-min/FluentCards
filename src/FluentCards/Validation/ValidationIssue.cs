namespace FluentCards;

/// <summary>
/// Represents a validation issue found in an Adaptive Card.
/// </summary>
public class ValidationIssue
{
    /// <summary>
    /// Gets the severity of the issue.
    /// </summary>
    public ValidationSeverity Severity { get; init; }
    
    /// <summary>
    /// Gets the path to the element with the issue (e.g., "body[0].items[2]").
    /// </summary>
    public string Path { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the error code for programmatic handling.
    /// </summary>
    public string Code { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets a human-readable description of the issue.
    /// </summary>
    public string Message { get; init; } = string.Empty;
}
