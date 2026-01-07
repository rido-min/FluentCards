using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ActionSetTests
{
    [Fact]
    public void ActionSet_WithMultipleActions_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new SubmitAction { Title = "Submit" },
                        new OpenUrlAction { Title = "Open", Url = "https://example.com" },
                        new ExecuteAction { Title = "Execute", Verb = "doAction" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"ActionSet\"", json);
        Assert.Contains("\"actions\":", json);
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
        Assert.Contains("\"type\": \"Action.Execute\"", json);
    }

    [Fact]
    public void ActionSet_WithDifferentActionTypes_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new OpenUrlAction { Title = "Website", Url = "https://example.com" },
                        new SubmitAction { Title = "Submit Form", Data = JsonDocument.Parse("{\"id\":1}").RootElement },
                        new ShowCardAction
                        {
                            Title = "Show Details",
                            Card = new AdaptiveCard
                            {
                                Body = new List<AdaptiveElement> { new TextBlock { Text = "Details" } }
                            }
                        },
                        new ToggleVisibilityAction { Title = "Toggle", TargetElements = new List<object> { "elem1" } }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"title\": \"Website\"", json);
        Assert.Contains("\"title\": \"Submit Form\"", json);
        Assert.Contains("\"title\": \"Show Details\"", json);
        Assert.Contains("\"title\": \"Toggle\"", json);
        Assert.Contains("\"type\": \"Action.ShowCard\"", json);
        Assert.Contains("\"type\": \"Action.ToggleVisibility\"", json);
    }

    [Fact]
    public void ActionSet_WithEmptyActions_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Actions = new List<AdaptiveAction>()
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var actionSet = deserializedCard.Body![0] as ActionSet;
        Assert.NotNull(actionSet);
        Assert.NotNull(actionSet.Actions);
        Assert.Empty(actionSet.Actions);
    }

    [Fact]
    public void ActionSet_RoundtripSerialization_PreservesActionOrderAndTypes()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Id = "actionSet1",
                    Actions = new List<AdaptiveAction>
                    {
                        new SubmitAction { Id = "submit1", Title = "Submit" },
                        new OpenUrlAction { Id = "open1", Title = "Open", Url = "https://example.com" },
                        new ExecuteAction { Id = "exec1", Title = "Execute", Verb = "action" }
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);

        var actionSet = deserializedCard.Body[0] as ActionSet;
        Assert.NotNull(actionSet);
        Assert.Equal("actionSet1", actionSet.Id);
        Assert.NotNull(actionSet.Actions);
        Assert.Equal(3, actionSet.Actions.Count);

        Assert.IsType<SubmitAction>(actionSet.Actions[0]);
        Assert.Equal("submit1", actionSet.Actions[0].Id);

        Assert.IsType<OpenUrlAction>(actionSet.Actions[1]);
        Assert.Equal("open1", actionSet.Actions[1].Id);

        Assert.IsType<ExecuteAction>(actionSet.Actions[2]);
        Assert.Equal("exec1", actionSet.Actions[2].Id);
    }

    [Fact]
    public void ActionSet_WithActionStyles_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new SubmitAction { Title = "Submit", Style = ActionStyle.Positive },
                        new ExecuteAction { Title = "Delete", Verb = "delete", Style = ActionStyle.Destructive }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"positive\"", json);
        Assert.Contains("\"style\": \"destructive\"", json);
    }

    [Fact]
    public void ActionSet_WithAdaptiveElementProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new ActionSet
                {
                    Id = "actions1",
                    IsVisible = true,
                    Spacing = "large",
                    Separator = true,
                    Actions = new List<AdaptiveAction>
                    {
                        new SubmitAction { Title = "Action" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"actions1\"", json);
        Assert.Contains("\"isVisible\": true", json);
        Assert.Contains("\"spacing\": \"large\"", json);
        Assert.Contains("\"separator\": true", json);
    }

    [Fact]
    public void ActionSet_NestedWithOtherElements_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Header" },
                new ActionSet
                {
                    Actions = new List<AdaptiveAction>
                    {
                        new SubmitAction { Title = "Submit" }
                    }
                },
                new TextBlock { Text = "Footer" }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Equal(3, deserializedCard.Body.Count);
        Assert.IsType<TextBlock>(deserializedCard.Body[0]);
        Assert.IsType<ActionSet>(deserializedCard.Body[1]);
        Assert.IsType<TextBlock>(deserializedCard.Body[2]);
    }
}
