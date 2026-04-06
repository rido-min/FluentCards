using Xunit;

namespace FluentCards.Tests;

public class InputTimeTests
{
    [Fact]
    public void BasicTimeInput_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime { Id = "meetingTime" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.Time\"", json);
        Assert.Contains("\"id\": \"meetingTime\"", json);
    }

    [Fact]
    public void TimeInput_WithValue_SerializesInHHMMFormat()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "startTime",
                    Value = "09:30"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"09:30\"", json);
    }

    [Fact]
    public void TimeInput_WithMinMax_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "businessHours",
                    Min = "08:00",
                    Max = "18:00"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"min\": \"08:00\"", json);
        Assert.Contains("\"max\": \"18:00\"", json);
    }

    [Fact]
    public void TimeInput_RoundtripSerialization_PreservesTimeValues()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "time1",
                    Value = "14:45",
                    Min = "09:00",
                    Max = "17:00",
                    Placeholder = "Select a time"
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
        
        var input = deserializedCard.Body[0] as InputTime;
        Assert.NotNull(input);
        Assert.Equal("time1", input.Id);
        Assert.Equal("14:45", input.Value);
        Assert.Equal("09:00", input.Min);
        Assert.Equal("17:00", input.Max);
        Assert.Equal("Select a time", input.Placeholder);
    }

    [Fact]
    public void TimeInput_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "appointmentTime",
                    Label = "Appointment Time",
                    IsRequired = true,
                    ErrorMessage = "Time is required",
                    Value = "10:30",
                    Min = "08:00",
                    Max = "18:00",
                    Placeholder = "HH:MM"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"appointmentTime\"", json);
        Assert.Contains("\"label\": \"Appointment Time\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Time is required\"", json);
        Assert.Contains("\"value\": \"10:30\"", json);
        Assert.Contains("\"min\": \"08:00\"", json);
        Assert.Contains("\"max\": \"18:00\"", json);
        Assert.Contains("\"placeholder\": \"HH:MM\"", json);
    }

    [Fact]
    public void TimeInput_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime { Id = "time" }
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
    public void TimeInput_MidnightTime_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "midnight",
                    Value = "00:00"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"00:00\"", json);
    }

    [Fact]
    public void TimeInput_EndOfDayTime_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "endOfDay",
                    Value = "23:59"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"23:59\"", json);
    }

    [Fact]
    public void TimeInput_WithPlaceholder_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "time",
                    Placeholder = "Enter time in HH:MM format"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"placeholder\": \"Enter time in HH:MM format\"", json);
    }

    [Fact]
    public void TimeInput_EmptyStringValue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "time",
                    Value = ""
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"value\": \"\"", json);
    }
}
