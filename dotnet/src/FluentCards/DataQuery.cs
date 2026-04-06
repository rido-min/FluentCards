using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a data query for dynamic choice sets (typeahead/filtered).
/// </summary>
/// <remarks>Added in Adaptive Cards 1.6.</remarks>
public class DataQuery
{
    /// <summary>
    /// Must be "Data.Query".
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Data.Query";

    /// <summary>
    /// The dataset to query.
    /// </summary>
    [JsonPropertyName("dataset")]
    public string? Dataset { get; set; }

    /// <summary>
    /// The number of results to return.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    /// <summary>
    /// The number of results to skip.
    /// </summary>
    [JsonPropertyName("skip")]
    public int? Skip { get; set; }
}
