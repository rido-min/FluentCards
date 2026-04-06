namespace FluentCards;

/// <summary>
/// When invoked, opens the specified URL.
/// </summary>
public class OpenUrlAction : AdaptiveAction
{
    /// <summary>
    /// The URL to open.
    /// </summary>
    public string Url { get; set; } = string.Empty;
}
