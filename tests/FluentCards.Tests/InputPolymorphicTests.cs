using Xunit;

namespace FluentCards.Tests;

public class InputPolymorphicTests
{
    [Fact]
    public void Card_WithMixedInputTypes_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "name", Placeholder = "Your name" },
                new InputNumber { Id = "age", Min = 0, Max = 120 },
                new InputDate { Id = "birthDate", Value = "2000-01-01" },
                new InputTime { Id = "appointmentTime", Value = "14:30" },
                new InputToggle { Id = "subscribe", Title = "Subscribe" },
                new InputChoiceSet 
                { 
                    Id = "color",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Red", Value = "r" }
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
        Assert.Equal(6, deserializedCard.Body.Count);
        
        Assert.IsType<InputText>(deserializedCard.Body[0]);
        Assert.IsType<InputNumber>(deserializedCard.Body[1]);
        Assert.IsType<InputDate>(deserializedCard.Body[2]);
        Assert.IsType<InputTime>(deserializedCard.Body[3]);
        Assert.IsType<InputToggle>(deserializedCard.Body[4]);
        Assert.IsType<InputChoiceSet>(deserializedCard.Body[5]);
    }

    [Fact]
    public void Card_WithInputsAndTextBlocks_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Please fill out the form:" },
                new InputText { Id = "email", Label = "Email" },
                new TextBlock { Text = "Select your preferences:" },
                new InputChoiceSet 
                { 
                    Id = "preference",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option A", Value = "a" }
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
        Assert.Equal(4, deserializedCard.Body.Count);
        
        Assert.IsType<TextBlock>(deserializedCard.Body[0]);
        Assert.IsType<InputText>(deserializedCard.Body[1]);
        Assert.IsType<TextBlock>(deserializedCard.Body[2]);
        Assert.IsType<InputChoiceSet>(deserializedCard.Body[3]);
    }

    [Fact]
    public void InputText_WithInlineSubmitAction_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "search",
                    InlineAction = new SubmitAction
                    {
                        Title = "Search",
                        IconUrl = "https://example.com/search.png"
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var input = deserializedCard.Body![0] as InputText;
        Assert.NotNull(input);
        Assert.NotNull(input.InlineAction);
        Assert.IsType<SubmitAction>(input.InlineAction);
        
        var action = input.InlineAction as SubmitAction;
        Assert.Equal("Search", action!.Title);
    }

    [Fact]
    public void InputText_WithInlineOpenUrlAction_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "url",
                    InlineAction = new OpenUrlAction
                    {
                        Title = "Go",
                        Url = "https://example.com"
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        var input = deserializedCard.Body![0] as InputText;
        Assert.NotNull(input);
        Assert.NotNull(input.InlineAction);
        Assert.IsType<OpenUrlAction>(input.InlineAction);
        
        var action = input.InlineAction as OpenUrlAction;
        Assert.Equal("https://example.com", action!.Url);
    }

    [Fact]
    public void CardWithActions_AndInputs_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "username", Label = "Username" },
                new InputText { Id = "password", Label = "Password", Style = TextInputStyle.Password }
            },
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Login" },
                new OpenUrlAction { Title = "Forgot Password?", Url = "https://example.com/reset" }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.Equal(2, deserializedCard.Body!.Count);
        Assert.Equal(2, deserializedCard.Actions!.Count);
        
        Assert.IsType<InputText>(deserializedCard.Body[0]);
        Assert.IsType<InputText>(deserializedCard.Body[1]);
        Assert.IsType<SubmitAction>(deserializedCard.Actions[0]);
        Assert.IsType<OpenUrlAction>(deserializedCard.Actions[1]);
    }

    [Fact]
    public void ComplexFormCard_WithAllInputTypes_DeserializesCorrectly()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Registration Form", Size = TextSize.Large, Weight = TextWeight.Bolder },
                new InputText { Id = "fullName", Label = "Full Name", IsRequired = true },
                new InputText { Id = "email", Label = "Email", Style = TextInputStyle.Email, IsRequired = true },
                new InputNumber { Id = "age", Label = "Age", Min = 18, Max = 100 },
                new InputDate { Id = "dob", Label = "Date of Birth" },
                new InputTime { Id = "preferredTime", Label = "Preferred Contact Time" },
                new InputToggle { Id = "newsletter", Title = "Subscribe to newsletter" },
                new InputChoiceSet 
                { 
                    Id = "country",
                    Label = "Country",
                    Style = ChoiceInputStyle.Compact,
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "United States", Value = "us" },
                        new Choice { Title = "Canada", Value = "ca" }
                    }
                }
            },
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Title = "Submit", Style = ActionStyle.Positive },
                new OpenUrlAction { Title = "Cancel", Url = "https://example.com" }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.Equal(8, deserializedCard.Body!.Count);
        Assert.Equal(2, deserializedCard.Actions!.Count);
        
        // Verify each element type
        Assert.IsType<TextBlock>(deserializedCard.Body[0]);
        Assert.IsType<InputText>(deserializedCard.Body[1]);
        Assert.IsType<InputText>(deserializedCard.Body[2]);
        Assert.IsType<InputNumber>(deserializedCard.Body[3]);
        Assert.IsType<InputDate>(deserializedCard.Body[4]);
        Assert.IsType<InputTime>(deserializedCard.Body[5]);
        Assert.IsType<InputToggle>(deserializedCard.Body[6]);
        Assert.IsType<InputChoiceSet>(deserializedCard.Body[7]);
        
        // Verify properties are preserved
        var emailInput = deserializedCard.Body[2] as InputText;
        Assert.Equal(TextInputStyle.Email, emailInput!.Style);
        Assert.True(emailInput.IsRequired);
        
        var choiceSet = deserializedCard.Body[7] as InputChoiceSet;
        Assert.Equal(2, choiceSet!.Choices.Count);
        Assert.Equal(ChoiceInputStyle.Compact, choiceSet.Style);
    }

    [Fact]
    public void DeserializeFromJson_ValidInputTypes_ParsesCorrectly()
    {
        // Arrange
        var json = @"{
  ""type"": ""AdaptiveCard"",
  ""version"": ""1.5"",
  ""body"": [
    {
      ""type"": ""Input.Text"",
      ""id"": ""textInput""
    },
    {
      ""type"": ""Input.Number"",
      ""id"": ""numberInput""
    },
    {
      ""type"": ""Input.Date"",
      ""id"": ""dateInput""
    },
    {
      ""type"": ""Input.Time"",
      ""id"": ""timeInput""
    },
    {
      ""type"": ""Input.Toggle"",
      ""id"": ""toggleInput"",
      ""title"": ""Toggle""
    },
    {
      ""type"": ""Input.ChoiceSet"",
      ""id"": ""choiceInput"",
      ""choices"": []
    }
  ]
}";

        // Act
        var card = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(card);
        Assert.Equal(6, card.Body!.Count);
        Assert.IsType<InputText>(card.Body[0]);
        Assert.IsType<InputNumber>(card.Body[1]);
        Assert.IsType<InputDate>(card.Body[2]);
        Assert.IsType<InputTime>(card.Body[3]);
        Assert.IsType<InputToggle>(card.Body[4]);
        Assert.IsType<InputChoiceSet>(card.Body[5]);
    }
}
