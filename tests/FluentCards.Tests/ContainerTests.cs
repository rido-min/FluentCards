using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class ContainerTests
{
    [Fact]
    public void EmptyContainer_Serialization_ContainsEmptyItemsArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container { Items = new List<AdaptiveElement>() }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Container\"", json);
        Assert.Contains("\"items\": []", json);
    }

    [Fact]
    public void ContainerWithNestedTextBlock_Serialization_ContainsNestedElements()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement>
                    {
                        new TextBlock { Text = "Hello from Container" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Container\"", json);
        Assert.Contains("\"items\":", json);
        Assert.Contains("\"type\": \"TextBlock\"", json);
        Assert.Contains("\"text\": \"Hello from Container\"", json);
    }

    [Fact]
    public void ContainerWithNestedContainer_Serialization_HandlesDeepNesting()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Id = "outer",
                    Items = new List<AdaptiveElement>
                    {
                        new Container
                        {
                            Id = "inner",
                            Items = new List<AdaptiveElement>
                            {
                                new TextBlock { Text = "Nested Text" }
                            }
                        }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);

        var outerContainer = deserializedCard.Body[0] as Container;
        Assert.NotNull(outerContainer);
        Assert.Equal("outer", outerContainer.Id);
        Assert.NotNull(outerContainer.Items);
        Assert.Single(outerContainer.Items);

        var innerContainer = outerContainer.Items[0] as Container;
        Assert.NotNull(innerContainer);
        Assert.Equal("inner", innerContainer.Id);
        Assert.NotNull(innerContainer.Items);
        Assert.Single(innerContainer.Items);

        var textBlock = innerContainer.Items[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Nested Text", textBlock.Text);
    }

    [Fact]
    public void ContainerWithStyle_Serialization_UsesCamelCase()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Style = ContainerStyle.Emphasis,
                    Items = new List<AdaptiveElement>()
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"emphasis\"", json);
        Assert.DoesNotContain("\"style\": \"Emphasis\"", json);
    }

    [Fact]
    public void ContainerWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Id = "container1",
                    Style = ContainerStyle.Accent,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Bleed = true,
                    MinHeight = "100px",
                    BackgroundImage = new BackgroundImage
                    {
                        Url = "https://example.com/bg.jpg",
                        FillMode = ImageFillMode.Cover
                    },
                    Items = new List<AdaptiveElement>
                    {
                        new TextBlock { Text = "Content" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"container1\"", json);
        Assert.Contains("\"style\": \"accent\"", json);
        Assert.Contains("\"verticalContentAlignment\": \"center\"", json);
        Assert.Contains("\"bleed\": true", json);
        Assert.Contains("\"minHeight\": \"100px\"", json);
        Assert.Contains("\"backgroundImage\":", json);
        Assert.Contains("\"url\": \"https://example.com/bg.jpg\"", json);
        Assert.Contains("\"fillMode\": \"cover\"", json);
    }

    [Fact]
    public void ContainerWithSelectAction_Serialization_ContainsAction()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement>(),
                    SelectAction = new OpenUrlAction
                    {
                        Url = "https://example.com",
                        Title = "Click Container"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"selectAction\":", json);
        Assert.Contains("\"type\": \"Action.OpenUrl\"", json);
    }

    [Fact]
    public void ContainerRoundtripSerialization_PreservesStructure()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Id = "test",
                    Style = ContainerStyle.Good,
                    Items = new List<AdaptiveElement>
                    {
                        new TextBlock { Text = "First" },
                        new TextBlock { Text = "Second" }
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

        var container = deserializedCard.Body[0] as Container;
        Assert.NotNull(container);
        Assert.Equal("test", container.Id);
        Assert.Equal(ContainerStyle.Good, container.Style);
        Assert.NotNull(container.Items);
        Assert.Equal(2, container.Items.Count);

        var first = container.Items[0] as TextBlock;
        Assert.NotNull(first);
        Assert.Equal("First", first.Text);

        var second = container.Items[1] as TextBlock;
        Assert.NotNull(second);
        Assert.Equal("Second", second.Text);
    }

    [Fact]
    public void ContainerWithMixedElements_Serialization_PreservesPolymorphicTypes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement>
                    {
                        new TextBlock { Text = "Text" },
                        new Image { Url = "https://example.com/img.jpg" },
                        new TextBlock { Text = "More Text" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Body);
        var container = deserializedCard.Body[0] as Container;
        Assert.NotNull(container);
        Assert.NotNull(container.Items);
        Assert.Equal(3, container.Items.Count);
        Assert.IsType<TextBlock>(container.Items[0]);
        Assert.IsType<Image>(container.Items[1]);
        Assert.IsType<TextBlock>(container.Items[2]);
    }

    [Fact]
    public void ContainerWithNullItems_Serialization_DoesNotIncludeItems()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container { Items = null }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Container\"", json);
        Assert.DoesNotContain("\"items\":", json);
    }

    [Fact]
    public void ContainerWithBackgroundImage_Serialization_ContainsCompleteBackgroundImage()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new Container
                {
                    Items = new List<AdaptiveElement>(),
                    BackgroundImage = new BackgroundImage
                    {
                        Url = "https://example.com/bg.jpg",
                        FillMode = ImageFillMode.RepeatHorizontally,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"backgroundImage\":", json);
        Assert.Contains("\"url\": \"https://example.com/bg.jpg\"", json);
        Assert.Contains("\"fillMode\": \"repeatHorizontally\"", json);
        Assert.Contains("\"horizontalAlignment\": \"left\"", json);
        Assert.Contains("\"verticalAlignment\": \"top\"", json);
    }
}
