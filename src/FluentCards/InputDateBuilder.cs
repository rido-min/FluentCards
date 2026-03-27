namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputDate elements.
/// </summary>
public class InputDateBuilder
{
    private readonly InputDate _input = new();

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the placeholder text for the input.
    /// </summary>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithPlaceholder(string placeholder)
    {
        _input.Placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Sets the default value for the input.
    /// </summary>
    /// <param name="value">The default value (ISO 8601 date format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithValue(string value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets the minimum date allowed.
    /// </summary>
    /// <param name="min">The minimum date (ISO 8601 date format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithMin(string min)
    {
        _input.Min = min;
        return this;
    }

    /// <summary>
    /// Sets the maximum date allowed.
    /// </summary>
    /// <param name="max">The maximum date (ISO 8601 date format).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithMax(string max)
    {
        _input.Max = max;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Sets the position of the label relative to the input.
    /// </summary>
    /// <param name="position">The label position.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithLabelPosition(InputLabelPosition position)
    {
        _input.LabelPosition = position;
        return this;
    }

    /// <summary>
    /// Sets the width of the label when displayed inline.
    /// </summary>
    /// <param name="width">The label width (e.g., "40%" or a pixel value).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithLabelWidth(string width)
    {
        _input.LabelWidth = width;
        return this;
    }

    /// <summary>
    /// Sets the visual style of the input.
    /// </summary>
    /// <param name="style">The input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputDateBuilder WithInputStyle(InputStyle style)
    {
        _input.InputStyle = style;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured InputDate.
    /// </summary>
    /// <returns>The configured InputDate instance.</returns>
    public InputDate Build()
    {
        return _input;
    }
}
