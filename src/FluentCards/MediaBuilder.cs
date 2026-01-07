namespace FluentCards;

/// <summary>
/// Fluent builder for creating Media elements.
/// </summary>
public class MediaBuilder
{
    private readonly Media _media = new() { Sources = new List<MediaSource>() };

    /// <summary>
    /// Sets the unique identifier for the media element.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithId(string id)
    {
        _media.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the poster image URL for video content.
    /// </summary>
    /// <param name="poster">The poster image URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithPoster(string poster)
    {
        _media.Poster = poster;
        return this;
    }

    /// <summary>
    /// Sets the alternate text for accessibility.
    /// </summary>
    /// <param name="altText">The alternate text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithAltText(string altText)
    {
        _media.AltText = altText;
        return this;
    }

    /// <summary>
    /// Adds a media source to the media element.
    /// </summary>
    /// <param name="url">The URL of the media source.</param>
    /// <param name="mimeType">The MIME type of the media source.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder AddSource(string url, string mimeType)
    {
        _media.Sources!.Add(new MediaSource { Url = url, MimeType = mimeType });
        return this;
    }

    /// <summary>
    /// Adds a media source to the media element.
    /// </summary>
    /// <param name="source">The media source to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder AddSource(MediaSource source)
    {
        _media.Sources!.Add(source);
        return this;
    }

    /// <summary>
    /// Builds and returns the configured Media.
    /// </summary>
    /// <returns>The configured Media instance.</returns>
    public Media Build()
    {
        return _media;
    }
}
