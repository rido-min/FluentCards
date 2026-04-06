namespace FluentCards;

/// <summary>
/// Fluent builder for creating AuthenticationConfiguration instances.
/// </summary>
public class AuthenticationBuilder
{
    private readonly AuthenticationConfiguration _auth = new();

    /// <summary>
    /// Sets the text to display for the authentication prompt.
    /// </summary>
    /// <param name="text">The prompt text.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AuthenticationBuilder WithText(string text)
    {
        _auth.Text = text;
        return this;
    }

    /// <summary>
    /// Sets the name of the connection to use for authentication.
    /// </summary>
    /// <param name="connectionName">The connection name.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AuthenticationBuilder WithConnectionName(string connectionName)
    {
        _auth.ConnectionName = connectionName;
        return this;
    }

    /// <summary>
    /// Configures the token exchange resource.
    /// </summary>
    /// <param name="tokenExchangeResource">The token exchange resource.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AuthenticationBuilder WithTokenExchangeResource(TokenExchangeResource tokenExchangeResource)
    {
        _auth.TokenExchangeResource = tokenExchangeResource;
        return this;
    }

    /// <summary>
    /// Adds an authentication button.
    /// </summary>
    /// <param name="button">The authentication button to add.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public AuthenticationBuilder AddButton(AuthCardButton button)
    {
        _auth.Buttons ??= new List<AuthCardButton>();
        _auth.Buttons.Add(button);
        return this;
    }

    /// <summary>
    /// Builds and returns the configured AuthenticationConfiguration.
    /// </summary>
    /// <returns>The configured AuthenticationConfiguration instance.</returns>
    public AuthenticationConfiguration Build()
    {
        return _auth;
    }
}
