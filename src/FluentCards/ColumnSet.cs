using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// ColumnSet divides a region into Columns, allowing elements to sit side-by-side.
/// </summary>
public class ColumnSet : AdaptiveElement
{
    /// <summary>
    /// The array of Columns to divide the region into.
    /// </summary>
    public List<Column>? Columns { get; set; }

    /// <summary>
    /// Style hint for the ColumnSet.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// Determines whether the element should bleed through its parent's padding.
    /// </summary>
    public bool? Bleed { get; set; }

    /// <summary>
    /// Specifies the minimum height of the column set (e.g., "100px").
    /// </summary>
    public string? MinHeight { get; set; }

    /// <summary>
    /// Controls the horizontal alignment of the ColumnSet.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// An Action that will be invoked when the ColumnSet is tapped or selected.
    /// </summary>
    public AdaptiveAction? SelectAction { get; set; }
}
