using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Base class for all input elements in an Adaptive Card.
/// </summary>
public abstract class InputElement : AdaptiveElement
{
    /// <summary>
    /// Unique identifier for the input.
    /// </summary>
    [JsonPropertyName("id")]
    public new string Id { get; set; } = string.Empty;

    /// <summary>
    /// Label for the input.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Whether input is required.
    /// </summary>
    [JsonPropertyName("isRequired")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsRequired { get; set; }

    /// <summary>
    /// Error message for validation failure.
    /// </summary>
    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; }
}
