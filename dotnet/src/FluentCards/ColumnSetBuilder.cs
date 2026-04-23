namespace FluentCards;

/// <summary>
/// Fluent builder for creating ColumnSet elements.
/// </summary>
public class ColumnSetBuilder
{
    private readonly ColumnSet _columnSet = new() { Columns = new List<Column>() };

    /// <summary>
    /// Sets the unique identifier for the column set.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithId(string id)
    {
        _columnSet.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the style of the column set.
    /// </summary>
    /// <param name="style">The container style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithStyle(ContainerStyle style)
    {
        _columnSet.Style = style;
        return this;
    }

    /// <summary>
    /// Enables or disables bleed for the column set.
    /// </summary>
    /// <param name="bleed">True to enable bleed, false to disable.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithBleed(bool bleed = true)
    {
        _columnSet.Bleed = bleed;
        return this;
    }

    /// <summary>
    /// Sets the minimum height of the column set.
    /// </summary>
    /// <param name="minHeight">The minimum height.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithMinHeight(string minHeight)
    {
        _columnSet.MinHeight = minHeight;
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of the column set.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithHorizontalAlignment(HorizontalAlignment alignment)
    {
        _columnSet.HorizontalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Configures the action to invoke when the column set is selected.
    /// </summary>
    /// <param name="configure">Action to configure the select action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithSelectAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _columnSet.SelectAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Adds a column to the column set.
    /// </summary>
    /// <param name="configure">Action to configure the column.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder AddColumn(Action<ColumnBuilder> configure)
    {
        var builder = new ColumnBuilder();
        configure(builder);
        _columnSet.Columns!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a column with a specific width to the column set.
    /// </summary>
    /// <param name="width">The width of the column.</param>
    /// <param name="configure">Action to configure the column.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder AddColumn(string width, Action<ColumnBuilder> configure)
    {
        var builder = new ColumnBuilder().WithWidth(width);
        configure(builder);
        _columnSet.Columns!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithSpacing(Spacing spacing)
    {
        _columnSet.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithSeparator(bool separator = true)
    {
        _columnSet.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithIsVisible(bool isVisible)
    {
        _columnSet.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithHeight(string height)
    {
        _columnSet.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithFallback(object fallback)
    {
        _columnSet.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithRequires(string key, string version)
    {
        _columnSet.Requires ??= new Dictionary<string, string>();
        _columnSet.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnSetBuilder WithRtl(bool rtl = true)
    {
        _columnSet.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured ColumnSet.
    /// </summary>
    /// <returns>The configured ColumnSet instance.</returns>
    public ColumnSet Build()
    {
        return _columnSet;
    }
}
