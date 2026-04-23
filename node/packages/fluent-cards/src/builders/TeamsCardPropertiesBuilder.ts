import type { TeamsCardProperties, Mention, TeamsCardWidth } from '../models.js';

/** Fluent builder for Teams card-level properties (width, mentions). */
export class TeamsCardPropertiesBuilder {
  private readonly properties: TeamsCardProperties = {};

  /** Sets the card width to Full. @returns The builder instance for method chaining. */
  withFullWidth(): this {
    this.properties.width = 'Full';
    return this;
  }

  /** Adds an @mention entity with auto-generated `<at>displayName</at>` text. @param displayName The user's display name. @param userId The Teams user ID. @returns The builder instance for method chaining. */
  addMention(displayName: string, userId: string): this {
    (this.properties.entities ??= []).push({
      type: 'mention',
      text: `<at>${displayName}</at>`,
      mentioned: { id: userId, name: displayName },
    });
    return this;
  }

  /** Builds and returns the configured Teams card properties. @returns The configured TeamsCardProperties. */
  build(): TeamsCardProperties {
    return this.properties;
  }
}
