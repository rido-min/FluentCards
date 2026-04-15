namespace FluentCards;

/// <summary>
/// Fluent builder for creating ImageSet elements.
/// </summary>
public class ImageSetBuilder
{
    private readonly ImageSet _imageSet = new() { Images = new List<Image>() };

    /// <summary>
    /// Sets the unique identifier for the image set.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithId(string id)
    {
        _imageSet.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the size for all images in the set.
    /// </summary>
    /// <param name="size">The image size.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithImageSize(ImageSize size)
    {
        _imageSet.ImageSize = size;
        return this;
    }

    /// <summary>
    /// Adds an image to the image set.
    /// </summary>
    /// <param name="configure">Action to configure the image.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder AddImage(Action<ImageBuilder> configure)
    {
        var builder = new ImageBuilder();
        configure(builder);
        _imageSet.Images!.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds an image to the image set.
    /// </summary>
    /// <param name="image">The image to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder AddImage(Image image)
    {
        _imageSet.Images!.Add(image);
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithSpacing(Spacing spacing)
    {
        _imageSet.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithSeparator(bool separator = true)
    {
        _imageSet.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithIsVisible(bool isVisible)
    {
        _imageSet.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithHeight(string height)
    {
        _imageSet.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithFallback(object fallback)
    {
        _imageSet.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithRequires(string key, string version)
    {
        _imageSet.Requires ??= new Dictionary<string, string>();
        _imageSet.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ImageSetBuilder WithRtl(bool rtl = true)
    {
        _imageSet.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured ImageSet.
    /// </summary>
    /// <returns>The configured ImageSet instance.</returns>
    public ImageSet Build()
    {
        return _imageSet;
    }
}
