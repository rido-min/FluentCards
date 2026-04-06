using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class FactSetTests
{
    [Fact]
    public void FactSet_WithMultipleFacts_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Title = "Name", Value = "John Doe" },
                        new Fact { Title = "Email", Value = "john@example.com" },
                        new Fact { Title = "Phone", Value = "+1-555-1234" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"FactSet\"", json);
        Assert.Contains("\"facts\":", json);
        Assert.Contains("\"title\": \"Name\"", json);
        Assert.Contains("\"value\": \"John Doe\"", json);
        Assert.Contains("\"title\": \"Email\"", json);
        Assert.Contains("\"value\": \"john@example.com\"", json);
        Assert.Contains("\"title\": \"Phone\"", json);
        // The + character may be escaped as \u002B in JSON
        Assert.True(json.Contains("\"value\": \"+1-555-1234\"") || json.Contains("\"value\": \"\\u002B1-555-1234\""), "Phone value not found in JSON");
    }

    [Fact]
    public void FactSet_WithEmptyFacts_HandlesGracefully()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Facts = new List<Fact>()
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var factSet = deserializedCard.Body![0] as FactSet;
        Assert.NotNull(factSet);
        Assert.NotNull(factSet.Facts);
        Assert.Empty(factSet.Facts);
    }

    [Fact]
    public void FactSet_SpecialCharactersInTitleValue_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Title = "Special: \"Chars\"", Value = "Value with\nnewline\tand\ttabs" },
                        new Fact { Title = "Unicode: 你好", Value = "Emoji: 🎉🚀" },
                        new Fact { Title = "Quotes & slashes", Value = "C:\\Path\\To\\File" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var factSet = deserializedCard.Body![0] as FactSet;
        Assert.NotNull(factSet);
        Assert.NotNull(factSet.Facts);
        Assert.Equal(3, factSet.Facts.Count);
        Assert.Equal("Special: \"Chars\"", factSet.Facts[0].Title);
        Assert.Equal("Value with\nnewline\tand\ttabs", factSet.Facts[0].Value);
        Assert.Equal("Unicode: 你好", factSet.Facts[1].Title);
        Assert.Equal("Emoji: 🎉🚀", factSet.Facts[1].Value);
        Assert.Equal("Quotes & slashes", factSet.Facts[2].Title);
        Assert.Equal("C:\\Path\\To\\File", factSet.Facts[2].Value);
    }

    [Fact]
    public void FactSet_RoundtripSerialization_PreservesFactOrder()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Id = "factSet1",
                    Facts = new List<Fact>
                    {
                        new Fact { Title = "First", Value = "1" },
                        new Fact { Title = "Second", Value = "2" },
                        new Fact { Title = "Third", Value = "3" },
                        new Fact { Title = "Fourth", Value = "4" }
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

        var factSet = deserializedCard.Body[0] as FactSet;
        Assert.NotNull(factSet);
        Assert.Equal("factSet1", factSet.Id);
        Assert.NotNull(factSet.Facts);
        Assert.Equal(4, factSet.Facts.Count);
        Assert.Equal("First", factSet.Facts[0].Title);
        Assert.Equal("1", factSet.Facts[0].Value);
        Assert.Equal("Second", factSet.Facts[1].Title);
        Assert.Equal("2", factSet.Facts[1].Value);
        Assert.Equal("Third", factSet.Facts[2].Title);
        Assert.Equal("3", factSet.Facts[2].Value);
        Assert.Equal("Fourth", factSet.Facts[3].Title);
        Assert.Equal("4", factSet.Facts[3].Value);
    }

    [Fact]
    public void FactSet_WithLongText_SerializesCorrectly()
    {
        // Arrange
        var longTitle = new string('A', 500);
        var longValue = new string('B', 1000);
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Facts = new List<Fact>
                    {
                        new Fact { Title = longTitle, Value = longValue }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var factSet = deserializedCard.Body![0] as FactSet;
        Assert.NotNull(factSet);
        Assert.NotNull(factSet.Facts);
        Assert.Single(factSet.Facts);
        Assert.Equal(longTitle, factSet.Facts[0].Title);
        Assert.Equal(longValue, factSet.Facts[0].Value);
    }

    [Fact]
    public void FactSet_WithAdaptiveElementProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new FactSet
                {
                    Id = "facts1",
                    IsVisible = false,
                    Spacing = Spacing.Medium,
                    Separator = true,
                    Facts = new List<Fact>
                    {
                        new Fact { Title = "Key", Value = "Value" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"facts1\"", json);
        Assert.Contains("\"isVisible\": false", json);
        Assert.Contains("\"spacing\": \"medium\"", json);
        Assert.Contains("\"separator\": true", json);
    }
}
