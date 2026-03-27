namespace FluentCards;

/// <summary>
/// Represents known Adaptive Cards schema versions.
/// </summary>
public enum AdaptiveCardVersion
{
    /// <summary>
    /// Adaptive Cards schema version 1.0.
    /// </summary>
    V1_0,

    /// <summary>
    /// Adaptive Cards schema version 1.1.
    /// </summary>
    V1_1,

    /// <summary>
    /// Adaptive Cards schema version 1.2.
    /// </summary>
    V1_2,

    /// <summary>
    /// Adaptive Cards schema version 1.3.
    /// </summary>
    V1_3,

    /// <summary>
    /// Adaptive Cards schema version 1.4.
    /// </summary>
    V1_4,

    /// <summary>
    /// Adaptive Cards schema version 1.5.
    /// </summary>
    V1_5,

    /// <summary>
    /// Adaptive Cards schema version 1.6.
    /// </summary>
    V1_6
}

/// <summary>
/// Conversion and parsing utilities for <see cref="AdaptiveCardVersion"/>.
/// </summary>
public static class AdaptiveCardVersionExtensions
{
    /// <summary>
    /// Converts an <see cref="AdaptiveCardVersion"/> to its string representation (e.g. <c>"1.5"</c>).
    /// </summary>
    /// <param name="version">The version to convert.</param>
    /// <returns>The version string such as <c>"1.0"</c>, <c>"1.5"</c>, etc.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="version"/> is not a defined enum value.</exception>
    public static string ToVersionString(this AdaptiveCardVersion version) => version switch
    {
        AdaptiveCardVersion.V1_0 => "1.0",
        AdaptiveCardVersion.V1_1 => "1.1",
        AdaptiveCardVersion.V1_2 => "1.2",
        AdaptiveCardVersion.V1_3 => "1.3",
        AdaptiveCardVersion.V1_4 => "1.4",
        AdaptiveCardVersion.V1_5 => "1.5",
        AdaptiveCardVersion.V1_6 => "1.6",
        _ => throw new ArgumentOutOfRangeException(nameof(version), version, $"Unknown AdaptiveCardVersion '{version}'.")
    };

    /// <summary>
    /// Returns the versioned Adaptive Cards schema URL for the specified version
    /// (e.g. <c>"https://adaptivecards.io/schemas/1.5.0/adaptive-card.json"</c>).
    /// </summary>
    /// <param name="version">The version whose schema URL to return.</param>
    /// <returns>The full schema URL for this version.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="version"/> is not a defined enum value.</exception>
    public static string ToSchemaUrl(this AdaptiveCardVersion version) => version switch
    {
        AdaptiveCardVersion.V1_0 => "https://adaptivecards.io/schemas/1.0.0/adaptive-card.json",
        AdaptiveCardVersion.V1_1 => "https://adaptivecards.io/schemas/1.1.0/adaptive-card.json",
        AdaptiveCardVersion.V1_2 => "https://adaptivecards.io/schemas/1.2.0/adaptive-card.json",
        AdaptiveCardVersion.V1_3 => "https://adaptivecards.io/schemas/1.3.0/adaptive-card.json",
        AdaptiveCardVersion.V1_4 => "https://adaptivecards.io/schemas/1.4.0/adaptive-card.json",
        AdaptiveCardVersion.V1_5 => "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
        AdaptiveCardVersion.V1_6 => "https://adaptivecards.io/schemas/1.6.0/adaptive-card.json",
        _ => throw new ArgumentOutOfRangeException(nameof(version), version, $"Unknown AdaptiveCardVersion '{version}'.")
    };

    /// <summary>
    /// Tries to parse a version string (e.g. <c>"1.5"</c>) into an <see cref="AdaptiveCardVersion"/> value.
    /// </summary>
    /// <param name="version">The version string to parse.</param>
    /// <param name="result">
    /// When this method returns <see langword="true"/>, contains the parsed <see cref="AdaptiveCardVersion"/>.
    /// When this method returns <see langword="false"/>, contains the default value.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="version"/> was a recognized version string; otherwise <see langword="false"/>.</returns>
    public static bool TryParse(string version, out AdaptiveCardVersion result)
    {
        switch (version)
        {
            case "1.0":
                result = AdaptiveCardVersion.V1_0;
                return true;
            case "1.1":
                result = AdaptiveCardVersion.V1_1;
                return true;
            case "1.2":
                result = AdaptiveCardVersion.V1_2;
                return true;
            case "1.3":
                result = AdaptiveCardVersion.V1_3;
                return true;
            case "1.4":
                result = AdaptiveCardVersion.V1_4;
                return true;
            case "1.5":
                result = AdaptiveCardVersion.V1_5;
                return true;
            case "1.6":
                result = AdaptiveCardVersion.V1_6;
                return true;
            default:
                result = default;
                return false;
        }
    }
}
