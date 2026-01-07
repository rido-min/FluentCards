using System.Text;
using FluentCards.Serialization;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class AdaptiveCardSerializerTests
{
    [Fact]
    public void Serialize_SimpleCard_ReturnsJsonString()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello, World!" }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);

        // Assert
        Assert.NotNull(json);
        Assert.Contains("\"type\":\"AdaptiveCard\"", json.Replace(" ", ""));
        Assert.Contains("Hello, World!", json);
    }

    [Fact]
    public void Serialize_WithIndentation_ReturnsFormattedJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Test" }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card, indented: true);

        // Assert
        Assert.Contains("\n", json);
        Assert.Contains("  ", json);
    }

    [Fact]
    public void Serialize_WithoutIndentation_ReturnsCompactJson()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Test" }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card, indented: false);

        // Assert
        Assert.DoesNotContain("\n  ", json);
    }

    [Fact]
    public void Deserialize_ValidJson_ReturnsCard()
    {
        // Arrange
        var json = @"{""type"":""AdaptiveCard"",""version"":""1.5"",""body"":[{""type"":""TextBlock"",""text"":""Hello""}]}";

        // Act
        var card = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(card);
        Assert.Equal("AdaptiveCard", card.Type);
        Assert.Single(card.Body!);
    }

    [Fact]
    public void RoundtripSerialization_PreservesAllProperties()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Version = "1.5",
            Body = new List<AdaptiveElement>
            {
                new TextBlock
                {
                    Text = "Test Text",
                    Size = TextSize.Medium,
                    Weight = TextWeight.Bolder
                }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(originalCard);
        var deserializedCard = AdaptiveCardSerializer.Deserialize(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.Equal(originalCard.Version, deserializedCard.Version);
        Assert.NotNull(deserializedCard.Body);
        Assert.Single(deserializedCard.Body);
        
        var textBlock = deserializedCard.Body[0] as TextBlock;
        Assert.NotNull(textBlock);
        Assert.Equal("Test Text", textBlock.Text);
        Assert.Equal(TextSize.Medium, textBlock.Size);
        Assert.Equal(TextWeight.Bolder, textBlock.Weight);
    }

    [Fact]
    public void TryDeserialize_ValidJson_ReturnsTrue()
    {
        // Arrange
        var json = @"{""type"":""AdaptiveCard"",""version"":""1.5""}";

        // Act
        var result = AdaptiveCardSerializer.TryDeserialize(json, out var card, out var errorMessage);

        // Assert
        Assert.True(result);
        Assert.NotNull(card);
        Assert.Null(errorMessage);
    }

    [Fact]
    public void TryDeserialize_InvalidJson_ReturnsFalseWithErrorMessage()
    {
        // Arrange
        var json = @"{invalid json}";

        // Act
        var result = AdaptiveCardSerializer.TryDeserialize(json, out var card, out var errorMessage);

        // Assert
        Assert.False(result);
        Assert.Null(card);
        Assert.NotNull(errorMessage);
    }

    [Fact]
    public void SerializeToUtf8Bytes_ReturnsValidUtf8()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello" }
            }
        };

        // Act
        var bytes = AdaptiveCardSerializer.SerializeToUtf8Bytes(card);

        // Assert
        Assert.NotEmpty(bytes);
        var json = Encoding.UTF8.GetString(bytes);
        Assert.Contains("Hello", json);
    }

    [Fact]
    public void DeserializeFromUtf8Bytes_ValidBytes_ReturnsCard()
    {
        // Arrange
        var json = @"{""type"":""AdaptiveCard"",""version"":""1.5""}";
        var bytes = Encoding.UTF8.GetBytes(json);

        // Act
        var card = AdaptiveCardSerializer.DeserializeFromUtf8Bytes(bytes);

        // Assert
        Assert.NotNull(card);
        Assert.Equal("AdaptiveCard", card.Type);
    }

    [Fact]
    public async Task SerializeAsync_WritesToStream()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Async Test" }
            }
        };
        using var stream = new MemoryStream();

        // Act
        await AdaptiveCardSerializer.SerializeAsync(stream, card);

        // Assert
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        Assert.Contains("Async Test", json);
    }

    [Fact]
    public async Task DeserializeAsync_ReadsFromStream()
    {
        // Arrange
        var json = @"{""type"":""AdaptiveCard"",""version"":""1.5"",""body"":[{""type"":""TextBlock"",""text"":""Stream Test""}]}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

        // Act
        var card = await AdaptiveCardSerializer.DeserializeAsync(stream);

        // Assert
        Assert.NotNull(card);
        Assert.Single(card.Body!);
        var textBlock = card.Body[0] as TextBlock;
        Assert.Equal("Stream Test", textBlock?.Text);
    }

    [Fact]
    public void Serialize_NullProperties_OmittedFromOutput()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Simple" }
            }
        };

        // Act
        var json = AdaptiveCardSerializer.Serialize(card);

        // Assert
        Assert.DoesNotContain("\"size\":", json);
        Assert.DoesNotContain("\"weight\":", json);
        Assert.DoesNotContain("\"color\":", json);
    }
}
