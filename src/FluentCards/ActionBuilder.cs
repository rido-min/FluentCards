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
    /// Creates a Submit action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder Submit(string? title = null)
    {
        _action = new SubmitAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates a ShowCard action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder ShowCard(string? title = null)
    {
        _action = new ShowCardAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates a ToggleVisibility action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder ToggleVisibility(string? title = null)
    {
        _action = new ToggleVisibilityAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates an Execute action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder Execute(string? title = null)
    {
        _action = new ExecuteAction
        {
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
    /// Sets the icon URL for the action.
    /// </summary>
    /// <param name="iconUrl">The icon URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithIconUrl(string iconUrl)
    {
        if (_action != null)
        {
            _action.IconUrl = iconUrl;
        }
        return this;
    }

    /// <summary>
    /// Sets the style for the action.
    /// </summary>
    /// <param name="style">The action style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithStyle(ActionStyle style)
    {
        if (_action != null)
        {
            _action.Style = style;
        }
        return this;
    }

    /// <summary>
    /// Sets whether the action is enabled.
    /// </summary>
    /// <param name="isEnabled">True if enabled, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithIsEnabled(bool isEnabled)
    {
        if (_action != null)
        {
            _action.IsEnabled = isEnabled;
        }
        return this;
    }

    /// <summary>
    /// Sets the tooltip for the action.
    /// </summary>
    /// <param name="tooltip">The tooltip text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithTooltip(string tooltip)
    {
        if (_action != null)
        {
            _action.Tooltip = tooltip;
        }
        return this;
    }

    /// <summary>
    /// Sets the fallback behavior for the action.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another action).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithFallback(object fallback)
    {
        if (_action != null)
        {
            _action.Fallback = fallback;
        }
        return this;
    }

    /// <summary>
    /// Sets the mode for the action (primary or secondary/overflow).
    /// </summary>
    /// <param name="mode">The action mode.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithMode(ActionMode mode)
    {
        if (_action != null)
        {
            _action.Mode = mode;
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
            throw new InvalidOperationException("No action type has been specified. Call OpenUrl(), Submit(), or another action method first.");
        }
        return _action;
    }
}
