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
    /// Builds and returns the configured InputToggle.
    /// </summary>
    /// <returns>The configured InputToggle instance.</returns>
    public InputToggle Build()
    {
        return _input;
    }
}
