using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Base class for all Adaptive Card elements.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TextBlock), "TextBlock")]
[JsonDerivedType(typeof(Image), "Image")]
[JsonDerivedType(typeof(Media), "Media")]
[JsonDerivedType(typeof(ImageSet), "ImageSet")]
[JsonDerivedType(typeof(RichTextBlock), "RichTextBlock")]
[JsonDerivedType(typeof(FactSet), "FactSet")]
[JsonDerivedType(typeof(ActionSet), "ActionSet")]
[JsonDerivedType(typeof(Table), "Table")]
[JsonDerivedType(typeof(Container), "Container")]
[JsonDerivedType(typeof(ColumnSet), "ColumnSet")]
[JsonDerivedType(typeof(InputText), "Input.Text")]
[JsonDerivedType(typeof(InputNumber), "Input.Number")]
[JsonDerivedType(typeof(InputDate), "Input.Date")]
[JsonDerivedType(typeof(InputTime), "Input.Time")]
[JsonDerivedType(typeof(InputToggle), "Input.Toggle")]
[JsonDerivedType(typeof(InputChoiceSet), "Input.ChoiceSet")]
public abstract class AdaptiveElement
{
    /// <summary>
    /// A unique identifier associated with the element.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// If false, the element will be hidden.
    /// </summary>
    public bool? IsVisible { get; set; }

    /// <summary>
    /// Controls the amount of spacing between this element and the preceding element.
    /// </summary>
    [JsonConverter(typeof(CamelCaseEnumConverter<Spacing>))]
    public Spacing? Spacing { get; set; }

    /// <summary>
    /// When true, draw a separating line at the top of the element.
    /// </summary>
    public bool? Separator { get; set; }

    /// <summary>
    /// Specifies the height of the element. Can be "auto" or "stretch".
    /// </summary>
    public string? Height { get; set; }

    /// <summary>
    /// Describes what to show when this element is unsupported. Can be "drop" or another element.
    /// </summary>
    [JsonConverter(typeof(FallbackConverter))]
    public object? Fallback { get; set; }

    /// <summary>
    /// A series of key/value pairs indicating features that the item requires with corresponding minimum version.
    /// </summary>
    public Dictionary<string, string>? Requires { get; set; }

    /// <summary>
    /// When true, the text should be displayed right-to-left.
    /// </summary>
    public bool? Rtl { get; set; }
}
