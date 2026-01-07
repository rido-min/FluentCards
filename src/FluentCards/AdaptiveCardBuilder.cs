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
    /// Adds an Image to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the Image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddImage(Action<ImageBuilder> configure)
    {
        var builder = new ImageBuilder();
        configure(builder);
        var image = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(image);

        return this;
    }

    /// <summary>
    /// Adds a Container to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the Container.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddContainer(Action<ContainerBuilder> configure)
    {
        var builder = new ContainerBuilder();
        configure(builder);
        var container = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(container);

        return this;
    }

    /// <summary>
    /// Adds a ColumnSet to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the ColumnSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddColumnSet(Action<ColumnSetBuilder> configure)
    {
        var builder = new ColumnSetBuilder();
        configure(builder);
        var columnSet = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(columnSet);

        return this;
    }

    /// <summary>
    /// Adds a FactSet to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the FactSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddFactSet(Action<FactSetBuilder> configure)
    {
        var builder = new FactSetBuilder();
        configure(builder);
        var factSet = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(factSet);

        return this;
    }

    /// <summary>
    /// Adds a RichTextBlock to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the RichTextBlock.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddRichTextBlock(Action<RichTextBlockBuilder> configure)
    {
        var builder = new RichTextBlockBuilder();
        configure(builder);
        var richTextBlock = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(richTextBlock);

        return this;
    }

    /// <summary>
    /// Adds an ActionSet to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the ActionSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddActionSet(Action<ActionSetBuilder> configure)
    {
        var builder = new ActionSetBuilder();
        configure(builder);
        var actionSet = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(actionSet);

        return this;
    }

    /// <summary>
    /// Adds a Media element to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the Media.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddMedia(Action<MediaBuilder> configure)
    {
        var builder = new MediaBuilder();
        configure(builder);
        var media = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(media);

        return this;
    }

    /// <summary>
    /// Adds an ImageSet to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the ImageSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddImageSet(Action<ImageSetBuilder> configure)
    {
        var builder = new ImageSetBuilder();
        configure(builder);
        var imageSet = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(imageSet);

        return this;
    }

    /// <summary>
    /// Adds a Table to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the Table.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddTable(Action<TableBuilder> configure)
    {
        var builder = new TableBuilder();
        configure(builder);
        var table = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(table);

        return this;
    }

    /// <summary>
    /// Adds an InputText to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputText.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputText(Action<InputTextBuilder> configure)
    {
        var builder = new InputTextBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

        return this;
    }

    /// <summary>
    /// Adds an InputNumber to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputNumber.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputNumber(Action<InputNumberBuilder> configure)
    {
        var builder = new InputNumberBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

        return this;
    }

    /// <summary>
    /// Adds an InputDate to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputDate.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputDate(Action<InputDateBuilder> configure)
    {
        var builder = new InputDateBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

        return this;
    }

    /// <summary>
    /// Adds an InputTime to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputTime.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputTime(Action<InputTimeBuilder> configure)
    {
        var builder = new InputTimeBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

        return this;
    }

    /// <summary>
    /// Adds an InputToggle to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputToggle.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputToggle(Action<InputToggleBuilder> configure)
    {
        var builder = new InputToggleBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

        return this;
    }

    /// <summary>
    /// Adds an InputChoiceSet to the card's body.
    /// </summary>
    /// <param name="configure">Action to configure the InputChoiceSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder AddInputChoiceSet(Action<InputChoiceSetBuilder> configure)
    {
        var builder = new InputChoiceSetBuilder();
        configure(builder);
        var input = builder.Build();

        _card.Body ??= new List<AdaptiveElement>();
        _card.Body.Add(input);

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
    /// Configures the refresh configuration for the card.
    /// </summary>
    /// <param name="configure">Action to configure the refresh configuration.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder WithRefresh(Action<RefreshBuilder> configure)
    {
        var builder = new RefreshBuilder();
        configure(builder);
        _card.Refresh = builder.Build();

        return this;
    }

    /// <summary>
    /// Configures the authentication configuration for the card.
    /// </summary>
    /// <param name="configure">Action to configure the authentication configuration.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder WithAuthentication(Action<AuthenticationBuilder> configure)
    {
        var builder = new AuthenticationBuilder();
        configure(builder);
        _card.Authentication = builder.Build();

        return this;
    }

    /// <summary>
    /// Sets the metadata web URL for the card.
    /// </summary>
    /// <param name="webUrl">The web URL for the card metadata.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AdaptiveCardBuilder WithMetadata(string webUrl)
    {
        _card.Metadata = new CardMetadata { WebUrl = webUrl };

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
