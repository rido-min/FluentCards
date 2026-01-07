using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ShowCardActionTests
{
    [Fact]
    public void ShowCardAction_WithNestedCard_SerializesCorrectly()
    {
        // Arrange
        var nestedCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "This is shown when the action is invoked" }
            }
        };

        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Show More",
                    Card = nestedCard
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ShowCard\"", json);
        Assert.Contains("\"title\": \"Show More\"", json);
        Assert.Contains("\"card\":", json);
        Assert.Contains("This is shown when the action is invoked", json);
    }

    [Fact]
    public void ShowCardAction_NestedCardContainsBodyElements_SerializesCorrectly()
    {
        // Arrange
        var nestedCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "First element" },
                new TextBlock { Text = "Second element" },
                new TextBlock { Text = "Third element" }
            }
        };

        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Expand",
                    Card = nestedCard
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ShowCard\"", json);
        Assert.Contains("First element", json);
        Assert.Contains("Second element", json);
        Assert.Contains("Third element", json);
    }

    [Fact]
    public void ShowCardAction_NestedCardContainsActions_SerializesCorrectly()
    {
        // Arrange
        var nestedCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Choose an option:" }
            },
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Yes" },
                new SubmitAction { Title = "No" }
            }
        };

        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Show Options",
                    Card = nestedCard
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Action.ShowCard\"", json);
        Assert.Contains("Choose an option:", json);
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"title\": \"Yes\"", json);
        Assert.Contains("\"title\": \"No\"", json);
    }

    [Fact]
    public void ShowCardAction_RoundtripSerialization_PreservesNestedCardStructure()
    {
        // Arrange
        var nestedCard = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Nested content", Id = "nested1" }
            }
        };

        var originalCard = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction
                {
                    Id = "show1",
                    Title = "Show Details",
                    IconUrl = "https://example.com/expand.png",
                    Style = ActionStyle.Default,
                    Card = nestedCard,
                    IsEnabled = true,
                    Tooltip = "Click to expand"
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
        
        var action = deserializedCard.Actions[0] as ShowCardAction;
        Assert.NotNull(action);
        Assert.Equal("show1", action.Id);
        Assert.Equal("Show Details", action.Title);
        Assert.Equal("https://example.com/expand.png", action.IconUrl);
        Assert.Equal(ActionStyle.Default, action.Style);
        Assert.True(action.IsEnabled);
        Assert.Equal("Click to expand", action.Tooltip);
        
        Assert.NotNull(action.Card);
        Assert.Equal("1.5", action.Card.Version);
        Assert.NotNull(action.Card.Body);
        Assert.Single(action.Card.Body);
        
        var textBlock = action.Card.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Nested content", textBlock.Text);
        Assert.Equal("nested1", textBlock.Id);
    }

    [Fact]
    public void ShowCardAction_DeeplyNested_SerializesCorrectly()
    {
        // Arrange - Card within ShowCard within card
        var innerMostCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Level 3" }
            }
        };

        var middleCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Level 2" }
            },
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Show Level 3",
                    Card = innerMostCard
                }
            }
        };

        var topCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Level 1" }
            },
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Show Level 2",
                    Card = middleCard
                }
            }
        };

        // Act
        var json = topCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var level1Action = deserializedCard.Actions![0] as ShowCardAction;
        Assert.NotNull(level1Action);
        Assert.Equal("Show Level 2", level1Action.Title);
        Assert.NotNull(level1Action.Card);
        
        var level2Action = level1Action.Card.Actions![0] as ShowCardAction;
        Assert.NotNull(level2Action);
        Assert.Equal("Show Level 3", level2Action.Title);
        Assert.NotNull(level2Action.Card);
        
        var level3Text = level2Action.Card.Body![0] as TextBlock;
        Assert.NotNull(level3Text);
        Assert.Equal("Level 3", level3Text.Text);
    }

    [Fact]
    public void ShowCardAction_WithBuilder_CreatesCorrectly()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .ShowCard("Expand")
                .WithId("show1")
                .WithStyle(ActionStyle.Default))
            .Build();

        // Assert
        Assert.NotNull(card.Actions);
        Assert.Single(card.Actions);
        var action = card.Actions[0] as ShowCardAction;
        Assert.NotNull(action);
        Assert.Equal("show1", action.Id);
        Assert.Equal("Expand", action.Title);
        Assert.Equal(ActionStyle.Default, action.Style);
    }

    [Fact]
    public void ShowCardAction_EmptyCard_SerializesCorrectly()
    {
        // Arrange
        var emptyCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>()
        };

        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction 
                { 
                    Title = "Show Empty",
                    Card = emptyCard
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var action = deserializedCard.Actions![0] as ShowCardAction;
        Assert.NotNull(action);
        Assert.NotNull(action.Card);
        Assert.NotNull(action.Card.Body);
        Assert.Empty(action.Card.Body);
    }

    [Fact]
    public void ShowCardAction_NullCard_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new ShowCardAction { Title = "Show" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"card\":", json);
    }
}
