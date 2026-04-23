namespace FluentCards;

/// <summary>
/// Fluent builder for creating FactSet elements.
/// </summary>
public class FactSetBuilder
{
    private readonly FactSet _factSet = new() { Facts = new List<Fact>() };

    /// <summary>
    /// Sets the unique identifier for the fact set.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithId(string id)
    {
        _factSet.Id = id;
        return this;
    }

    /// <summary>
    /// Adds a fact to the fact set.
    /// </summary>
    /// <param name="title">The title of the fact.</param>
    /// <param name="value">The value of the fact.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder AddFact(string title, string value)
    {
        _factSet.Facts!.Add(new Fact { Title = title, Value = value });
        return this;
    }

    /// <summary>
    /// Adds a fact to the fact set.
    /// </summary>
    /// <param name="fact">The fact to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder AddFact(Fact fact)
    {
        _factSet.Facts!.Add(fact);
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithSpacing(Spacing spacing)
    {
        _factSet.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithSeparator(bool separator = true)
    {
        _factSet.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithIsVisible(bool isVisible)
    {
        _factSet.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithHeight(string height)
    {
        _factSet.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithFallback(object fallback)
    {
        _factSet.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithRequires(string key, string version)
    {
        _factSet.Requires ??= new Dictionary<string, string>();
        _factSet.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public FactSetBuilder WithRtl(bool rtl = true)
    {
        _factSet.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured FactSet.
    /// </summary>
    /// <returns>The configured FactSet instance.</returns>
    public FactSet Build()
    {
        return _factSet;
    }
}
