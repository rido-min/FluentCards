using Xunit;

namespace FluentCards.Tests.Samples;

public class TeamsAdaptiveCardTests
{
    [Fact]
    public void TeamsAdaptiveCards_CreateApprovalCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateApprovalCard();

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
        var card = TeamsAdaptiveCards.CreateApprovalCard();

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
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard();

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
        var card = TeamsAdaptiveCards.CreateStatusUpdateCard();

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
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard();

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
        var card = TeamsAdaptiveCards.CreateTaskUpdateCard();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Contains(card.Actions, a => a is OpenUrlAction { Title: "View Task" });
        Assert.Contains(card.Actions, a => a is SubmitAction { Title: "Acknowledge" });
    }

    [Fact]
    public void TeamsAdaptiveCards_CreateMeetingReminderCard_ProducesValidCard()
    {
        // Act
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard();

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
        var card = TeamsAdaptiveCards.CreateMeetingReminderCard();

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
        var card = TeamsAdaptiveCards.CreateExpenseReportCard();

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
        var card = TeamsAdaptiveCards.CreateExpenseReportCard();

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
            TeamsAdaptiveCards.CreateApprovalCard(),
            TeamsAdaptiveCards.CreateStatusUpdateCard(),
            TeamsAdaptiveCards.CreateTaskUpdateCard(),
            TeamsAdaptiveCards.CreateMeetingReminderCard(),
            TeamsAdaptiveCards.CreateExpenseReportCard()
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
