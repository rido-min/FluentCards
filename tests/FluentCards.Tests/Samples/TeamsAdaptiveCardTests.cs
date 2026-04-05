using Xunit;

namespace FluentCards.Tests.Samples;

public class TeamsAdaptiveCardTests
{
    [Fact]
    public void TeamsAdaptiveCards_CreateApprovalCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateApprovalCard(
            requesterName: "Mia Alvarez",
            submittedDate: "Submitted April 1, 2025",
            title: "Expense Report Approval",
            category: "Travel & Accommodation",
            amount: "$1,250.00",
            businessUnit: "Engineering",
            dueDate: "April 8, 2025",
            description: "Team offsite travel expenses.",
            requesterImageUrl: "https://adaptivecards.io/content/cats/1.png");

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
        var card = TeamsAdaptiveCards.CreateApprovalCard(
            requesterName: "Mia Alvarez",
            submittedDate: "Submitted April 1, 2025",
            title: "Expense Report Approval",
            category: "Travel & Accommodation",
            amount: "$1,250.00",
            businessUnit: "Engineering",
            dueDate: "April 8, 2025",
            description: "Team offsite travel expenses.");

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
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard(
            cardTitle: "Project Status Update",
            teamName: "Teams Engineering",
            updateDate: "April 5, 2025",
            project: "Q2 Feature Release",
            status: "🟡 At Risk",
            sprint: "Sprint 14 of 16",
            completion: "68%",
            updatedBy: "Jordan Lee",
            notes: "The authentication module integration is behind schedule.",
            projectUrl: "https://example.com/projects/q2-release");

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
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard(
            cardTitle: "Project Status Update",
            teamName: "Teams Engineering",
            updateDate: "April 5, 2025",
            project: "Q2 Feature Release",
            status: "🟡 At Risk",
            sprint: "Sprint 14 of 16",
            completion: "68%",
            updatedBy: "Jordan Lee",
            notes: "The authentication module integration is behind schedule.",
            projectUrl: "https://example.com/projects/q2-release");

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
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard(
            taskName: "Implement OAuth2 token refresh flow",
            project: "Q2 Feature Release",
            assignedBy: "Sam Rivera",
            dueDate: "April 11, 2025",
            estimate: "3 days",
            priority: "🔴 High",
            description: "Implement the silent token refresh mechanism.",
            taskUrl: "https://example.com/tasks/oauth2-token-refresh");

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
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard(
            taskName: "Implement OAuth2 token refresh flow",
            project: "Q2 Feature Release",
            assignedBy: "Sam Rivera",
            dueDate: "April 11, 2025",
            estimate: "3 days",
            priority: "🔴 High",
            description: "Implement the silent token refresh mechanism.",
            taskUrl: "https://example.com/tasks/oauth2-token-refresh");

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Task" });
        Assert.Contains(card.Actions, a => a is SubmitAction { Title: "Acknowledge" });
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateMeetingReminderCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard(
            meetingTitle: "Q2 Planning Kickoff",
            organizer: "Alex Chen",
            date: "Monday, April 7, 2025",
            time: "2:00 PM – 3:00 PM (PST)",
            location: "Microsoft Teams",
            attendees: "12 people",
            agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
            joinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
            detailsUrl: "https://example.com/calendar/q2-planning");

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
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard(
            meetingTitle: "Q2 Planning Kickoff",
            organizer: "Alex Chen",
            date: "Monday, April 7, 2025",
            time: "2:00 PM – 3:00 PM (PST)",
            location: "Microsoft Teams",
            attendees: "12 people",
            agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
            joinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
            detailsUrl: "https://example.com/calendar/q2-planning");

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
        var card = TeamsAdaptiveCards.CreateExpenseReportCard(
            employeeName: "Chris Morgan",
            employeeJobTitle: "Senior Developer • Engineering",
            reportId: "EXP-2025-0412",
            submittedDate: "April 5, 2025",
            category: "Conference & Training",
            totalAmount: "$3,480.00",
            currency: "USD",
            description: "Attendance at Microsoft Build 2025.",
            reportUrl: "https://example.com/expenses/EXP-2025-0412",
            employeeImageUrl: "https://adaptivecards.io/content/cats/2.png");

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
        var card = TeamsAdaptiveCards.CreateExpenseReportCard(
            employeeName: "Chris Morgan",
            employeeJobTitle: "Senior Developer • Engineering",
            reportId: "EXP-2025-0412",
            submittedDate: "April 5, 2025",
            category: "Conference & Training",
            totalAmount: "$3,480.00",
            currency: "USD",
            description: "Attendance at Microsoft Build 2025.",
            reportUrl: "https://example.com/expenses/EXP-2025-0412");

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
            TeamsAdaptiveCards.CreateApprovalCard(
                requesterName: "Mia Alvarez",
                submittedDate: "Submitted April 1, 2025",
                title: "Expense Report Approval",
                category: "Travel & Accommodation",
                amount: "$1,250.00",
                businessUnit: "Engineering",
                dueDate: "April 8, 2025",
                description: "Team offsite travel expenses."),
            TeamsAdaptiveCards.CreateStatusUpdateCard(
                cardTitle: "Project Status Update",
                teamName: "Teams Engineering",
                updateDate: "April 5, 2025",
                project: "Q2 Feature Release",
                status: "🟡 At Risk",
                sprint: "Sprint 14 of 16",
                completion: "68%",
                updatedBy: "Jordan Lee",
                notes: "The authentication module integration is behind schedule.",
                projectUrl: "https://example.com/projects/q2-release"),
            TeamsAdaptiveCards.CreateTaskUpdateCard(
                taskName: "Implement OAuth2 token refresh flow",
                project: "Q2 Feature Release",
                assignedBy: "Sam Rivera",
                dueDate: "April 11, 2025",
                estimate: "3 days",
                priority: "🔴 High",
                description: "Implement the silent token refresh mechanism.",
                taskUrl: "https://example.com/tasks/oauth2-token-refresh"),
            TeamsAdaptiveCards.CreateMeetingReminderCard(
                meetingTitle: "Q2 Planning Kickoff",
                organizer: "Alex Chen",
                date: "Monday, April 7, 2025",
                time: "2:00 PM – 3:00 PM (PST)",
                location: "Microsoft Teams",
                attendees: "12 people",
                agenda: "Agenda: Review Q2 objectives, assign team leads, and align on delivery milestones.",
                joinUrl: "https://teams.microsoft.com/l/meetup-join/sample",
                detailsUrl: "https://example.com/calendar/q2-planning"),
            TeamsAdaptiveCards.CreateExpenseReportCard(
                employeeName: "Chris Morgan",
                employeeJobTitle: "Senior Developer • Engineering",
                reportId: "EXP-2025-0412",
                submittedDate: "April 5, 2025",
                category: "Conference & Training",
                totalAmount: "$3,480.00",
                currency: "USD",
                description: "Attendance at Microsoft Build 2025.",
                reportUrl: "https://example.com/expenses/EXP-2025-0412")
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
