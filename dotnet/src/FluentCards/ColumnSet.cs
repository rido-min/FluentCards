using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays a set of columns arranged horizontally.
/// </summary>
public class ColumnSet : AdaptiveElement
{
    /// <summary>
    /// The columns in the set.
    /// </summary>
    [JsonPropertyName("columns")]
    public List<Column>? Columns { get; set; }

    /// <summary>
    /// The style of the column set.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// If true, allow content to bleed through the column set's padding.
    /// </summary>
    [JsonPropertyName("bleed")]
    public bool? Bleed { get; set; }

    /// <summary>
    /// Minimum height of the column set.
    /// </summary>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// The horizontal alignment of the column set.
    /// </summary>
    [JsonPropertyName("horizontalAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<HorizontalAlignment>))]
    public HorizontalAlignment? HorizontalAlignment { get; set; }

    /// <summary>
    /// Action to invoke when the column set is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }
}
