using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Card width options for Teams-specific rendering.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<TeamsCardWidth>))]
public enum TeamsCardWidth
{
    /// <summary>
    /// Full-width card rendering in Teams.
    /// </summary>
    Full
}
