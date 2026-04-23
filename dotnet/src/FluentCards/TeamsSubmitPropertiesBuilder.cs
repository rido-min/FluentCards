using System.Text.Json.Serialization;

namespace FluentCards;

/// <summary>
/// Fluent builder for <see cref="TeamsSubmitActionProperties"/>.
/// </summary>
public class TeamsSubmitPropertiesBuilder
{
    private readonly TeamsSubmitActionProperties _properties = new();

    /// <summary>
    /// Hides the feedback UI after the submit action is invoked.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsSubmitPropertiesBuilder WithFeedbackHidden()
    {
        _properties.Feedback ??= new TeamsSubmitActionFeedback();
        _properties.Feedback.Hide = true;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="TeamsSubmitActionProperties"/>.
    /// </summary>
    /// <returns>The configured Teams submit action properties.</returns>
    public TeamsSubmitActionProperties Build()
    {
        return _properties;
    }
}
