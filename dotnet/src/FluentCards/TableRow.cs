using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a row in a table.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.5.</remarks>
public class TableRow
{
    /// <summary>
    /// The type discriminator for TableRow.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "TableRow";

    /// <summary>
    /// The cells in this row.
    /// </summary>
    [JsonPropertyName("cells")]
    public List<TableCell>? Cells { get; set; }

    /// <summary>
    /// The style of this row.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// The horizontal alignment for cells in this row.
    /// </summary>
    [JsonPropertyName("horizontalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalCellContentAlignment { get; set; }

    /// <summary>
    /// The vertical alignment for cells in this row.
    /// </summary>
    [JsonPropertyName("verticalCellContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalCellContentAlignment { get; set; }
}
