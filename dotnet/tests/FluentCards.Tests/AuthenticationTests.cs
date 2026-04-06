using System.Text.Json;
using Xunit;

namespace FluentCards.Tests;

public class AuthenticationTests
{
    [Fact]
    public void CardWithAuthentication_Serialization_ContainsAuthenticationProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Text = "Please sign in to continue",
                ConnectionName = "myConnection"
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"authentication\":", json);
        Assert.Contains("\"text\": \"Please sign in to continue\"", json);
        Assert.Contains("\"connectionName\": \"myConnection\"", json);
    }

    [Fact]
    public void AuthenticationWithTokenExchangeResource_Serialization_ContainsResource()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                TokenExchangeResource = new TokenExchangeResource
                {
                    Id = "token123",
                    Uri = "https://example.com/token",
                    ProviderId = "provider456"
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"authentication\":", json);
        Assert.Contains("\"tokenExchangeResource\":", json);
        Assert.Contains("\"id\": \"token123\"", json);
        Assert.Contains("\"uri\": \"https://example.com/token\"", json);
        Assert.Contains("\"providerId\": \"provider456\"", json);
    }

    [Fact]
    public void AuthenticationWithButtons_Serialization_ContainsButtonsArray()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Buttons = new List<AuthCardButton>
                {
                    new AuthCardButton
                    {
                        Type = "signIn",
                        Title = "Sign in with Microsoft",
                        Image = "https://example.com/ms-logo.png",
                        Value = "signin-value"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"authentication\":", json);
        Assert.Contains("\"buttons\":", json);
        Assert.Contains("\"type\": \"signIn\"", json);
        Assert.Contains("\"title\": \"Sign in with Microsoft\"", json);
        Assert.Contains("\"image\": \"https://example.com/ms-logo.png\"", json);
        Assert.Contains("\"value\": \"signin-value\"", json);
    }

    [Fact]
    public void AuthCardButtonWithAllProperties_Serialization_ContainsAllFields()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Buttons = new List<AuthCardButton>
                {
                    new AuthCardButton
                    {
                        Type = "oauth",
                        Title = "Login",
                        Image = "https://example.com/icon.png",
                        Value = "oauth-token"
                    }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"oauth\"", json);
        Assert.Contains("\"title\": \"Login\"", json);
        Assert.Contains("\"image\": \"https://example.com/icon.png\"", json);
        Assert.Contains("\"value\": \"oauth-token\"", json);
    }

    [Fact]
    public void RoundtripSerialization_WithAuthentication_PreservesConfiguration()
    {
        // Arrange
        var originalCard = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Text = "Sign in required",
                ConnectionName = "testConnection",
                TokenExchangeResource = new TokenExchangeResource
                {
                    Id = "resource1",
                    Uri = "https://api.example.com/auth",
                    ProviderId = "azuread"
                },
                Buttons = new List<AuthCardButton>
                {
                    new AuthCardButton
                    {
                        Title = "Sign In",
                        Value = "signin-action"
                    }
                }
            }
        };

        // Act
        var json = originalCard.ToJson();
        var deserializedCard = AdaptiveCardExtensions.FromJson(json);

        // Assert
        Assert.NotNull(deserializedCard);
        Assert.NotNull(deserializedCard.Authentication);
        Assert.Equal("Sign in required", deserializedCard.Authentication.Text);
        Assert.Equal("testConnection", deserializedCard.Authentication.ConnectionName);

        Assert.NotNull(deserializedCard.Authentication.TokenExchangeResource);
        Assert.Equal("resource1", deserializedCard.Authentication.TokenExchangeResource.Id);
        Assert.Equal("https://api.example.com/auth", deserializedCard.Authentication.TokenExchangeResource.Uri);
        Assert.Equal("azuread", deserializedCard.Authentication.TokenExchangeResource.ProviderId);

        Assert.NotNull(deserializedCard.Authentication.Buttons);
        Assert.Single(deserializedCard.Authentication.Buttons);
        Assert.Equal("Sign In", deserializedCard.Authentication.Buttons[0].Title);
        Assert.Equal("signin-action", deserializedCard.Authentication.Buttons[0].Value);
    }

    [Fact]
    public void CardWithoutAuthentication_DoesNotSerializeAuthenticationProperty()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Body = new List<AdaptiveElement>
            {
                new TextBlock { Text = "Hello" }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.DoesNotContain("\"authentication\":", json);
    }

    [Fact]
    public void AuthenticationWithMultipleButtons_Serialization_ContainsAllButtons()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Buttons = new List<AuthCardButton>
                {
                    new AuthCardButton { Title = "Microsoft" },
                    new AuthCardButton { Title = "Google" },
                    new AuthCardButton { Title = "Facebook" }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"buttons\":", json);
        Assert.Contains("\"title\": \"Microsoft\"", json);
        Assert.Contains("\"title\": \"Google\"", json);
        Assert.Contains("\"title\": \"Facebook\"", json);
    }

    [Fact]
    public void AuthCardButtonDefaultType_Serialization_ContainsSignInType()
    {
        // Arrange
        var card = new AdaptiveCard
        {
            Authentication = new AuthenticationConfiguration
            {
                Buttons = new List<AuthCardButton>
                {
                    new AuthCardButton { Title = "Sign In" }
                }
            }
        };

        // Act
        var json = card.ToJson();

        // Assert
        Assert.Contains("\"type\": \"signIn\"", json);
    }
}
