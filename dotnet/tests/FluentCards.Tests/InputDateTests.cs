using Xunit;

namespace FluentCards.Tests;

public class InputDateTests
{
    [Fact]
    public void BasicDateInput_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate { Id = "dueDate" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.Date\"", json);
        Assert.Contains("\"id\": \"dueDate\"", json);
    }

    [Fact]
    public void DateInput_WithValue_SerializesInYYYYMMDDFormat()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "eventDate",
                    Value = "2024-12-31"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"2024-12-31\"", json);
    }

    [Fact]
    public void DateInput_WithMinMax_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "birthDate",
                    Min = "1900-01-01",
                    Max = "2024-12-31"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"min\": \"1900-01-01\"", json);
        Assert.Contains("\"max\": \"2024-12-31\"", json);
    }

    [Fact]
    public void DateInput_RoundtripSerialization_PreservesDateValues()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "date1",
                    Value = "2024-06-15",
                    Min = "2024-01-01",
                    Max = "2024-12-31",
                    Placeholder = "Select a date"
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
        
        var input = deserializedCard.Body[0] as InputDate;
        Assert.NotNull(input);
        Assert.Equal("date1", input.Id);
        Assert.Equal("2024-06-15", input.Value);
        Assert.Equal("2024-01-01", input.Min);
        Assert.Equal("2024-12-31", input.Max);
        Assert.Equal("Select a date", input.Placeholder);
    }

    [Fact]
    public void DateInput_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "appointmentDate",
                    Label = "Appointment Date",
                    IsRequired = true,
                    ErrorMessage = "Date is required",
                    Value = "2024-03-15",
                    Min = "2024-01-01",
                    Max = "2024-12-31",
                    Placeholder = "YYYY-MM-DD"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"appointmentDate\"", json);
        Assert.Contains("\"label\": \"Appointment Date\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Date is required\"", json);
        Assert.Contains("\"value\": \"2024-03-15\"", json);
        Assert.Contains("\"min\": \"2024-01-01\"", json);
        Assert.Contains("\"max\": \"2024-12-31\"", json);
        Assert.Contains("\"placeholder\": \"YYYY-MM-DD\"", json);
    }

    [Fact]
    public void DateInput_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate { Id = "date" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"value\":", json);
        Assert.DoesNotContain("\"min\":", json);
        Assert.DoesNotContain("\"max\":", json);
        Assert.DoesNotContain("\"placeholder\":", json);
        Assert.DoesNotContain("\"label\":", json);
        Assert.DoesNotContain("\"errorMessage\":", json);
    }

    [Fact]
    public void DateInput_WithPlaceholder_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "date",
                    Placeholder = "Select your preferred date"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"placeholder\": \"Select your preferred date\"", json);
    }

    [Fact]
    public void DateInput_EmptyStringValue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "date",
                    Value = ""
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"\"", json);
    }

    [Fact]
    public void DateInput_LeapYearDate_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "leapDate",
                    Value = "2024-02-29"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"2024-02-29\"", json);
    }
}
