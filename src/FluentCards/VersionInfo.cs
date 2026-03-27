namespace FluentCards;

/// <summary>
/// Maps Adaptive Card types and properties to the schema version that introduced them.
/// </summary>
public static class VersionInfo
{
    /// <summary>
    /// Returns the <see cref="AdaptiveCardVersion"/> that introduced the specified element, action,
    /// input, or sub-type.
    /// </summary>
    /// <param name="elementType">
    /// The Adaptive Cards type discriminator string (e.g. <c>"TextBlock"</c>, <c>"Input.Text"</c>,
    /// <c>"Action.OpenUrl"</c>, <c>"Table"</c>).
    /// </param>
    /// <returns>The version that introduced the type.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="elementType"/> is not a recognized type.</exception>
    public static AdaptiveCardVersion GetElementVersion(string elementType) => elementType switch
    {
        // V1.0 — base elements
        "TextBlock" => AdaptiveCardVersion.V1_0,
        "Image" => AdaptiveCardVersion.V1_0,
        "Container" => AdaptiveCardVersion.V1_0,
        "ColumnSet" => AdaptiveCardVersion.V1_0,
        "FactSet" => AdaptiveCardVersion.V1_0,
        "ImageSet" => AdaptiveCardVersion.V1_0,

        // V1.0 — sub-types
        "Column" => AdaptiveCardVersion.V1_0,
        "Fact" => AdaptiveCardVersion.V1_0,
        "Choice" => AdaptiveCardVersion.V1_0,

        // V1.0 — actions
        "Action.OpenUrl" => AdaptiveCardVersion.V1_0,
        "Action.Submit" => AdaptiveCardVersion.V1_0,
        "Action.ShowCard" => AdaptiveCardVersion.V1_0,

        // V1.0 — inputs
        "Input.Text" => AdaptiveCardVersion.V1_0,
        "Input.Number" => AdaptiveCardVersion.V1_0,
        "Input.Date" => AdaptiveCardVersion.V1_0,
        "Input.Time" => AdaptiveCardVersion.V1_0,
        "Input.Toggle" => AdaptiveCardVersion.V1_0,
        "Input.ChoiceSet" => AdaptiveCardVersion.V1_0,

        // V1.1 — new elements and sub-types
        "Media" => AdaptiveCardVersion.V1_1,
        "MediaSource" => AdaptiveCardVersion.V1_1,

        // V1.2 — new elements, sub-types, and actions
        "RichTextBlock" => AdaptiveCardVersion.V1_2,
        "ActionSet" => AdaptiveCardVersion.V1_2,
        "TextRun" => AdaptiveCardVersion.V1_2,
        "BackgroundImage" => AdaptiveCardVersion.V1_2,
        "TargetElement" => AdaptiveCardVersion.V1_2,
        "Action.ToggleVisibility" => AdaptiveCardVersion.V1_2,

        // V1.3 — new enums
        "AssociatedInputs" => AdaptiveCardVersion.V1_3,

        // V1.4 — new actions and types
        "Action.Execute" => AdaptiveCardVersion.V1_4,
        "RefreshConfiguration" => AdaptiveCardVersion.V1_4,
        "AuthenticationConfiguration" => AdaptiveCardVersion.V1_4,
        "TokenExchangeResource" => AdaptiveCardVersion.V1_4,
        "AuthCardButton" => AdaptiveCardVersion.V1_4,

        // V1.5 — new elements, sub-types, and enums
        "Table" => AdaptiveCardVersion.V1_5,
        "TableRow" => AdaptiveCardVersion.V1_5,
        "TableCell" => AdaptiveCardVersion.V1_5,
        "TableColumnDefinition" => AdaptiveCardVersion.V1_5,
        "ActionMode" => AdaptiveCardVersion.V1_5,
        "TextBlockStyle" => AdaptiveCardVersion.V1_5,

        // V1.6 — new sub-types, enums, and enum values
        "CaptionSource" => AdaptiveCardVersion.V1_6,
        "DataQuery" => AdaptiveCardVersion.V1_6,
        "CardMetadata" => AdaptiveCardVersion.V1_6,
        "InputLabelPosition" => AdaptiveCardVersion.V1_6,
        "InputStyle" => AdaptiveCardVersion.V1_6,
        "ChoiceInputStyle.Filtered" => AdaptiveCardVersion.V1_6,

        _ => throw new ArgumentException($"Unknown Adaptive Card element type '{elementType}'.", nameof(elementType))
    };

    /// <summary>
    /// Returns the <see cref="AdaptiveCardVersion"/> that introduced the specified
    /// <see cref="AdaptiveCard"/> property.
    /// </summary>
    /// <param name="propertyName">
    /// The JSON property name of the card-level property (e.g. <c>"refresh"</c>, <c>"rtl"</c>).
    /// </param>
    /// <returns>The version that introduced the property.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="propertyName"/> is not a recognized property.</exception>
    public static AdaptiveCardVersion GetCardPropertyVersion(string propertyName) => propertyName switch
    {
        // V1.0 — base card properties
        "type" => AdaptiveCardVersion.V1_0,
        "version" => AdaptiveCardVersion.V1_0,
        "$schema" => AdaptiveCardVersion.V1_0,
        "body" => AdaptiveCardVersion.V1_0,
        "actions" => AdaptiveCardVersion.V1_0,
        "fallbackText" => AdaptiveCardVersion.V1_0,
        "speak" => AdaptiveCardVersion.V1_0,
        "lang" => AdaptiveCardVersion.V1_0,

        // V1.1 — select action
        "selectAction" => AdaptiveCardVersion.V1_1,

        // V1.2 — layout properties
        "minHeight" => AdaptiveCardVersion.V1_2,
        "verticalContentAlignment" => AdaptiveCardVersion.V1_2,

        // V1.4 — refresh and authentication
        "refresh" => AdaptiveCardVersion.V1_4,
        "authentication" => AdaptiveCardVersion.V1_4,

        // V1.5 — right-to-left support
        "rtl" => AdaptiveCardVersion.V1_5,

        // V1.6 — card metadata
        "metadata" => AdaptiveCardVersion.V1_6,

        _ => throw new ArgumentException($"Unknown AdaptiveCard property '{propertyName}'.", nameof(propertyName))
    };
}
