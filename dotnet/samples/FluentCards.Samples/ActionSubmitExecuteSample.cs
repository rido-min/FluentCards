using System.Text.Json;

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
        return new AdaptiveCard
        {
            Version = "1.4",
            Body =
            [
                new TextBlock
                {
                    Text = "welcome to ac 11",
                    Size = TextSize.Large,
                    Weight = TextWeight.Bolder
                },
                new TextBlock
                {
                    Text = "click the buttons below"
                }
            ],
            Actions =
            [
                new ExecuteAction
                {
                    Data = JsonSerializer.SerializeToElement(new { message = "button clicked !!" }),
                    Verb = "testAction",
                    Title = "Test AC Action"
                },
                new SubmitAction
                {
                    Data = JsonSerializer.SerializeToElement(new
                    {
                        msteams = new
                        {
                            type = "task/fetch"
                        }
                    }),
                    Title = "Open Task Module"
                },
                new ExecuteAction
                {
                    Verb = "requestFileUpload",
                    Title = "Request File Upload"
                }
            ]
        };
    }
}
