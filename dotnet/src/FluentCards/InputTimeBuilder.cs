namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputTime elements.
/// </summary>
public class InputTimeBuilder
{
    private readonly InputTime _input = new();

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the placeholder text for the input.
    /// </summary>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithPlaceholder(string placeholder)
    {
        _input.Placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Sets the default value for the input.
    /// </summary>
    /// <param name="value">The default value (ISO 8601 time format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithValue(string value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets the minimum time allowed.
    /// </summary>
    /// <param name="min">The minimum time (ISO 8601 time format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithMin(string min)
    {
        _input.Min = min;
        return this;
    }

    /// <summary>
    /// Sets the maximum time allowed.
    /// </summary>
    /// <param name="max">The maximum time (ISO 8601 time format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithMax(string max)
    {
        _input.Max = max;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Sets the position of the label relative to the input.
    /// </summary>
    /// <param name="position">The label position.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithLabelPosition(InputLabelPosition position)
    {
        _input.LabelPosition = position;
        return this;
    }

    /// <summary>
    /// Sets the width of the label when displayed inline.
    /// </summary>
    /// <param name="width">The label width (e.g., "40%" or a pixel value).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithLabelWidth(string width)
    {
        _input.LabelWidth = width;
        return this;
    }

    /// <summary>
    /// Sets the visual style of the input.
    /// </summary>
    /// <param name="style">The input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithInputStyle(InputStyle style)
    {
        _input.InputStyle = style;
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithSpacing(Spacing spacing)
    {
        _input.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithSeparator(bool separator = true)
    {
        _input.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithIsVisible(bool isVisible)
    {
        _input.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithHeight(string height)
    {
        _input.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithFallback(object fallback)
    {
        _input.Fallback = fallback;
        return this;
    }
    /// <summary>
    /// Sets the feature requirements for the element.
    /// </summary>
    /// <param name="key">The feature key.</param>
    /// <param name="version">The minimum version required.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithRequires(string key, string version)
    {
        _input.Requires ??= new Dictionary<string, string>();
        _input.Requires[key] = version;
        return this;
    }
    /// <summary>
    /// Sets whether content should be presented right to left.
    /// </summary>
    /// <param name="rtl">True for right-to-left.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTimeBuilder WithRtl(bool rtl = true)
    {
        _input.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured InputTime.
    /// </summary>
    /// <returns>The configured InputTime instance.</returns>
    public InputTime Build()
    {
        return _input;
    }
}
