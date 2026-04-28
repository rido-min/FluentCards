using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

/// <summary>
/// Tests for ActionBuilder.WithData overloads (Issue #65).
/// Covers WithData(string), WithData(JsonElement), WithData&lt;T&gt;() on Submit and Execute actions,
/// verifying JSON output equivalence across overloads, and edge cases.
/// </summary>
public class WithDataOverloadTests
{
    private const string SimplePayload = """{"formId":"123","action":"save"}""";

    #region WithData(string) — Submit

    [Fact]
    public void Submit_WithDataString_SetsDataProperty()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(SimplePayload))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("123", action.Data.Value.GetProperty("formId").GetString());
        Assert.Equal("save", action.Data.Value.GetProperty("action").GetString());
    }

    [Fact]
    public void Submit_WithDataString_SerializesToJson()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(SimplePayload))
            .Build();

        var json = card.ToJson();
        Assert.Contains("\"data\":", json);
        Assert.Contains("\"formId\":", json);
    }

    #endregion

    #region WithData(string) — Execute

    [Fact]
    public void Execute_WithDataString_SetsDataProperty()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("doAction")
                .WithData(SimplePayload))
            .Build();

        var action = Assert.IsType<ExecuteAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("123", action.Data.Value.GetProperty("formId").GetString());
    }

    [Fact]
    public void Execute_WithDataString_SerializesToJson()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("doAction")
                .WithData(SimplePayload))
            .Build();

        var json = card.ToJson();
        Assert.Contains("\"data\":", json);
        Assert.Contains("\"formId\":", json);
    }

    #endregion

    #region WithData(JsonElement) — Submit

    [Fact]
    public void Submit_WithDataJsonElement_SetsDataProperty()
    {
        using var doc = JsonDocument.Parse(SimplePayload);
        var element = doc.RootElement;

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(element))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("123", action.Data.Value.GetProperty("formId").GetString());
        Assert.Equal("save", action.Data.Value.GetProperty("action").GetString());
    }

    [Fact]
    public void Submit_WithDataJsonElement_ClonesInput()
    {
        // Verify the builder clones the JsonElement so the source document can be disposed
        JsonElement? dataAfterDispose;
        using (var doc = JsonDocument.Parse(SimplePayload))
        {
            var card = AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Submit("Save")
                    .WithData(doc.RootElement))
                .Build();

            dataAfterDispose = (card.Actions![0] as SubmitAction)?.Data;
        }
        // If Clone() works, accessing properties after document disposal should still work
        Assert.NotNull(dataAfterDispose);
        Assert.Equal("123", dataAfterDispose.Value.GetProperty("formId").GetString());
    }

    #endregion

    #region WithData(JsonElement) — Execute

    [Fact]
    public void Execute_WithDataJsonElement_SetsDataProperty()
    {
        using var doc = JsonDocument.Parse(SimplePayload);

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("doAction")
                .WithData(doc.RootElement))
            .Build();

        var action = Assert.IsType<ExecuteAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("123", action.Data.Value.GetProperty("formId").GetString());
    }

    [Fact]
    public void Execute_WithDataJsonElement_ClonesInput()
    {
        JsonElement? dataAfterDispose;
        using (var doc = JsonDocument.Parse(SimplePayload))
        {
            var card = AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Execute("Run")
                    .WithVerb("doAction")
                    .WithData(doc.RootElement))
                .Build();

            dataAfterDispose = (card.Actions![0] as ExecuteAction)?.Data;
        }
        Assert.NotNull(dataAfterDispose);
        Assert.Equal("123", dataAfterDispose.Value.GetProperty("formId").GetString());
    }

    #endregion

    #region WithData<T>() — Submit

    [Fact]
    public void Submit_WithDataGeneric_RegisteredType_SetsDataProperty()
    {
        // Fact is registered in FluentCardsJsonContext
        var fact = new Fact { Title = "key", Value = "val" };

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(fact))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("key", action.Data.Value.GetProperty("title").GetString());
        Assert.Equal("val", action.Data.Value.GetProperty("value").GetString());
    }

    [Fact]
    public void Submit_WithDataGeneric_UnregisteredType_ThrowsInvalidOperation()
    {
        var unregistered = new UnregisteredPayload { Id = 1, Name = "test" };

        var ex = Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Submit("Save")
                    .WithData(unregistered))
                .Build());

        Assert.Contains("not registered in FluentCardsJsonContext", ex.Message);
    }

    #endregion

    #region WithData<T>() — Execute

    [Fact]
    public void Execute_WithDataGeneric_RegisteredType_SetsDataProperty()
    {
        var fact = new Fact { Title = "command", Value = "refresh" };

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("refresh")
                .WithData(fact))
            .Build();

        var action = Assert.IsType<ExecuteAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal("command", action.Data.Value.GetProperty("title").GetString());
        Assert.Equal("refresh", action.Data.Value.GetProperty("value").GetString());
    }

    [Fact]
    public void Execute_WithDataGeneric_UnregisteredType_ThrowsInvalidOperation()
    {
        var unregistered = new UnregisteredPayload { Id = 1, Name = "test" };

        var ex = Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Execute("Run")
                    .WithVerb("action")
                    .WithData(unregistered))
                .Build());

        Assert.Contains("not registered in FluentCardsJsonContext", ex.Message);
    }

    #endregion

    #region Overload Equivalence — Same JSON Output

    [Fact]
    public void Submit_AllOverloads_ProduceIdenticalJson()
    {
        // Build via WithData(string)
        var cardFromString = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(SimplePayload))
            .Build();

        // Build via WithData(JsonElement)
        using var doc = JsonDocument.Parse(SimplePayload);
        var cardFromElement = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(doc.RootElement))
            .Build();

        var jsonFromString = cardFromString.ToJson();
        var jsonFromElement = cardFromElement.ToJson();

        Assert.Equal(jsonFromString, jsonFromElement);
    }

    [Fact]
    public void Execute_AllOverloads_ProduceIdenticalJson()
    {
        // Build via WithData(string)
        var cardFromString = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("doAction")
                .WithData(SimplePayload))
            .Build();

        // Build via WithData(JsonElement)
        using var doc = JsonDocument.Parse(SimplePayload);
        var cardFromElement = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("doAction")
                .WithData(doc.RootElement))
            .Build();

        var jsonFromString = cardFromString.ToJson();
        var jsonFromElement = cardFromElement.ToJson();

        Assert.Equal(jsonFromString, jsonFromElement);
    }

    [Fact]
    public void Submit_StringAndElement_RoundtripProduceSameResult()
    {
        var cardFromString = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(SimplePayload))
            .Build();

        using var doc = JsonDocument.Parse(SimplePayload);
        var cardFromElement = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(doc.RootElement))
            .Build();

        // Round-trip both through serialization/deserialization
        var rt1 = AdaptiveCardExtensions.FromJson(cardFromString.ToJson());
        var rt2 = AdaptiveCardExtensions.FromJson(cardFromElement.ToJson());

        var action1 = Assert.IsType<SubmitAction>(Assert.Single(rt1!.Actions!));
        var action2 = Assert.IsType<SubmitAction>(Assert.Single(rt2!.Actions!));

        Assert.Equal(
            action1.Data!.Value.GetRawText(),
            action2.Data!.Value.GetRawText());
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Submit_WithDataString_EmptyObject_SetsEmptyData()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData("{}"))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.Object, action.Data.Value.ValueKind);
    }

    [Fact]
    public void Execute_WithDataString_EmptyObject_SetsEmptyData()
    {
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("action")
                .WithData("{}"))
            .Build();

        var action = Assert.IsType<ExecuteAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.Object, action.Data.Value.ValueKind);
    }

    [Fact]
    public void Submit_WithDataString_NestedObjects_PreservesStructure()
    {
        var nested = """{"outer":{"inner":{"deep":"value"},"list":[1,2,3]}}""";

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData(nested))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);

        var outer = action.Data.Value.GetProperty("outer");
        Assert.Equal("value", outer.GetProperty("inner").GetProperty("deep").GetString());
        Assert.Equal(3, outer.GetProperty("list").GetArrayLength());
    }

    [Fact]
    public void Execute_WithDataString_NestedObjects_PreservesStructure()
    {
        var nested = """{"outer":{"inner":{"deep":"value"},"list":[1,2,3]}}""";

        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Execute("Run")
                .WithVerb("action")
                .WithData(nested))
            .Build();

        var action = Assert.IsType<ExecuteAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);

        var outer = action.Data.Value.GetProperty("outer");
        Assert.Equal("value", outer.GetProperty("inner").GetProperty("deep").GetString());
        Assert.Equal(3, outer.GetProperty("list").GetArrayLength());
    }

    [Fact]
    public void Submit_WithDataString_InvalidJson_Throws()
    {
        Assert.ThrowsAny<JsonException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Submit("Save")
                    .WithData("not-json"))
                .Build());
    }

    [Fact]
    public void Execute_WithDataString_InvalidJson_Throws()
    {
        Assert.ThrowsAny<JsonException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Execute("Run")
                    .WithVerb("action")
                    .WithData("not-json"))
                .Build());
    }

    [Fact]
    public void OpenUrl_WithData_ThrowsInvalidOperation()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .OpenUrl("https://example.com", "Open")
                    .WithData(SimplePayload))
                .Build());

        Assert.Contains("WithData()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Fact]
    public void ShowCard_WithData_ThrowsInvalidOperation()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .ShowCard("Show")
                    .WithData(SimplePayload))
                .Build());

        Assert.Contains("WithData()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Fact]
    public void ToggleVisibility_WithData_ThrowsInvalidOperation()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .ToggleVisibility("Toggle")
                    .WithData(SimplePayload))
                .Build());

        Assert.Contains("WithData()", ex.Message);
        Assert.Contains("Submit or Execute", ex.Message);
    }

    [Fact]
    public void WithData_BeforeActionTypeSet_ThrowsInvalidOperation()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var builder = new ActionBuilder();
            builder.WithData(SimplePayload);
        });
    }

    [Fact]
    public void WithDataJsonElement_BeforeActionTypeSet_ThrowsInvalidOperation()
    {
        using var doc = JsonDocument.Parse(SimplePayload);
        Assert.Throws<InvalidOperationException>(() =>
        {
            var builder = new ActionBuilder();
            builder.WithData(doc.RootElement);
        });
    }

    [Fact]
    public void WithDataGeneric_BeforeActionTypeSet_ThrowsInvalidOperation()
    {
        var fact = new Fact { Title = "key", Value = "val" };
        Assert.Throws<InvalidOperationException>(() =>
        {
            var builder = new ActionBuilder();
            builder.WithData(fact);
        });
    }

    [Fact]
    public void Submit_WithDataString_NumericArray_ParsesCorrectly()
    {
        // Arrays are valid JSON — verify they parse through WithData(string)
        var card = AdaptiveCardBuilder.Create()
            .AddAction(a => a
                .Submit("Save")
                .WithData("[1,2,3]"))
            .Build();

        var action = Assert.IsType<SubmitAction>(Assert.Single(card.Actions!));
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.Array, action.Data.Value.ValueKind);
    }

    [Fact]
    public void Submit_WithData_ThenWithTeamsData_ThrowsConflict()
    {
        Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Submit("Save")
                    .WithData(SimplePayload)
                    .WithTeamsData(td => td.WithTaskFetch()))
                .Build());
    }

    [Fact]
    public void Submit_WithTeamsData_ThenWithData_ThrowsConflict()
    {
        Assert.Throws<InvalidOperationException>(() =>
            AdaptiveCardBuilder.Create()
                .AddAction(a => a
                    .Submit("Save")
                    .WithTeamsData(td => td.WithTaskFetch())
                    .WithData(SimplePayload))
                .Build());
    }

    #endregion

    /// <summary>
    /// Helper type NOT registered in FluentCardsJsonContext — used to test WithData&lt;T&gt;() error handling.
    /// </summary>
    private sealed class UnregisteredPayload
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
