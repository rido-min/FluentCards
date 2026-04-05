namespace FluentCards;

/// <summary>
/// Provides helper methods for creating Microsoft Teams-style Adaptive Cards using the FluentCards fluent API,
/// reflecting common patterns from the Teams Adaptive Card Samples collection.
/// </summary>
public static class TeamsAdaptiveCards
{
    /// <summary>
    /// Creates an approval request card with Approve and Decline actions.
    /// Reflects the Teams approval sample pattern where a requester asks for sign-off on an item.
    /// </summary>
    /// <returns>An Adaptive Card representing an approval request.</returns>
    public static AdaptiveCard CreateApprovalCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img
                        .WithUrl("https://adaptivecards.io/content/cats/1.png")
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText("Mia Alvarez")
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText("Submitted April 1, 2025")
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddTextBlock(tb => tb
                .WithText("Expense Report Approval")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Category", "Travel & Accommodation")
                .AddFact("Amount", "$1,250.00")
                .AddFact("Business Unit", "Engineering")
                .AddFact("Due Date", "April 8, 2025"))
            .AddTextBlock(tb => tb
                .WithText("Team offsite travel expenses including flights, hotel, and ground transportation for the Q2 planning session.")
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .Submit("Approve")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .Submit("Decline")
                .WithStyle(ActionStyle.Destructive))
            .Build();
    }

    /// <summary>
    /// Creates a status update notification card showing the current state of a project or task.
    /// Reflects the Teams status-update sample pattern used in project tracking scenarios.
    /// </summary>
    /// <returns>An Adaptive Card representing a status update notification.</returns>
    public static AdaptiveCard CreateStatusUpdateCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddColumnSet(cs => cs
                    .AddColumn("stretch", col => col
                        .AddTextBlock(tb => tb
                            .WithText("Project Status Update")
                            .WithSize(TextSize.Large)
                            .WithWeight(TextWeight.Bolder)
                            .WithWrap(true))
                        .AddTextBlock(tb => tb
                            .WithText("Teams Engineering • April 5, 2025")
                            .WithIsSubtle()
                            .WithSize(TextSize.Small)
                            .WithWrap(true)))))
            .AddFactSet(fs => fs
                .AddFact("Project", "Q2 Feature Release")
                .AddFact("Status", "🟡 At Risk")
                .AddFact("Sprint", "Sprint 14 of 16")
                .AddFact("Completion", "68%")
                .AddFact("Updated By", "Jordan Lee"))
            .AddTextBlock(tb => tb
                .WithText("The authentication module integration is behind schedule due to a dependency on the identity service upgrade. A revised timeline has been shared with stakeholders.")
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl("https://example.com/projects/q2-release")
                .WithTitle("View Project"))
            .Build();
    }

    /// <summary>
    /// Creates a task assignment notification card informing the recipient of a newly assigned task.
    /// Reflects the Teams task-update sample pattern used in work tracking integrations.
    /// </summary>
    /// <returns>An Adaptive Card representing a task assignment notification.</returns>
    public static AdaptiveCard CreateTaskUpdateCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn("stretch", col => col
                    .AddTextBlock(tb => tb
                        .WithText("Task Assigned to You")
                        .WithSize(TextSize.Large)
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true)))
                .AddColumn("auto", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText("🔴 High")
                        .WithColor(TextColor.Attention)
                        .WithWeight(TextWeight.Bolder))))
            .AddFactSet(fs => fs
                .AddFact("Task", "Implement OAuth2 token refresh flow")
                .AddFact("Project", "Q2 Feature Release")
                .AddFact("Assigned By", "Sam Rivera")
                .AddFact("Due Date", "April 11, 2025")
                .AddFact("Estimate", "3 days"))
            .AddTextBlock(tb => tb
                .WithText("Implement the silent token refresh mechanism for the new authentication service. Ensure backward compatibility with existing sessions and add telemetry for failure scenarios.")
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl("https://example.com/tasks/oauth2-token-refresh")
                .WithTitle("View Task"))
            .AddAction(a => a
                .Submit("Acknowledge")
                .WithStyle(ActionStyle.Positive))
            .Build();
    }

    /// <summary>
    /// Creates a meeting reminder card with meeting details and a join link.
    /// Reflects the Teams meeting-invite sample pattern used in calendar integration scenarios.
    /// </summary>
    /// <returns>An Adaptive Card representing a meeting reminder.</returns>
    public static AdaptiveCard CreateMeetingReminderCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("⏰ Meeting Starting Soon")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddTextBlock(tb => tb
                .WithText("Q2 Planning Kickoff")
                .WithSize(TextSize.Medium)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Organizer", "Alex Chen")
                .AddFact("Date", "Monday, April 7, 2025")
                .AddFact("Time", "2:00 PM – 3:00 PM (PST)")
                .AddFact("Location", "Microsoft Teams")
                .AddFact("Attendees", "12 people"))
            .AddTextBlock(tb => tb
                .WithText("Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.")
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl("https://teams.microsoft.com/l/meetup-join/sample")
                .WithTitle("Join Meeting")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .OpenUrl("https://example.com/calendar/q2-planning")
                .WithTitle("View Details"))
            .Build();
    }

    /// <summary>
    /// Creates an expense report card for finance team review with Approve and Reject actions.
    /// Reflects the Teams expense-report sample pattern used in finance approval workflows.
    /// </summary>
    /// <returns>An Adaptive Card representing an expense report for review.</returns>
    public static AdaptiveCard CreateExpenseReportCard()
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddTextBlock(tb => tb
                    .WithText("Expense Report Submitted")
                    .WithSize(TextSize.Large)
                    .WithWeight(TextWeight.Bolder)
                    .WithWrap(true))
                .AddTextBlock(tb => tb
                    .WithText("Awaiting your review and approval")
                    .WithIsSubtle()
                    .WithWrap(true)))
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img
                        .WithUrl("https://adaptivecards.io/content/cats/2.png")
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText("Chris Morgan")
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText("Senior Developer • Engineering")
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddFactSet(fs => fs
                .AddFact("Report ID", "EXP-2025-0412")
                .AddFact("Submitted", "April 5, 2025")
                .AddFact("Category", "Conference & Training")
                .AddFact("Total Amount", "$3,480.00")
                .AddFact("Currency", "USD"))
            .AddTextBlock(tb => tb
                .WithText("Attendance at Microsoft Build 2025 including conference registration, flights, and hotel (4 nights).")
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .Submit("Approve")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .Submit("Reject")
                .WithStyle(ActionStyle.Destructive))
            .AddAction(a => a
                .OpenUrl("https://example.com/expenses/EXP-2025-0412")
                .WithTitle("View Report"))
            .Build();
    }
}
