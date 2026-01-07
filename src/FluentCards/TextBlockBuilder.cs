namespace FluentCards;

/// <summary>
/// Fluent builder for creating TextBlock elements.
/// </summary>
public class TextBlockBuilder
{
    private readonly TextBlock _textBlock = new();

    /// <summary>
    /// Sets the unique identifier for the TextBlock.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithId(string id)
    {
        _textBlock.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the text to display.
    /// </summary>
    /// <param name="text">The text content.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithText(string text)
    {
        _textBlock.Text = text;
        return this;
    }

    /// <summary>
    /// Sets the size of the text.
    /// </summary>
    /// <param name="size">The text size.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithSize(TextSize size)
    {
        _textBlock.Size = size;
        return this;
    }

    /// <summary>
    /// Sets the weight (boldness) of the text.
    /// </summary>
    /// <param name="weight">The text weight.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithWeight(TextWeight weight)
    {
        _textBlock.Weight = weight;
        return this;
    }

    /// <summary>
    /// Sets the color of the text.
    /// </summary>
    /// <param name="color">The text color.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithColor(TextColor color)
    {
        _textBlock.Color = color;
        return this;
    }

    /// <summary>
    /// Sets whether the text should wrap.
    /// </summary>
    /// <param name="wrap">True to allow text wrapping, false to clip text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithWrap(bool wrap)
    {
        _textBlock.Wrap = wrap;
        return this;
    }

    /// <summary>
    /// Sets the maximum number of lines to display.
    /// </summary>
    /// <param name="maxLines">The maximum number of lines.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithMaxLines(int maxLines)
    {
        _textBlock.MaxLines = maxLines;
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of the text.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextBlockBuilder WithHorizontalAlignment(HorizontalAlignment alignment)
    {
        _textBlock.HorizontalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured TextBlock.
    /// </summary>
    /// <returns>The configured TextBlock instance.</returns>
    public TextBlock Build()
    {
        return _textBlock;
    }
}
