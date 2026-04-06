namespace FluentCards;

/// <summary>
/// Fluent builder for creating RefreshConfiguration instances.
/// </summary>
public class RefreshBuilder
{
    private readonly RefreshConfiguration _refresh = new();

    /// <summary>
    /// Configures the action to invoke when refreshing the card.
    /// </summary>
    /// <param name="configure">Action to configure the refresh action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RefreshBuilder WithAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _refresh.Action = builder.Build();
        return this;
    }

    /// <summary>
    /// Adds a user ID for which the refresh should trigger.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RefreshBuilder AddUserId(string userId)
    {
        _refresh.UserIds ??= new List<string>();
        _refresh.UserIds.Add(userId);
        return this;
    }

    /// <summary>
    /// Sets the expiration time for the card.
    /// </summary>
    /// <param name="expires">ISO 8601 datetime string indicating when the card expires.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RefreshBuilder WithExpires(string expires)
    {
        _refresh.Expires = expires;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured RefreshConfiguration.
    /// </summary>
    /// <returns>The configured RefreshConfiguration instance.</returns>
    public RefreshConfiguration Build()
    {
        return _refresh;
    }
}
