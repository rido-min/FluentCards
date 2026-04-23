namespace FluentCards;

/// <summary>
/// Fluent builder for creating RichTextBlock elements.
/// </summary>
public class RichTextBlockBuilder
{
    private readonly RichTextBlock _richText = new() { Inlines = new List<object>() };

    /// <summary>
    /// Sets the unique identifier for the rich text block.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithId(string id)
    {
        _richText.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of the text.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithHorizontalAlignment(HorizontalAlignment alignment)
    {
        _richText.HorizontalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Adds plain text to the rich text block.
    /// </summary>
    /// <param name="text">The text to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder AddText(string text)
    {
        _richText.Inlines!.Add(text);
        return this;
    }

    /// <summary>
    /// Adds a TextRun to the rich text block.
    /// </summary>
    /// <param name="configure">Action to configure the TextRun.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder AddTextRun(Action<TextRunBuilder> configure)
    {
        var builder = new TextRunBuilder();
        configure(builder);
        _richText.Inlines!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithSpacing(Spacing spacing)
    {
        _richText.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithSeparator(bool separator = true)
    {
        _richText.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithIsVisible(bool isVisible)
    {
        _richText.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithHeight(string height)
    {
        _richText.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithFallback(object fallback)
    {
        _richText.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithRequires(string key, string version)
    {
        _richText.Requires ??= new Dictionary<string, string>();
        _richText.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public RichTextBlockBuilder WithRtl(bool rtl = true)
    {
        _richText.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured RichTextBlock.
    /// </summary>
    /// <returns>The configured RichTextBlock instance.</returns>
    public RichTextBlock Build()
    {
        return _richText;
    }
}
