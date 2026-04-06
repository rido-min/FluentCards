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
    /// <param name="input">The structured input parameters for the approval card.</param>
    /// <returns>An Adaptive Card representing an approval request.</returns>
    public static AdaptiveCard CreateApprovalCard(ApprovalCardInput input)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img
                        .WithUrl(input.RequesterImageUrl ?? string.Empty)
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText(input.RequesterName)
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText(input.SubmittedDate)
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddTextBlock(tb => tb
                .WithText(input.Title)
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Category", input.Category)
                .AddFact("Amount", input.Amount)
                .AddFact("Business Unit", input.BusinessUnit)
                .AddFact("Due Date", input.DueDate))
            .AddTextBlock(tb => tb
                .WithText(input.Description)
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
    /// <param name="input">The structured input parameters for the status update card.</param>
    /// <returns>An Adaptive Card representing a status update notification.</returns>
    public static AdaptiveCard CreateStatusUpdateCard(StatusUpdateCardInput input)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddColumnSet(cs => cs
                    .AddColumn("stretch", col => col
                        .AddTextBlock(tb => tb
                            .WithText(input.CardTitle)
                            .WithSize(TextSize.Large)
                            .WithWeight(TextWeight.Bolder)
                            .WithWrap(true))
                        .AddTextBlock(tb => tb
                            .WithText($"{input.TeamName} • {input.UpdateDate}")
                            .WithIsSubtle()
                            .WithSize(TextSize.Small)
                            .WithWrap(true)))))
            .AddFactSet(fs => fs
                .AddFact("Project", input.Project)
                .AddFact("Status", input.Status)
                .AddFact("Sprint", input.Sprint)
                .AddFact("Completion", input.Completion)
                .AddFact("Updated By", input.UpdatedBy))
            .AddTextBlock(tb => tb
                .WithText(input.Notes)
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl(input.ProjectUrl)
                .WithTitle("View Project"))
            .Build();
    }

    /// <summary>
    /// Creates a task assignment notification card informing the recipient of a newly assigned task.
    /// Reflects the Teams task-update sample pattern used in work tracking integrations.
    /// </summary>
    /// <param name="input">The structured input parameters for the task update card.</param>
    /// <returns>An Adaptive Card representing a task assignment notification.</returns>
    public static AdaptiveCard CreateTaskUpdateCard(TaskUpdateCardInput input)
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
                        .WithText(input.Priority)
                        .WithColor(TextColor.Attention)
                        .WithWeight(TextWeight.Bolder))))
            .AddFactSet(fs => fs
                .AddFact("Task", input.TaskName)
                .AddFact("Project", input.Project)
                .AddFact("Assigned By", input.AssignedBy)
                .AddFact("Due Date", input.DueDate)
                .AddFact("Estimate", input.Estimate))
            .AddTextBlock(tb => tb
                .WithText(input.Description)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl(input.TaskUrl)
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
    /// <param name="input">The structured input parameters for the meeting reminder card.</param>
    /// <returns>An Adaptive Card representing a meeting reminder.</returns>
    public static AdaptiveCard CreateMeetingReminderCard(MeetingReminderCardInput input)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("⏰ Meeting Starting Soon")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddTextBlock(tb => tb
                .WithText(input.MeetingTitle)
                .WithSize(TextSize.Medium)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Organizer", input.Organizer)
                .AddFact("Date", input.Date)
                .AddFact("Time", input.Time)
                .AddFact("Location", input.Location)
                .AddFact("Attendees", input.Attendees))
            .AddTextBlock(tb => tb
                .WithText(input.Agenda)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl(input.JoinUrl)
                .WithTitle("Join Meeting")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .OpenUrl(input.DetailsUrl)
                .WithTitle("View Details"))
            .Build();
    }

    /// <summary>
    /// Creates an expense report card for finance team review with Approve and Reject actions.
    /// Reflects the Teams expense-report sample pattern used in finance approval workflows.
    /// </summary>
    /// <param name="input">The structured input parameters for the expense report card.</param>
    /// <returns>An Adaptive Card representing an expense report for review.</returns>
    public static AdaptiveCard CreateExpenseReportCard(ExpenseReportCardInput input)
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
                        .WithUrl(input.EmployeeImageUrl ?? string.Empty)
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText(input.EmployeeName)
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText(input.EmployeeJobTitle)
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddFactSet(fs => fs
                .AddFact("Report ID", input.ReportId)
                .AddFact("Submitted", input.SubmittedDate)
                .AddFact("Category", input.Category)
                .AddFact("Total Amount", input.TotalAmount)
                .AddFact("Currency", input.Currency))
            .AddTextBlock(tb => tb
                .WithText(input.Description)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .Submit("Approve")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .Submit("Reject")
                .WithStyle(ActionStyle.Destructive))
            .AddAction(a => a
                .OpenUrl(input.ReportUrl)
                .WithTitle("View Report"))
            .Build();
    }
}
