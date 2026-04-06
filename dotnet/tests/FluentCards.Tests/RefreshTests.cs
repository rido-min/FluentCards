using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class RefreshTests
{
    [Fact]
    public void CardWithRefreshAction_Serialization_ContainsRefreshProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration
            {
                Action = new ExecuteAction
                {
                    Verb = "refreshData"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"refresh\":", json);
        Assert.Contains("\"action\":", json);
        Assert.Contains("\"verb\": \"refreshData\"", json);
        Assert.Contains("\"type\": \"Action.Execute\"", json);
    }

    [Fact]
    public void RefreshWithUserIds_Serialization_ContainsUserIdsArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration
            {
                UserIds = new List<string> { "user1", "user2", "user3" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"refresh\":", json);
        Assert.Contains("\"userIds\":", json);
        Assert.Contains("\"user1\"", json);
        Assert.Contains("\"user2\"", json);
        Assert.Contains("\"user3\"", json);
    }

    [Fact]
    public void RefreshWithExpires_Serialization_ContainsExpiresProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration
            {
                Expires = "2026-12-31T23:59:59Z"
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"refresh\":", json);
        Assert.Contains("\"expires\": \"2026-12-31T23:59:59Z\"", json);
    }

    [Fact]
    public void RoundtripSerialization_WithRefresh_PreservesConfiguration()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration
            {
                Action = new ExecuteAction
                {
                    Verb = "doRefresh",
                    Title = "Refresh"
                },
                UserIds = new List<string> { "alice", "bob" },
                Expires = "2026-06-15T12:00:00Z"
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Refresh);
        Assert.NotNull(deserializedCard.Refresh.Action);
        var executeAction = deserializedCard.Refresh.Action as ExecuteAction;
        Assert.NotNull(executeAction);
        Assert.Equal("doRefresh", executeAction.Verb);
        Assert.Equal("Refresh", deserializedCard.Refresh.Action.Title);
        Assert.NotNull(deserializedCard.Refresh.UserIds);
        Assert.Equal(2, deserializedCard.Refresh.UserIds.Count);
        Assert.Contains("alice", deserializedCard.Refresh.UserIds);
        Assert.Contains("bob", deserializedCard.Refresh.UserIds);
        Assert.Equal("2026-06-15T12:00:00Z", deserializedCard.Refresh.Expires);
    }

    [Fact]
    public void EmptyRefreshObject_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration()
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Refresh);
        Assert.Null(deserializedCard.Refresh.Action);
        Assert.Null(deserializedCard.Refresh.UserIds);
        Assert.Null(deserializedCard.Refresh.Expires);
    }

    [Fact]
    public void CardWithoutRefresh_DoesNotSerializeRefreshProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"refresh\":", json);
    }

    [Fact]
    public void RefreshWithActionAndData_Serialization_ContainsDataProperty()
    {
        // Arrange
        var dataElement = JsonDocument.Parse("{\"key\": \"value\"}").RootElement;
        var card = new AdaptiveCard
        {
            Refresh = new RefreshConfiguration
            {
                Action = new ExecuteAction
                {
                    Verb = "getData",
                    Data = dataElement
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"refresh\":", json);
        Assert.Contains("\"action\":", json);
        Assert.Contains("\"data\":", json);
        Assert.Contains("\"key\": \"value\"", json);
    }
}
