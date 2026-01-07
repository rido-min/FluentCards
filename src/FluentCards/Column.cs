using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Defines a container that is part of a ColumnSet.
/// </summary>
public class Column : AdaptiveElement
{
    /// <summary>
    /// The card elements to render inside the Column.
    /// </summary>
    public List<AdaptiveElement>? Items { get; set; }

    /// <summary>
    /// "auto", "stretch", a number representing relative width (e.g., "1", "2"), or a pixel width (e.g., "50px").
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Style hint for the column.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// Defines how the content should be vertically aligned within the column.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// Determines whether the element should bleed through its parent's padding.
    /// </summary>
    public bool? Bleed { get; set; }

    /// <summary>
    /// Specifies the background image for the column.
    /// </summary>
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Specifies the minimum height of the column (e.g., "100px").
    /// </summary>
    public string? MinHeight { get; set; }

    /// <summary>
    /// An Action that will be invoked when the column is tapped or selected.
    /// </summary>
    public AdaptiveAction? SelectAction { get; set; }
}
