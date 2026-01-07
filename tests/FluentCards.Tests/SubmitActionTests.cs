using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class SubmitActionTests
{
    [Fact]
    public void SubmitAction_WithTitleOnly_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Submit Form" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"title\": \"Submit Form\"", json);
    }

    [Fact]
    public void SubmitAction_WithDataObject_SerializesCorrectly()
    {
        // Arrange
        var dataObject = JsonDocument.Parse("{\"formId\": \"123\", \"action\": \"save\"}").RootElement;
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction 
                { 
                    Title = "Save",
                    Data = dataObject
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"data\":", json);
        Assert.Contains("\"formId\": \"123\"", json);
        Assert.Contains("\"action\": \"save\"", json);
    }

    [Fact]
    public void SubmitAction_WithAssociatedInputs_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction 
                { 
                    Title = "Submit",
                    AssociatedInputs = AssociatedInputs.None
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"associatedInputs\": \"none\"", json);
    }

    [Fact]
    public void SubmitAction_RoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var dataObject = JsonDocument.Parse("{\"key\": \"value\"}").RootElement;
        var originalCard = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction
                {
                    Id = "submit1",
                    Title = "Submit",
                    IconUrl = "https://example.com/icon.png",
                    Style = ActionStyle.Positive,
                    Data = dataObject,
                    AssociatedInputs = AssociatedInputs.Auto,
                    IsEnabled = true,
                    Tooltip = "Click to submit"
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
        
        var action = deserializedCard.Actions[0] as SubmitAction;
        Assert.NotNull(action);
        Assert.Equal("submit1", action.Id);
        Assert.Equal("Submit", action.Title);
        Assert.Equal("https://example.com/icon.png", action.IconUrl);
        Assert.Equal(ActionStyle.Positive, action.Style);
        Assert.Equal(AssociatedInputs.Auto, action.AssociatedInputs);
        Assert.True(action.IsEnabled);
        Assert.Equal("Click to submit", action.Tooltip);
        Assert.NotNull(action.Data);
    }

    [Fact]
    public void SubmitAction_DataProperty_HandlesComplexNestedObjects()
    {
        // Arrange
        var complexData = JsonDocument.Parse(@"{
            ""user"": {
                ""id"": 123,
                ""name"": ""John Doe"",
                ""roles"": [""admin"", ""user""]
            },
            ""settings"": {
                ""notifications"": true,
                ""theme"": ""dark""
            }
        }").RootElement;
        
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction 
                { 
                    Title = "Submit",
                    Data = complexData
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var action = deserializedCard.Actions![0] as SubmitAction;
        Assert.NotNull(action);
        Assert.NotNull(action.Data);
        
        // Verify nested structure
        var userData = action.Data.Value.GetProperty("user");
        Assert.Equal(123, userData.GetProperty("id").GetInt32());
        Assert.Equal("John Doe", userData.GetProperty("name").GetString());
    }

    [Fact]
    public void SubmitAction_WithBuilder_CreatesCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Submit Form")
                .WithId("submit1")
                .WithStyle(ActionStyle.Positive))
            .Build();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);
        var action = card.Actions[0] as SubmitAction;
        Assert.NotNull(action);
        Assert.Equal("submit1", action.Id);
        Assert.Equal("Submit Form", action.Title);
        Assert.Equal(ActionStyle.Positive, action.Style);
    }

    [Fact]
    public void SubmitAction_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Submit" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"data\":", json);
        Assert.DoesNotContain("\"associatedInputs\":", json);
        Assert.DoesNotContain("\"iconUrl\":", json);
        Assert.DoesNotContain("\"style\":", json);
        Assert.DoesNotContain("\"isEnabled\":", json);
        Assert.DoesNotContain("\"tooltip\":", json);
    }
}
