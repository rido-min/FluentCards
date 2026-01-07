using Xunit;

namespace FluentCards.Tests;

public class InputNumberTests
{
    [Fact]
    public void BasicNumberInput_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber { Id = "quantity" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.Number\"", json);
        Assert.Contains("\"id\": \"quantity\"", json);
    }

    [Fact]
    public void NumberInput_WithMinMax_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "age",
                    Min = 0,
                    Max = 120
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"min\": 0", json);
        Assert.Contains("\"max\": 120", json);
    }

    [Fact]
    public void NumberInput_WithDecimalValue_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "price",
                    Value = 19.99,
                    Min = 0.01,
                    Max = 999.99
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": 19.99", json);
        Assert.Contains("\"min\": 0.01", json);
        Assert.Contains("\"max\": 999.99", json);
    }

    [Fact]
    public void NumberInput_NullValue_OmittedFromOutput()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "number",
                    Placeholder = "Enter a number"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"value\":", json);
        Assert.DoesNotContain("\"min\":", json);
        Assert.DoesNotContain("\"max\":", json);
    }

    [Fact]
    public void NumberInput_RoundtripSerialization_PreservesNumericPrecision()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "decimal",
                    Value = 123.456789,
                    Min = -100.5,
                    Max = 1000.75,
                    Placeholder = "Enter decimal"
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
        
        var input = deserializedCard.Body[0] as InputNumber;
        Assert.NotNull(input);
        Assert.Equal("decimal", input.Id);
        Assert.Equal(123.456789, input.Value);
        Assert.Equal(-100.5, input.Min);
        Assert.Equal(1000.75, input.Max);
        Assert.Equal("Enter decimal", input.Placeholder);
    }

    [Fact]
    public void NumberInput_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "rating",
                    Label = "Rating (1-5)",
                    IsRequired = true,
                    ErrorMessage = "Please provide a rating",
                    Min = 1,
                    Max = 5,
                    Value = 3,
                    Placeholder = "1-5"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"rating\"", json);
        Assert.Contains("\"label\": \"Rating (1-5)\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Please provide a rating\"", json);
        Assert.Contains("\"min\": 1", json);
        Assert.Contains("\"max\": 5", json);
        Assert.Contains("\"value\": 3", json);
        Assert.Contains("\"placeholder\": \"1-5\"", json);
    }

    [Fact]
    public void NumberInput_NegativeNumbers_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "temperature",
                    Value = -10.5,
                    Min = -50,
                    Max = 50
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": -10.5", json);
        Assert.Contains("\"min\": -50", json);
        Assert.Contains("\"max\": 50", json);
    }

    [Fact]
    public void NumberInput_ZeroValue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "count",
                    Value = 0
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": 0", json);
    }

    [Fact]
    public void NumberInput_VeryLargeNumbers_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "bigNumber",
                    Value = 999999999.999999,
                    Min = -999999999.999999,
                    Max = 999999999.999999
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": 999999999.999999", json);
        Assert.Contains("\"min\": -999999999.999999", json);
        Assert.Contains("\"max\": 999999999.999999", json);
    }

    [Fact]
    public void NumberInput_IntegerValues_SerializeWithoutDecimal()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "wholeNumber",
                    Value = 42,
                    Min = 1,
                    Max = 100
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": 42", json);
        Assert.Contains("\"min\": 1", json);
        Assert.Contains("\"max\": 100", json);
    }
}
