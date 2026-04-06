namespace FluentCards;

/// <summary>
/// When invoked, shows the specified card as a dropdown or inline.
/// </summary>
public class ShowCardAction : AdaptiveAction
{
    /// <summary>
    /// The card to show when this action is invoked.
    /// </summary>
    public AdaptiveCard? Card { get; set; }
}
