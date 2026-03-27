namespace FluentCards.Samples;

/// <summary>
/// Demonstrates creating a people picker card using dynamic data queries.
/// </summary>
public static class PeoplePickerSample
{
    /// <summary>
    /// Creates a people picker card that searches users from Microsoft Graph.
    /// </summary>
    /// <returns>An Adaptive Card with a dynamic people picker and submit action.</returns>
    public static AdaptiveCard CreatePeoplePickerCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.6")
            .AddInputChoiceSet(i => i
                .WithId("people-picker")
                .WithLabel("Select users in the whole organization")
                .IsMultiSelect()
                .WithValue("user1,user2")
                .WithChoicesData("graph.microsoft.com/users"))
            .AddAction(a => a
                .Submit("Submit"))
            .Build();
    }
}
