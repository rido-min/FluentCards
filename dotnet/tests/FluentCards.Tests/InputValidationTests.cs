using Xunit;

namespace FluentCards.Tests;

public class InputValidationTests
{
    [Fact]
    public void InputText_IsRequiredTrue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "required",
                    IsRequired = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isRequired\": true", json);
    }

    [Fact]
    public void InputText_IsRequiredFalse_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "optional",
                    IsRequired = false
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"isRequired\":", json);
    }

    [Fact]
    public void InputNumber_WithErrorMessage_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "age",
                    ErrorMessage = "Age must be between 0 and 120"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"errorMessage\": \"Age must be between 0 and 120\"", json);
    }

    [Fact]
    public void InputDate_WithLabel_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "birthDate",
                    Label = "Date of Birth"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"label\": \"Date of Birth\"", json);
    }

    [Fact]
    public void InputTime_AllValidationProperties_Serialize()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputTime
                {
                    Id = "meetingTime",
                    Label = "Meeting Time",
                    IsRequired = true,
                    ErrorMessage = "Meeting time is required"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"label\": \"Meeting Time\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Meeting time is required\"", json);
    }

    [Fact]
    public void InputToggle_WithValidationProperties_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputToggle
                {
                    Id = "terms",
                    Title = "Accept Terms",
                    Label = "Terms and Conditions",
                    IsRequired = true,
                    ErrorMessage = "You must accept the terms"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"label\": \"Terms and Conditions\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"You must accept the terms\"", json);
    }

    [Fact]
    public void InputChoiceSet_WithValidationProperties_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputChoiceSet
                {
                    Id = "country",
                    Label = "Country",
                    IsRequired = true,
                    ErrorMessage = "Please select a country",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "USA", Value = "us" }
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"label\": \"Country\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Please select a country\"", json);
    }

    [Fact]
    public void MultipleInputs_MixedValidationProperties_SerializeCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "name",
                    Label = "Name",
                    IsRequired = true,
                    ErrorMessage = "Name is required"
                },
                new InputNumber
                {
                    Id = "age",
                    Label = "Age",
                    IsRequired = false
                },
                new InputDate
                {
                    Id = "date",
                    Label = "Date"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        // First input should have all properties
        Assert.Contains("\"label\": \"Name\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Name is required\"", json);
        
        // Second input should have label but not isRequired (false)
        Assert.Contains("\"label\": \"Age\"", json);
        
        // Third input should only have label
        Assert.Contains("\"label\": \"Date\"", json);
    }

    [Fact]
    public void AllInputTypes_SupportIsRequired()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "text", IsRequired = true },
                new InputNumber { Id = "number", IsRequired = true },
                new InputDate { Id = "date", IsRequired = true },
                new InputTime { Id = "time", IsRequired = true },
                new InputToggle { Id = "toggle", Title = "Toggle", IsRequired = true },
                new InputChoiceSet { Id = "choice", IsRequired = true, Choices = new List<Choice>() }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        // Count occurrences of "isRequired": true - should be 6
        var count = 0;
        var index = 0;
        while ((index = json.IndexOf("\"isRequired\": true", index)) != -1)
        {
            count++;
            index += "\"isRequired\": true".Length;
        }
        Assert.Equal(6, count);
    }

    [Fact]
    public void AllInputTypes_SupportLabel()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "text", Label = "Text Label" },
                new InputNumber { Id = "number", Label = "Number Label" },
                new InputDate { Id = "date", Label = "Date Label" },
                new InputTime { Id = "time", Label = "Time Label" },
                new InputToggle { Id = "toggle", Title = "Toggle", Label = "Toggle Label" },
                new InputChoiceSet { Id = "choice", Label = "Choice Label", Choices = new List<Choice>() }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"label\": \"Text Label\"", json);
        Assert.Contains("\"label\": \"Number Label\"", json);
        Assert.Contains("\"label\": \"Date Label\"", json);
        Assert.Contains("\"label\": \"Time Label\"", json);
        Assert.Contains("\"label\": \"Toggle Label\"", json);
        Assert.Contains("\"label\": \"Choice Label\"", json);
    }

    [Fact]
    public void AllInputTypes_SupportErrorMessage()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "text", ErrorMessage = "Text error" },
                new InputNumber { Id = "number", ErrorMessage = "Number error" },
                new InputDate { Id = "date", ErrorMessage = "Date error" },
                new InputTime { Id = "time", ErrorMessage = "Time error" },
                new InputToggle { Id = "toggle", Title = "Toggle", ErrorMessage = "Toggle error" },
                new InputChoiceSet { Id = "choice", ErrorMessage = "Choice error", Choices = new List<Choice>() }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"errorMessage\": \"Text error\"", json);
        Assert.Contains("\"errorMessage\": \"Number error\"", json);
        Assert.Contains("\"errorMessage\": \"Date error\"", json);
        Assert.Contains("\"errorMessage\": \"Time error\"", json);
        Assert.Contains("\"errorMessage\": \"Toggle error\"", json);
        Assert.Contains("\"errorMessage\": \"Choice error\"", json);
    }
}
