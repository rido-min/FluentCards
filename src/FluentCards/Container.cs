using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Containers group items together.
/// </summary>
public class Container : AdaptiveElement
{
    /// <summary>
    /// The card elements to render inside the Container.
    /// </summary>
    public List<AdaptiveElement>? Items { get; set; }

    /// <summary>
    /// Style hint for the container.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<ContainerStyle>))]
    public ContainerStyle? Style { get; set; }

    /// <summary>
    /// Defines how the content should be vertically aligned within the container.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<VerticalAlignment>))]
    public VerticalAlignment? VerticalContentAlignment { get; set; }

    /// <summary>
    /// Determines whether the element should bleed through its parent's padding.
    /// </summary>
    public bool? Bleed { get; set; }

    /// <summary>
    /// Specifies the background image for the container.
    /// </summary>
    public BackgroundImage? BackgroundImage { get; set; }

    /// <summary>
    /// Specifies the minimum height of the container (e.g., "100px").
    /// </summary>
    public string? MinHeight { get; set; }

    /// <summary>
    /// An Action that will be invoked when the container is tapped or selected.
    /// </summary>
    public AdaptiveAction? SelectAction { get; set; }
}
