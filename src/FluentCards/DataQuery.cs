using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Represents a data query for dynamic choice sets (typeahead/filtered).
/// </summary>
public class DataQuery
{
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
