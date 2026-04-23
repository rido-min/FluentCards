namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputToggle elements.
/// </summary>
public class InputToggleBuilder
{
    private readonly InputToggle _input = new();

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the title displayed next to the toggle.
    /// </summary>
    /// <param name="title">The title text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithTitle(string title)
    {
        _input.Title = title;
        return this;
    }

    /// <summary>
    /// Sets the value to submit when the toggle is on.
    /// </summary>
    /// <param name="valueOn">The value when on.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithValueOn(string valueOn)
    {
        _input.ValueOn = valueOn;
        return this;
    }

    /// <summary>
    /// Sets the value to submit when the toggle is off.
    /// </summary>
    /// <param name="valueOff">The value when off.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithValueOff(string valueOff)
    {
        _input.ValueOff = valueOff;
        return this;
    }

    /// <summary>
    /// Sets the default value for the toggle.
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithValue(string value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Sets whether the title text should wrap.
    /// </summary>
    /// <param name="wrap">True to wrap, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithWrap(bool wrap)
    {
        _input.Wrap = wrap;
        return this;
    }

    /// <summary>
    /// Sets the position of the label relative to the input.
    /// </summary>
    /// <param name="position">The label position.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithLabelPosition(InputLabelPosition position)
    {
        _input.LabelPosition = position;
        return this;
    }

    /// <summary>
    /// Sets the width of the label when displayed inline.
    /// </summary>
    /// <param name="width">The label width (e.g., "40%" or a pixel value).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithLabelWidth(string width)
    {
        _input.LabelWidth = width;
        return this;
    }

    /// <summary>
    /// Sets the visual style of the input.
    /// </summary>
    /// <param name="style">The input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithInputStyle(InputStyle style)
    {
        _input.InputStyle = style;
        return this;
    }

    /// <summary>
    /// Sets the spacing between this element and the preceding element.
    /// </summary>
    /// <param name="spacing">The spacing value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithSpacing(Spacing spacing)
    {
        _input.Spacing = spacing;
        return this;
    }
    /// <summary>
    /// Sets whether a separator line is drawn at the top of the element.
    /// </summary>
    /// <param name="separator">True to show separator.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithSeparator(bool separator = true)
    {
        _input.Separator = separator;
        return this;
    }
    /// <summary>
    /// Sets whether the element is visible.
    /// </summary>
    /// <param name="isVisible">True if visible, false if hidden.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithIsVisible(bool isVisible)
    {
        _input.IsVisible = isVisible;
        return this;
    }
    /// <summary>
    /// Sets the height of the element.
    /// </summary>
    /// <param name="height">The height ("auto" or "stretch").</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithHeight(string height)
    {
        _input.Height = height;
        return this;
    }
    /// <summary>
    /// Sets the fallback behavior for the element.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another element).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputToggleBuilder WithFallback(object fallback)
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
    public InputToggleBuilder WithRequires(string key, string version)
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
    public InputToggleBuilder WithRtl(bool rtl = true)
    {
        _input.Rtl = rtl;
        return this;
    }
    /// <summary>
    /// Builds and returns the configured InputToggle.
    /// </summary>
    /// <returns>The configured InputToggle instance.</returns>
    public InputToggle Build()
    {
        return _input;
    }
}
