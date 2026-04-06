namespace FluentCards;

/// <summary>
/// Fluent builder for creating Image elements.
/// </summary>
public class ImageBuilder
{
    private readonly Image _image = new();

    /// <summary>
    /// Sets the URL of the image.
    /// </summary>
    /// <param name="url">The image URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithUrl(string url)
    {
        _image.Url = url;
        return this;
    }

    /// <summary>
    /// Sets the alternate text for the image.
    /// </summary>
    /// <param name="altText">The alternate text for accessibility.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithAltText(string altText)
    {
        _image.AltText = altText;
        return this;
    }

    /// <summary>
    /// Sets the size of the image.
    /// </summary>
    /// <param name="size">The image size.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithSize(ImageSize size)
    {
        _image.Size = size;
        return this;
    }

    /// <summary>
    /// Sets the style of the image.
    /// </summary>
    /// <param name="style">The image style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithStyle(ImageStyle style)
    {
        _image.Style = style;
        return this;
    }

    /// <summary>
    /// Sets the width of the image.
    /// </summary>
    /// <param name="width">The image width.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithWidth(string width)
    {
        _image.Width = width;
        return this;
    }

    /// <summary>
    /// Sets the height of the image.
    /// </summary>
    /// <param name="height">The image height.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithHeight(string height)
    {
        _image.Height = height;
        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of the image.
    /// </summary>
    /// <param name="alignment">The horizontal alignment.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithHorizontalAlignment(HorizontalAlignment alignment)
    {
        _image.HorizontalAlignment = alignment;
        return this;
    }

    /// <summary>
    /// Sets the background color for the image.
    /// </summary>
    /// <param name="color">The background color.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithBackgroundColor(string color)
    {
        _image.BackgroundColor = color;
        return this;
    }

    /// <summary>
    /// Configures the action to invoke when the image is selected.
    /// </summary>
    /// <param name="configure">Action to configure the select action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageBuilder WithSelectAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _image.SelectAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Builds and returns the configured Image.
    /// </summary>
    /// <returns>The configured Image instance.</returns>
    public Image Build()
    {
        return _image;
    }
}
