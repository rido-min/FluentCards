namespace FluentCards;

/// <summary>
/// Fluent builder for creating TextRun elements.
/// </summary>
public class TextRunBuilder
{
    private readonly TextRun _textRun = new();

    /// <summary>
    /// Sets the text content.
    /// </summary>
    /// <param name="text">The text content.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder WithText(string text)
    {
        _textRun.Text = text;
        return this;
    }

    /// <summary>
    /// Sets the size of the text.
    /// </summary>
    /// <param name="size">The text size.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder WithSize(TextSize size)
    {
        _textRun.Size = size;
        return this;
    }

    /// <summary>
    /// Sets the weight of the text.
    /// </summary>
    /// <param name="weight">The text weight.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder WithWeight(TextWeight weight)
    {
        _textRun.Weight = weight;
        return this;
    }

    /// <summary>
    /// Sets the color of the text.
    /// </summary>
    /// <param name="color">The text color.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder WithColor(TextColor color)
    {
        _textRun.Color = color;
        return this;
    }

    /// <summary>
    /// Sets whether the text is subtle.
    /// </summary>
    /// <param name="subtle">True for subtle, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder IsSubtle(bool subtle = true)
    {
        _textRun.IsSubtle = subtle;
        return this;
    }

    /// <summary>
    /// Sets whether the text is italic.
    /// </summary>
    /// <param name="italic">True for italic, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder IsItalic(bool italic = true)
    {
        _textRun.Italic = italic;
        return this;
    }

    /// <summary>
    /// Sets whether the text has strikethrough.
    /// </summary>
    /// <param name="strikethrough">True for strikethrough, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder IsStrikethrough(bool strikethrough = true)
    {
        _textRun.Strikethrough = strikethrough;
        return this;
    }

    /// <summary>
    /// Sets whether the text is underlined.
    /// </summary>
    /// <param name="underline">True for underline, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder IsUnderline(bool underline = true)
    {
        _textRun.Underline = underline;
        return this;
    }

    /// <summary>
    /// Sets whether the text is highlighted.
    /// </summary>
    /// <param name="highlight">True for highlight, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder IsHighlight(bool highlight = true)
    {
        _textRun.Highlight = highlight;
        return this;
    }

    /// <summary>
    /// Configures the action to invoke when the text is selected.
    /// </summary>
    /// <param name="configure">Action to configure the select action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public TextRunBuilder WithSelectAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _textRun.SelectAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Builds and returns the configured TextRun.
    /// </summary>
    /// <returns>The configured TextRun instance.</returns>
    public TextRun Build()
    {
        return _textRun;
    }
}
