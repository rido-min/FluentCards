namespace FluentCards;

/// <summary>
/// Exception thrown when Adaptive Card validation fails.
/// </summary>
public class AdaptiveCardValidationException : Exception
{
    /// <summary>
    /// Gets the list of validation errors that caused the exception.
    /// </summary>
    public IReadOnlyList<ValidationIssue> Errors { get; }
    
    /// <summary>
    /// Initializes a new instance of <see cref="AdaptiveCardValidationException"/>.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    public AdaptiveCardValidationException(IReadOnlyList<ValidationIssue> errors)
        : base(FormatMessage(errors))
    {
        Errors = errors;
    }
    
    private static string FormatMessage(IReadOnlyList<ValidationIssue> errors)
    {
        if (errors.Count == 1)
        {
            return $"Adaptive Card validation failed: {errors[0].Message}";
        }
        
        return $"Adaptive Card validation failed with {errors.Count} errors:\n" +
               string.Join("\n", errors.Select(e => $"  - [{e.Path}] {e.Message}"));
    }
}
