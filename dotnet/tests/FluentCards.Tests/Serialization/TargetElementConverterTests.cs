using System.Text.Json;
using FluentCards.Serialization;
using Xunit;

namespace FluentCards.Tests.Serialization;

public class TargetElementConverterTests
{
    [Fact]
    public void Deserialize_ArrayWithOnlyStrings_ReturnsStringList()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.ToggleVisibility"",
            ""targetElements"": [""element1"", ""element2"", ""element3""]
        }";

        // Act
        var action = JsonSerializer.Deserialize<ToggleVisibilityAction>(json, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.TargetElements);
        Assert.Equal(3, action.TargetElements.Count);
        Assert.All(action.TargetElements, item => Assert.IsType<string>(item));
        Assert.Equal("element1", action.TargetElements[0]);
        Assert.Equal("element2", action.TargetElements[1]);
        Assert.Equal("element3", action.TargetElements[2]);
    }

    [Fact]
    public void Deserialize_ArrayWithOnlyObjects_ReturnsTargetElementList()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.ToggleVisibility"",
            ""targetElements"": [
                {""elementId"": ""elem1"", ""isVisible"": true},
                {""elementId"": ""elem2"", ""isVisible"": false}
            ]
        }";

        // Act
        var action = JsonSerializer.Deserialize<ToggleVisibilityAction>(json, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.TargetElements);
        Assert.Equal(2, action.TargetElements.Count);
        Assert.All(action.TargetElements, item => Assert.IsType<TargetElement>(item));
        
        var elem1 = action.TargetElements[0] as TargetElement;
        var elem2 = action.TargetElements[1] as TargetElement;
        Assert.Equal("elem1", elem1?.ElementId);
        Assert.True(elem1?.IsVisible);
        Assert.Equal("elem2", elem2?.ElementId);
        Assert.False(elem2?.IsVisible);
    }

    [Fact]
    public void Deserialize_MixedArray_ReturnsMixedList()
    {
        // Arrange
        var json = @"{
            ""type"": ""Action.ToggleVisibility"",
            ""targetElements"": [
                ""simpleId"",
                {""elementId"": ""complexId"", ""isVisible"": true},
                ""anotherId""
            ]
        }";

        // Act
        var action = JsonSerializer.Deserialize<ToggleVisibilityAction>(json, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.NotNull(action);
        Assert.NotNull(action.TargetElements);
        Assert.Equal(3, action.TargetElements.Count);
        Assert.IsType<string>(action.TargetElements[0]);
        Assert.IsType<TargetElement>(action.TargetElements[1]);
        Assert.IsType<string>(action.TargetElements[2]);
        
        Assert.Equal("simpleId", action.TargetElements[0]);
        var elem = action.TargetElements[1] as TargetElement;
        Assert.Equal("complexId", elem?.ElementId);
        Assert.Equal("anotherId", action.TargetElements[2]);
    }

    [Fact]
    public void Serialize_MixedArray_PreservesTypes()
    {
        // Arrange
        var action = new ToggleVisibilityAction
        {
            TargetElements = new List<object>
            {
                "stringId",
                new TargetElement { ElementId = "objectId", IsVisible = true }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.Contains("\"stringId\"", json);
        Assert.Contains("\"elementId\":\"objectId\"", json.Replace(" ", ""));
        Assert.Contains("\"isVisible\":true", json.Replace(" ", ""));
    }

    [Fact]
    public void Serialize_EmptyArray_SerializesCorrectly()
    {
        // Arrange
        var action = new ToggleVisibilityAction
        {
            TargetElements = new List<object>()
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.Contains("\"targetElements\":[]", json.Replace(" ", ""));
    }

    [Fact]
    public void Serialize_NullValue_SerializesAsNull()
    {
        // Arrange
        var action = new ToggleVisibilityAction
        {
            TargetElements = null
        };

        // Act
        var json = JsonSerializer.Serialize(action, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        // Null values are omitted by default
        Assert.DoesNotContain("targetElements", json);
    }

    [Fact]
    public void Roundtrip_MixedArray_PreservesStructure()
    {
        // Arrange
        var original = new ToggleVisibilityAction
        {
            TargetElements = new List<object>
            {
                "id1",
                new TargetElement { ElementId = "id2", IsVisible = false },
                "id3"
            }
        };

        // Act
        var json = JsonSerializer.Serialize(original, FluentCardsJsonContext.Default.ToggleVisibilityAction);
        var deserialized = JsonSerializer.Deserialize<ToggleVisibilityAction>(json, FluentCardsJsonContext.Default.ToggleVisibilityAction);

        // Assert
        Assert.NotNull(deserialized);
        Assert.NotNull(deserialized.TargetElements);
        Assert.Equal(3, deserialized.TargetElements.Count);
        Assert.Equal("id1", deserialized.TargetElements[0]);
        Assert.IsType<TargetElement>(deserialized.TargetElements[1]);
        Assert.Equal("id3", deserialized.TargetElements[2]);
    }
}
