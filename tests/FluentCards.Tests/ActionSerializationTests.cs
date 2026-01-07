using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ActionSerializationTests
{
    [Fact]
    public void PolymorphicDeserialization_CardWithMixedActionTypes_DeserializesCorrectly()
    {
        // Arrange
        var jsonWithMixedActions = @"{
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""body"": [
                { ""type"": ""TextBlock"", ""text"": ""Select an action:"" }
            ],
            ""actions"": [
                {
                    ""type"": ""Action.OpenUrl"",
                    ""title"": ""Visit Website"",
                    ""url"": ""https://example.com""
                },
                {
                    ""type"": ""Action.Submit"",
                    ""title"": ""Submit Form"",
                    ""data"": { ""formId"": ""123"" }
                },
                {
                    ""type"": ""Action.ShowCard"",
                    ""title"": ""Show More"",
                    ""card"": {
                        ""type"": ""AdaptiveCard"",
                        ""version"": ""1.5"",
                        ""body"": [
                            { ""type"": ""TextBlock"", ""text"": ""Details"" }
                        ]
                    }
                },
                {
                    ""type"": ""Action.ToggleVisibility"",
                    ""title"": ""Toggle"",
                    ""targetElements"": [""element1"", ""element2""]
                },
                {
                    ""type"": ""Action.Execute"",
                    ""title"": ""Execute"",
                    ""verb"": ""doAction""
                }
            ]
        }";

        // Act
        var card = AdaptiveCardExtensions.FromJson(jsonWithMixedActions);

        // Assert
        Assert.NotNull(card);
        Assert.NotNull(card.Actions);
        Assert.Equal(5, card.Actions.Count);
        
        Assert.IsType<OpenUrlAction>(card.Actions[0]);
        Assert.IsType<SubmitAction>(card.Actions[1]);
        Assert.IsType<ShowCardAction>(card.Actions[2]);
        Assert.IsType<ToggleVisibilityAction>(card.Actions[3]);
        Assert.IsType<ExecuteAction>(card.Actions[4]);
        
        var openUrlAction = card.Actions[0] as OpenUrlAction;
        Assert.Equal("Visit Website", openUrlAction!.Title);
        Assert.Equal("https://example.com", openUrlAction.Url);
        
        var submitAction = card.Actions[1] as SubmitAction;
        Assert.Equal("Submit Form", submitAction!.Title);
        Assert.NotNull(submitAction.Data);
        
        var showCardAction = card.Actions[2] as ShowCardAction;
        Assert.Equal("Show More", showCardAction!.Title);
        Assert.NotNull(showCardAction.Card);
        
        var toggleAction = card.Actions[3] as ToggleVisibilityAction;
        Assert.Equal("Toggle", toggleAction!.Title);
        Assert.NotNull(toggleAction.TargetElements);
        Assert.Equal(2, toggleAction.TargetElements.Count);
        
        var executeAction = card.Actions[4] as ExecuteAction;
        Assert.Equal("Execute", executeAction!.Title);
        Assert.Equal("doAction", executeAction.Verb);
    }

    [Fact]
    public void PolymorphicSerialization_ActionShowCard_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Main card" }
            },
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction
                {
                    Title = "Show Details",
                    Card = new AdaptiveCard
                    {
                        Body = new List<AdaptiveElement>
                        {
                            new TextBlock { Text = "Detail view" }
                        },
                        Actions = new List<AdaptiveAction>
                        {
                            new SubmitAction { Title = "Submit" },
                            new OpenUrlAction { Title = "Learn More", Url = "https://example.com" }
                        }
                    }
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
        
        var showCardAction = deserializedCard.Actions[0] as ShowCardAction;
        Assert.NotNull(showCardAction);
        Assert.NotNull(showCardAction.Card);
        Assert.NotNull(showCardAction.Card.Actions);
        Assert.Equal(2, showCardAction.Card.Actions.Count);
        Assert.IsType<SubmitAction>(showCardAction.Card.Actions[0]);
        Assert.IsType<OpenUrlAction>(showCardAction.Card.Actions[1]);
    }

    [Fact]
    public void PolymorphicDeserialization_UnknownActionType_ThrowsException()
    {
        // Arrange
        var jsonWithUnknownAction = @"{
            ""type"": ""AdaptiveCard"",
            ""version"": ""1.5"",
            ""body"": [
                { ""type"": ""TextBlock"", ""text"": ""Test"" }
            ],
            ""actions"": [
                {
                    ""type"": ""Action.OpenUrl"",
                    ""title"": ""Known Action"",
                    ""url"": ""https://example.com""
                },
                {
                    ""type"": ""Action.Unknown"",
                    ""title"": ""Unknown Action""
                }
            ]
        }";

        // Act & Assert - .NET's polymorphic deserialization throws on unknown types
        var exception = Record.Exception(() => AdaptiveCardExtensions.FromJson(jsonWithUnknownAction));
        
        Assert.NotNull(exception);
        Assert.IsType<JsonException>(exception);
    }

    [Fact]
    public void ActionStyle_SerializesAsCamelCase()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Positive", Style = ActionStyle.Positive },
                new ExecuteAction { Title = "Destructive", Style = ActionStyle.Destructive, Verb = "delete" },
                new ToggleVisibilityAction { Title = "Default", Style = ActionStyle.Default }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"positive\"", json);
        Assert.Contains("\"style\": \"destructive\"", json);
        Assert.Contains("\"style\": \"default\"", json);
    }

    [Fact]
    public void AssociatedInputs_SerializesAsCamelCase()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Auto", AssociatedInputs = AssociatedInputs.Auto },
                new ExecuteAction { Title = "None", Verb = "execute", AssociatedInputs = AssociatedInputs.None }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"associatedInputs\": \"auto\"", json);
        Assert.Contains("\"associatedInputs\": \"none\"", json);
    }

    [Fact]
    public void DefaultStyle_OmittedFromJson()
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
        Assert.DoesNotContain("\"style\":", json);
    }

    [Fact]
    public void AllActionProperties_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction
                {
                    Id = "submit1",
                    Title = "Submit",
                    IconUrl = "https://example.com/icon.png",
                    Style = ActionStyle.Positive,
                    IsEnabled = false,
                    Tooltip = "Submit the form"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"submit1\"", json);
        Assert.Contains("\"title\": \"Submit\"", json);
        Assert.Contains("\"iconUrl\": \"https://example.com/icon.png\"", json);
        Assert.Contains("\"style\": \"positive\"", json);
        Assert.Contains("\"isEnabled\": false", json);
        Assert.Contains("\"tooltip\": \"Submit the form\"", json);
    }

    [Fact]
    public void SpecialCharactersInTooltip_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction
                {
                    Title = "Submit",
                    Tooltip = "Click to submit\nNew line\tTab\"Quotes\""
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
        Assert.Equal("Click to submit\nNew line\tTab\"Quotes\"", action.Tooltip);
    }

    [Fact]
    public void MultipleActionsOfSameType_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Submit 1", Id = "s1" },
                new SubmitAction { Title = "Submit 2", Id = "s2" },
                new SubmitAction { Title = "Submit 3", Id = "s3" }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Actions);
        Assert.Equal(3, deserializedCard.Actions.Count);
        Assert.All(deserializedCard.Actions, action => Assert.IsType<SubmitAction>(action));
        Assert.Equal("s1", deserializedCard.Actions[0].Id);
        Assert.Equal("s2", deserializedCard.Actions[1].Id);
        Assert.Equal("s3", deserializedCard.Actions[2].Id);
    }

    [Fact]
    public void ActionWithIsEnabledTrue_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Enabled", IsEnabled = true }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isEnabled\": true", json);
    }

    [Fact]
    public void ActionWithIsEnabledFalse_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ExecuteAction { Title = "Disabled", Verb = "action", IsEnabled = false }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isEnabled\": false", json);
    }

    [Fact]
    public void ComplexCardWithAllActionTypes_RoundtripCorrectly()
    {
        // Arrange
        var dataObject = JsonDocument.Parse("{\"value\": 123}").RootElement;
        var originalCard = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Main Content", Id = "main" }
            },
            Actions = new List<AdaptiveAction>
            {
                new OpenUrlAction { Title = "Open", Url = "https://example.com", Style = ActionStyle.Default },
                new SubmitAction { Title = "Submit", Data = dataObject, Style = ActionStyle.Positive },
                new ShowCardAction 
                { 
                    Title = "Show", 
                    Card = new AdaptiveCard 
                    { 
                        Body = new List<AdaptiveElement> { new TextBlock { Text = "Inner" } } 
                    }
                },
                new ToggleVisibilityAction { Title = "Toggle", TargetElements = new List<object> { "main" } },
                new ExecuteAction { Title = "Execute", Verb = "exec", Style = ActionStyle.Destructive }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.Equal(5, deserializedCard.Actions!.Count);
        Assert.IsType<OpenUrlAction>(deserializedCard.Actions[0]);
        Assert.IsType<SubmitAction>(deserializedCard.Actions[1]);
        Assert.IsType<ShowCardAction>(deserializedCard.Actions[2]);
        Assert.IsType<ToggleVisibilityAction>(deserializedCard.Actions[3]);
        Assert.IsType<ExecuteAction>(deserializedCard.Actions[4]);
        
        // Verify styles are preserved
        Assert.Equal(ActionStyle.Default, deserializedCard.Actions[0].Style);
        Assert.Equal(ActionStyle.Positive, deserializedCard.Actions[1].Style);
        Assert.Equal(ActionStyle.Destructive, deserializedCard.Actions[4].Style);
    }
}
