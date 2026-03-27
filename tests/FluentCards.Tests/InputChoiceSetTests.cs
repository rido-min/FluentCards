using Xunit;

namespace FluentCards.Tests;

public class InputChoiceSetTests
{
    [Fact]
    public void BasicChoiceSet_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet 
                { 
                    Id = "color",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Red", Value = "r" },
                        new Choice { Title = "Green", Value = "g" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.ChoiceSet\"", json);
        Assert.Contains("\"id\": \"color\"", json);
    }

    [Fact]
    public void ChoiceSet_WithChoicesArray_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "size",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Small", Value = "s" },
                        new Choice { Title = "Medium", Value = "m" },
                        new Choice { Title = "Large", Value = "l" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"choices\":", json);
        Assert.Contains("\"title\": \"Small\"", json);
        Assert.Contains("\"value\": \"s\"", json);
        Assert.Contains("\"title\": \"Medium\"", json);
        Assert.Contains("\"value\": \"m\"", json);
        Assert.Contains("\"title\": \"Large\"", json);
        Assert.Contains("\"value\": \"l\"", json);
    }

    [Fact]
    public void ChoiceSet_WithMultiSelect_SerializesCommaSeparatedValues()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "toppings",
                    IsMultiSelect = true,
                    Value = "cheese,pepperoni,mushrooms",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Cheese", Value = "cheese" },
                        new Choice { Title = "Pepperoni", Value = "pepperoni" },
                        new Choice { Title = "Mushrooms", Value = "mushrooms" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isMultiSelect\": true", json);
        Assert.Contains("\"value\": \"cheese,pepperoni,mushrooms\"", json);
    }

    [Fact]
    public void ChoiceSet_DifferentStyles_SerializeCorrectly()
    {
        // Arrange & Act & Assert
        var styles = new[]
        {
            (ChoiceInputStyle.Compact, "compact"),
            (ChoiceInputStyle.Expanded, "expanded"),
            (ChoiceInputStyle.Filtered, "filtered")
        };

        foreach (var (style, expectedValue) in styles)
        {
            var card = new AdaptiveCard
            {
                Body = new List<AdaptiveElement>
                {
                    new InputChoiceSet 
                    { 
                        Id = "choice",
                        Style = style,
                        Choices = new List<Choice>
                        {
                            new Choice { Title = "Option 1", Value = "1" }
                        }
                    }
                }
            };

            var json = card.ToJson();
            Assert.Contains($"\"style\": \"{expectedValue}\"", json);
        }
    }

    [Fact]
    public void ChoiceSet_EmptyChoicesArray_SerializesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "emptyChoice",
                    Choices = new List<Choice>()
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"choices\": []", json);
    }

    [Fact]
    public void ChoiceSet_RoundtripSerialization_PreservesChoicesAndSelection()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "country",
                    Value = "us",
                    Placeholder = "Select a country",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "United States", Value = "us" },
                        new Choice { Title = "Canada", Value = "ca" },
                        new Choice { Title = "Mexico", Value = "mx" }
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
        
        var choiceSet = deserializedCard.Body[0] as InputChoiceSet;
        Assert.NotNull(choiceSet);
        Assert.Equal("country", choiceSet.Id);
        Assert.Equal("us", choiceSet.Value);
        Assert.Equal("Select a country", choiceSet.Placeholder);
        Assert.NotNull(choiceSet.Choices);
        Assert.Equal(3, choiceSet.Choices.Count);
        Assert.Equal("United States", choiceSet.Choices[0].Title);
        Assert.Equal("us", choiceSet.Choices[0].Value);
    }

    [Fact]
    public void ChoiceSet_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "preferences",
                    Label = "Select Preferences",
                    IsRequired = true,
                    ErrorMessage = "Please select at least one",
                    IsMultiSelect = true,
                    Style = ChoiceInputStyle.Expanded,
                    Value = "option1,option2",
                    Placeholder = "Choose options",
                    Wrap = true,
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option 1", Value = "option1" },
                        new Choice { Title = "Option 2", Value = "option2" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"preferences\"", json);
        Assert.Contains("\"label\": \"Select Preferences\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Please select at least one\"", json);
        Assert.Contains("\"isMultiSelect\": true", json);
        Assert.Contains("\"style\": \"expanded\"", json);
        Assert.Contains("\"value\": \"option1,option2\"", json);
        Assert.Contains("\"placeholder\": \"Choose options\"", json);
        Assert.Contains("\"wrap\": true", json);
    }

    [Fact]
    public void ChoiceSet_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "simple",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Yes", Value = "y" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"isMultiSelect\":", json);
        Assert.DoesNotContain("\"style\":", json);
        // Note: Cannot check for "value" since Choice objects have value properties
        Assert.DoesNotContain("\"placeholder\":", json);
        Assert.DoesNotContain("\"wrap\":", json);
        Assert.DoesNotContain("\"label\":", json);
        Assert.DoesNotContain("\"errorMessage\":", json);
    }

    [Fact]
    public void ChoiceSet_SpecialCharactersInChoiceTitles_Serialize()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "special",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option with \"quotes\"", Value = "1" },
                        new Choice { Title = "Option with 'apostrophe'", Value = "2" },
                        new Choice { Title = "Option with <brackets>", Value = "3" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"choices\":", json);
        // Quotes may be escaped as \" or \u0022, just check the word is present
        Assert.Contains("quotes", json);
    }

    [Fact]
    public void ChoiceSet_CompactStyleWithPlaceholder_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "dropdown",
                    Style = ChoiceInputStyle.Compact,
                    Placeholder = "-- Select an option --",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option 1", Value = "1" },
                        new Choice { Title = "Option 2", Value = "2" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"compact\"", json);
        Assert.Contains("\"placeholder\": \"-- Select an option --\"", json);
    }

    [Fact]
    public void ChoiceSet_FilteredStyle_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "searchable",
                    Style = ChoiceInputStyle.Filtered,
                    Placeholder = "Type to search...",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Apple", Value = "apple" },
                        new Choice { Title = "Banana", Value = "banana" },
                        new Choice { Title = "Cherry", Value = "cherry" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"filtered\"", json);
    }

    [Fact]
    public void ChoiceSet_WithChoicesData_SerializesDataQuery()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "people-picker",
                    IsMultiSelect = true,
                    Choices = new List<Choice>(),
                    ChoicesData = new DataQuery
                    {
                        Dataset = "graph.microsoft.com/users"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"choices.data\":", json);
        Assert.Contains("\"type\": \"Data.Query\"", json);
        Assert.Contains("\"dataset\": \"graph.microsoft.com/users\"", json);
    }

    [Fact]
    public void ChoiceSet_WithChoicesData_RoundtripPreservesDataQuery()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "people-picker",
                    IsMultiSelect = true,
                    Value = "user1,user2",
                    Choices = new List<Choice>(),
                    ChoicesData = new DataQuery
                    {
                        Dataset = "graph.microsoft.com/users"
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var choiceSet = deserializedCard.Body![0] as InputChoiceSet;
        Assert.NotNull(choiceSet);
        Assert.NotNull(choiceSet.ChoicesData);
        Assert.Equal("Data.Query", choiceSet.ChoicesData.Type);
        Assert.Equal("graph.microsoft.com/users", choiceSet.ChoicesData.Dataset);
        Assert.True(choiceSet.IsMultiSelect);
        Assert.Equal("user1,user2", choiceSet.Value);
    }

    [Fact]
    public void ChoiceSet_WithoutChoicesData_OmitsFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "simple",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Yes", Value = "y" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("choices.data", json);
    }

    [Fact]
    public void ChoiceSet_WithWrapTrue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "wrapped",
                    Wrap = true,
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "This is a very long choice title that should wrap", Value = "1" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"wrap\": true", json);
    }

    [Fact]
    public void ChoiceSet_SingleSelection_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "single",
                    Value = "option2",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option 1", Value = "option1" },
                        new Choice { Title = "Option 2", Value = "option2" },
                        new Choice { Title = "Option 3", Value = "option3" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"option2\"", json);
        Assert.DoesNotContain("\"isMultiSelect\":", json);
    }
}
