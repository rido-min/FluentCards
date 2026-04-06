namespace FluentCards;

/// <summary>Input parameters for <see cref="TeamsAdaptiveCards.CreateApprovalCard"/>.</summary>
public record ApprovalCardInput(
    string RequesterName,
    string SubmittedDate,
    string Title,
    string Category,
    string Amount,
    string BusinessUnit,
    string DueDate,
    string Description,
    string? RequesterImageUrl = null);

/// <summary>Input parameters for <see cref="TeamsAdaptiveCards.CreateStatusUpdateCard"/>.</summary>
public record StatusUpdateCardInput(
    string CardTitle,
    string TeamName,
    string UpdateDate,
    string Project,
    string Status,
    string Sprint,
    string Completion,
    string UpdatedBy,
    string Notes,
    string ProjectUrl);

/// <summary>Input parameters for <see cref="TeamsAdaptiveCards.CreateTaskUpdateCard"/>.</summary>
public record TaskUpdateCardInput(
    string TaskName,
    string Project,
    string AssignedBy,
    string DueDate,
    string Estimate,
    string Priority,
    string Description,
    string TaskUrl);

/// <summary>Input parameters for <see cref="TeamsAdaptiveCards.CreateMeetingReminderCard"/>.</summary>
public record MeetingReminderCardInput(
    string MeetingTitle,
    string Organizer,
    string Date,
    string Time,
    string Location,
    string Attendees,
    string Agenda,
    string JoinUrl,
    string DetailsUrl);

/// <summary>Input parameters for <see cref="TeamsAdaptiveCards.CreateExpenseReportCard"/>.</summary>
public record ExpenseReportCardInput(
    string EmployeeName,
    string EmployeeJobTitle,
    string ReportId,
    string SubmittedDate,
    string Category,
    string TotalAmount,
    string Currency,
    string Description,
    string ReportUrl,
    string? EmployeeImageUrl = null);
