using System.Text;
using System.Text.Json;
using FluentCards.Serialization;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class SerializationEdgeCaseTests
{
    [Fact]
    public void Serialize_VeryLargeCard_Succeeds()
    {
        // Arrange - Create a card with 1000+ elements
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>()
        };
        
        for (int i = 0; i < 1000; i++)
        {
            card.Body.Add(new TextBlock { Text = $"Element {i}" });
        }

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(1000, deserialized.Body?.Count);
    }

    [Fact]
    public void Serialize_DeeplyNestedStructure_Succeeds()
    {
        // Arrange - Create a structure with 10 levels of nesting
        var card = new AdaptiveCard { Body = new List<AdaptiveElement>() };
        var currentContainer = new Container { Items = new List<AdaptiveElement>() };
        card.Body.Add(currentContainer);

        for (int i = 0; i < 10; i++)
        {
            var newContainer = new Container
            {
                Id = $"container{i}",
                Items = new List<AdaptiveElement>
                {
                    new TextBlock { Text = $"Level {i}" }
                }
            };
            currentContainer.Items.Add(newContainer);
            currentContainer = newContainer;
        }

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Body);
    }

    [Fact]
    public void Serialize_UnicodeCharactersInAllProperties_PreservesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Id = "测试",
                    Text = "Hello 世界 🌍 émojis 🎉"
                },
                new InputText
                {
                    Id = "input_日本語",
                    Label = "ラベル",
                    Placeholder = "प्लेसहोल्डर"
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var textBlock = deserialized.Body![0] as TextBlock;
        var inputText = deserialized.Body[1] as InputText;
        Assert.Equal("测试", textBlock?.Id);
        Assert.Equal("Hello 世界 🌍 émojis 🎉", textBlock?.Text);
        Assert.Equal("input_日本語", inputText?.Id);
        Assert.Equal("ラベル", inputText?.Label);
        Assert.Equal("प्लेसहोल्डर", inputText?.Placeholder);
    }

    [Fact]
    public void Serialize_EmptyStrings_PreservesEmptyStrings()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "" },
                new InputText { Id = "input1", Placeholder = "" }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var textBlock = deserialized.Body![0] as TextBlock;
        Assert.Equal("", textBlock?.Text);
    }

    [Fact]
    public void Deserialize_MalformedJson_ThrowsException()
    {
        // Arrange
        var malformedJson = @"{""type"": ""AdaptiveCard"", ""version"": ";

        // Act & Assert
        Assert.Throws<JsonException>(() => AdaptiveCardSerializer.Deserialize(malformedJson));
    }

    [Fact]
    public void TryDeserialize_MalformedJson_ReturnsFalse()
    {
        // Arrange
        var malformedJson = @"{""type"": ""AdaptiveCard"", ""version"": ";

        // Act
        var result = AdaptiveCardSerializer.TryDeserialize(malformedJson, out var card, out var errorMessage);

        // Assert
        Assert.False(result);
        Assert.Null(card);
        Assert.NotNull(errorMessage);
        Assert.NotEmpty(errorMessage);
    }

    [Fact]
    public void Serialize_NumericEdgeCases_PreservesValues()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputNumber
                {
                    Id = "num1",
                    Min = double.MinValue,
                    Max = double.MaxValue,
                    Value = 0.0
                },
                new InputNumber
                {
                    Id = "num2",
                    Value = -123.456
                },
                new InputNumber
                {
                    Id = "num3",
                    Value = 999999999.999999
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var num1 = deserialized.Body![0] as InputNumber;
        var num2 = deserialized.Body[1] as InputNumber;
        var num3 = deserialized.Body[2] as InputNumber;
        
        Assert.Equal(double.MinValue, num1?.Min);
        Assert.Equal(double.MaxValue, num1?.Max);
        Assert.Equal(-123.456, num2?.Value ?? 0);
        Assert.Equal(999999999.999999, num3?.Value ?? 0, precision: 5);
    }

    [Fact]
    public void Serialize_DateTimeEdgeCases_PreservesFormat()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputDate
                {
                    Id = "date1",
                    Value = "2024-01-01",
                    Min = "1900-01-01",
                    Max = "2099-12-31"
                },
                new InputTime
                {
                    Id = "time1",
                    Value = "23:59",
                    Min = "00:00",
                    Max = "23:59"
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var date = deserialized.Body![0] as InputDate;
        var time = deserialized.Body[1] as InputTime;
        
        Assert.Equal("2024-01-01", date?.Value);
        Assert.Equal("1900-01-01", date?.Min);
        Assert.Equal("2099-12-31", date?.Max);
        Assert.Equal("23:59", time?.Value);
    }

    [Fact]
    public void Serialize_MaximumStringLength_Succeeds()
    {
        // Arrange
        var veryLongString = new string('X', 100000); // 100k characters
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = veryLongString }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var textBlock = deserialized.Body![0] as TextBlock;
        Assert.Equal(100000, textBlock?.Text?.Length);
    }

    [Fact]
    public void SerializeToUtf8Bytes_VsStringSerialize_ProducesSameResult()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Test with émojis 🎉" }
            }
        };

        // Act
        var jsonString = AdaptiveCardSerializer.Serialize(card);
        var jsonBytes = AdaptiveCardSerializer.SerializeToUtf8Bytes(card);
        var jsonFromBytes = Encoding.UTF8.GetString(jsonBytes);

        // Assert
        // Note: Both use the same context, so they should produce same format
        // Remove whitespace differences for comparison
        var normalizedString = jsonString.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        var normalizedFromBytes = jsonFromBytes.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        Assert.Equal(normalizedString, normalizedFromBytes);
    }

    [Fact]
    public void Serialize_ComplexActionData_PreservesStructure()
    {
        // Arrange
        var complexData = JsonDocument.Parse(@"{
            ""userId"": 123,
            ""preferences"": {
                ""notifications"": true,
                ""theme"": ""dark"",
                ""languages"": [""en"", ""es"", ""fr""]
            },
            ""metadata"": {
                ""timestamp"": ""2024-01-01T00:00:00Z"",
                ""source"": ""mobile-app""
            }
        }").RootElement;

        var card = new AdaptiveCard
        {
            Actions = new List<AdaptiveAction>
            {
                new SubmitAction { Data = complexData }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var submitAction = deserialized.Actions![0] as SubmitAction;
        Assert.NotNull(submitAction?.Data);
        Assert.Equal(123, submitAction.Data.Value.GetProperty("userId").GetInt32());
        Assert.True(submitAction.Data.Value.GetProperty("preferences").GetProperty("notifications").GetBoolean());
        Assert.Equal(3, submitAction.Data.Value.GetProperty("preferences").GetProperty("languages").GetArrayLength());
    }

    [Fact]
    public void Serialize_RichTextBlockWithComplexFormatting_PreservesAll()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new RichTextBlock
                {
                    Inlines = new List<object>
                    {
                        "Plain text ",
                        new TextRun
                        {
                            Text = "bold red",
                            Weight = TextWeight.Bolder,
                            Color = TextColor.Attention,
                            Size = TextSize.Large,
                            Italic = true,
                            Underline = true,
                            Strikethrough = false
                        },
                        " more plain ",
                        new TextRun
                        {
                            Text = "link",
                            Color = TextColor.Accent,
                            SelectAction = new OpenUrlAction { Url = "https://example.com" }
                        }
                    }
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        var richTextBlock = deserialized.Body![0] as RichTextBlock;
        Assert.NotNull(richTextBlock);
        Assert.Equal(4, richTextBlock.Inlines?.Count);
        
        var run1 = richTextBlock.Inlines![1] as TextRun;
        Assert.NotNull(run1);
        Assert.Equal("bold red", run1.Text);
        Assert.Equal(TextWeight.Bolder, run1.Weight);
        Assert.Equal(TextColor.Attention, run1.Color);
        Assert.True(run1.Italic);
        Assert.True(run1.Underline);
        
        var run2 = richTextBlock.Inlines[3] as TextRun;
        Assert.NotNull(run2?.SelectAction);
    }

    [Fact]
    public void Deserialize_InvalidJsonStructure_HandlesGracefully()
    {
        // Arrange
        var invalidJson = @"{""type"": ""NotAnAdaptiveCard"", ""version"": ""1.5""}";

        // Act
        var card = AdaptiveCardSerializer.Deserialize(invalidJson);

        // Assert
        // Should deserialize but with unexpected type
        Assert.NotNull(card);
    }

    [Fact]
    public void Serialize_AllInputTypes_PreservesCorrectly()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new InputText { Id = "text1", Value = "test" },
                new InputNumber { Id = "num1", Value = 42.5 },
                new InputDate { Id = "date1", Value = "2024-01-01" },
                new InputTime { Id = "time1", Value = "14:30" },
                new InputToggle { Id = "toggle1", Title = "Accept", Value = "true" },
                new InputChoiceSet
                {
                    Id = "choice1",
                    Choices = new List<Choice>
                    {
                        new Choice { Title = "Option 1", Value = "opt1" },
                        new Choice { Title = "Option 2", Value = "opt2" }
                    }
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);
        var deserialized = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(6, deserialized.Body?.Count);
        Assert.IsType<InputText>(deserialized.Body![0]);
        Assert.IsType<InputNumber>(deserialized.Body[1]);
        Assert.IsType<InputDate>(deserialized.Body[2]);
        Assert.IsType<InputTime>(deserialized.Body[3]);
        Assert.IsType<InputToggle>(deserialized.Body[4]);
        Assert.IsType<InputChoiceSet>(deserialized.Body[5]);
    }
}
