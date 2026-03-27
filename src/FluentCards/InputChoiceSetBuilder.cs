namespace FluentCards;

/// <summary>
/// Fluent builder for creating InputChoiceSet elements.
/// </summary>
public class InputChoiceSetBuilder
{
    private readonly InputChoiceSet _input = new() { Choices = new List<Choice>() };

    /// <summary>
    /// Sets the unique identifier for the input.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithId(string id)
    {
        _input.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the label for the input.
    /// </summary>
    /// <param name="label">The input label.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithLabel(string label)
    {
        _input.Label = label;
        return this;
    }

    /// <summary>
    /// Sets the style for displaying choices.
    /// </summary>
    /// <param name="style">The choice input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithStyle(ChoiceInputStyle style)
    {
        _input.Style = style;
        return this;
    }

    /// <summary>
    /// Sets whether multiple choices can be selected.
    /// </summary>
    /// <param name="multiSelect">True for multi-select, false for single select.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder IsMultiSelect(bool multiSelect = true)
    {
        _input.IsMultiSelect = multiSelect;
        return this;
    }

    /// <summary>
    /// Sets the default selected value(s).
    /// </summary>
    /// <param name="value">The default value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithValue(string value)
    {
        _input.Value = value;
        return this;
    }

    /// <summary>
    /// Sets the placeholder text for the input.
    /// </summary>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithPlaceholder(string placeholder)
    {
        _input.Placeholder = placeholder;
        return this;
    }

    /// <summary>
    /// Sets whether the input is required.
    /// </summary>
    /// <param name="required">True if required, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder IsRequired(bool required = true)
    {
        _input.IsRequired = required;
        return this;
    }

    /// <summary>
    /// Sets the error message to display when validation fails.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithErrorMessage(string message)
    {
        _input.ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Sets whether choice text should wrap.
    /// </summary>
    /// <param name="wrap">True to wrap, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithWrap(bool wrap)
    {
        _input.Wrap = wrap;
        return this;
    }

    /// <summary>
    /// Adds a choice to the input.
    /// </summary>
    /// <param name="title">The display text for the choice.</param>
    /// <param name="value">The internal value for the choice.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder AddChoice(string title, string value)
    {
        _input.Choices!.Add(new Choice { Title = title, Value = value });
        return this;
    }

    /// <summary>
    /// Adds a choice to the input.
    /// </summary>
    /// <param name="choice">The choice to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder AddChoice(Choice choice)
    {
        _input.Choices!.Add(choice);
        return this;
    }

    /// <summary>
    /// Sets the position of the label relative to the input.
    /// </summary>
    /// <param name="position">The label position.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithLabelPosition(InputLabelPosition position)
    {
        _input.LabelPosition = position;
        return this;
    }

    /// <summary>
    /// Sets the width of the label when displayed inline.
    /// </summary>
    /// <param name="width">The label width (e.g., "40%" or a pixel value).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithLabelWidth(string width)
    {
        _input.LabelWidth = width;
        return this;
    }

    /// <summary>
    /// Sets the visual style of the input.
    /// </summary>
    /// <param name="style">The input style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public InputChoiceSetBuilder WithInputStyle(InputStyle style)
    {
        _input.InputStyle = style;
        return this;
    }

    /// <summary>
    /// Builds and returns the configured InputChoiceSet.
    /// </summary>
    /// <returns>The configured InputChoiceSet instance.</returns>
    public InputChoiceSet Build()
    {
        return _input;
    }
}
