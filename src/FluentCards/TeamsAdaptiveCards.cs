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
    /// <param name="requesterName">The name of the person requesting approval.</param>
    /// <param name="submittedDate">The date the request was submitted, formatted as a display string.</param>
    /// <param name="title">The title of the approval request.</param>
    /// <param name="category">The category of the item being approved.</param>
    /// <param name="amount">The monetary amount associated with the request.</param>
    /// <param name="businessUnit">The business unit submitting the request.</param>
    /// <param name="dueDate">The due date for the approval decision, formatted as a display string.</param>
    /// <param name="description">A description or justification for the approval request.</param>
    /// <param name="requesterImageUrl">An optional URL for the requester's profile image.</param>
    /// <returns>An Adaptive Card representing an approval request.</returns>
    public static AdaptiveCard CreateApprovalCard(
        string requesterName,
        string submittedDate,
        string title,
        string category,
        string amount,
        string businessUnit,
        string dueDate,
        string description,
        string? requesterImageUrl = null)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddColumnSet(cs => cs
                .AddColumn("auto", col => col
                    .AddImage(img => img
                        .WithUrl(requesterImageUrl ?? string.Empty)
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText(requesterName)
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText(submittedDate)
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddTextBlock(tb => tb
                .WithText(title)
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Category", category)
                .AddFact("Amount", amount)
                .AddFact("Business Unit", businessUnit)
                .AddFact("Due Date", dueDate))
            .AddTextBlock(tb => tb
                .WithText(description)
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
    /// <param name="cardTitle">The heading title of the status update card.</param>
    /// <param name="teamName">The name of the team or department posting the update.</param>
    /// <param name="updateDate">The date of the update, formatted as a display string.</param>
    /// <param name="project">The name of the project being reported on.</param>
    /// <param name="status">The current status of the project (e.g., "🟡 At Risk").</param>
    /// <param name="sprint">The current sprint identifier (e.g., "Sprint 14 of 16").</param>
    /// <param name="completion">The percentage of completion as a display string (e.g., "68%").</param>
    /// <param name="updatedBy">The name of the person posting the update.</param>
    /// <param name="notes">Narrative notes describing the current state or blockers.</param>
    /// <param name="projectUrl">A URL linking to the full project details.</param>
    /// <returns>An Adaptive Card representing a status update notification.</returns>
    public static AdaptiveCard CreateStatusUpdateCard(
        string cardTitle,
        string teamName,
        string updateDate,
        string project,
        string status,
        string sprint,
        string completion,
        string updatedBy,
        string notes,
        string projectUrl)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddContainer(c => c
                .WithStyle(ContainerStyle.Emphasis)
                .AddColumnSet(cs => cs
                    .AddColumn("stretch", col => col
                        .AddTextBlock(tb => tb
                            .WithText(cardTitle)
                            .WithSize(TextSize.Large)
                            .WithWeight(TextWeight.Bolder)
                            .WithWrap(true))
                        .AddTextBlock(tb => tb
                            .WithText($"{teamName} • {updateDate}")
                            .WithIsSubtle()
                            .WithSize(TextSize.Small)
                            .WithWrap(true)))))
            .AddFactSet(fs => fs
                .AddFact("Project", project)
                .AddFact("Status", status)
                .AddFact("Sprint", sprint)
                .AddFact("Completion", completion)
                .AddFact("Updated By", updatedBy))
            .AddTextBlock(tb => tb
                .WithText(notes)
                .WithWrap(true))
            .AddAction(a => a
                .OpenUrl(projectUrl)
                .WithTitle("View Project"))
            .Build();
    }

    /// <summary>
    /// Creates a task assignment notification card informing the recipient of a newly assigned task.
    /// Reflects the Teams task-update sample pattern used in work tracking integrations.
    /// </summary>
    /// <param name="taskName">The name or title of the assigned task.</param>
    /// <param name="project">The project the task belongs to.</param>
    /// <param name="assignedBy">The name of the person who assigned the task.</param>
    /// <param name="dueDate">The due date of the task, formatted as a display string.</param>
    /// <param name="estimate">The time estimate for completing the task (e.g., "3 days").</param>
    /// <param name="priority">The priority label of the task (e.g., "🔴 High").</param>
    /// <param name="description">A detailed description of the work to be done.</param>
    /// <param name="taskUrl">A URL linking to the full task details.</param>
    /// <returns>An Adaptive Card representing a task assignment notification.</returns>
    public static AdaptiveCard CreateTaskUpdateCard(
        string taskName,
        string project,
        string assignedBy,
        string dueDate,
        string estimate,
        string priority,
        string description,
        string taskUrl)
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
                        .WithText(priority)
                        .WithColor(TextColor.Attention)
                        .WithWeight(TextWeight.Bolder))))
            .AddFactSet(fs => fs
                .AddFact("Task", taskName)
                .AddFact("Project", project)
                .AddFact("Assigned By", assignedBy)
                .AddFact("Due Date", dueDate)
                .AddFact("Estimate", estimate))
            .AddTextBlock(tb => tb
                .WithText(description)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl(taskUrl)
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
    /// <param name="meetingTitle">The title of the meeting.</param>
    /// <param name="organizer">The name of the meeting organizer.</param>
    /// <param name="date">The date of the meeting, formatted as a display string.</param>
    /// <param name="time">The time range of the meeting, formatted as a display string (e.g., "2:00 PM – 3:00 PM (PST)").</param>
    /// <param name="location">The location or platform where the meeting takes place.</param>
    /// <param name="attendees">A display string describing the attendees (e.g., "12 people").</param>
    /// <param name="agenda">A brief agenda or description of the meeting topics.</param>
    /// <param name="joinUrl">A URL to join the meeting directly.</param>
    /// <param name="detailsUrl">A URL linking to the full meeting or calendar details.</param>
    /// <returns>An Adaptive Card representing a meeting reminder.</returns>
    public static AdaptiveCard CreateMeetingReminderCard(
        string meetingTitle,
        string organizer,
        string date,
        string time,
        string location,
        string attendees,
        string agenda,
        string joinUrl,
        string detailsUrl)
    {
        return AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("⏰ Meeting Starting Soon")
                .WithSize(TextSize.Large)
                .WithWeight(TextWeight.Bolder)
                .WithWrap(true))
            .AddTextBlock(tb => tb
                .WithText(meetingTitle)
                .WithSize(TextSize.Medium)
                .WithWrap(true))
            .AddFactSet(fs => fs
                .AddFact("Organizer", organizer)
                .AddFact("Date", date)
                .AddFact("Time", time)
                .AddFact("Location", location)
                .AddFact("Attendees", attendees))
            .AddTextBlock(tb => tb
                .WithText(agenda)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .OpenUrl(joinUrl)
                .WithTitle("Join Meeting")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .OpenUrl(detailsUrl)
                .WithTitle("View Details"))
            .Build();
    }

    /// <summary>
    /// Creates an expense report card for finance team review with Approve and Reject actions.
    /// Reflects the Teams expense-report sample pattern used in finance approval workflows.
    /// </summary>
    /// <param name="employeeName">The full name of the employee who submitted the report.</param>
    /// <param name="employeeJobTitle">The job title and department of the employee.</param>
    /// <param name="reportId">The unique identifier for the expense report.</param>
    /// <param name="submittedDate">The date the report was submitted, formatted as a display string.</param>
    /// <param name="category">The expense category (e.g., "Conference &amp; Training").</param>
    /// <param name="totalAmount">The total monetary amount of the report.</param>
    /// <param name="currency">The currency code (e.g., "USD").</param>
    /// <param name="description">A description of the expenses incurred.</param>
    /// <param name="reportUrl">A URL linking to the full expense report.</param>
    /// <param name="employeeImageUrl">An optional URL for the employee's profile image.</param>
    /// <returns>An Adaptive Card representing an expense report for review.</returns>
    public static AdaptiveCard CreateExpenseReportCard(
        string employeeName,
        string employeeJobTitle,
        string reportId,
        string submittedDate,
        string category,
        string totalAmount,
        string currency,
        string description,
        string reportUrl,
        string? employeeImageUrl = null)
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
                        .WithUrl(employeeImageUrl ?? string.Empty)
                        .WithSize(ImageSize.Small)
                        .WithStyle(ImageStyle.Person)))
                .AddColumn("stretch", col => col
                    .WithVerticalContentAlignment(VerticalAlignment.Center)
                    .AddTextBlock(tb => tb
                        .WithText(employeeName)
                        .WithWeight(TextWeight.Bolder)
                        .WithWrap(true))
                    .AddTextBlock(tb => tb
                        .WithText(employeeJobTitle)
                        .WithIsSubtle()
                        .WithSize(TextSize.Small)
                        .WithWrap(true))))
            .AddFactSet(fs => fs
                .AddFact("Report ID", reportId)
                .AddFact("Submitted", submittedDate)
                .AddFact("Category", category)
                .AddFact("Total Amount", totalAmount)
                .AddFact("Currency", currency))
            .AddTextBlock(tb => tb
                .WithText(description)
                .WithWrap(true)
                .WithIsSubtle())
            .AddAction(a => a
                .Submit("Approve")
                .WithStyle(ActionStyle.Positive))
            .AddAction(a => a
                .Submit("Reject")
                .WithStyle(ActionStyle.Destructive))
            .AddAction(a => a
                .OpenUrl(reportUrl)
                .WithTitle("View Report"))
            .Build();
    }
}
