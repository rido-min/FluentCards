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
    /// Builds and returns the configured ColumnSet.
    /// </summary>
    /// <returns>The configured ColumnSet instance.</returns>
    public ColumnSet Build()
    {
        return _columnSet;
    }
}
