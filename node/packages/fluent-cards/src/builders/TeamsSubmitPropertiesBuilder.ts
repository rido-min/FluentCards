import type { TeamsSubmitActionProperties } from '../models.js';

/** Fluent builder for Teams submit action properties (feedback control). */
export class TeamsSubmitPropertiesBuilder {
  private readonly properties: TeamsSubmitActionProperties = {};

  /** Hides the feedback UI after the submit action is invoked. @returns The builder instance for method chaining. */
  withFeedbackHidden(): this {
    this.properties.feedback = { hide: true };
    return this;
  }

  /** Builds and returns the configured Teams submit action properties. @returns The configured TeamsSubmitActionProperties. */
  build(): TeamsSubmitActionProperties {
    return this.properties;
  }
}
