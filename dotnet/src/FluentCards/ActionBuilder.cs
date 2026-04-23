using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace FluentCards;

/// <summary>
/// Fluent builder for creating AdaptiveAction elements.
/// </summary>
public class ActionBuilder
{
    private AdaptiveAction? _action;
    private bool _dataSet;
    private bool _teamsDataSet;
    private bool _teamsSubmitTypedSet;
    private bool _teamsSubmitRawSet;

    /// <summary>
    /// Creates an OpenUrl action.
    /// </summary>
    /// <param name="url">The URL to open.</param>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder OpenUrl(string url, string? title = null)
    {
        _action = new OpenUrlAction
        {
            Url = url,
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates a Submit action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder Submit(string? title = null)
    {
        _action = new SubmitAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates a ShowCard action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder ShowCard(string? title = null)
    {
        _action = new ShowCardAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates a ToggleVisibility action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder ToggleVisibility(string? title = null)
    {
        _action = new ToggleVisibilityAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Creates an Execute action.
    /// </summary>
    /// <param name="title">The title/label for the action.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder Execute(string? title = null)
    {
        _action = new ExecuteAction
        {
            Title = title
        };
        return this;
    }

    /// <summary>
    /// Sets the unique identifier for the action.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithId(string id)
    {
        EnsureActionTypeSet();
        _action.Id = id;
        return this;
    }

    /// <summary>
    /// Sets the title for the action.
    /// </summary>
    /// <param name="title">The action title.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithTitle(string title)
    {
        EnsureActionTypeSet();
        _action.Title = title;
        return this;
    }

    /// <summary>
    /// Sets the icon URL for the action.
    /// </summary>
    /// <param name="iconUrl">The icon URL.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithIconUrl(string iconUrl)
    {
        EnsureActionTypeSet();
        _action.IconUrl = iconUrl;
        return this;
    }

    /// <summary>
    /// Sets the style for the action.
    /// </summary>
    /// <param name="style">The action style.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithStyle(ActionStyle style)
    {
        EnsureActionTypeSet();
        _action.Style = style;
        return this;
    }

    /// <summary>
    /// Sets whether the action is enabled.
    /// </summary>
    /// <param name="isEnabled">True if enabled, false otherwise.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithIsEnabled(bool isEnabled)
    {
        EnsureActionTypeSet();
        _action.IsEnabled = isEnabled;
        return this;
    }

    /// <summary>
    /// Sets the tooltip for the action.
    /// </summary>
    /// <param name="tooltip">The tooltip text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithTooltip(string tooltip)
    {
        EnsureActionTypeSet();
        _action.Tooltip = tooltip;
        return this;
    }

    /// <summary>
    /// Sets the data payload for Submit and Execute actions.
    /// </summary>
    /// <param name="data">The data payload as a JSON element.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="WithTeamsData"/> or <see cref="WithTeamsTaskFetch"/> was already called.</exception>
    public ActionBuilder WithData(JsonElement data)
    {
        EnsureActionTypeSet();
        if (_teamsDataSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithData and WithTeamsData on the same action. Use WithTeamsData to combine msteams properties with custom data, or WithData for raw JSON.");
        }
        if (_action is SubmitAction submitAction)
        {
            submitAction.Data = data.Clone();
            _dataSet = true;
        }
        else if (_action is ExecuteAction executeAction)
        {
            executeAction.Data = data.Clone();
            _dataSet = true;
        }
        return this;
    }

    /// <summary>
    /// Sets the data payload for Submit and Execute actions from a JSON string.
    /// </summary>
    /// <param name="jsonData">The JSON payload string.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="WithTeamsData"/> or <see cref="WithTeamsTaskFetch"/> was already called.</exception>
    public ActionBuilder WithData(string jsonData)
    {
        EnsureActionTypeSet();
        if (_teamsDataSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithData and WithTeamsData on the same action. Use WithTeamsData to combine msteams properties with custom data, or WithData for raw JSON.");
        }
        if (_action is not SubmitAction && _action is not ExecuteAction)
        {
            return this;
        }

        using var document = JsonDocument.Parse(jsonData);
        return WithData(document.RootElement);
    }

    /// <summary>
    /// Sets the data payload for Submit and Execute actions by serializing an object to a <see cref="JsonElement"/>.
    /// Uses the source-generated <see cref="FluentCardsJsonContext"/> for AOT-compatible serialization.
    /// </summary>
    /// <typeparam name="T">The type of the data object. Must be registered in <see cref="FluentCardsJsonContext"/>.</typeparam>
    /// <param name="data">The data object to serialize.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <see cref="WithTeamsData"/> or <see cref="WithTeamsTaskFetch"/> was already called,
    /// or when the type is not registered in the source-generated context.</exception>
    public ActionBuilder WithData<T>(T data)
    {
        EnsureActionTypeSet();
        if (_teamsDataSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithData and WithTeamsData on the same action. Use WithTeamsData to combine msteams properties with custom data, or WithData for raw JSON.");
        }
        if (_action is not SubmitAction && _action is not ExecuteAction)
        {
            return this;
        }

        var typeInfo = FluentCardsJsonContext.Default.GetTypeInfo(typeof(T))
            ?? throw new InvalidOperationException(
                $"Type '{typeof(T).Name}' is not registered in FluentCardsJsonContext. " +
                "Add [JsonSerializable(typeof({type}))] to FluentCardsJsonContext to use WithData<T>().");

        var element = JsonSerializer.SerializeToElement(data, typeInfo);
        return WithData(element);
    }

    /// <summary>
    /// Sets which inputs are associated with Submit and Execute actions.
    /// </summary>
    /// <param name="associatedInputs">The associated inputs setting.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithAssociatedInputs(AssociatedInputs associatedInputs)
    {
        EnsureActionTypeSet();
        if (_action is SubmitAction submitAction)
        {
            submitAction.AssociatedInputs = associatedInputs;
        }
        else if (_action is ExecuteAction executeAction)
        {
            executeAction.AssociatedInputs = associatedInputs;
        }
        return this;
    }

    /// <summary>
    /// Sets the semantic verb for Execute actions.
    /// </summary>
    /// <param name="verb">The action verb.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithVerb(string verb)
    {
        EnsureActionTypeSet();
        if (_action is ExecuteAction executeAction)
        {
            executeAction.Verb = verb;
        }
        return this;
    }

    /// <summary>
    /// Sets the fallback behavior for the action.
    /// </summary>
    /// <param name="fallback">The fallback value ("drop" or another action).</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithFallback(object fallback)
    {
        EnsureActionTypeSet();
        _action.Fallback = fallback;
        return this;
    }

    /// <summary>
    /// Sets the mode for the action (primary or secondary/overflow).
    /// </summary>
    /// <param name="mode">The action mode.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithMode(ActionMode mode)
    {
        EnsureActionTypeSet();
        _action.Mode = mode;
        return this;
    }

    /// <summary>
    /// Sets the feature requirements for the action.
    /// </summary>
    /// <param name="feature">The feature name.</param>
    /// <param name="version">The minimum required version.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public ActionBuilder WithRequires(string feature, string version)
    {
        EnsureActionTypeSet();
        _action.Requires ??= new Dictionary<string, string>();
        _action.Requires[feature] = version;
        return this;
    }

    /// <summary>
    /// Sets the action data to <c>{ "msteams": { "type": "task/fetch" } }</c> for Submit actions.
    /// Shorthand for <c>WithTeamsData(td =&gt; td.WithTaskFetch())</c>.
    /// </summary>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action is not a Submit action,
    /// or when <see cref="WithData(JsonElement)"/> or <see cref="WithTeamsData"/> was already called.</exception>
    public ActionBuilder WithTeamsTaskFetch()
    {
        EnsureSubmitOnly(nameof(WithTeamsTaskFetch));
        EnsureNoDataConflict();
        var builder = new TeamsDataBuilder();
        builder.WithTaskFetch();
        SetTeamsData(builder.Build());
        return this;
    }

    /// <summary>
    /// Configures a Teams-specific data payload with both <c>msteams</c> properties and custom data.
    /// Only available on Submit actions.
    /// </summary>
    /// <param name="configure">Action to configure the Teams data builder.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action is not a Submit action,
    /// or when <see cref="WithData(JsonElement)"/> was already called.</exception>
    public ActionBuilder WithTeamsData(Action<TeamsDataBuilder> configure)
    {
        EnsureSubmitOnly(nameof(WithTeamsData));
        EnsureNoDataConflict();
        var builder = new TeamsDataBuilder();
        configure(builder);
        SetTeamsData(builder.Build());
        return this;
    }

    /// <summary>
    /// Configures Microsoft Teams–specific submit action properties (e.g. feedback control).
    /// Only available on Submit actions.
    /// </summary>
    /// <param name="configure">Action to configure the Teams submit properties builder.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action is not a Submit action,
    /// or when <see cref="WithTeamsSubmitRaw(string)"/> was already called.</exception>
    public ActionBuilder WithTeamsSubmitFeedback(Action<TeamsSubmitPropertiesBuilder> configure)
    {
        EnsureSubmitOnly(nameof(WithTeamsSubmitFeedback));
        if (_teamsSubmitRawSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithTeamsSubmitFeedback and WithTeamsSubmitRaw on the same action. Use one or the other.");
        }
        var builder = new TeamsSubmitPropertiesBuilder();
        configure(builder);
        ((SubmitAction)_action!).Msteams = builder.Build();
        _teamsSubmitTypedSet = true;
        return this;
    }

    /// <summary>
    /// Sets the Teams <c>msteams</c> action property from a raw JSON string (escape hatch).
    /// Only available on Submit actions.
    /// </summary>
    /// <param name="rawJson">A JSON object string for the msteams value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action is not a Submit action,
    /// or when <see cref="WithTeamsSubmitFeedback"/> was already called.</exception>
    /// <exception cref="ArgumentException">Thrown when the JSON is not an object.</exception>
    public ActionBuilder WithTeamsSubmitRaw(string rawJson)
    {
        EnsureSubmitOnly(nameof(WithTeamsSubmitRaw));
        if (_teamsSubmitTypedSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithTeamsSubmitFeedback and WithTeamsSubmitRaw on the same action. Use one or the other.");
        }
        using var doc = JsonDocument.Parse(rawJson);
        if (doc.RootElement.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The msteams value must be a JSON object.", nameof(rawJson));
        }
        ((SubmitAction)_action!).Msteams = JsonSerializer.Deserialize<TeamsSubmitActionProperties>(
            doc.RootElement, FluentCardsJsonContext.Default.TeamsSubmitActionProperties);
        _teamsSubmitRawSet = true;
        return this;
    }

    /// <summary>
    /// Sets the Teams <c>msteams</c> action property from a <see cref="JsonElement"/> (escape hatch).
    /// Only available on Submit actions.
    /// </summary>
    /// <param name="json">A JSON object element for the msteams value.</param>
    /// <returns>The builder instance for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the action is not a Submit action,
    /// or when <see cref="WithTeamsSubmitFeedback"/> was already called.</exception>
    /// <exception cref="ArgumentException">Thrown when the element is not an object.</exception>
    public ActionBuilder WithTeamsSubmitRaw(JsonElement json)
    {
        EnsureSubmitOnly(nameof(WithTeamsSubmitRaw));
        if (_teamsSubmitTypedSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithTeamsSubmitFeedback and WithTeamsSubmitRaw on the same action. Use one or the other.");
        }
        if (json.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("The msteams value must be a JSON object.", nameof(json));
        }
        ((SubmitAction)_action!).Msteams = JsonSerializer.Deserialize<TeamsSubmitActionProperties>(
            json, FluentCardsJsonContext.Default.TeamsSubmitActionProperties);
        _teamsSubmitRawSet = true;
        return this;
    }

    private void EnsureSubmitOnly(string methodName)
    {
        EnsureActionTypeSet();
        if (_action is not SubmitAction)
        {
            throw new InvalidOperationException(
                $"{methodName} is only available on Submit actions. Call Submit() before using this method.");
        }
    }

    private void EnsureNoDataConflict()
    {
        if (_dataSet)
        {
            throw new InvalidOperationException(
                "Cannot use both WithData and WithTeamsData on the same action. Use WithTeamsData to combine msteams properties with custom data, or WithData for raw JSON.");
        }
    }

    private void SetTeamsData(JsonElement data)
    {
        ((SubmitAction)_action!).Data = data;
        _teamsDataSet = true;
    }

    [System.Diagnostics.CodeAnalysis.MemberNotNull(nameof(_action))]
    private void EnsureActionTypeSet()
    {
        if (_action == null)
        {
            throw new InvalidOperationException(
                "No action type has been specified. Call OpenUrl(), Submit(), ShowCard(), ToggleVisibility(), or Execute() before setting properties.");
        }
    }

    /// <summary>
    /// Builds and returns the configured AdaptiveAction.
    /// </summary>
    /// <returns>The configured AdaptiveAction instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no action type has been specified.</exception>
    public AdaptiveAction Build()
    {
        if (_action == null)
        {
            throw new InvalidOperationException("No action type has been specified. Call OpenUrl(), Submit(), or another action method first.");
        }
        return _action;
    }

}
