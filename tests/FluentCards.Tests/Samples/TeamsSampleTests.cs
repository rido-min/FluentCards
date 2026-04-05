using FluentCards.Samples;
using Xunit;

namespace FluentCards.Tests.Samples;

public class TeamsSampleTests
{
    [Fact]
    public void TeamsSamples_CreateApprovalCard_ProducesValidCard()
    {
        // Act
        var card = TeamsSamples.CreateApprovalCard();

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
    public void TeamsSamples_CreateApprovalCard_HasApproveAndDeclineActions()
    {
        // Act
        var card = TeamsSamples.CreateApprovalCard();

        // Assert
        Assert.NotNull(card.Actions);
        var submitActions = card.Actions.OfType<SubmitAction>().ToList();
        Assert.Equal(2, submitActions.Count);
        Assert.Contains(submitActions, a => a.Title == "Approve" && a.Style == ActionStyle.Positive);
        Assert.Contains(submitActions, a => a.Title == "Decline" && a.Style == ActionStyle.Destructive);
    }

    [Fact]
    public void TeamsSamples_CreateStatusUpdateCard_ProducesValidCard()
    {
        // Act
        var card = TeamsSamples.CreateStatusUpdateCard();

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
    public void TeamsSamples_CreateStatusUpdateCard_HasViewProjectAction()
    {
        // Act
        var card = TeamsSamples.CreateStatusUpdateCard();

        // Assert
        Assert.NotNull(card.Actions);
        var openUrl = Assert.Single(card.Actions.OfType<OpenUrlAction>());
        Assert.Equal("View Project", openUrl.Title);
        Assert.NotNull(openUrl.Url);
    }

    [Fact]
    public void TeamsSamples_CreateTaskUpdateCard_ProducesValidCard()
    {
        // Act
        var card = TeamsSamples.CreateTaskUpdateCard();

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
    public void TeamsSamples_CreateTaskUpdateCard_HasViewAndAcknowledgeActions()
    {
        // Act
        var card = TeamsSamples.CreateTaskUpdateCard();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Task" });
        Assert.Contains(card.Actions, a => a is SubmitAction { Title: "Acknowledge" });
    }

    [Fact]
    public void TeamsSamples_CreateMeetingReminderCard_ProducesValidCard()
    {
        // Act
        var card = TeamsSamples.CreateMeetingReminderCard();

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
    public void TeamsSamples_CreateMeetingReminderCard_HasJoinMeetingAction()
    {
        // Act
        var card = TeamsSamples.CreateMeetingReminderCard();

        // Assert
        Assert.NotNull(card.Actions);
        var joinAction = card.Actions.OfType<OpenUrlAction>().First(a => a.Title == "Join Meeting");
        Assert.Equal(ActionStyle.Positive, joinAction.Style);
        Assert.NotNull(joinAction.Url);
    }

    [Fact]
    public void TeamsSamples_CreateExpenseReportCard_ProducesValidCard()
    {
        // Act
        var card = TeamsSamples.CreateExpenseReportCard();

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
    public void TeamsSamples_CreateExpenseReportCard_HasApproveRejectAndViewActions()
    {
        // Act
        var card = TeamsSamples.CreateExpenseReportCard();

        // Assert
        Assert.NotNull(card.Actions);
        var submitActions = card.Actions.OfType<SubmitAction>().ToList();
        Assert.Contains(submitActions, a => a.Title == "Approve" && a.Style == ActionStyle.Positive);
        Assert.Contains(submitActions, a => a.Title == "Reject" && a.Style == ActionStyle.Destructive);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Report" });
    }

    [Fact]
    public void AllTeamsSamples_SerializeToValidJson()
    {
        // Arrange
        var cards = new[]
        {
            TeamsSamples.CreateApprovalCard(),
            TeamsSamples.CreateStatusUpdateCard(),
            TeamsSamples.CreateTaskUpdateCard(),
            TeamsSamples.CreateMeetingReminderCard(),
            TeamsSamples.CreateExpenseReportCard()
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
