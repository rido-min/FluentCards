using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Time picker input.
/// </summary>
public class InputTime : InputElement
{
    /// <summary>
    /// Minimum time (HH:MM format).
    /// </summary>
    [JsonPropertyName("min")]
    public string? Min { get; set; }

    /// <summary>
    /// Maximum time (HH:MM format).
    /// </summary>
    [JsonPropertyName("max")]
    public string? Max { get; set; }

    /// <summary>
    /// Placeholder text.
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Initial value (HH:MM format).
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
