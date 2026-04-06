import type { AuthenticationConfiguration, AuthCardButton, TokenExchangeResource } from '../models.js';

/** Fluent builder for {@link AuthenticationConfiguration}. */
export class AuthenticationBuilder {
  private readonly auth: AuthenticationConfiguration = {};

  withText(text: string): this { this.auth.text = text; return this; }
  withConnectionName(connectionName: string): this { this.auth.connectionName = connectionName; return this; }

  withTokenExchangeResource(resource: TokenExchangeResource): this {
    this.auth.tokenExchangeResource = resource;
    return this;
  }

  addButton(button: AuthCardButton): this {
    (this.auth.buttons ??= []).push(button);
    return this;
  }

  build(): AuthenticationConfiguration { return this.auth; }
}
