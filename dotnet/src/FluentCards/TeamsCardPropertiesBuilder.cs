namespace FluentCards;

/// <summary>
/// Fluent builder for <see cref="TeamsCardProperties"/>.
/// </summary>
public class TeamsCardPropertiesBuilder
{
    private readonly TeamsCardProperties _properties = new();

    /// <summary>
    /// Sets the card width to <see cref="TeamsCardWidth.Full"/>.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsCardPropertiesBuilder WithFullWidth()
    {
        _properties.Width = TeamsCardWidth.Full;
        return this;
    }

    /// <summary>
    /// Adds an @mention entity. The mention text is auto-generated as
    /// <c>&lt;at&gt;displayName&lt;/at&gt;</c>.
    /// </summary>
    /// <param name="displayName">The user's display name (e.g. "John Doe").</param>
    /// <param name="userId">The Teams user ID (e.g. "29:1241241...").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TeamsCardPropertiesBuilder AddMention(string displayName, string userId)
    {
        _properties.Entities ??= new List<Mention>();
        _properties.Entities.Add(new Mention
        {
            Text = $"<at>{displayName}</at>",
            Mentioned = new MentionedEntity
            {
                Id = userId,
                Name = displayName
            }
        });
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="TeamsCardProperties"/>.
    /// </summary>
    /// <returns>The configured Teams card properties.</returns>
    public TeamsCardProperties Build()
    {
        return _properties;
    }
}
