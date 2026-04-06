import type { RefreshConfiguration } from '../models.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link RefreshConfiguration}. */
export class RefreshBuilder {
  private readonly refresh: RefreshConfiguration = {};

  withAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.refresh.action = b.build();
    return this;
  }

  addUserId(userId: string): this {
    (this.refresh.userIds ??= []).push(userId);
    return this;
  }

  withExpires(expires: string): this {
    this.refresh.expires = expires;
    return this;
  }

  build(): RefreshConfiguration { return this.refresh; }
}
