using FluentCards.Samples;
using Xunit;

namespace FluentCards.Tests.Samples;

public class SampleCardTests
{
    [Fact]
    public void BasicCardSample_CreateWelcomeCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateWelcomeCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.NotEmpty(card.Body);
        Assert.NotNull(card.Actions);
        Assert.NotEmpty(card.Actions);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void BasicCardSample_CreateNotificationCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateNotificationCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void BasicCardSample_CreateImageCard_ProducesValidCard()
    {
        // Act
        var card = BasicCardSample.CreateImageCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Image);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateContactForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateContactForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is InputText);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateSurveyForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateSurveyForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is InputChoiceSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void FormCardSample_CreateRegistrationForm_ProducesValidCard()
    {
        // Act
        var card = FormCardSample.CreateRegistrationForm();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateTwoColumnCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateTwoColumnCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is ColumnSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateStyledContainerCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateStyledContainerCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Container);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateFactSetCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateFactSetCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is FactSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void LayoutCardSample_CreateNestedContainerCard_ProducesValidCard()
    {
        // Act
        var card = LayoutCardSample.CreateNestedContainerCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateRichTextCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateRichTextCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is RichTextBlock);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateImageSetCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateImageSetCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is ImageSet);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateTableCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateTableCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Table);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateMediaCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateMediaCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        Assert.NotNull(card.Body);
        Assert.Contains(card.Body, e => e is Media);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void RichContentSample_CreateComprehensiveCard_ProducesValidCard()
    {
        // Act
        var card = RichContentSample.CreateComprehensiveCard();

        // Assert
        Assert.NotNull(card);
        Assert.Equal("1.5", card.Version);
        
        // Validate the card
        var issues = AdaptiveCardValidator.Validate(card);
        Assert.Empty(issues);
    }

    [Fact]
    public void AllSamples_SerializeToValidJson()
    {
        // Arrange
        var cards = new[]
        {
            BasicCardSample.CreateWelcomeCard(),
            BasicCardSample.CreateNotificationCard(),
            BasicCardSample.CreateImageCard(),
            FormCardSample.CreateContactForm(),
            FormCardSample.CreateSurveyForm(),
            FormCardSample.CreateRegistrationForm(),
            LayoutCardSample.CreateTwoColumnCard(),
            LayoutCardSample.CreateStyledContainerCard(),
            LayoutCardSample.CreateFactSetCard(),
            LayoutCardSample.CreateNestedContainerCard(),
            RichContentSample.CreateRichTextCard(),
            RichContentSample.CreateImageSetCard(),
            RichContentSample.CreateTableCard(),
            RichContentSample.CreateMediaCard(),
            RichContentSample.CreateComprehensiveCard(),
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
            
            // Verify it can be deserialized back
            var deserializedCard = AdaptiveCardExtensions.FromJson(json);
            Assert.NotNull(deserializedCard);
        }
    }
}
