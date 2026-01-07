using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Date picker input.
/// </summary>
public class InputDate : InputElement
{
    /// <summary>
    /// Minimum date (YYYY-MM-DD format).
    /// </summary>
    [JsonPropertyName("min")]
    public string? Min { get; set; }

    /// <summary>
    /// Maximum date (YYYY-MM-DD format).
    /// </summary>
    [JsonPropertyName("max")]
    public string? Max { get; set; }

    /// <summary>
    /// Placeholder text.
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Initial value (YYYY-MM-DD format).
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
