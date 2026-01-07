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
    /// Builds and returns the configured InputTime.
    /// </summary>
    /// <returns>The configured InputTime instance.</returns>
    public InputTime Build()
    {
        return _input;
    }
}
