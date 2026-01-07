namespace FluentCards;

/// <summary>
/// Fluent builder for creating AdaptiveAction elements.
/// </summary>
public class ActionBuilder
{
    private AdaptiveAction? _action;

    /// <summary>
    /// Creates an OpenUrl action.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder OpenUrl(string url, string? title = null)
    {
        _action = new OpenUrlAction
        {
            Url = url,
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Sets the unique identifier for the action.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithId(string id)
    {
        if (_action != null)
        {
            _action.Id = id;
        }
        return this;
    }

    /// <summary>
    /// Sets the title for the action.
    /// </summary>
    /// <param name="title">The action title.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithTitle(string title)
    {
        if (_action != null)
        {
            _action.Title = title;
        }
        return this;
    }

    /// <summary>
    /// Builds and returns the configured AdaptiveAction.
    /// </summary>
    /// <returns>The configured AdaptiveAction instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no action type has been specified.</exception>
    public AdaptiveAction Build()
    {
        if (_action == null)
        {
            throw new InvalidOperationException("No action type has been specified. Call OpenUrl() or another action method first.");
        }
        return _action;
    }
}
