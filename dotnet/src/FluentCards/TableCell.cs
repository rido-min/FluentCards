using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a cell in a table.
/// </summary>
/// <remarks>Added in Adaptive Cards 1.5.</remarks>
public class TableCell
{
    /// <summary>
    /// The type discriminator for TableCell.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "TableCell";

    /// <summary>
    /// The elements to display in this cell.
    /// </summary>
    [JsonPropertyName("items")]
    public List<AdaptiveElement>? Items { get; set; }

    /// <summary>
    /// Action to invoke when the cell is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }

    /// <summary>
    /// The style of this cell.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// The vertical alignment of content in this cell.
    /// </summary>
    [JsonPropertyName("verticalContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// If true, allow the cell content to bleed to the edge.
    /// </summary>
    [JsonPropertyName("bleed")]
    public bool? Bleed { get; set; }

    /// <summary>
    /// Background image for the cell.
    /// </summary>
    [JsonPropertyName("backgroundImage")]
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Minimum height of the cell.
    /// </summary>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// If true, render the cell content right-to-left.
    /// </summary>
    [JsonPropertyName("rtl")]
    public bool? Rtl { get; set; }
}
