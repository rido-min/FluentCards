using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ExecuteActionTests
{
    [Fact]
    public void ExecuteAction_WithVerb_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction 
                { 
                    Title = "Execute",
                    Verb = "doAction"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Execute\"", json);
        Assert.Contains("\"title\": \"Execute\"", json);
        Assert.Contains("\"verb\": \"doAction\"", json);
    }

    [Fact]
    public void ExecuteAction_WithData_SerializesCorrectly()
    {
        // Arrange
        var dataObject = JsonDocument.Parse("{\"command\": \"refresh\", \"targetId\": \"123\"}").RootElement;
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction 
                { 
                    Title = "Refresh",
                    Verb = "refresh",
                    Data = dataObject
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Execute\"", json);
        Assert.Contains("\"verb\": \"refresh\"", json);
        Assert.Contains("\"data\":", json);
        Assert.Contains("\"command\": \"refresh\"", json);
        Assert.Contains("\"targetId\": \"123\"", json);
    }

    [Fact]
    public void ExecuteAction_RoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var dataObject = JsonDocument.Parse("{\"value\": 42}").RootElement;
        var originalCard = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction
                {
                    Id = "execute1",
                    Title = "Execute Action",
                    IconUrl = "https://example.com/execute.png",
                    Style = ActionStyle.Destructive,
                    Verb = "deleteItem",
                    Data = dataObject,
                    AssociatedInputs = AssociatedInputs.None,
                    IsEnabled = false,
                    Tooltip = "Execute this action"
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Actions);
        Assert.Single(deserializedCard.Actions);
        
        var action = deserializedCard.Actions[0] as ExecuteAction;
        Assert.NotNull(action);
        Assert.Equal("execute1", action.Id);
        Assert.Equal("Execute Action", action.Title);
        Assert.Equal("https://example.com/execute.png", action.IconUrl);
        Assert.Equal(ActionStyle.Destructive, action.Style);
        Assert.Equal("deleteItem", action.Verb);
        Assert.NotNull(action.Data);
        Assert.Equal(42, action.Data.Value.GetProperty("value").GetInt32());
        Assert.Equal(AssociatedInputs.None, action.AssociatedInputs);
        Assert.False(action.IsEnabled);
        Assert.Equal("Execute this action", action.Tooltip);
    }

    [Fact]
    public void ExecuteAction_WithBuilder_CreatesCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Execute")
                .WithId("exec1")
                .WithStyle(ActionStyle.Positive))
            .Build();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);
        var action = card.Actions[0] as ExecuteAction;
        Assert.NotNull(action);
        Assert.Equal("exec1", action.Id);
        Assert.Equal("Execute", action.Title);
        Assert.Equal(ActionStyle.Positive, action.Style);
    }

    [Fact]
    public void ExecuteAction_AssociatedInputsAuto_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction 
                { 
                    Title = "Submit",
                    Verb = "submit",
                    AssociatedInputs = AssociatedInputs.Auto
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Execute\"", json);
        Assert.Contains("\"associatedInputs\": \"auto\"", json);
    }

    [Fact]
    public void ExecuteAction_VeryLongVerb_SerializesCorrectly()
    {
        // Arrange
        var longVerb = new string('a', 1000);
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction 
                { 
                    Title = "Execute",
                    Verb = longVerb
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var action = deserializedCard.Actions![0] as ExecuteAction;
        Assert.NotNull(action);
        Assert.Equal(1000, action.Verb?.Length);
        Assert.Equal(longVerb, action.Verb);
    }

    [Fact]
    public void ExecuteAction_ComplexNestedData_SerializesCorrectly()
    {
        // Arrange
        var complexData = JsonDocument.Parse(@"{
            ""operation"": ""update"",
            ""items"": [
                { ""id"": 1, ""name"": ""Item 1"" },
                { ""id"": 2, ""name"": ""Item 2"" }
            ],
            ""metadata"": {
                ""timestamp"": ""2024-01-01T00:00:00Z"",
                ""user"": ""admin""
            }
        }").RootElement;
        
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction 
                { 
                    Title = "Update",
                    Verb = "update",
                    Data = complexData
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var action = deserializedCard.Actions![0] as ExecuteAction;
        Assert.NotNull(action);
        Assert.NotNull(action.Data);
        
        Assert.Equal("update", action.Data.Value.GetProperty("operation").GetString());
        var items = action.Data.Value.GetProperty("items");
        Assert.Equal(2, items.GetArrayLength());
        Assert.Equal("Item 1", items[0].GetProperty("name").GetString());
    }

    [Fact]
    public void ExecuteAction_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction { Title = "Execute" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"verb\":", json);
        Assert.DoesNotContain("\"data\":", json);
        Assert.DoesNotContain("\"associatedInputs\":", json);
        Assert.DoesNotContain("\"iconUrl\":", json);
        Assert.DoesNotContain("\"style\":", json);
        Assert.DoesNotContain("\"isEnabled\":", json);
        Assert.DoesNotContain("\"tooltip\":", json);
    }

    [Fact]
    public void ExecuteAction_AllActionStyles_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction { Title = "Default", Style = ActionStyle.Default, Verb = "default" },
                new ExecuteAction { Title = "Positive", Style = ActionStyle.Positive, Verb = "positive" },
                new ExecuteAction { Title = "Destructive", Style = ActionStyle.Destructive, Verb = "destructive" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"default\"", json);
        Assert.Contains("\"style\": \"positive\"", json);
        Assert.Contains("\"style\": \"destructive\"", json);
    }
}
