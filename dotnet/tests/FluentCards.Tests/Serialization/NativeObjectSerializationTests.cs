using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class NativeObjectSerializationTests
{
    private static AdaptiveCard CreateTestCard() =>
        AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .AddTextBlock(tb => tb
                .WithText("Hello, World!")
                .WithSize(TextSize.Large))
            .Build();

    [Fact]
    public void SerializeToElement_ReturnsJsonElementWithCorrectStructure()
    {
        var card = CreateTestCard();

        var element = AdaptiveCardSerializer.SerializeToElement(card);

        Assert.Equal(JsonValueKind.Object, element.ValueKind);
        Assert.Equal("AdaptiveCard", element.GetProperty("type").GetString());
        Assert.Equal("1.5", element.GetProperty("version").GetString());
        Assert.True(element.TryGetProperty("body", out var body));
        Assert.Equal(JsonValueKind.Array, body.ValueKind);
        Assert.Equal(1, body.GetArrayLength());
    }

    [Fact]
    public void SerializeToElement_ProducesEquivalentOutputToSerialize()
    {
        var card = CreateTestCard();

        using var doc = JsonDocument.Parse(AdaptiveCardSerializer.Serialize(card));
        var fromString = doc.RootElement;
        var fromElement = AdaptiveCardSerializer.SerializeToElement(card);

        Assert.Equal(fromString.GetProperty("type").GetString(), fromElement.GetProperty("type").GetString());
        Assert.Equal(fromString.GetProperty("version").GetString(), fromElement.GetProperty("version").GetString());
    }

    [Fact]
    public void SerializeToElement_OmitsNullProperties()
    {
        var card = CreateTestCard();

        var element = AdaptiveCardSerializer.SerializeToElement(card);

        Assert.False(element.TryGetProperty("actions", out _));
        Assert.False(element.TryGetProperty("refresh", out _));
    }

    [Fact]
    public void SerializeToNode_ReturnsMutableJsonNode()
    {
        var card = CreateTestCard();

        var node = AdaptiveCardSerializer.SerializeToNode(card);

        Assert.NotNull(node);
        Assert.Equal("AdaptiveCard", node["type"]?.GetValue<string>());

        // Verify mutability — add a property
        node["customProp"] = "test";
        Assert.Equal("test", node["customProp"]?.GetValue<string>());
    }

    [Fact]
    public void SerializeToNode_CanModifyAndReserialize()
    {
        var card = CreateTestCard();

        var node = AdaptiveCardSerializer.SerializeToNode(card);
        Assert.NotNull(node);

        node["$schema"] = "https://adaptivecards.io/schemas/adaptive-card.json";
        var json = node.ToJsonString();

        Assert.Contains("\"$schema\":\"https://adaptivecards.io/schemas/adaptive-card.json\"", json);
        Assert.Contains("\"type\":\"AdaptiveCard\"", json);
    }

    [Fact]
    public void ToJsonElement_ExtensionMethod_ReturnsJsonElement()
    {
        var card = CreateTestCard();

        var element = card.ToJsonElement();

        Assert.Equal(JsonValueKind.Object, element.ValueKind);
        Assert.Equal("AdaptiveCard", element.GetProperty("type").GetString());
    }

    [Fact]
    public void ToJsonNode_ExtensionMethod_ReturnsMutableJsonNode()
    {
        var card = CreateTestCard();

        var node = card.ToJsonNode();

        Assert.NotNull(node);
        Assert.Equal("1.5", node["version"]?.GetValue<string>());
    }

    [Fact]
    public void ToJsonElement_CanBeEmbeddedInLargerPayload()
    {
        var card = CreateTestCard();

        // Simulate embedding a card into a larger message payload
        var cardElement = card.ToJsonElement();
        var wrapper = new JsonObject
        {
            ["contentType"] = "application/vnd.microsoft.card.adaptive",
            ["content"] = JsonNode.Parse(cardElement.GetRawText())
        };

        var json = wrapper.ToJsonString();
        Assert.Contains("\"contentType\":\"application/vnd.microsoft.card.adaptive\"", json);
        Assert.Contains("\"type\":\"AdaptiveCard\"", json);
    }

    [Fact]
    public void SerializeToElement_WithActions_PreservesActionData()
    {
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.4")
            .AddAction(a => a
                .Execute("Test")
                .WithVerb("doSomething")
                .WithData("""{"key":"value"}"""))
            .Build();

        var element = AdaptiveCardSerializer.SerializeToElement(card);

        var actions = element.GetProperty("actions");
        Assert.Equal(1, actions.GetArrayLength());
        var action = actions[0];
        Assert.Equal("Action.Execute", action.GetProperty("type").GetString());
        Assert.Equal("value", action.GetProperty("data").GetProperty("key").GetString());
    }
}
