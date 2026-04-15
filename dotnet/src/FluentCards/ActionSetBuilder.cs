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
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithSpacing(Spacing spacing)
    {
        _actionSet.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithSeparator(bool separator = true)
    {
        _actionSet.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithIsVisible(bool isVisible)
    {
        _actionSet.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithHeight(string height)
    {
        _actionSet.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithFallback(object fallback)
    {
        _actionSet.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithRequires(string key, string version)
    {
        _actionSet.Requires ??= new Dictionary<string, string>();
        _actionSet.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionSetBuilder WithRtl(bool rtl = true)
    {
        _actionSet.Rtl = rtl;
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
