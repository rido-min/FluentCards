using Xunit;

namespace FluentCards.Tests;

public class AdaptiveCardVersionTests
{
    [Theory]
    [InlineData(AdaptiveCardVersion.V1_0, "1.0")]
    [InlineData(AdaptiveCardVersion.V1_1, "1.1")]
    [InlineData(AdaptiveCardVersion.V1_2, "1.2")]
    [InlineData(AdaptiveCardVersion.V1_3, "1.3")]
    [InlineData(AdaptiveCardVersion.V1_4, "1.4")]
    [InlineData(AdaptiveCardVersion.V1_5, "1.5")]
    [InlineData(AdaptiveCardVersion.V1_6, "1.6")]
    public void ToVersionString_KnownVersion_ReturnsExpectedString(
        AdaptiveCardVersion version, string expected)
    {
        // Act
        var result = version.ToVersionString();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ToVersionString_UndefinedEnumValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var undefined = (AdaptiveCardVersion)99;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => undefined.ToVersionString());
    }

    [Theory]
    [InlineData(AdaptiveCardVersion.V1_0, "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_1, "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_2, "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_3, "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_4, "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_5, "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json")]
    [InlineData(AdaptiveCardVersion.V1_6, "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json")]
    public void ToSchemaUrl_KnownVersion_ReturnsVersionedSchemaUrl(
        AdaptiveCardVersion version, string expectedUrl)
    {
        // Act
        var result = version.ToSchemaUrl();

        // Assert
        Assert.Equal(expectedUrl, result);
    }

    [Fact]
    public void ToSchemaUrl_UndefinedEnumValue_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var undefined = (AdaptiveCardVersion)99;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => undefined.ToSchemaUrl());
    }

    [Theory]
    [InlineData("1.0", AdaptiveCardVersion.V1_0)]
    [InlineData("1.1", AdaptiveCardVersion.V1_1)]
    [InlineData("1.2", AdaptiveCardVersion.V1_2)]
    [InlineData("1.3", AdaptiveCardVersion.V1_3)]
    [InlineData("1.4", AdaptiveCardVersion.V1_4)]
    [InlineData("1.5", AdaptiveCardVersion.V1_5)]
    [InlineData("1.6", AdaptiveCardVersion.V1_6)]
    public void TryParse_ValidVersionString_ReturnsTrueAndParsesCorrectly(
        string input, AdaptiveCardVersion expected)
    {
        // Act
        var success = AdaptiveCardVersionExtensions.TryParse(input, out var result);

        // Assert
        Assert.True(success);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TryParse_UnknownVersion_ReturnsFalse()
    {
        // Act
        var success = AdaptiveCardVersionExtensions.TryParse("2.0", out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(default(AdaptiveCardVersion), result);
    }

    [Fact]
    public void TryParse_NullInput_ReturnsFalse()
    {
        // Act
        var success = AdaptiveCardVersionExtensions.TryParse(null!, out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(default(AdaptiveCardVersion), result);
    }

    [Fact]
    public void TryParse_EmptyString_ReturnsFalse()
    {
        // Act
        var success = AdaptiveCardVersionExtensions.TryParse("", out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(default(AdaptiveCardVersion), result);
    }

    [Fact]
    public void TryParse_WhitespaceString_ReturnsFalse()
    {
        // Act
        var success = AdaptiveCardVersionExtensions.TryParse(" ", out var result);

        // Assert
        Assert.False(success);
        Assert.Equal(default(AdaptiveCardVersion), result);
    }

    [Theory]
    [InlineData(AdaptiveCardVersion.V1_0)]
    [InlineData(AdaptiveCardVersion.V1_1)]
    [InlineData(AdaptiveCardVersion.V1_2)]
    [InlineData(AdaptiveCardVersion.V1_3)]
    [InlineData(AdaptiveCardVersion.V1_4)]
    [InlineData(AdaptiveCardVersion.V1_5)]
    [InlineData(AdaptiveCardVersion.V1_6)]
    public void TryParse_RoundTrip_AllMembersReturnOriginalValue(
        AdaptiveCardVersion original)
    {
        // Arrange
        var versionString = original.ToVersionString();

        // Act
        var success = AdaptiveCardVersionExtensions.TryParse(versionString, out var roundTripped);

        // Assert
        Assert.True(success);
        Assert.Equal(original, roundTripped);
    }

    [Fact]
    public void WithVersion_EnumOverload_SetsBothVersionAndSchema()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .Build();

        // Assert
        Assert.Equal("1.5", card.Version);
        Assert.Equal("https://adaptivecards.io/schemas/1.5.0/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithVersion_KnownString_SetsBothVersionAndSchema()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("1.5")
            .Build();

        // Assert
        Assert.Equal("1.5", card.Version);
        Assert.Equal("https://adaptivecards.io/schemas/1.5.0/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithVersion_UnknownString_SetsVersionOnly_LeavesSchemaAtDefault()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion("2.0")
            .Build();

        // Assert
        Assert.Equal("2.0", card.Version);
        Assert.Equal("http://adaptivecards.io/schemas/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithSchema_AfterWithVersion_OverridesSchemaUrl()
    {
        // Arrange & Act
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .WithSchema("https://example.com/custom-schema.json")
            .Build();

        // Assert
        Assert.Equal("1.5", card.Version);
        Assert.Equal("https://example.com/custom-schema.json", card.Schema);
    }

    [Fact]
    public void WithVersion_UnknownStringAfterKnownEnum_ResetsSchemaToDefault()
    {
        // Arrange & Act — chaining a known version then an unknown version
        // must not leave the schema URL pointing at the previous known version.
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .WithVersion("2.0")
            .Build();

        // Assert
        Assert.Equal("2.0", card.Version);
        Assert.Equal("http://adaptivecards.io/schemas/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithVersion_EmptyString_SetsEmptyVersionAndDefaultSchema()
    {
        // Arrange & Act — empty string is not a known version, so it falls
        // through to the unknown-version path.
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .WithVersion("")
            .Build();

        // Assert
        Assert.Equal("", card.Version);
        Assert.Equal("http://adaptivecards.io/schemas/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithVersion_MultipleKnownVersions_UpdatesBothProperties()
    {
        // Arrange & Act — each known-version call should fully replace
        // both Version and Schema with the new version's values.
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .WithVersion(AdaptiveCardVersion.V1_6)
            .Build();

        // Assert
        Assert.Equal("1.6", card.Version);
        Assert.Equal("https://adaptivecards.io/schemas/1.6.0/adaptive-card.json", card.Schema);
    }

    [Fact]
    public void WithVersion_AfterManualWithSchema_OverridesSchemaWithKnownVersionUrl()
    {
        // Arrange & Act — a manual WithSchema override should be replaced
        // when a subsequent WithVersion sets a known version.
        var card = AdaptiveCardBuilder.Create()
            .WithVersion(AdaptiveCardVersion.V1_5)
            .WithSchema("https://example.com/custom-schema.json")
            .WithVersion(AdaptiveCardVersion.V1_6)
            .Build();

        // Assert
        Assert.Equal("1.6", card.Version);
        Assert.Equal("https://adaptivecards.io/schemas/1.6.0/adaptive-card.json", card.Schema);
    }
}
