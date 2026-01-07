namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputText elements.
/// </summary>
public class InputTextBuilder
{
    private readonly InputText _input = new();

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the placeholder text for the input.
    /// </summary>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithPlaceholder(string placeholder)
    {
        _input.Placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Sets the default value for the input.
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithValue(string value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets the maximum length for the input.
    /// </summary>
    /// <param name="maxLength">The maximum length.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithMaxLength(int maxLength)
    {
        _input.MaxLength = maxLength;
        return this;
    }

    /// <summary>
    /// Sets whether the input allows multiple lines of text.
    /// </summary>
    /// <param name="multiline">True for multiline, false for single line.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder IsMultiline(bool multiline = true)
    {
        _input.IsMultiline = multiline;
        return this;
    }

    /// <summary>
    /// Sets the style of the text input.
    /// </summary>
    /// <param name="style">The text input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithStyle(TextInputStyle style)
    {
        _input.Style = style;
        return this;
    }

    /// <summary>
    /// Sets the regular expression for input validation.
    /// </summary>
    /// <param name="regex">The validation regex.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithRegex(string regex)
    {
        _input.Regex = regex;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Configures the inline action for the input.
    /// </summary>
    /// <param name="configure">Action to configure the inline action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputTextBuilder WithInlineAction(Action<ActionBuilder> configure)
    {
        var builder = new ActionBuilder();
        configure(builder);
        _input.InlineAction = builder.Build();
        return this;
    }

    /// <summary>
    /// Builds and returns the configured InputText.
    /// </summary>
    /// <returns>The configured InputText instance.</returns>
    public InputText Build()
    {
        return _input;
    }
}
