using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Displays a series of facts (key-value pairs).
/// </summary>
public class FactSet : AdaptiveElement
{
    /// <summary>
    /// The facts to display.
    /// </summary>
    [JsonPropertyName("facts")]
    public List<Fact>? Facts { get; set; }
}
