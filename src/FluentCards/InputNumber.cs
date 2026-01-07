using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Allows users to enter a number.
/// </summary>
public class InputNumber : AdaptiveElement
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
    /// The default value for the input.
    /// </summary>
    [JsonPropertyName("value")]
    public double? Value { get; set; }

    /// <summary>
    /// The minimum value allowed.
    /// </summary>
    [JsonPropertyName("min")]
    public double? Min { get; set; }

    /// <summary>
    /// The maximum value allowed.
    /// </summary>
    [JsonPropertyName("max")]
    public double? Max { get; set; }

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
}
