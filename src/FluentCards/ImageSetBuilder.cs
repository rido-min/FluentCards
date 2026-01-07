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
    /// Builds and returns the configured ImageSet.
    /// </summary>
    /// <returns>The configured ImageSet instance.</returns>
    public ImageSet Build()
    {
        return _imageSet;
    }
}
