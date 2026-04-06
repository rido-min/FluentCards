using FluentCards;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/cards/{cardType}", (string cardType) =>
{
    var json = cardType switch
    {
        "approval" => TeamsAdaptiveCards.CreateApprovalCard(new ApprovalCardInput(
            RequesterName: "Mia Alvarez",
            SubmittedDate: "Submitted April 1, 2025",
            Title: "Expense Report Approval",
            Category: "Travel & Accommodation",
            Amount: "$1,250.00",
            BusinessUnit: "Engineering",
            DueDate: "April 8, 2025",
            Description: "Team offsite travel expenses.",
            RequesterImageUrl: "https://adaptivecards.io/content/cats/1.png")).ToJson(),

        "status-update" => TeamsAdaptiveCards.CreateStatusUpdateCard(new StatusUpdateCardInput(
            CardTitle: "Project Status Update",
            TeamName: "Teams Engineering",
            UpdateDate: "April 5, 2025",
            Project: "Q2 Feature Release",
            Status: "🟡 At Risk",
            Sprint: "Sprint 14 of 16",
            Completion: "68%",
            UpdatedBy: "Jordan Lee",
            Notes: "The authentication module integration is behind schedule due to dependency changes in the IdP library.",
            ProjectUrl: "https://example.com/projects/q2-release")).ToJson(),

        "task-update" => TeamsAdaptiveCards.CreateTaskUpdateCard(new TaskUpdateCardInput(
            TaskName: "Implement OAuth2 token refresh flow",
            Project: "Q2 Feature Release",
            AssignedBy: "Sam Rivera",
            DueDate: "April 11, 2025",
            Estimate: "3 days",
            Priority: "🔴 High",
            Description: "Implement the silent token refresh mechanism using MSAL.",
            TaskUrl: "https://example.com/tasks/oauth2-token-refresh")).ToJson(),

        "meeting-reminder" => TeamsAdaptiveCards.CreateMeetingReminderCard(new MeetingReminderCardInput(
            MeetingTitle: "Q2 Planning Kickoff",
            Organizer: "Alex Chen",
            Date: "Monday, April 7, 2025",
            Time: "2:00 PM – 3:00 PM (PST)",
            Location: "Microsoft Teams",
            Attendees: "12 people",
            Agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
            JoinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
            DetailsUrl: "https://example.com/calendar/q2-planning")).ToJson(),

        "expense-report" => TeamsAdaptiveCards.CreateExpenseReportCard(new ExpenseReportCardInput(
            EmployeeName: "Chris Morgan",
            EmployeeJobTitle: "Senior Developer • Engineering",
            ReportId: "EXP-2025-0412",
            SubmittedDate: "April 5, 2025",
            Category: "Conference & Training",
            TotalAmount: "$3,480.00",
            Currency: "USD",
            Description: "Attendance at Microsoft Build 2025.",
            ReportUrl: "https://example.com/expenses/EXP-2025-0412",
            EmployeeImageUrl: "https://adaptivecards.io/content/cats/2.png")).ToJson(),

        _ => null
    };

    return json is not null
        ? Results.Content(json, "application/json")
        : Results.NotFound();
});

app.Run("http://localhost:5250");
