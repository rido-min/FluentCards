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
    /// Adds a caption source to the media element.
    /// </summary>
    /// <param name="label">The label for the caption (e.g., language name).</param>
    /// <param name="url">The URL of the caption source.</param>
    /// <param name="mimeType">The MIME type of the caption source.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder AddCaptionSource(string label, string url, string mimeType)
    {
        _media.CaptionSources ??= new List<CaptionSource>();
        _media.CaptionSources.Add(new CaptionSource { Label = label, Url = url, MimeType = mimeType });
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithSpacing(Spacing spacing)
    {
        _media.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithSeparator(bool separator = true)
    {
        _media.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithIsVisible(bool isVisible)
    {
        _media.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithHeight(string height)
    {
        _media.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithFallback(object fallback)
    {
        _media.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithRequires(string key, string version)
    {
        _media.Requires ??= new Dictionary<string, string>();
        _media.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public MediaBuilder WithRtl(bool rtl = true)
    {
        _media.Rtl = rtl;
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
