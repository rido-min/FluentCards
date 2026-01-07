namespace FluentCards;

/// <summary>
/// Specifies how a background image fills its container.
/// </summary>
public enum ImageFillMode
{
    /// <summary>
    /// Cover the entire container while maintaining aspect ratio.
    /// </summary>
    Cover,

    /// <summary>
    /// Repeat the image horizontally.
    /// </summary>
    RepeatHorizontally,

    /// <summary>
    /// Repeat the image vertically.
    /// </summary>
    RepeatVertically,

    /// <summary>
    /// Repeat the image both horizontally and vertically.
    /// </summary>
    Repeat
}
