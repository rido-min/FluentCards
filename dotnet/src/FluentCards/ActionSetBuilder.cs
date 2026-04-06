namespace FluentCards;

/// <summary>
/// Fluent builder for creating ActionSet elements.
/// </summary>
public class ActionSetBuilder
{
    private readonly ActionSet _actionSet = new() { Actions = new List<AdaptiveAction>() };

    /// <summary>
    /// Sets the unique identifier for the action set.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithId(string id)
    {
        _actionSet.Id = id;
        return this;
    }

    /// <summary>
    /// Adds an action to the action set.
    /// </summary>
    /// <param name="configure">Action to configure the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder AddAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _actionSet.Actions!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Builds and returns the configured ActionSet.
    /// </summary>
    /// <returns>The configured ActionSet instance.</returns>
    public ActionSet Build()
    {
        return _actionSet;
    }
}
