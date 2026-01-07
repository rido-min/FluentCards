using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Allows users to enter a time.
/// </summary>
public class InputTime : AdaptiveElement
{
    /// <summary>
    /// The label to display for this input.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Placeholder text to display when the input is empty.
    /// </summary>
    [JsonPropertyName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The default value for the input (ISO 8601 time format).
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }

    /// <summary>
    /// The minimum time allowed (ISO 8601 time format).
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
    /// The maximum time allowed (ISO 8601 time format).
    /// Maximum time (HH:MM format).
    /// </summary>
    [JsonPropertyName("max")]
    public string? Max { get; set; }

    /// <summary>
    /// If true, this input is required.
    /// </summary>
    [JsonPropertyName("isRequired")]
    public bool? IsRequired { get; set; }

    /// <summary>
    /// Error message to display when validation fails.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
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
