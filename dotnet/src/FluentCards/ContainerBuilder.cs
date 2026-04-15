namespace FluentCards;

/// <summary>
/// Fluent builder for creating Container elements.
/// </summary>
public class ContainerBuilder
{
    private readonly Container _container = new() { Items = new List<AdaptiveElement>() };

    /// <summary>
    /// Sets the unique identifier for the container.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithId(string id)
    {
        _container.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the style of the container.
    /// </summary>
    /// <param name="style">The container style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithStyle(ContainerStyle style)
    {
        _container.Style = style;
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment of content within the container.
    /// </summary>
    /// <param name="alignment">The vertical alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithVerticalContentAlignment(VerticalAlignment alignment)
    {
        _container.VerticalContentAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Enables or disables bleed for the container.
    /// </summary>
    /// <param name="bleed">True to enable bleed, false to disable.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithBleed(bool bleed = true)
    {
        _container.Bleed = bleed;
        return this;
    }

    /// <summary>
    /// Sets the minimum height of the container.
    /// </summary>
    /// <param name="minHeight">The minimum height.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithMinHeight(string minHeight)
    {
        _container.MinHeight = minHeight;
        return this;
    }

    /// <summary>
    /// Configures the background image for the container.
    /// </summary>
    /// <param name="configure">Action to configure the background image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithBackgroundImage(Action<BackgroundImageBuilder> configure)
    {
        var builder = new BackgroundImageBuilder();
        configure(builder);
        _container.BackgroundImage = builder.Build();
        return this;
    }

    /// <summary>
    /// Configures the action to invoke when the container is selected.
    /// </summary>
    /// <param name="configure">Action to configure the select action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithSelectAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _container.SelectAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Adds a TextBlock to the container.
    /// </summary>
    /// <param name="configure">Action to configure the TextBlock.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddTextBlock(Action<TextBlockBuilder> configure)
    {
        var builder = new TextBlockBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an Image to the container.
    /// </summary>
    /// <param name="configure">Action to configure the Image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddImage(Action<ImageBuilder> configure)
    {
        var builder = new ImageBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a nested Container to the container.
    /// </summary>
    /// <param name="configure">Action to configure the Container.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddContainer(Action<ContainerBuilder> configure)
    {
        var builder = new ContainerBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a ColumnSet to the container.
    /// </summary>
    /// <param name="configure">Action to configure the ColumnSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddColumnSet(Action<ColumnSetBuilder> configure)
    {
        var builder = new ColumnSetBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a FactSet to the container.
    /// </summary>
    /// <param name="configure">Action to configure the FactSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddFactSet(Action<FactSetBuilder> configure)
    {
        var builder = new FactSetBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a RichTextBlock to the container.
    /// </summary>
    /// <param name="configure">Action to configure the RichTextBlock.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddRichTextBlock(Action<RichTextBlockBuilder> configure)
    {
        var builder = new RichTextBlockBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an ActionSet to the container.
    /// </summary>
    /// <param name="configure">Action to configure the ActionSet.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddActionSet(Action<ActionSetBuilder> configure)
    {
        var builder = new ActionSetBuilder();
        configure(builder);
        _container.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an AdaptiveElement to the container.
    /// </summary>
    /// <param name="element">The element to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder AddElement(AdaptiveElement element)
    {
        _container.Items!.Add(element);
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithSpacing(Spacing spacing)
    {
        _container.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithSeparator(bool separator = true)
    {
        _container.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithIsVisible(bool isVisible)
    {
        _container.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithHeight(string height)
    {
        _container.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithFallback(object fallback)
    {
        _container.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithRequires(string key, string version)
    {
        _container.Requires ??= new Dictionary<string, string>();
        _container.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ContainerBuilder WithRtl(bool rtl = true)
    {
        _container.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured Container.
    /// </summary>
    /// <returns>The configured Container instance.</returns>
    public Container Build()
    {
        return _container;
    }
}
