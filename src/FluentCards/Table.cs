using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays tabular data.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.5.</remarks>
public class Table: AdaptiveElement
{
    /// <summary>
    /// The column definitions for the table.
    /// </summary>
    [JsonPropertyName("columns")]
    public List<TableColumnDefinition>? Columns { get; set; }

    /// <summary>
    /// The rows in the table.
    /// </summary>
    [JsonPropertyName("rows")]
    public List<TableRow>? Rows { get; set; }

    /// <summary>
    /// If true, treat the first row as a header.
    /// </summary>
    [JsonPropertyName("firstRowAsHeader")]
    public bool? FirstRowAsHeader { get; set; }

    /// <summary>
    /// If true, show grid lines.
    /// </summary>
    [JsonPropertyName("showGridLines")]
    public bool? ShowGridLines { get; set; }

    /// <summary>
    /// The style for grid lines.
    /// </summary>
    [JsonPropertyName("gridStyle")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? GridStyle { get; set; }

    /// <summary>
    /// Default horizontal alignment for cell content.
    /// </summary>
    [JsonPropertyName("horizontalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalCellContentAlignment { get; set; }

    /// <summary>
    /// Default vertical alignment for cell content.
    /// </summary>
    [JsonPropertyName("verticalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalCellContentAlignment { get; set; }
}
