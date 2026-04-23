namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating Submit and Execute actions with custom verbs and data payloads.
/// </summary>
public static class ActionSubmitExecuteSample
{
    /// <summary>
    /// Creates a card with Action.Execute and Action.Submit actions.
    /// </summary>
    /// <returns>An Adaptive Card with Execute and Submit actions.</returns>
    public static AdaptiveCard CreateActionSubmitExecuteCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.4")
            .AddTextBlock(tb => tb
                .WithText("welcome to ac 11")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder))
            .AddTextBlock(tb => tb
                .WithText("click the buttons below"))
            .AddAction(a => a
                .Execute("Test AC Action")
                .WithData("{\"message\":\"button clicked !!\"}")
                .WithVerb("testAction"))
            .AddAction(a => a
                .Submit("Open Task Module")
                .WithData("{\"msteams\":{\"type\":\"task/fetch\"}}"))
            .AddAction(a => a
                .Execute("Request File Upload")
                .WithVerb("requestFileUpload"))
            .Build();
    }
}
