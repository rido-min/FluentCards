using System.Text.Json;
using FluentCards.Serialization;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class ActionDataConverterTests
{
    [Fact]
    public void Serialize_SimpleObject_PreservesStructure()
    {
        // Arrange
        var data = JsonDocument.Parse(@"{""key"": ""value"", ""number"": 42}").RootElement;
        var action = new SubmitAction
        {
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.Contains("\"data\":{\"key\":\"value\",\"number\":42}", json.Replace(" ", "").Replace("\r", "").Replace("\n", ""));
    }

    [Fact]
    public void Serialize_NestedObject_PreservesStructure()
    {
        // Arrange
        var data = JsonDocument.Parse(@"{
            ""user"": {
                ""name"": ""John"",
                ""age"": 30
            },
            ""active"": true
        }").RootElement;
        var action = new ExecuteAction
        {
            Verb = "submit",
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.ExecuteAction);

        // Assert
        Assert.Contains("\"user\":{\"name\":\"John\",\"age\":30}", json.Replace(" ", "").Replace("\r", "").Replace("\n", ""));
        Assert.Contains("\"active\":true", json.Replace(" ", "").Replace("\r", "").Replace("\n", ""));
    }

    [Fact]
    public void Serialize_ArrayData_PreservesStructure()
    {
        // Arrange
        var data = JsonDocument.Parse(@"[1, 2, 3, 4, 5]").RootElement;
        var action = new SubmitAction
        {
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.Contains("\"data\":[1,2,3,4,5]", json.Replace(" ", "").Replace("\r", "").Replace("\n", ""));
    }

    [Fact]
    public void Deserialize_SimpleObject_PreservesJsonElement()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.Submit"",
            ""data"": {
                ""key"": ""value"",
                ""number"": 42
            }
        }";

        // Act
        var action = JsonSerializer.Deserialize<SubmitAction>(json, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.Object, action.Data.Value.ValueKind);
        Assert.Equal("value", action.Data.Value.GetProperty("key").GetString());
        Assert.Equal(42, action.Data.Value.GetProperty("number").GetInt32());
    }

    [Fact]
    public void Deserialize_NullData_ReturnsNull()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.Submit"",
            ""data"": null
        }";

        // Act
        var action = JsonSerializer.Deserialize<SubmitAction>(json, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.NotNull(action);
        Assert.Null(action.Data);
    }

    [Fact]
    public void Serialize_NullData_OmittedFromOutput()
    {
        // Arrange
        var action = new SubmitAction
        {
            Data = null
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        // Null values are omitted by default
        Assert.DoesNotContain("\"data\"", json);
    }

    [Fact]
    public void Serialize_EmptyObject_SerializesCorrectly()
    {
        // Arrange
        var data = JsonDocument.Parse(@"{}").RootElement;
        var action = new ExecuteAction
        {
            Verb = "action",
            Data = data
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.ExecuteAction);

        // Assert
        Assert.Contains("\"data\":{}", json.Replace(" ", ""));
    }

    [Fact]
    public void Roundtrip_ComplexData_PreservesStructure()
    {
        // Arrange
        var originalData = JsonDocument.Parse(@"{
            ""string"": ""text"",
            ""number"": 123,
            ""boolean"": true,
            ""array"": [1, 2, 3],
            ""object"": {
                ""nested"": ""value""
            }
        }").RootElement;
        var original = new SubmitAction
        {
            Data = originalData
        };

        // Act
        var json = JsonSerializer.Serialize(original, FluentCardsJsonContext.Default.SubmitAction);
        var deserialized = JsonSerializer.Deserialize<SubmitAction>(json, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.Data);
        Assert.Equal("text", deserialized.Data.Value.GetProperty("string").GetString());
        Assert.Equal(123, deserialized.Data.Value.GetProperty("number").GetInt32());
        Assert.True(deserialized.Data.Value.GetProperty("boolean").GetBoolean());
        Assert.Equal(3, deserialized.Data.Value.GetProperty("array").GetArrayLength());
        Assert.Equal("value", deserialized.Data.Value.GetProperty("object").GetProperty("nested").GetString());
    }

    [Fact]
    public void Deserialize_StringValue_PreservesType()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.Submit"",
            ""data"": ""simple string""
        }";

        // Act
        var action = JsonSerializer.Deserialize<SubmitAction>(json, FluentCardsJsonContext.Default.SubmitAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.String, action.Data.Value.ValueKind);
        Assert.Equal("simple string", action.Data.Value.GetString());
    }

    [Fact]
    public void Deserialize_NumberValue_PreservesType()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.Execute"",
            ""verb"": ""test"",
            ""data"": 42.5
        }";

        // Act
        var action = JsonSerializer.Deserialize<ExecuteAction>(json, FluentCardsJsonContext.Default.ExecuteAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.Data);
        Assert.Equal(JsonValueKind.Number, action.Data.Value.ValueKind);
        Assert.Equal(42.5, action.Data.Value.GetDouble());
    }
}
