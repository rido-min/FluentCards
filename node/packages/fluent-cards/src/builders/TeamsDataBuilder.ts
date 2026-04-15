/** Fluent builder for Teams-specific action data payloads containing an optional `msteams` object and custom properties. */
export class TeamsDataBuilder {
  private msteamsValue: Record<string, unknown> | undefined;
  private readonly customProperties: Record<string, unknown> = {};

  /** Sets the `msteams` object to `{ type: 'task/fetch' }`. @returns The builder instance for method chaining. */
  withTaskFetch(): this {
    this.msteamsValue = { type: 'task/fetch' };
    return this;
  }

  /** Sets the `msteams` object from a plain object. @param value The msteams object value. Must be a plain object. @returns The builder instance for method chaining. */
  withMsteams(value: Record<string, unknown>): this {
    if (typeof value !== 'object' || value === null || Array.isArray(value)) {
      throw new Error('The msteams value must be a plain object.');
    }
    this.msteamsValue = { ...value };
    return this;
  }

  /** Adds a custom property to the data payload. @param key The property name. Cannot be "msteams". @param value The property value. @returns The builder instance for method chaining. */
  withProperty(key: string, value: unknown): this {
    if (key.toLowerCase() === 'msteams') {
      throw new Error("Cannot use 'msteams' as a property key. Use withMsteams() or withTaskFetch() instead.");
    }
    this.customProperties[key] = value;
    return this;
  }

  /** Builds and returns the data payload as a plain object. @returns The data object containing msteams and custom properties. */
  build(): Record<string, unknown> {
    const result: Record<string, unknown> = {};
    if (this.msteamsValue) {
      result.msteams = this.msteamsValue;
    }
    Object.assign(result, this.customProperties);
    return result;
  }
}
