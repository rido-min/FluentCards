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
    /// Builds and returns the configured Table.
    /// </summary>
    /// <returns>The configured Table instance.</returns>
    public Table Build()
    {
        return _table;
    }
}
