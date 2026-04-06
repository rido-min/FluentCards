import { Spacing } from '../../enums.js';
import type { InputToggle } from '../../models.js';

/** Fluent builder for {@link InputToggle} elements. */
export class InputToggleBuilder {
  private readonly input: InputToggle = { type: 'Input.Toggle', id: '', title: '' };

  withId(id: string): this { this.input.id = id; return this; }
  withTitle(title: string): this { this.input.title = title; return this; }
  withLabel(label: string): this { this.input.label = label; return this; }
  withValue(value: string): this { this.input.value = value; return this; }
  withValueOn(valueOn: string): this { this.input.valueOn = valueOn; return this; }
  withValueOff(valueOff: string): this { this.input.valueOff = valueOff; return this; }
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  build(): InputToggle { return this.input; }
}
