using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ToggleVisibilityActionTests
{
    [Fact]
    public void ToggleVisibilityAction_WithStringTargets_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction 
                { 
                    Title = "Toggle",
                    TargetElements = new List<object> { "element1", "element2", "element3" }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ToggleVisibility\"", json);
        Assert.Contains("\"title\": \"Toggle\"", json);
        Assert.Contains("\"targetElements\":", json);
        Assert.Contains("element1", json);
        Assert.Contains("element2", json);
        Assert.Contains("element3", json);
    }

    [Fact]
    public void ToggleVisibilityAction_WithTargetElementObjects_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction 
                { 
                    Title = "Toggle",
                    TargetElements = new List<object> 
                    { 
                        new TargetElement { ElementId = "element1", IsVisible = true },
                        new TargetElement { ElementId = "element2", IsVisible = false },
                        new TargetElement { ElementId = "element3", IsVisible = null }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ToggleVisibility\"", json);
        Assert.Contains("\"elementId\": \"element1\"", json);
        Assert.Contains("\"isVisible\": true", json);
        Assert.Contains("\"elementId\": \"element2\"", json);
        Assert.Contains("\"isVisible\": false", json);
        Assert.Contains("\"elementId\": \"element3\"", json);
    }

    [Fact]
    public void ToggleVisibilityAction_MixedStringAndObjects_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction 
                { 
                    Title = "Toggle Mix",
                    TargetElements = new List<object> 
                    { 
                        "element1",
                        new TargetElement { ElementId = "element2", IsVisible = true },
                        "element3"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ToggleVisibility\"", json);
        Assert.Contains("element1", json);
        Assert.Contains("\"elementId\": \"element2\"", json);
        Assert.Contains("\"isVisible\": true", json);
        Assert.Contains("element3", json);
    }

    [Fact]
    public void ToggleVisibilityAction_RoundtripSerialization_PreservesTargetElements()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Id = "text1", Text = "Element 1" },
                new TextBlock { Id = "text2", Text = "Element 2" }
            },
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction
                {
                    Id = "toggle1",
                    Title = "Toggle Elements",
                    IconUrl = "https://example.com/toggle.png",
                    Style = ActionStyle.Default,
                    TargetElements = new List<object> { "text1", "text2" },
                    IsEnabled = true,
                    Tooltip = "Toggle visibility"
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
        
        var action = deserializedCard.Actions[0] as ToggleVisibilityAction;
        Assert.NotNull(action);
        Assert.Equal("toggle1", action.Id);
        Assert.Equal("Toggle Elements", action.Title);
        Assert.Equal("https://example.com/toggle.png", action.IconUrl);
        Assert.Equal(ActionStyle.Default, action.Style);
        Assert.True(action.IsEnabled);
        Assert.Equal("Toggle visibility", action.Tooltip);
        Assert.NotNull(action.TargetElements);
        Assert.Equal(2, action.TargetElements.Count);
    }

    [Fact]
    public void ToggleVisibilityAction_WithBuilder_CreatesCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .ToggleVisibility("Show/Hide")
                .WithId("toggle1")
                .WithStyle(ActionStyle.Default))
            .Build();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);
        var action = card.Actions[0] as ToggleVisibilityAction;
        Assert.NotNull(action);
        Assert.Equal("toggle1", action.Id);
        Assert.Equal("Show/Hide", action.Title);
        Assert.Equal(ActionStyle.Default, action.Style);
    }

    [Fact]
    public void ToggleVisibilityAction_EmptyTargetElements_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction 
                { 
                    Title = "Toggle",
                    TargetElements = new List<object>()
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ToggleVisibility\"", json);
        Assert.Contains("\"targetElements\": []", json);
    }

    [Fact]
    public void ToggleVisibilityAction_NullTargetElements_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction { Title = "Toggle" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"targetElements\":", json);
    }

    [Fact]
    public void ToggleVisibilityAction_TargetElementNullIsVisible_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction 
                { 
                    Title = "Toggle",
                    TargetElements = new List<object> 
                    { 
                        new TargetElement { ElementId = "element1", IsVisible = null }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"elementId\": \"element1\"", json);
        // IsVisible should be omitted when null
        var lines = json.Split('\n');
        var elementIdLine = lines.FirstOrDefault(l => l.Contains("element1"));
        Assert.NotNull(elementIdLine);
        
        var elementIdLineIndex = Array.IndexOf(lines, elementIdLine);
        
        // Check the surrounding lines don't have isVisible
        for (int i = Math.Max(0, elementIdLineIndex - 2); i < Math.Min(lines.Length, elementIdLineIndex + 3); i++)
        {
            Assert.DoesNotContain("isVisible", lines[i]);
        }
    }

    [Fact]
    public void ToggleVisibilityAction_AllStyles_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ToggleVisibilityAction { Title = "Default", Style = ActionStyle.Default },
                new ToggleVisibilityAction { Title = "Positive", Style = ActionStyle.Positive },
                new ToggleVisibilityAction { Title = "Destructive", Style = ActionStyle.Destructive }
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
