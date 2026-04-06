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
    /// Builds and returns the configured RichTextBlock.
    /// </summary>
    /// <returns>The configured RichTextBlock instance.</returns>
    public RichTextBlock Build()
    {
        return _richText;
    }
}
