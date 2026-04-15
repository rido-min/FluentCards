namespace FluentCards;

/// <summary>
/// Fluent builder for creating Table elements.
/// </summary>
public class TableBuilder
{
    private readonly Table _table = new() 
    { 
        Columns = new List<TableColumnDefinition>(),
        Rows = new List<TableRow>()
    };

    /// <summary>
    /// Sets the unique identifier for the table.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithId(string id)
    {
        _table.Id = id;
        return this;
    }

    /// <summary>
    /// Sets whether to treat the first row as a header.
    /// </summary>
    /// <param name="firstRowAsHeader">True to treat first row as header, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithFirstRowAsHeader(bool firstRowAsHeader = true)
    {
        _table.FirstRowAsHeader = firstRowAsHeader;
        return this;
    }

    /// <summary>
    /// Sets whether to show grid lines.
    /// </summary>
    /// <param name="showGridLines">True to show grid lines, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithShowGridLines(bool showGridLines = true)
    {
        _table.ShowGridLines = showGridLines;
        return this;
    }

    /// <summary>
    /// Sets the grid style.
    /// </summary>
    /// <param name="gridStyle">The grid style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithGridStyle(ContainerStyle gridStyle)
    {
        _table.GridStyle = gridStyle;
        return this;
    }

    /// <summary>
    /// Sets the default horizontal alignment for cell content.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithHorizontalCellContentAlignment(HorizontalAlignment alignment)
    {
        _table.HorizontalCellContentAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Sets the default vertical alignment for cell content.
    /// </summary>
    /// <param name="alignment">The vertical alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithVerticalCellContentAlignment(VerticalAlignment alignment)
    {
        _table.VerticalCellContentAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Adds a column definition to the table.
    /// </summary>
    /// <param name="column">The column definition to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder AddColumn(TableColumnDefinition column)
    {
        _table.Columns!.Add(column);
        return this;
    }

    /// <summary>
    /// Adds a row to the table.
    /// </summary>
    /// <param name="row">The row to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder AddRow(TableRow row)
    {
        _table.Rows!.Add(row);
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithSpacing(Spacing spacing)
    {
        _table.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithSeparator(bool separator = true)
    {
        _table.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithIsVisible(bool isVisible)
    {
        _table.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithHeight(string height)
    {
        _table.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithFallback(object fallback)
    {
        _table.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithRequires(string key, string version)
    {
        _table.Requires ??= new Dictionary<string, string>();
        _table.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TableBuilder WithRtl(bool rtl = true)
    {
        _table.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured Table.
    /// </summary>
    /// <returns>The configured Table instance.</returns>
    public Table Build()
    {
        return _table;
    }
}
