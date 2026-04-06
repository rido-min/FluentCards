using Xunit;

namespace FluentCards.Tests;

public class InputTextTests
{
    [Fact]
    public void BasicTextInput_Serializes_WithIdAndType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "textInput1" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"Input.Text\"", json);
        Assert.Contains("\"id\": \"textInput1\"", json);
    }

    [Fact]
    public void MultilineTextInput_Serializes_IsMultilineAsTrue()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText 
                { 
                    Id = "comments",
                    IsMultiline = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"isMultiline\": true", json);
    }

    [Fact]
    public void TextInput_WithAllProperties_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "fullInput",
                    Label = "Email Address",
                    IsRequired = true,
                    ErrorMessage = "Email is required",
                    IsMultiline = false,
                    MaxLength = 100,
                    Placeholder = "Enter your email",
                    Value = "test@example.com",
                    Style = TextInputStyle.Email,
                    Regex = @"^[^@]+@[^@]+\.[^@]+$"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"id\": \"fullInput\"", json);
        Assert.Contains("\"label\": \"Email Address\"", json);
        Assert.Contains("\"isRequired\": true", json);
        Assert.Contains("\"errorMessage\": \"Email is required\"", json);
        Assert.Contains("\"maxLength\": 100", json);
        Assert.Contains("\"placeholder\": \"Enter your email\"", json);
        Assert.Contains("\"value\": \"test@example.com\"", json);
        Assert.Contains("\"style\": \"email\"", json);
        Assert.Contains("\"regex\":", json);
        // Note: The exact escaping may vary (e.g., + might be \u002B), so just check it's present
        Assert.Contains("^", json);
        Assert.Contains("@", json);
    }

    [Fact]
    public void TextInput_WithInlineAction_SerializesNestedAction()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "searchInput",
                    Placeholder = "Search...",
                    InlineAction = new SubmitAction
                    {
                        Title = "Search",
                        IconUrl = "https://example.com/search.png"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"inlineAction\":", json);
        Assert.Contains("\"type\": \"Action.Submit\"", json);
        Assert.Contains("\"title\": \"Search\"", json);
    }

    [Fact]
    public void TextInput_RegexPattern_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "phoneInput",
                    Regex = @"^\d{3}-\d{3}-\d{4}$",
                    Placeholder = "123-456-7890"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert - Note: In JSON, backslashes are escaped, so \d becomes \\d
        // In C# string literal, we need to escape again: \\\\d
        Assert.Contains("\"regex\": \"^\\\\d{3}-\\\\d{3}-\\\\d{4}$\"", json);
    }

    [Fact]
    public void TextInput_RoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "input1",
                    Label = "Username",
                    IsRequired = true,
                    ErrorMessage = "Required field",
                    IsMultiline = false,
                    MaxLength = 50,
                    Placeholder = "Enter username",
                    Value = "john_doe",
                    Style = TextInputStyle.Text
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
        
        var input = deserializedCard.Body[0] as InputText;
        Assert.NotNull(input);
        Assert.Equal("input1", input.Id);
        Assert.Equal("Username", input.Label);
        Assert.True(input.IsRequired);
        Assert.Equal("Required field", input.ErrorMessage);
        Assert.False(input.IsMultiline);
        Assert.Equal(50, input.MaxLength);
        Assert.Equal("Enter username", input.Placeholder);
        Assert.Equal("john_doe", input.Value);
        Assert.Equal(TextInputStyle.Text, input.Style);
    }

    [Fact]
    public void TextInput_NullProperties_OmittedFromJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "simpleInput" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"isMultiline\":", json);
        Assert.DoesNotContain("\"maxLength\":", json);
        Assert.DoesNotContain("\"placeholder\":", json);
        Assert.DoesNotContain("\"value\":", json);
        Assert.DoesNotContain("\"style\":", json);
        Assert.DoesNotContain("\"inlineAction\":", json);
        Assert.DoesNotContain("\"regex\":", json);
        Assert.DoesNotContain("\"label\":", json);
        Assert.DoesNotContain("\"errorMessage\":", json);
    }

    [Fact]
    public void TextInput_DifferentStyles_SerializeCorrectly()
    {
        // Arrange & Act & Assert
        var styles = new[]
        {
            (TextInputStyle.Text, "text"),
            (TextInputStyle.Tel, "tel"),
            (TextInputStyle.Url, "url"),
            (TextInputStyle.Email, "email"),
            (TextInputStyle.Password, "password")
        };

        foreach (var (style, expectedValue) in styles)
        {
            var card = new AdaptiveCard
            {
                Body = new List<AdaptiveElement>
                {
                    new InputText { Id = "input", Style = style }
                }
            };

            var json = card.ToJson();
            Assert.Contains($"\"style\": \"{expectedValue}\"", json);
        }
    }

    [Fact]
    public void TextInput_WithPasswordStyle_SerializesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "passwordInput",
                    Label = "Password",
                    Style = TextInputStyle.Password,
                    IsRequired = true
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"style\": \"password\"", json);
        Assert.Contains("\"isRequired\": true", json);
    }

    [Fact]
    public void TextInput_EmptyStringValue_Serializes()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "input",
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
    public void TextInput_VeryLongPlaceholder_Serializes()
    {
        // Arrange
        var longText = new string('a', 500);
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText
                {
                    Id = "input",
                    Placeholder = longText
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"placeholder\":", json);
        Assert.Contains(longText, json);
    }
}
