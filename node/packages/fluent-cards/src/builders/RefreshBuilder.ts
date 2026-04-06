import type { RefreshConfiguration } from '../models.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link RefreshConfiguration}. */
export class RefreshBuilder {
  private readonly refresh: RefreshConfiguration = {};

  /** Sets the action used to refresh the card. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.refresh.action = b.build();
    return this;
  }

  /** Adds a user ID that is permitted to trigger the refresh. @param userId The user ID. @returns The builder instance for method chaining. */
  addUserId(userId: string): this {
    (this.refresh.userIds ??= []).push(userId);
    return this;
  }

  /** Sets the expiry timestamp for the refresh. @param expires The ISO 8601 expiry timestamp. @returns The builder instance for method chaining. */
  withExpires(expires: string): this {
    this.refresh.expires = expires;
    return this;
  }

  /** Builds and returns the configured RefreshConfiguration. @returns The configured RefreshConfiguration instance. */
  build(): RefreshConfiguration { return this.refresh; }
}
