namespace FluentCards;

/// <summary>
/// Fluent builder for creating BackgroundImage instances.
/// </summary>
public class BackgroundImageBuilder
{
    private readonly BackgroundImage _bg = new();

    /// <summary>
    /// Sets the URL of the background image.
    /// </summary>
    /// <param name="url">The background image URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public BackgroundImageBuilder WithUrl(string url)
    {
        _bg.Url = url;
        return this;
    }

    /// <summary>
    /// Sets the fill mode for the background image.
    /// </summary>
    /// <param name="fillMode">The fill mode.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public BackgroundImageBuilder WithFillMode(BackgroundImageFillMode fillMode)
    {
        _bg.FillMode = fillMode;
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of the background image.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public BackgroundImageBuilder WithHorizontalAlignment(HorizontalAlignment alignment)
    {
        _bg.HorizontalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Sets the vertical alignment of the background image.
    /// </summary>
    /// <param name="alignment">The vertical alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public BackgroundImageBuilder WithVerticalAlignment(VerticalAlignment alignment)
    {
        _bg.VerticalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured BackgroundImage.
    /// </summary>
    /// <returns>The configured BackgroundImage instance.</returns>
    public BackgroundImage Build()
    {
        return _bg;
    }
}
