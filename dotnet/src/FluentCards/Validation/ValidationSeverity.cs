namespace FluentCards;

/// <summary>
/// Specifies the severity of a validation issue.
/// </summary>
public enum ValidationSeverity
{
    /// <summary>
    /// The issue is informational and does not affect rendering.
    /// </summary>
    Info,
    
    /// <summary>
    /// The issue may cause unexpected behavior in some hosts.
    /// </summary>
    Warning,
    
    /// <summary>
    /// The issue will cause the card to fail to render.
    /// </summary>
    Error
}
