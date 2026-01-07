using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines a column in a table.
/// </summary>
public class TableColumnDefinition
{
    /// <summary>
    /// The width of the column (number or "auto").
    /// </summary>
    [JsonPropertyName("width")]
    public string? Width { get; set; }

    /// <summary>
    /// The horizontal alignment for cells in this column.
    /// </summary>
    [JsonPropertyName("horizontalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalCellContentAlignment { get; set; }

    /// <summary>
    /// The vertical alignment for cells in this column.
    /// </summary>
    [JsonPropertyName("verticalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalCellContentAlignment { get; set; }
}
