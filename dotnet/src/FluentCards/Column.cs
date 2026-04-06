using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a column within a ColumnSet.
/// </summary>
public class Column
{
    /// <summary>
    /// The type discriminator for Column.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "Column";

    /// <summary>
    /// The items to display in the column.
    /// </summary>
    [JsonPropertyName("items")]
    public List<AdaptiveElement>? Items { get; set; }

    /// <summary>
    /// The width of the column. Can be "auto", "stretch", a pixel value (e.g., "50px"), or a weight (e.g., "2").
    /// </summary>
    [JsonPropertyName("width")]
    public string? Width { get; set; }

    /// <summary>
    /// The style of the column.
    /// </summary>
    [JsonPropertyName("style")]
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// The vertical alignment of content within the column.
    /// </summary>
    [JsonPropertyName("verticalContentAlignment")]
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// If true, allow content to bleed through the column's padding.
    /// </summary>
    [JsonPropertyName("bleed")]
    public bool? Bleed { get; set; }

    /// <summary>
    /// Minimum height of the column.
    /// </summary>
    [JsonPropertyName("minHeight")]
    public string? MinHeight { get; set; }

    /// <summary>
    /// Background image for the column.
    /// </summary>
    [JsonPropertyName("backgroundImage")]
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Action to invoke when the column is selected.
    /// </summary>
    [JsonPropertyName("selectAction")]
    public AdaptiveAction? SelectAction { get; set; }

    /// <summary>
    /// A unique identifier associated with the column.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// If false, the column will be hidden.
    /// </summary>
    [JsonPropertyName("isVisible")]
    public bool? IsVisible { get; set; }

    /// <summary>
    /// Controls the amount of spacing between this column and the preceding column.
    /// </summary>
    [JsonPropertyName("spacing")]
    [JsonConverter(typeof(CamelCaseEnumConverter<Spacing>))]
    public Spacing? Spacing { get; set; }

    /// <summary>
    /// When true, draw a separating line at the left of the column.
    /// </summary>
    [JsonPropertyName("separator")]
    public bool? Separator { get; set; }

    /// <summary>
    /// Specifies the height of the column. Can be "auto" or "stretch".
    /// </summary>
    [JsonPropertyName("height")]
    public string? Height { get; set; }

    /// <summary>
    /// Describes what to show when this column is unsupported.
    /// </summary>
    [JsonPropertyName("fallback")]
    [JsonConverter(typeof(FallbackConverter))]
    public object? Fallback { get; set; }

    /// <summary>
    /// A series of key/value pairs indicating features that the column requires.
    /// </summary>
    [JsonPropertyName("requires")]
    public Dictionary<string, string>? Requires { get; set; }

    /// <summary>
    /// When true, content in this column should be presented right to left.
    /// </summary>
    [JsonPropertyName("rtl")]
    public bool? Rtl { get; set; }
}
