using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Feedback settings for a Teams submit action.
/// </summary>
public class TeamsSubmitActionFeedback
{
    /// <summary>
    /// When true, hides the feedback UI after the submit action is invoked.
    /// </summary>
    [JsonPropertyName("hide")]
    public bool? Hide { get; set; }
}
