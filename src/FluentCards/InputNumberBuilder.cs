namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputNumber elements.
/// </summary>
public class InputNumberBuilder
{
    private readonly InputNumber _input = new();

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the placeholder text for the input.
    /// </summary>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithPlaceholder(string placeholder)
    {
        _input.Placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Sets the default value for the input.
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithValue(double value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets the minimum value allowed.
    /// </summary>
    /// <param name="min">The minimum value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithMin(double min)
    {
        _input.Min = min;
        return this;
    }

    /// <summary>
    /// Sets the maximum value allowed.
    /// </summary>
    /// <param name="max">The maximum value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithMax(double max)
    {
        _input.Max = max;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputNumberBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured InputNumber.
    /// </summary>
    /// <returns>The configured InputNumber instance.</returns>
    public InputNumber Build()
    {
        return _input;
    }
}
