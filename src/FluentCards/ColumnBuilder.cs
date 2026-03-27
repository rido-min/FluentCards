namespace FluentCards;

/// <summary>
/// Fluent builder for creating Column elements.
/// </summary>
public class ColumnBuilder
{
    private readonly Column _column = new() { Items = new List<AdaptiveElement>() };

    /// <summary>
    /// Sets the width of the column.
    /// </summary>
    /// <param name="width">The column width (e.g., "auto", "stretch", "50px", or "2").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithWidth(string width)
    {
        _column.Width = width;
        return this;
    }

    /// <summary>
    /// Sets the unique identifier for the column.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithId(string id)
    {
        _column.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the style of the column.
    /// </summary>
    /// <param name="style">The container style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithStyle(ContainerStyle style)
    {
        _column.Style = style;
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment of content within the column.
    /// </summary>
    /// <param name="alignment">The vertical alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithVerticalContentAlignment(VerticalAlignment alignment)
    {
        _column.VerticalContentAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Enables or disables bleed for the column.
    /// </summary>
    /// <param name="bleed">True to enable bleed, false to disable.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithBleed(bool bleed = true)
    {
        _column.Bleed = bleed;
        return this;
    }

    /// <summary>
    /// Sets the minimum height of the column.
    /// </summary>
    /// <param name="minHeight">The minimum height.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithMinHeight(string minHeight)
    {
        _column.MinHeight = minHeight;
        return this;
    }

    /// <summary>
    /// Configures the background image for the column.
    /// </summary>
    /// <param name="configure">Action to configure the background image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithBackgroundImage(Action<BackgroundImageBuilder> configure)
    {
        var builder = new BackgroundImageBuilder();
        configure(builder);
        _column.BackgroundImage = builder.Build();
        return this;
    }

    /// <summary>
    /// Configures the action to invoke when the column is selected.
    /// </summary>
    /// <param name="configure">Action to configure the select action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithSelectAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _column.SelectAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Adds a TextBlock to the column.
    /// </summary>
    /// <param name="configure">Action to configure the TextBlock.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder AddTextBlock(Action<TextBlockBuilder> configure)
    {
        var builder = new TextBlockBuilder();
        configure(builder);
        _column.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an Image to the column.
    /// </summary>
    /// <param name="configure">Action to configure the Image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder AddImage(Action<ImageBuilder> configure)
    {
        var builder = new ImageBuilder();
        configure(builder);
        _column.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a Container to the column.
    /// </summary>
    /// <param name="configure">Action to configure the Container.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder AddContainer(Action<ContainerBuilder> configure)
    {
        var builder = new ContainerBuilder();
        configure(builder);
        _column.Items!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Sets the fallback behavior for the column.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithFallback(object fallback)
    {
        _column.Fallback = fallback;
        return this;
    }

    /// <summary>
    /// Sets the height of the column.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithHeight(string height)
    {
        _column.Height = height;
        return this;
    }

    /// <summary>
    /// Sets whether a separator line is drawn at the left of the column.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithSeparator(bool separator = true)
    {
        _column.Separator = separator;
        return this;
    }

    /// <summary>
    /// Sets the spacing between this column and the preceding column.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithSpacing(Spacing spacing)
    {
        _column.Spacing = spacing;
        return this;
    }

    /// <summary>
    /// Sets whether the column is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithIsVisible(bool isVisible)
    {
        _column.IsVisible = isVisible;
        return this;
    }

    /// <summary>
    /// Sets the feature requirements for the column.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithRequires(string key, string version)
    {
        _column.Requires ??= new Dictionary<string, string>();
        _column.Requires[key] = version;
        return this;
    }

    /// <summary>
    /// Sets whether content in this column should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ColumnBuilder WithRtl(bool rtl = true)
    {
        _column.Rtl = rtl;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured Column.
    /// </summary>
    /// <returns>The configured Column instance.</returns>
    public Column Build()
    {
        return _column;
    }
}
