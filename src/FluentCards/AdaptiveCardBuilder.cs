namespace FluentCards;

/// <summary>
/// Fluent builder for creating AdaptiveCard instances.
/// </summary>
public class AdaptiveCardBuilder
{
    private readonly AdaptiveCard _card = new();

    /// <summary>
    /// Creates a new instance of AdaptiveCardBuilder.
    /// </summary>
    /// <returns>A new AdaptiveCardBuilder instance.</returns>
    public static AdaptiveCardBuilder Create()
    {
        return new AdaptiveCardBuilder();
    }

    /// <summary>
    /// Sets the version of the AdaptiveCard schema.
    /// </summary>
    /// <param name="version">The schema version (e.g., "1.5").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder WithVersion(string version)
    {
        _card.Version = version;
        return this;
    }

    /// <summary>
    /// Sets the schema URL for the AdaptiveCard.
    /// </summary>
    /// <param name="schema">The schema URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder WithSchema(string? schema)
    {
        _card.Schema = schema;
        return this;
    }

    /// <summary>
    /// Adds a TextBlock to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the TextBlock.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddTextBlock(Action<TextBlockBuilder> configure)
    {
        var builder = new TextBlockBuilder();
        configure(builder);
        var textBlock = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(textBlock);

        return this;
    }

    /// <summary>
    /// Adds an action to the card's action bar.
    /// </summary>
    /// <param name="configure">Action to configure the AdaptiveAction.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        var action = builder.Build();

        _card.Actions ??= new List<AdaptiveAction>();
        _card.Actions.Add(action);

        return this;
    }

    /// <summary>
    /// Builds and returns the configured AdaptiveCard.
    /// </summary>
    /// <returns>The configured AdaptiveCard instance.</returns>
    public AdaptiveCard Build()
    {
        return _card;
    }
}
