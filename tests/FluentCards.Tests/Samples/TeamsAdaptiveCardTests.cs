using Xunit;

namespace FluentCards.Tests.Samples;

public class TeamsAdaptiveCardTests
{
    [Fact]
    public void TeamsAdaptiveCards_CreateApprovalCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateApprovalCard(new ApprovalCardInput(
            RequesterName: "Mia Alvarez",
            SubmittedDate: "Submitted April 1, 2025",
            Title: "Expense Report Approval",
            Category: "Travel & Accommodation",
            Amount: "$1,250.00",
            BusinessUnit: "Engineering",
            DueDate: "April 8, 2025",
            Description: "Team offsite travel expenses.",
            RequesterImageUrl: "https://adaptivecards.io/content/cats/1.png"));

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.Contains(card.Body, e => e is ColumnSet);
        Assert.Contains(card.Body, e => e is FactSet);
        Assert.NotNull(card.Actions);
        Assert.Equal(2, card.Actions.Count);

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateApprovalCard_HasApproveAndDeclineActions()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateApprovalCard(new ApprovalCardInput(
            RequesterName: "Mia Alvarez",
            SubmittedDate: "Submitted April 1, 2025",
            Title: "Expense Report Approval",
            Category: "Travel & Accommodation",
            Amount: "$1,250.00",
            BusinessUnit: "Engineering",
            DueDate: "April 8, 2025",
            Description: "Team offsite travel expenses."));

        // Assert
        Assert.NotNull(card.Actions);
        var submitActions = card.Actions.OfType<SubmitAction>().ToList();
        Assert.Equal(2, submitActions.Count);
        Assert.Contains(submitActions, a => a.Title == "Approve" && a.Style == ActionStyle.Positive);
        Assert.Contains(submitActions, a => a.Title == "Decline" && a.Style == ActionStyle.Destructive);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateStatusUpdateCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard(new StatusUpdateCardInput(
            CardTitle: "Project Status Update",
            TeamName: "Teams Engineering",
            UpdateDate: "April 5, 2025",
            Project: "Q2 Feature Release",
            Status: "🟡 At Risk",
            Sprint: "Sprint 14 of 16",
            Completion: "68%",
            UpdatedBy: "Jordan Lee",
            Notes: "The authentication module integration is behind schedule.",
            ProjectUrl: "https://example.com/projects/q2-release"));

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.Contains(card.Body, e => e is Container);
        Assert.Contains(card.Body, e => e is FactSet);
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateStatusUpdateCard_HasViewProjectAction()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard(new StatusUpdateCardInput(
            CardTitle: "Project Status Update",
            TeamName: "Teams Engineering",
            UpdateDate: "April 5, 2025",
            Project: "Q2 Feature Release",
            Status: "🟡 At Risk",
            Sprint: "Sprint 14 of 16",
            Completion: "68%",
            UpdatedBy: "Jordan Lee",
            Notes: "The authentication module integration is behind schedule.",
            ProjectUrl: "https://example.com/projects/q2-release"));

        // Assert
        Assert.NotNull(card.Actions);
        var openUrl = Assert.Single(card.Actions.OfType<OpenUrlAction>());
        Assert.Equal("View Project", openUrl.Title);
        Assert.NotNull(openUrl.Url);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateTaskUpdateCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard(new TaskUpdateCardInput(
            TaskName: "Implement OAuth2 token refresh flow",
            Project: "Q2 Feature Release",
            AssignedBy: "Sam Rivera",
            DueDate: "April 11, 2025",
            Estimate: "3 days",
            Priority: "🔴 High",
            Description: "Implement the silent token refresh mechanism.",
            TaskUrl: "https://example.com/tasks/oauth2-token-refresh"));

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.Contains(card.Body, e => e is ColumnSet);
        Assert.Contains(card.Body, e => e is FactSet);
        Assert.NotNull(card.Actions);
        Assert.Equal(2, card.Actions.Count);

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateTaskUpdateCard_HasViewAndAcknowledgeActions()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard(new TaskUpdateCardInput(
            TaskName: "Implement OAuth2 token refresh flow",
            Project: "Q2 Feature Release",
            AssignedBy: "Sam Rivera",
            DueDate: "April 11, 2025",
            Estimate: "3 days",
            Priority: "🔴 High",
            Description: "Implement the silent token refresh mechanism.",
            TaskUrl: "https://example.com/tasks/oauth2-token-refresh"));

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Task" });
        Assert.Contains(card.Actions, a => a is SubmitAction { Title: "Acknowledge" });
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateMeetingReminderCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard(new MeetingReminderCardInput(
            MeetingTitle: "Q2 Planning Kickoff",
            Organizer: "Alex Chen",
            Date: "Monday, April 7, 2025",
            Time: "2:00 PM – 3:00 PM (PST)",
            Location: "Microsoft Teams",
            Attendees: "12 people",
            Agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
            JoinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
            DetailsUrl: "https://example.com/calendar/q2-planning"));

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.Contains(card.Body, e => e is FactSet);
        Assert.NotNull(card.Actions);
        Assert.Equal(2, card.Actions.Count);

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateMeetingReminderCard_HasJoinMeetingAction()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard(new MeetingReminderCardInput(
            MeetingTitle: "Q2 Planning Kickoff",
            Organizer: "Alex Chen",
            Date: "Monday, April 7, 2025",
            Time: "2:00 PM – 3:00 PM (PST)",
            Location: "Microsoft Teams",
            Attendees: "12 people",
            Agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
            JoinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
            DetailsUrl: "https://example.com/calendar/q2-planning"));

        // Assert
        Assert.NotNull(card.Actions);
        var joinAction = card.Actions.OfType<OpenUrlAction>().First(a => a.Title == "Join Meeting");
        Assert.Equal(ActionStyle.Positive, joinAction.Style);
        Assert.NotNull(joinAction.Url);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateExpenseReportCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateExpenseReportCard(new ExpenseReportCardInput(
            EmployeeName: "Chris Morgan",
            EmployeeJobTitle: "Senior Developer • Engineering",
            ReportId: "EXP-2025-0412",
            SubmittedDate: "April 5, 2025",
            Category: "Conference & Training",
            TotalAmount: "$3,480.00",
            Currency: "USD",
            Description: "Attendance at Microsoft Build 2025.",
            ReportUrl: "https://example.com/expenses/EXP-2025-0412",
            EmployeeImageUrl: "https://adaptivecards.io/content/cats/2.png"));

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.Contains(card.Body, e => e is Container);
        Assert.Contains(card.Body, e => e is ColumnSet);
        Assert.Contains(card.Body, e => e is FactSet);
        Assert.NotNull(card.Actions);
        Assert.Equal(3, card.Actions.Count);

        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateExpenseReportCard_HasApproveRejectAndViewActions()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateExpenseReportCard(new ExpenseReportCardInput(
            EmployeeName: "Chris Morgan",
            EmployeeJobTitle: "Senior Developer • Engineering",
            ReportId: "EXP-2025-0412",
            SubmittedDate: "April 5, 2025",
            Category: "Conference & Training",
            TotalAmount: "$3,480.00",
            Currency: "USD",
            Description: "Attendance at Microsoft Build 2025.",
            ReportUrl: "https://example.com/expenses/EXP-2025-0412"));

        // Assert
        Assert.NotNull(card.Actions);
        var submitActions = card.Actions.OfType<SubmitAction>().ToList();
        Assert.Contains(submitActions, a => a.Title == "Approve" && a.Style == ActionStyle.Positive);
        Assert.Contains(submitActions, a => a.Title == "Reject" && a.Style == ActionStyle.Destructive);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Report" });
    }

    [Fact]
    public void AllTeamsAdaptiveCards_SerializeToValidJson()
    {
        // Arrange
        var cards = new[]
        {
            TeamsAdaptiveCards.CreateApprovalCard(new ApprovalCardInput(
                RequesterName: "Mia Alvarez",
                SubmittedDate: "Submitted April 1, 2025",
                Title: "Expense Report Approval",
                Category: "Travel & Accommodation",
                Amount: "$1,250.00",
                BusinessUnit: "Engineering",
                DueDate: "April 8, 2025",
                Description: "Team offsite travel expenses.")),
            TeamsAdaptiveCards.CreateStatusUpdateCard(new StatusUpdateCardInput(
                CardTitle: "Project Status Update",
                TeamName: "Teams Engineering",
                UpdateDate: "April 5, 2025",
                Project: "Q2 Feature Release",
                Status: "🟡 At Risk",
                Sprint: "Sprint 14 of 16",
                Completion: "68%",
                UpdatedBy: "Jordan Lee",
                Notes: "The authentication module integration is behind schedule.",
                ProjectUrl: "https://example.com/projects/q2-release")),
            TeamsAdaptiveCards.CreateTaskUpdateCard(new TaskUpdateCardInput(
                TaskName: "Implement OAuth2 token refresh flow",
                Project: "Q2 Feature Release",
                AssignedBy: "Sam Rivera",
                DueDate: "April 11, 2025",
                Estimate: "3 days",
                Priority: "🔴 High",
                Description: "Implement the silent token refresh mechanism.",
                TaskUrl: "https://example.com/tasks/oauth2-token-refresh")),
            TeamsAdaptiveCards.CreateMeetingReminderCard(new MeetingReminderCardInput(
                MeetingTitle: "Q2 Planning Kickoff",
                Organizer: "Alex Chen",
                Date: "Monday, April 7, 2025",
                Time: "2:00 PM – 3:00 PM (PST)",
                Location: "Microsoft Teams",
                Attendees: "12 people",
                Agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
                JoinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
                DetailsUrl: "https://example.com/calendar/q2-planning")),
            TeamsAdaptiveCards.CreateExpenseReportCard(new ExpenseReportCardInput(
                EmployeeName: "Chris Morgan",
                EmployeeJobTitle: "Senior Developer • Engineering",
                ReportId: "EXP-2025-0412",
                SubmittedDate: "April 5, 2025",
                Category: "Conference & Training",
                TotalAmount: "$3,480.00",
                Currency: "USD",
                Description: "Attendance at Microsoft Build 2025.",
                ReportUrl: "https://example.com/expenses/EXP-2025-0412"))
        };

        // Act & Assert
        foreach (var card in cards)
        {
            var json = card.ToJson();
            Assert.NotNull(json);
            Assert.NotEmpty(json);

            var deserialized = AdaptiveCardExtensions.FromJson(json);
            Assert.NotNull(deserialized);
        }
    }
}
