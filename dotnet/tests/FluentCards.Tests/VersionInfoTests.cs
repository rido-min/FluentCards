using Xunit;

namespace FluentCards.Tests;

public class VersionInfoTests
{
    // ── GetElementVersion ──────────────────────────────────────────────

    [Theory]
    [InlineData("TextBlock")]
    [InlineData("Image")]
    [InlineData("Container")]
    [InlineData("ColumnSet")]
    [InlineData("FactSet")]
    [InlineData("ImageSet")]
    [InlineData("Column")]
    [InlineData("Fact")]
    [InlineData("Choice")]
    [InlineData("Action.OpenUrl")]
    [InlineData("Action.Submit")]
    [InlineData("Action.ShowCard")]
    [InlineData("Input.Text")]
    [InlineData("Input.Number")]
    [InlineData("Input.Date")]
    [InlineData("Input.Time")]
    [InlineData("Input.Toggle")]
    [InlineData("Input.ChoiceSet")]
    public void GetElementVersion_V10Types_ReturnsV10(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_0, result);
    }

    [Theory]
    [InlineData("Media")]
    [InlineData("MediaSource")]
    public void GetElementVersion_V11Types_ReturnsV11(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_1, result);
    }

    [Theory]
    [InlineData("RichTextBlock")]
    [InlineData("ActionSet")]
    [InlineData("TextRun")]
    [InlineData("BackgroundImage")]
    [InlineData("TargetElement")]
    [InlineData("Action.ToggleVisibility")]
    public void GetElementVersion_V12Types_ReturnsV12(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_2, result);
    }

    [Fact]
    public void GetElementVersion_AssociatedInputs_ReturnsV13()
    {
        // Act
        var result = VersionInfo.GetElementVersion("AssociatedInputs");

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_3, result);
    }

    [Theory]
    [InlineData("Action.Execute")]
    [InlineData("RefreshConfiguration")]
    [InlineData("AuthenticationConfiguration")]
    [InlineData("TokenExchangeResource")]
    [InlineData("AuthCardButton")]
    public void GetElementVersion_V14Types_ReturnsV14(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_4, result);
    }

    [Theory]
    [InlineData("Table")]
    [InlineData("TableRow")]
    [InlineData("TableCell")]
    [InlineData("TableColumnDefinition")]
    [InlineData("ActionMode")]
    [InlineData("TextBlockStyle")]
    public void GetElementVersion_V15Types_ReturnsV15(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_5, result);
    }

    [Theory]
    [InlineData("CaptionSource")]
    [InlineData("DataQuery")]
    [InlineData("CardMetadata")]
    [InlineData("InputLabelPosition")]
    [InlineData("InputStyle")]
    [InlineData("ChoiceInputStyle.Filtered")]
    public void GetElementVersion_V16Types_ReturnsV16(string elementType)
    {
        // Act
        var result = VersionInfo.GetElementVersion(elementType);

        // Assert
        Assert.Equal(AdaptiveCardVersion.V1_6, result);
    }

    [Fact]
    public void GetElementVersion_UnknownType_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(
            () => VersionInfo.GetElementVersion("FooBar"));

        Assert.Contains("FooBar", ex.Message);
    }

    // ── GetCardPropertyVersion ─────────────────────────────────────────

    [Theory]
    [InlineData("type", AdaptiveCardVersion.V1_0)]
    [InlineData("version", AdaptiveCardVersion.V1_0)]
    [InlineData("$schema", AdaptiveCardVersion.V1_0)]
    [InlineData("body", AdaptiveCardVersion.V1_0)]
    [InlineData("actions", AdaptiveCardVersion.V1_0)]
    [InlineData("fallbackText", AdaptiveCardVersion.V1_0)]
    [InlineData("speak", AdaptiveCardVersion.V1_0)]
    [InlineData("lang", AdaptiveCardVersion.V1_0)]
    [InlineData("selectAction", AdaptiveCardVersion.V1_1)]
    [InlineData("minHeight", AdaptiveCardVersion.V1_2)]
    [InlineData("verticalContentAlignment", AdaptiveCardVersion.V1_2)]
    [InlineData("refresh", AdaptiveCardVersion.V1_4)]
    [InlineData("authentication", AdaptiveCardVersion.V1_4)]
    [InlineData("rtl", AdaptiveCardVersion.V1_5)]
    [InlineData("metadata", AdaptiveCardVersion.V1_6)]
    public void GetCardPropertyVersion_KnownProperty_ReturnsExpectedVersion(
        string propertyName, AdaptiveCardVersion expected)
    {
        // Act
        var result = VersionInfo.GetCardPropertyVersion(propertyName);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetCardPropertyVersion_UnknownProperty_ThrowsArgumentException()
    {
        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(
            () => VersionInfo.GetCardPropertyVersion("foobar"));

        Assert.Contains("foobar", ex.Message);
    }
}
