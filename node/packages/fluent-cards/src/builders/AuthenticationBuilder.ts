import type { AuthenticationConfiguration, AuthCardButton, TokenExchangeResource } from '../models.js';

/** Fluent builder for {@link AuthenticationConfiguration}. */
export class AuthenticationBuilder {
  private readonly auth: AuthenticationConfiguration = {};

  /** Sets the descriptive text shown in the authentication prompt. @param text The prompt text. @returns The builder instance for method chaining. */
  withText(text: string): this { this.auth.text = text; return this; }
  /** Sets the connection name for OAuth authentication. @param connectionName The connection name. @returns The builder instance for method chaining. */
  withConnectionName(connectionName: string): this { this.auth.connectionName = connectionName; return this; }

  /** Sets the token exchange resource for SSO authentication. @param resource The token exchange resource. @returns The builder instance for method chaining. */
  withTokenExchangeResource(resource: TokenExchangeResource): this {
    this.auth.tokenExchangeResource = resource;
    return this;
  }

  /** Adds a button to the authentication prompt. @param button The authentication button. @returns The builder instance for method chaining. */
  addButton(button: AuthCardButton): this {
    (this.auth.buttons ??= []).push(button);
    return this;
  }

  /** Builds and returns the configured AuthenticationConfiguration. @returns The configured AuthenticationConfiguration instance. */
  build(): AuthenticationConfiguration { return this.auth; }
}
